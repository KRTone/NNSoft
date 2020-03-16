#include "pch.h"
#include "Functions.h"
#include <WTypes.h>
#include <OAIdl.h>
#include <atlsafe.h>
#include <iostream>
#include <comdef.h>
#include <thread>
#include <stdlib.h>
using namespace std;


HRESULT LoadServiceInfo(LPSTR serviceName, LPSTR* servicePath, LPSTR* serviceGroup)
{
    DWORD bytesNeeded;
    SC_HANDLE manager = OpenSCManagerA(NULL, NULL, SC_MANAGER_ALL_ACCESS);

    if (manager == INVALID_HANDLE_VALUE)
    {
        return S_FALSE;
    }

    SC_HANDLE service = OpenServiceA(manager, serviceName, SERVICE_QUERY_CONFIG);

    if (service == INVALID_HANDLE_VALUE)
    {
        return S_FALSE;
    }

    QueryServiceConfigA(
        service,
        NULL,
        0,
        &bytesNeeded);

    LPQUERY_SERVICE_CONFIG lpsc = (LPQUERY_SERVICE_CONFIG)malloc(bytesNeeded * sizeof * lpsc);

    QueryServiceConfigA(
        service,
        lpsc,
        bytesNeeded,
        &bytesNeeded
    );
    *servicePath = _strdup(lpsc->lpBinaryPathName);
    *serviceGroup = _strdup(lpsc->lpLoadOrderGroup);//lpsc->lpLoadOrderGroup;

    free(lpsc);
    CloseServiceHandle(manager);
    CloseServiceHandle(service);
    return S_OK;
}

int _GetServiceCount() 
{
    SC_HANDLE manager = OpenSCManagerA(NULL, NULL, SC_MANAGER_ALL_ACCESS);

    if (manager == INVALID_HANDLE_VALUE)
    {
        return S_FALSE;
    }

    DWORD bytesNeeded;
    DWORD servicesCount;

    BOOL status = EnumServicesStatusExA(
        manager,
        SC_ENUM_PROCESS_INFO,
        SERVICE_WIN32,
        SERVICE_STATE_ALL,
        NULL,
        0,
        &bytesNeeded,
        &servicesCount,
        NULL,
        NULL
    );

    PBYTE lpBytes = (PBYTE)malloc(bytesNeeded * sizeof lpBytes);

    status = EnumServicesStatusExA(
        manager,
        SC_ENUM_PROCESS_INFO,
        SERVICE_WIN32,
        SERVICE_STATE_ALL,
        lpBytes,
        bytesNeeded,
        &bytesNeeded,
        &servicesCount,
        NULL,
        NULL
    );

    free(lpBytes);
    CloseServiceHandle(manager);
    return servicesCount;
}

HRESULT EnumerateServices(ServiceInfo* services)
{
    SC_HANDLE manager = OpenSCManagerA(NULL, NULL, SC_MANAGER_ALL_ACCESS);

    if (manager == INVALID_HANDLE_VALUE) 
    {
        return S_FALSE;
    }

    DWORD bytesNeeded;
    DWORD servicesCount;

    BOOL status = EnumServicesStatusExA(
        manager,
        SC_ENUM_PROCESS_INFO,
        SERVICE_WIN32,
        SERVICE_STATE_ALL,
        NULL,
        0,
        &bytesNeeded,
        &servicesCount,
        NULL,
        NULL
    );

    PBYTE lpServiceBytes = (PBYTE)malloc(bytesNeeded);

    status = EnumServicesStatusExA(
        manager,
        SC_ENUM_PROCESS_INFO,
        SERVICE_WIN32,
        SERVICE_STATE_ALL,
        lpServiceBytes,
        bytesNeeded,
        &bytesNeeded,
        &servicesCount,
        NULL,
        NULL
    );

    if (status == false)
    {
        return S_FALSE;
    }

    LPENUM_SERVICE_STATUS_PROCESS lpServiceStatus = (LPENUM_SERVICE_STATUS_PROCESS)lpServiceBytes;

    cout << "Services loaded" << endl;

    for (DWORD i = 0; i < servicesCount; i++) {
        LPSTR groupName = nullptr;
        LPSTR Path = nullptr;

        HRESULT result = LoadServiceInfo(lpServiceStatus[i].lpServiceName, &Path, &groupName);

        ServiceInfo service;
        service.description = _strdup(lpServiceStatus[i].lpDisplayName);
        service.group = groupName;
        service.id = lpServiceStatus[i].ServiceStatusProcess.dwProcessId;
        service.name = _strdup(lpServiceStatus[i].lpServiceName);
        service.path = Path;
        service.state = lpServiceStatus[i].ServiceStatusProcess.dwCurrentState;
        services[i] = service;

        if (result == S_FALSE)
            return S_FALSE;
    }

    cout << "Extension Service data is loaded" << endl;

    free(lpServiceBytes);
    CloseServiceHandle(manager);
    return S_OK;
}

extern "C" HRESULT _GetServices(ServiceInfo* services)
{
    cout << "Start Enumerate Services" << endl;
    HRESULT result = EnumerateServices(services);
    cout << "End Enumerate Services" << endl;
    cout << "Exit From calling method" << endl;
    return result;
}

void GetServiceStatus(ServiceInfo* serviceInfo, SC_HANDLE hdService)
{
    SERVICE_STATUS_PROCESS serviceStatus;
    DWORD bytesNeeded;
    QueryServiceStatusEx(hdService, SC_STATUS_PROCESS_INFO, (LPBYTE)&serviceStatus, sizeof SERVICE_STATUS_PROCESS, &bytesNeeded);

    serviceInfo->id = serviceStatus.dwProcessId;
    serviceInfo->state = serviceStatus.dwCurrentState;
}

void WaitWhileStatus(ServiceInfo* serviceInfo, SC_HANDLE service, DWORD status) {
    int count = 1;
    do
    {
        count++;
        std::this_thread::sleep_for(1s);
        GetServiceStatus(serviceInfo, service);
        std::cout << "Checking Service Status...\n";
    } while (serviceInfo->state != status || count == 5);
}

HRESULT SetServiceState(ServiceInfo* serviceInfo, DWORD operation) 
{
    BOOL result;
    SC_HANDLE manager = OpenSCManager(NULL, NULL, SC_MANAGER_ALL_ACCESS);
    SC_HANDLE service = OpenServiceA(manager, serviceInfo->name, SC_MANAGER_ALL_ACCESS);

    SERVICE_STATUS_PROCESS serviceStatus;
    DWORD bytesNeeded;
    QueryServiceStatusEx(service, SC_STATUS_PROCESS_INFO, (LPBYTE)&serviceStatus, sizeof SERVICE_STATUS_PROCESS, &bytesNeeded);

    if (serviceStatus.dwCurrentState != operation)
    {
        if (ControlService(service, operation, (LPSERVICE_STATUS)&serviceStatus))
        {
            cout << "Status changed successully" << endl;
            result = true;
        }
        else
        {
            cout << "Status has not changed" << endl;
            result = false;
        }
    }
    
    WaitWhileStatus(serviceInfo, service, operation);

    CloseServiceHandle(service);
    CloseServiceHandle(manager);
    return result ? S_OK : S_FALSE;
}

extern "C" HRESULT _StopService(ServiceInfo* service)
{
    cout << "StopService("<< service->name << ")" << endl;
    return SetServiceState(service, SERVICE_CONTROL_STOP);
}

HRESULT StartService(ServiceInfo* serviceInfo)
{
    SC_HANDLE manager = OpenSCManager(NULL, NULL, SC_MANAGER_ALL_ACCESS);
    SC_HANDLE service = OpenServiceA(manager, serviceInfo->name, SC_MANAGER_ALL_ACCESS);
    BOOL result = StartServiceA(service, NULL, NULL);

    WaitWhileStatus(serviceInfo, service, SERVICE_RUNNING);

    result ? cout << "Service started" : cout << "Something went wrong. Service has not started";
    cout << endl;
    return result ? S_OK : S_FALSE;
}

extern "C" HRESULT _StartService(ServiceInfo * service)
{
    cout << "StartService(" << service->name << ")" << endl;
    HRESULT result = StartService(service);
    return result;
}

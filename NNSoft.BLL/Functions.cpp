#include "pch.h"
#include "Functions.h"
#include <WTypes.h>
#include <OAIdl.h>
#include <atlsafe.h>
#include <iostream>
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

int GetServiceCount() 
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
        service.Description = _strdup(lpServiceStatus[i].lpDisplayName);
        service.Group = groupName;
        service.Id = lpServiceStatus[i].ServiceStatusProcess.dwProcessId;
        service.Name = _strdup(lpServiceStatus[i].lpServiceName);
        service.Path = Path;
        service.State = lpServiceStatus[i].ServiceStatusProcess.dwCurrentState;
        services[i] = service;

        if (result == S_FALSE)
            return S_FALSE;
    }

    cout << "Extension Service data is loaded" << endl;

    free(lpServiceBytes);
    CloseServiceHandle(manager);
    return S_OK;
}

extern "C" HRESULT GetServices(ServiceInfo* services)
{
    cout << "Start Enumerate Services" << endl;
    HRESULT result = EnumerateServices(services);
    cout << "End Enumerate Services" << endl;
    cout << "Exit From calling method" << endl;
    return result;
}

HRESULT SetServiceState(LPCSTR serviceName, DWORD operation) 
{
    BOOL result;
    SC_HANDLE manager = OpenSCManager(NULL, NULL, SC_MANAGER_ALL_ACCESS);
    SC_HANDLE service = OpenServiceA(manager, serviceName, SC_MANAGER_ALL_ACCESS);

    SERVICE_STATUS_PROCESS serviceStatus;
    DWORD bytesNeeded;
    QueryServiceStatusEx(service, SC_STATUS_PROCESS_INFO, (LPBYTE)&serviceStatus, sizeof SERVICE_STATUS_PROCESS, &bytesNeeded);

    if (serviceStatus.dwCurrentState != operation)
    {
        if (ControlService(manager, operation, (LPSERVICE_STATUS)&serviceStatus))
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

    CloseServiceHandle(service);
    CloseServiceHandle(manager);
    return result ? S_OK : S_FALSE;
}

extern "C" HRESULT StopService(ServiceInfo service)
{
    cout << "StartService" << endl;
    return SetServiceState(service.Name, SERVICE_CONTROL_STOP);
}

HRESULT StartService(LPCSTR serviceName)
{
    SC_HANDLE manager = OpenSCManager(NULL, NULL, SC_MANAGER_ALL_ACCESS);
    SC_HANDLE service = OpenServiceA(manager, serviceName, SC_MANAGER_ALL_ACCESS);
    BOOL result = StartServiceA(service, NULL, NULL);
    result ? cout << "Service started" : cout << "Something went wrong. Service has not started";
    cout << endl;
    return result ? S_OK : S_FALSE;
}

extern "C" HRESULT StartService(ServiceInfo service)
{
    cout << "StartService" << endl;
    return StartService(service.Name);
}

#include "pch.h"
#include "Functions.h"
#include <WTypes.h>
#include <OAIdl.h>
#include <atlsafe.h>
#define SIZE_BUF 4096
using namespace std;


extern "C" HRESULT GetServices(SAFEARRAY **ppsa)
{
    SC_HANDLE handle = OpenSCManagerA(nullptr, nullptr, SC_MANAGER_ALL_ACCESS);
    try
    {
        ENUM_SERVICE_STATUS Status[SIZE_BUF];
        DWORD size = sizeof(Status);
        DWORD needed = 0;
        DWORD count = 0;
        DWORD resumeHandle = 0;
        if (EnumServicesStatusA(
            handle,
            SERVICE_WIN32,
            SERVICE_STATE_ALL,
            (LPENUM_SERVICE_STATUSA)&Status,
            size,
            &needed,
            &count,
            &resumeHandle))
        {
            CComSafeArray<BSTR> sa(count);

            for (unsigned int indx = 0; indx < count; indx++)
            {
                CComBSTR bstr = Status[indx].lpServiceName;
                HRESULT hr = sa.SetAt(indx, bstr.Detach(), FALSE);
                if (FAILED(hr))
                    return hr;
            }
            *ppsa = sa.Detach();
        }
        CloseServiceHandle(handle);
        return S_FALSE;
    }
    catch (const CAtlException & e)
    {
        CloseServiceHandle(handle);
        return e;
    }
    CloseServiceHandle(handle);
    return S_OK;
}


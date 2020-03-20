#pragma once
#include "pch.h"
#include "Functions.h"
#include <WTypes.h>
#include <OAIdl.h>
#include <atlsafe.h>
using namespace std;

typedef struct _ServiceInfo
{
    char* name;
    char* description;
    char* group;
    int id;
    int state;
    char* path;
} ServiceInfo, *LPServiceInfo;

extern "C" __declspec(dllexport) HRESULT _GetServices(ServiceInfo** services, LPDWORD servicesCount);
extern "C" __declspec(dllexport) HRESULT _StopService(ServiceInfo* service);
extern "C" __declspec(dllexport) HRESULT _StartService(ServiceInfo* service);

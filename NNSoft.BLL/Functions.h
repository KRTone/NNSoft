#pragma once
#include "pch.h"
#include "Functions.h"
#include <WTypes.h>
#include <OAIdl.h>
#include <atlsafe.h>
using namespace std;

typedef struct _ServiceInfo
{
    char* Name;
    char* Description;
    char* Group;
    int Id;
    int State;
    char* Path;
} ServiceInfo, *LPServiceInfo;

extern "C" __declspec(dllexport) HRESULT GetServices(ServiceInfo* services);
extern "C" __declspec(dllexport) int GetServiceCount();

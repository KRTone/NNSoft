#pragma once
#include "pch.h"
#include "Functions.h"
#include <WTypes.h>
#include <OAIdl.h>
#include <atlsafe.h>
using namespace std;

extern "C" __declspec(dllexport) HRESULT GetServices(SAFEARRAY **ppsa);

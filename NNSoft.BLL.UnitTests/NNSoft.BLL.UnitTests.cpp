#include "pch.h"
#include "CppUnitTest.h"
#include "../NNSoft.BLL/Functions.h"
#include "../NNSoft.BLL/NullPtrException.h"

using namespace Microsoft::VisualStudio::CppUnitTestFramework;

namespace NNSoftBLLUnitTests
{
	TEST_CLASS(NNSoftBLLUnitTests)
	{
	public:
		TEST_METHOD(Functions_GetServices_OK)
		{
			LPServiceInfo services;
			DWORD servicesCount;
			HRESULT result = _GetServices(&services, &servicesCount);
			Assert::AreEqual(result, S_OK);
			Assert::AreNotEqual(services->name, NULL);
			free(services);
		}

		TEST_METHOD(Functions_StopService_OK)
		{
			LPServiceInfo service = new ServiceInfo();
			service->name = "AdobeARMService";
			HRESULT result = _StopService(service);
			Assert::AreEqual(result, S_OK);
			delete service;
			service = nullptr;
		}

		TEST_METHOD(Functions_StartService_OK)
		{
			LPServiceInfo service = new ServiceInfo();
			service->name = "AdobeARMService";
			HRESULT result = _StartService(service);
			Assert::AreEqual(result, S_OK);
			delete service;
			service = nullptr;
		}

		TEST_METHOD(Functions_StartService_NullPtr)
		{
			try
			{
				_StartService(NULL);
			}
			catch (const std::exception& ex)
			{
				if (typeid(ex) != typeid(NullPointerException))
					Assert::Fail();
			}
		}

		TEST_METHOD(Functions_StopService_NullPtr)
		{
			try
			{
				_StopService(NULL);
			}
			catch (const std::exception & ex)
			{
				if (typeid(ex) != typeid(NullPointerException))
					Assert::Fail();
			}
		}

		TEST_METHOD(Functions_GetServices_NullPtr)
		{
			try
			{
				_GetServices(NULL, NULL);
			}
			catch (const std::exception & ex)
			{
				if (typeid(ex) != typeid(NullPointerException))
					Assert::Fail();
			}
		}
	};
}

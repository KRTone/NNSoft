#include "pch.h"
#include "CppUnitTest.h"
#include "../NNSoft.BLL/Functions.h"

using namespace Microsoft::VisualStudio::CppUnitTestFramework;

namespace NNSoftBLLUnitTests
{
	TEST_CLASS(NNSoftBLLUnitTests)
	{
	public:
		TEST_METHOD(Functions_GetServices_OK)
		{
			LPServiceInfo services = (LPServiceInfo)malloc(_GetServiceCount() * sizeof *services);
			HRESULT result = _GetServices(services);
			Assert::AreEqual(result, S_OK);
			free(services);
		}

		TEST_METHOD(Functions_GetServiceCount_OK)
		{
			int count = _GetServiceCount();

			Assert::AreNotEqual(count, 0);
		}

		TEST_METHOD(Functions_StopService_OK)
		{
			LPServiceInfo services = new ServiceInfo();
			services->name = "AdobeARMService";
			HRESULT result = _StopService(services);
			Assert::AreEqual(result, S_OK);
			free(services);
		}

		TEST_METHOD(Functions_StartService_OK)
		{
			LPServiceInfo services = new ServiceInfo();
			services->name = "AdobeARMService";
			HRESULT result = _StartService(services);
			Assert::AreEqual(result, S_OK);
			free(services);
		}
	};
}

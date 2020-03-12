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
			LPServiceInfo services = (LPServiceInfo)malloc(GetServiceCount() * sizeof *services);
			HRESULT result = GetServices(services);
			Assert::AreEqual(result, S_OK);
			free(services);
		}

		TEST_METHOD(Functions_GetServiceCount_OK)
		{
			int count = GetServiceCount();

			Assert::AreNotEqual(count, 0);
		}
	};
}

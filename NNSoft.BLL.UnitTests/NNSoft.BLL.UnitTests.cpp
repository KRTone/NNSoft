#include "pch.h"
#include "CppUnitTest.h"
#include "../NNSoft.BLL/Functions.h"
#include <consoleapi2.h>

using namespace Microsoft::VisualStudio::CppUnitTestFramework;

namespace NNSoftBLLUnitTests
{
	TEST_CLASS(NNSoftBLLUnitTests)
	{
	public:
		
		TEST_METHOD(TestMethod1)
		{
			SAFEARRAY* b;
			auto bc = GetServices(&b);
		}
	};
}

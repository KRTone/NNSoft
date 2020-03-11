using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace NNSoft.PL.Utils
{
    class BLLInvoke
    {
        [DllImport("NNSoft.BLL.dll")]
        public static extern int GetServices();
    }
}

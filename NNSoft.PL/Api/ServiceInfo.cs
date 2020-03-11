using System.Runtime.InteropServices;

namespace NNSoft.PL.Api
{
    public class ServiceInfo
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 100)]
        public string Name;
        public int Pid;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 100)]
        public string DisplayName;
        public int State;
        public string Group;
        public string Path;
    }
}

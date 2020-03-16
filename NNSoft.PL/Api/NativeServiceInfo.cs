using System.Runtime.InteropServices;

namespace NNSoft.PL.Api
{
    [StructLayout(LayoutKind.Sequential)]
    public class NativeServiceInfo
    {
        public string name;

        public string description;

        public string group;

        public int id;

        public int state;

        public string path;
    }
}
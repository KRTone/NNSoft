using System.Runtime.InteropServices;

namespace NNSoft.PL.Api
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct NativeServiceInfo
    {
        public string Name;

        public string Description;

        public string Group;

        public int Id;

        public int State;

        public string Path;
    }
}
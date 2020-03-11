using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NNSoft.PL.Common
{
    public class ServiceInfo
    {
        public string Name { get; set; }
        public int Pid { get; set; }
        public string DisplayName { get; set; }
        public int State { get; set; }
        public string Group { get; set; }
        public string Path { get; set; }
    }
}

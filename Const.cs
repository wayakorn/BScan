using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BScan
{
    class Const
    {
        public const int PropertyIdControlType = 30003;
        public const int PropertyIdName = 30005;
        public const int ControlTypeIdHyperlink = 50005;

        public const int PatternIdInvoke = 10000;
        public const int PatternIdValue = 10002;

        public const bool Verbose = false;
        public const string SearchPattern = "bing.com/search"; // was "bing.com/"

        public const int MaxLinkLevel = 8;
        public const int MaxLoopCount = 100;
        public const int MaxExceptionCount = 20;
    }
}

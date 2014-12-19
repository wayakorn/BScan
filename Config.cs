using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BScan
{
    class Config
    {
        // TODO: read these from a file
        private int _maxVisitsLower = 33;
        private int _maxVisitsUpper = 99;
        private int _waitStartupLower = 60 * 2; // 2 mins
        private int _waitStartupUpper = 60 * 60; // 1 hour
        private int _waitInvokeLower = 10;
        private int _waitInvokeUpper = 40;

        public readonly int MaxVisits = 0;
        public readonly int WaitStartup = 0;

        public Config()
        {
            MaxVisits = Util.GetRandom(_maxVisitsLower, _maxVisitsUpper);
            WaitStartup = Util.GetRandom(_waitStartupLower, _waitStartupUpper);

            Console.WriteLine(@"MaxVisits={0}, WaitStartup={1}", MaxVisits, WaitStartup);
        }

        public int GetWaitInvoke()
        {
            return Util.GetRandom(_waitInvokeLower, _waitInvokeUpper);
        }
    }
}

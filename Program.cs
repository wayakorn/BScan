using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BScan
{
    class Program
    {
        static void Main(string[] args)
        {
            Browser browser = Browser.Create();
            List<BLink> links = browser.ScanLinks();
            foreach (BLink link in links)
            {
                Console.WriteLine(link);
            }
        }
    }
}

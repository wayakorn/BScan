using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace BScan
{
    class Program
    {
        Config _config = new Config();
        Process _process;
        Browser _browser;
        static void Main(string[] args)
        {
            Program program = new Program();
            program.Run(args);
        }
        void Run(string[] args)
        {
            Util.Wait(_config.WaitStartup);

            int currentLinkLevel = 0;
            List<string> visitedUrls = new List<string>();

            int loopCount = 0;
            int exceptionCount = 0;

            while (visitedUrls.Count < _config.MaxVisits)
            {
                try
                {
                    if (currentLinkLevel >= Const.MaxLinkLevel)
                    {
                        Console.WriteLine(@"LinkLevel is now at {0}, resetting to 0", currentLinkLevel);
                        currentLinkLevel = 0;
                    }

                    if (currentLinkLevel == 0)
                        StartBing();

                    List<BLink> links = _browser.ScanLinks();
                    Console.WriteLine(@"Found {0} BLink on the current page", links.Count);
                    if (links.Count == 0)
                    {
                        currentLinkLevel = 0;
                        continue;
                    }

                    currentLinkLevel++;
                    Console.WriteLine(@"Current Link Layer is {0}", currentLinkLevel);

                    if (Const.Verbose)
                    {
                        // Print all L1 links found
                        if (currentLinkLevel == 1)
                        {
                            foreach (BLink ll in links)
                                Console.WriteLine(ll);
                        }
                    }

                    bool invoked = false;
                    while (!invoked)
                    {
                        int pick = Util.GetRandom(0, links.Count);
                        BLink link = links[pick];
                        links.RemoveAt(pick);

                        Console.WriteLine(@"Invoking: {0}", link);
                        if (link.Invoke())
                        {
                            invoked = true;
                            visitedUrls.Add(link.Url);

                            Console.WriteLine(@"[{0}] Visit {1}/{2}", DateTime.Now, visitedUrls.Count, _config.MaxVisits);

                            Util.Wait(_config.GetWaitInvoke());
                        }
                    }

                    // Rescan the links
                    links = _browser.ScanLinks();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Encountered an exception:");
                    Console.WriteLine(ex);

                    currentLinkLevel = 0;

                    exceptionCount++;
                    Console.WriteLine(@"Exception count is {0}", exceptionCount);

                    if (exceptionCount > Const.MaxExceptionCount)
                        break;

                    Util.Wait(60);
                }

                loopCount++;
                Console.WriteLine(@"Loop count is {0}", loopCount);

                if (loopCount >= Const.MaxLoopCount)
                    break;
            }        
        }
        void StartBing()
        {
            Console.WriteLine(@"Starting Bing...");
            Console.WriteLine(@"[{0}] Starting Bing...", DateTime.Now);

            if (_process != null)
            {
                try
                {
                    _process.Kill();
                }
                catch (InvalidOperationException ex)
                {
                    Console.WriteLine(@"Process.Kill threw an exception {0}", ex.Message);
                }
            }

            for (_browser = null; _browser == null;)
            {
                // Start IE Bing homepage
                _process = new Process();
                _process.StartInfo.FileName = @"C:\Program Files\Internet Explorer\iexplore.exe";
                _process.StartInfo.Arguments = @"http://www.bing.com";
                _process.Start();

                Util.Wait(10);

                // Attach the Browser object to a new IEFrame instance just started
                _browser = Browser.Create();
            }
        }
    }
}

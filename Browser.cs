using System;
using System.Collections.Generic;
using System.Diagnostics;
using interop.UIAutomationCore;

namespace BScan
{
    class Browser
    {
        private IUIAutomation _automation; // Main UIA object required by any UIA client app.
        private IUIAutomationElement _ie; // UIA element representing the browser window.
        public static Browser Create()
        {
            IUIAutomation automation = new CUIAutomation();

            IntPtr hwnd = Win32.FindWindow("IEFrame", null);
            if (hwnd == IntPtr.Zero)
            {
                Console.WriteLine(@"Cannot find IEFrame");
            }
            else
            {
                IUIAutomationElement ie = automation.ElementFromHandle(hwnd);
                if (ie != null)
                {
                    return new Browser(automation, ie);
                }
            }
            return null;
        }

        public Browser(IUIAutomation automation, IUIAutomationElement ie)
        {
            Debug.Assert(automation != null);
            Debug.Assert(ie != null);
            _automation = automation;
            _ie = ie;
        }

        public List<BLink> ScanLinks()
        {
            List<BLink> links = new List<BLink>();
        
            IUIAutomationCondition conditionControlView = _automation.ControlViewCondition;
            IUIAutomationCondition conditionHyperlink = _automation.CreatePropertyCondition(Const.PropertyIdControlType, Const.ControlTypeIdHyperlink);
            IUIAutomationCondition condition = _automation.CreateAndCondition(conditionControlView, conditionHyperlink);
            IUIAutomationElementArray elementArray = _ie.FindAll(TreeScope.TreeScope_Descendants, condition);

            for (int idxLink = 0; idxLink < elementArray.Length; ++idxLink)
            {
                IUIAutomationElement elementLink = elementArray.GetElement(idxLink);
                if (elementLink != null)
                {
                    BLink newLink = BLink.Create(_automation, elementLink);
                    if (newLink != null)
                    {
                        links.Add(newLink);
                    }
                }
            }
            return links;
        }

    }
}

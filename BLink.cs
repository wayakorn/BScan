using System;
using System.Collections.Generic;
using System.Diagnostics;
using interop.UIAutomationCore;

namespace BScan
{
    class BLink
    {
        private IUIAutomation _automation;
        private IUIAutomationElement _element;
        private string _name;
        private string _url;

        public static BLink Create(IUIAutomation automation, IUIAutomationElement element)
        {
            BLink link = new BLink(automation, element);
            if ((link._name != null) && (link._url != null))
            {
                string url = link._url.ToLower();
                if (url.IndexOf("bing.com/") > 0)
                {
                    return link;
                }

            }
            return null;
        }

        private BLink(IUIAutomation automation, IUIAutomationElement element)
        {
            Debug.Assert(automation != null);
            Debug.Assert(element != null);

            _automation = automation;

            string strNameFound = element.CurrentName;
            if ((strNameFound != null) && (strNameFound.Trim().Length > 0))
            {
                // We have a usable name.
                _name = strNameFound.Trim();
            }
            else
            {
                // The hyperlink has no usable name. Consider using the name of a child element of the hyperlink.

                // Use a Tree Walker here to try to find a child element of the hyperlink 
                // that has a name. Tree walking is a time consuming action, so would be
                // avoided by a shipping app if alternatives like FindFirst, FindAll, or 
                // BuildUpdatedCache could get the required data.

                IUIAutomationTreeWalker controlWalker = _automation.ControlViewWalker;
                        
                IUIAutomationElement elementChild = controlWalker.GetFirstChildElement(element);
                while (elementChild != null)
                {
                    string strNameChild = elementChild.CurrentName;
                    if ((strNameChild != null) && (strNameChild.Trim().Length > 0))
                    {
                        // Use the name of this child element.
                        _name = strNameChild.Trim();
                        break;
                    }

                    // Continue to the next child element.
                    elementChild = controlWalker.GetNextSiblingElement(elementChild);
                }
            }
            
            
            IUIAutomationValuePattern valuePattern = (IUIAutomationValuePattern)element.GetCurrentPattern(Const.PatternIdValue);
            if (valuePattern != null)
            {
                _url = valuePattern.CurrentValue;
            }
        }

        public override string ToString()
        {
            return _name + "\r\n  ( " + _url + " )";
        }
    }
}

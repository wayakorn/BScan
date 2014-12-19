using System;
using System.Collections.Generic;
using System.Diagnostics;
using interop.UIAutomationCore;

namespace BScan
{
    class BLink
    {
        public readonly string Name = "";
        public readonly string Url = "";

        private IUIAutomation _automation;
        private IUIAutomationElement _element;

        public static BLink Create(IUIAutomation automation, IUIAutomationElement element)
        {
            BLink link = new BLink(automation, element);
            if ((link.Name != null) && (link.Url != null))
            {
                string url = link.Url.ToLower();
                if (url.IndexOf(Const.SearchPattern) > 0)
                {
                    if (link._element != null)
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
                Name = strNameFound.Trim();
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
                        Name = strNameChild.Trim();
                        break;
                    }

                    // Continue to the next child element.
                    elementChild = controlWalker.GetNextSiblingElement(elementChild);
                }
            }
            
            IUIAutomationValuePattern valuePattern = (IUIAutomationValuePattern)element.GetCurrentPattern(Const.PatternIdValue);
            if (valuePattern != null)
            {
                Url = valuePattern.CurrentValue;
            }

            // If this link has both the innerText (name) and url, save it
            if (Name.Length > 0 && Url.Length > 0)
            {
                _element = element;
            }
        }

        public override string ToString()
        {
            return Name + "\r\n  ( " + Url + " )";
        }

        public bool Invoke()
        {
            if (_element != null)
            {
                IUIAutomationInvokePattern pattern = (IUIAutomationInvokePattern)_element.GetCurrentPattern(Const.PatternIdInvoke);
                if (pattern != null)
                {
                    try
                    {
                        pattern.Invoke();
                        return true;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(@"Invoke failed: {0}", e.Message);
                    }
                }
            }
            return false;
        }
    }
}

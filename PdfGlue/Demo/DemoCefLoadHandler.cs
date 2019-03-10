
using Xilium.CefGlue;


namespace PdfGlue
{


    internal class DemoCefLoadHandler
        : CefLoadHandler
    {

        protected override void OnLoadStart(CefBrowser browser, CefFrame frame, CefTransitionType transitionType)
        {
            // base.OnLoadStart(browser, frame, transitionType);

            // A single CefBrowser instance can handle multiple requests
            // for a single URL if there are frames (i.e. <FRAME>, <IFRAME>).
            if (frame.IsMain)
            {
                System.Console.WriteLine("START: {0}", browser.GetMainFrame().Url);
            }

        }


        protected override void OnLoadEnd(CefBrowser browser, CefFrame frame, int httpStatusCode)
        {
            if (frame.IsMain)
            {
                System.Console.WriteLine("END: {0}, {1}", browser.GetMainFrame().Url, httpStatusCode);
            }
        }


    } // End Class DemoCefLoadHandler 


} // End Namespace PdfGlue 

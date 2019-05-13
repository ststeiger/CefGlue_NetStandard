
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
            } // End if (frame.IsMain) 

        } // End Sub OnLoadStart 


        // createBrowser("data:text/html,test", ...)
        // Don't use loadString. Either pass to createbrowser or loadurl.
        public class foo : CefDownloadImageCallback
        {
            protected override void OnDownloadImageFinished(string imageUrl, int httpStatusCode, CefImage image)
            {
                int width;
                int height;
                CefBinaryValue cbv = image.GetAsPng(1, true, out width, out height);
                byte[] ba = cbv.ToArray();
            }
        }


        protected override void OnLoadEnd(CefBrowser browser, CefFrame frame, int httpStatusCode)
        {
            if (frame.IsMain)
            {
                // https://bitbucket.org/chromiumembedded/cef/wiki/GeneralUsage#markdown-header-off-screen-rendering
                // https://gist.github.com/jankurianski/e0ac2d1006f3a42216be
                // browser.GetMainFrame().LoadString("content", "http://dummy.com");

                // browser.GetMainFrame().GetSource()
                // browser.GetMainFrame().GetText();


                // browser.GetHost().HasDevTools
                // browser.GetHost().DownloadImage("imageUrl", false, 1000, true, new foo());


                // frame.LoadString()


                //frame.GetSource()

                // browser.GetHost().DownloadImage("", true, 1024, true, null);

                //browser.GetHost().



                // browser.GetHost().DownloadImage


                // browser.GetMainFrame().

                // browser.GetHost().PrintToPdf("path", null, null);


                System.Console.WriteLine("END: {0}, {1}", browser.GetMainFrame().Url, httpStatusCode);
            } // End if (frame.IsMain) 

        } // End Sub OnLoadEnd 


    } // End Class DemoCefLoadHandler 


} // End Namespace PdfGlue 

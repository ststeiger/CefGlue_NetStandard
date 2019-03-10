
using Xilium.CefGlue;


namespace PdfGlue
{


    internal class DemoCefRenderHandler
        : CefRenderHandler
    {
        private readonly int _windowHeight;
        private readonly int _windowWidth;

        public DemoCefRenderHandler(int windowWidth, int windowHeight)
        {
            _windowWidth = windowWidth;
            _windowHeight = windowHeight;
        }


        protected override bool GetRootScreenRect(CefBrowser browser, ref CefRectangle rect)
        {
            return GetViewRectImpl(browser, ref rect);
        }


        protected override bool GetScreenPoint(CefBrowser browser, int viewX, int viewY, ref int screenX, ref int screenY)
        {
            screenX = viewX;
            screenY = viewY;
            return true;
        }


        protected bool GetViewRectImpl(CefBrowser browser, ref CefRectangle rect)
        {
            rect.X = 0;
            rect.Y = 0;
            rect.Width = _windowWidth;
            rect.Height = _windowHeight;

            return true;
        }


        protected override void GetViewRect(CefBrowser browser, out CefRectangle rect)
        {
            rect = new CefRectangle();
            GetViewRectImpl(browser, ref rect);

            // return true;
        }


        protected override bool GetScreenInfo(CefBrowser browser, CefScreenInfo screenInfo)
        {
            return false;
        }


        class MyPrintCallback 
            : CefPdfPrintCallback
        {
            protected override void OnPdfPrintFinished(string path, bool ok)
            {
                System.Console.WriteLine("Printing finished...");
                // throw new System.NotImplementedException();
            }
        }



        public int CmToMicrons(double cm)
        {
            int microns = (int)System.Math.Ceiling((cm * 10 * 1000));
            return microns;
        }

        bool isPainting = false;


        protected override void OnPaint(CefBrowser browser, CefPaintElementType type, CefRectangle[] dirtyRects
            , System.IntPtr buffer, int width, int height)
        {
            if (isPainting == true)
                return;

            isPainting = true;

            // Save the provided buffer (a bitmap image) as a PNG.
            using (System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(width, height, width * 4, System.Drawing.Imaging.PixelFormat.Format32bppRgb, buffer))
            {
                bitmap.Save(@"LastOnPaint.png", System.Drawing.Imaging.ImageFormat.Png);
            }


            CefPdfPrintSettings ps = new CefPdfPrintSettings();
            ps.PageWidth = CmToMicrons(21);
            ps.PageHeight = CmToMicrons(29.7);

            // A0
            // ps.PageWidth = CmToMicrons(84.1);
            // ps.PageHeight = CmToMicrons(118.9);

            // 5= zuviel
            // 2,3,4 = OK
            // ps.PageWidth = CmToMicrons(15*84.1);
            // ps.PageHeight = CmToMicrons(15*118.9);



            ps.Landscape = false;
            ps.MarginLeft = 0;
            ps.MarginTop = 0;
            ps.MarginRight = 0;
            ps.MarginBottom = 0;
            ps.BackgroundsEnabled = true;
            ps.SelectionOnly = false;


            browser.GetHost().PrintToPdf("AAA.pdf", ps, new MyPrintCallback());
            // browser.GetHost().CloseBrowser();
            // browser.Dispose(); // We have the image - stop re-rendering
        }


        protected override void OnPopupSize(CefBrowser browser, CefRectangle rect)
        {
        }


        protected override void OnCursorChange(CefBrowser browser,
            System.IntPtr cursorHandle, CefCursorType type, CefCursorInfo customCursorInfo)
        {
            // throw new NotImplementedException();
        }


        protected override void OnScrollOffsetChanged(CefBrowser browser, double x, double y)
        {
            // throw new NotImplementedException();
        }


        protected override CefAccessibilityHandler GetAccessibilityHandler()
        {
            // throw new NotImplementedException();
            return null;
        }


        protected override void OnAcceleratedPaint(CefBrowser browser, CefPaintElementType type,
            CefRectangle[] dirtyRects, System.IntPtr sharedHandle)
        {
            // throw new NotImplementedException();
        }


        protected override void OnImeCompositionRangeChanged(CefBrowser browser, CefRange selectedRange, CefRectangle[] characterBounds)
        {
            // throw new NotImplementedException();
        }


    } // End Class DemoCefRenderHandler 


} // End Namespace PdfGlue 

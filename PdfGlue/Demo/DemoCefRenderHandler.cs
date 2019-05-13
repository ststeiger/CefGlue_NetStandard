
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
        } // End Constructor 



        protected override bool GetRootScreenRect(CefBrowser browser, ref CefRectangle rect)
        {
            System.Console.WriteLine("GetRootScreenRect");
            
            return GetViewRectImpl(browser, ref rect);
        } // End Function GetRootScreenRect 


        protected override bool GetScreenPoint(CefBrowser browser, int viewX, int viewY, ref int screenX, ref int screenY)
        {
            System.Console.WriteLine("GetScreenPoint");
            
            screenX = viewX;
            screenY = viewY;
            return true;
        } // End Function GetScreenPoint 
        
        
        
        protected bool GetViewRectImpl(CefBrowser browser, ref CefRectangle rect)
        {
            System.Console.WriteLine("GetViewRectImpl");
            
            rect.X = 0;
            rect.Y = 0;
            rect.Width = _windowWidth;
            rect.Height = _windowHeight;

            return true;
        } // End Function GetViewRectImpl 

        protected override void GetViewRect(CefBrowser browser, out CefRectangle rect)
        {
            System.Console.WriteLine("GetViewRect");
            
            rect = new CefRectangle();
            GetViewRectImpl(browser, ref rect);

            // return true;
        } // End Sub GetViewRect 


        protected override bool GetScreenInfo(CefBrowser browser, CefScreenInfo screenInfo)
        {
            System.Console.WriteLine("GetScreenInfo");
            
            return false;
        } // End Function GetScreenInfo 

        bool isPainting = false;

        protected override void OnPaint(CefBrowser browser, CefPaintElementType type, CefRectangle[] dirtyRects
            , System.IntPtr buffer, int width, int height)
        {
            System.Console.WriteLine("Entering OnPaint");
            
            // if (isPainting == true) return;

            isPainting = true;

            // Save the provided buffer (a bitmap image) as a PNG.
            using (System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(width, height, width * 4, System.Drawing.Imaging.PixelFormat.Format32bppRgb, buffer))
            {
                System.Console.WriteLine("PixelFormat: " + bitmap.PixelFormat);
                System.Console.WriteLine("HorizontalResolution: " + bitmap.HorizontalResolution);
                System.Console.WriteLine("VerticalResolution: " + bitmap.VerticalResolution);

                bitmap.Save("/opt/pdfglue/LastOnPaint.bmp");

                bitmap.Save(@"/opt/pdfglue/LastOnPaint.png", System.Drawing.Imaging.ImageFormat.Png);
            } // End Using bitmap 


            // CefPdfPrintSettings ps = new CefPdfPrintSettings();
            // ps.PageWidth = CmToMicrons(21);
            // ps.PageHeight = CmToMicrons(29.7);

            // A0
            // ps.PageWidth = CmToMicrons(84.1);
            // ps.PageHeight = CmToMicrons(118.9);

            // 5= zuviel
            // 2,3,4 = OK
            // ps.PageWidth = CmToMicrons(15*84.1);
            // ps.PageHeight = CmToMicrons(15*118.9);



            //ps.Landscape = false;
            //ps.MarginLeft = 0;
            //ps.MarginTop = 0;
            //ps.MarginRight = 0;
            //ps.MarginBottom = 0;
            //ps.BackgroundsEnabled = true;
            //ps.SelectionOnly = false;

            CefPdfPrintSettings ps = new PageSize(PageSize_t.A4).PrintSettings;

            browser.GetHost().PrintToPdf("/opt/pdfglue/AAA.pdf", ps, new PdfPrintCallback());
            // browser.GetHost().CloseBrowser();
            // browser.Dispose(); // We have the image - stop re-rendering
            
            System.Console.WriteLine("Exiting OnPaint");
        } // End Sub OnPaint 




        protected override CefAccessibilityHandler GetAccessibilityHandler()
        { return null; } // throw new NotImplementedException();
        

        protected override void OnPopupSize(CefBrowser browser, CefRectangle rect)
        { }


        protected override void OnCursorChange(CefBrowser browser,
            System.IntPtr cursorHandle, CefCursorType type, CefCursorInfo customCursorInfo)
        { } // throw new NotImplementedException();


        protected override void OnScrollOffsetChanged(CefBrowser browser, double x, double y)
        { } // throw new NotImplementedException();


        protected override void OnAcceleratedPaint(CefBrowser browser, CefPaintElementType type,
            CefRectangle[] dirtyRects, System.IntPtr sharedHandle)
        { } // throw new NotImplementedException();


        protected override void OnImeCompositionRangeChanged(CefBrowser browser, CefRange selectedRange, CefRectangle[] characterBounds)
        { } // throw new NotImplementedException();


    } // End Class DemoCefRenderHandler 


} // End Namespace PdfGlue 


namespace PdfGlue
{


    public enum PageSize_t
    {
        A0,
        A1,
        A2,
        A3,
        A4
    } // End Enum PageSize_t 


    public class PageSize
    {

        private static int CmToMicrons(double cm)
        {
            int microns = (int)System.Math.Ceiling((cm * 10 * 1000));
            return microns;
        } // End Function CmToMicrons 


        public double Width;
        public double Height;
        public bool Landscape;


        protected void SetPageSize(PageSize_t size)
        {
            if (size == PageSize_t.A0)
            {
                this.Width = 84.1;
                this.Height = 118.9;
            }
            else if (size == PageSize_t.A1)
            {
                this.Width = 59.4;
                this.Height = 84.1;
            }
            else if (size == PageSize_t.A2)
            {
                this.Width = 42.0;
                this.Height = 59.4;
            }
            else if (size == PageSize_t.A3)
            {
                this.Width = 29.7;
                this.Height = 42.0;
            }
            else if (size == PageSize_t.A4)
            {
                this.Width = 21.0;
                this.Height = 29.7;
            }

        } // End Sub SetPageSize 


        public PageSize(PageSize_t size, bool landScape)
        {
            SetPageSize(size);
            this.Landscape = landScape;
        } // End Constructor 


        public PageSize(PageSize_t size)
            : this(size, false)
        { } // End Constructor 


        public PageSize(double widthInCm, double heightInCm, bool landScape)
        {
            this.Width = CmToMicrons(widthInCm);
            this.Height = CmToMicrons(heightInCm);
            this.Landscape = landScape;
        } // End Constructor 


        public PageSize()
            : this(21.0, 29.7, false)
        { } // End Constructor 


        public Xilium.CefGlue.CefPdfPrintSettings PrintSettings
        {
            get
            {
                Xilium.CefGlue.CefPdfPrintSettings ps = new Xilium.CefGlue.CefPdfPrintSettings();
                ps.PageWidth = CmToMicrons(21);
                ps.PageHeight = CmToMicrons(29.7);
                ps.Landscape = this.Landscape;
                ps.MarginLeft = 0;
                ps.MarginTop = 0;
                ps.MarginRight = 0;
                ps.MarginBottom = 0;
                ps.BackgroundsEnabled = true;
                ps.SelectionOnly = false;

                return ps;
            }

        } // End Property PrintSettings 


    } // End Class PageSize 


} // End Namespace PdfGlue

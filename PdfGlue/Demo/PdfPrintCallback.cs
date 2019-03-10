
namespace PdfGlue
{


    public class PdfPrintCallback
        : Xilium.CefGlue.CefPdfPrintCallback
    {

        protected override void OnPdfPrintFinished(string path, bool ok)
        {
            System.Console.WriteLine("Printing finished...");
            // throw new System.NotImplementedException();
        } // End Sub OnPdfPrintFinished 

    } // End Class MyPrintCallback


} // End Namespace PdfGlue 

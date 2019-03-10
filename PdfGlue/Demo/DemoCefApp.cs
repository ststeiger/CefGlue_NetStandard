
using Xilium.CefGlue;


namespace PdfGlue
{
    

    internal class DemoCefApp 
        : CefApp
    {

        protected override void OnBeforeCommandLineProcessing(string processType, CefCommandLine commandLine)
        {

            commandLine.AppendSwitch("disable-gpu");
            commandLine.AppendSwitch("disable-gpu-compositing");

            commandLine.AppendSwitch("disable-extensions");
            commandLine.AppendSwitch("disable-pinch");

            base.OnBeforeCommandLineProcessing(processType, commandLine);
        }

        protected override void OnRegisterCustomSchemes(CefSchemeRegistrar registrar)
        {
            base.OnRegisterCustomSchemes(registrar);
        }
        

    } // End Class DemoCefApp 


} // End Namespace PdfGlue 


using Xilium.CefGlue;


namespace PdfGlue
{
    

    internal class DemoCefApp 
        : CefApp
    {

        protected override void OnBeforeCommandLineProcessing(string processType, CefCommandLine commandLine)
        {
            System.Console.WriteLine("Entering OnBeforeCommandLineProcessing");

            // https://simpleit.rocks/linux/ubuntu/fixing-common-google-chrome-gpu-process-error-message-in-linux/
            // commandLine.AppendSwitch("headless");
            commandLine.AppendSwitch("no-sandbox");
            commandLine.AppendSwitch("disable-software-rasterizer");

            commandLine.AppendSwitch("disable-gpu");
            commandLine.AppendSwitch("disable-gpu-compositing");

            commandLine.AppendSwitch("disable-extensions");
            commandLine.AppendSwitch("disable-pinch");
            
            System.Console.WriteLine("Entering base.OnBeforeCommandLineProcessing");
            base.OnBeforeCommandLineProcessing(processType, commandLine);
            System.Console.WriteLine("Exiting  OnBeforeCommandLineProcessing");
        }


        protected override void OnRegisterCustomSchemes(CefSchemeRegistrar registrar)
        {
            System.Console.WriteLine("Entering OnRegisterCustomSchemes");
            base.OnRegisterCustomSchemes(registrar);
            System.Console.WriteLine("Exiting OnRegisterCustomSchemes");
        }
        
        
    } // End Class DemoCefApp 
    
    
} // End Namespace PdfGlue 

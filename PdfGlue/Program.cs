
using Xilium.CefGlue;


namespace PdfGlue
{


    // https://gitlab.com/xiliumhq/chromiumembedded/cefglue
    // https://bitbucket.org/chromiumfx/chromiumfx
    // https://github.com/cefsharp/CefSharp
    // https://github.com/mono/CppSharp

    // Note to self: 
    // Used CEF-Version in AnyCefGlue\Interop\version.g.cs 
    static class Program
    {


        // https://www.joelverhagen.com/blog/2013/12/headless-chromium-in-c-with-cefglue/
        // http://opensource.spotify.com/cefbuilds/index.html
        // https://github.com/spajak/cef-pdf
        [System.STAThread]
        internal static void Main(string[] args)
        {
#if false
	            Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new Form1());
#endif

            // CefFiles.DownloadCefForPlatform(@"D:\inetpub\mycef");


            // CefFiles.Cleanup(); return;

            // Load CEF. This checks for the correct CEF version.
            
            System.Console.WriteLine("Loading CEF");
            CefRuntime.Load();
            System.Console.WriteLine("CEF loaded");
            
            // Start the secondary CEF process.
            System.Console.WriteLine("New MainArgs");
            CefMainArgs cefMainArgs = new CefMainArgs(new string[0]);
            System.Console.WriteLine("New MainArgs completed");
            
            System.Console.WriteLine("New DemoCefApp ");
            DemoCefApp cefApp = new DemoCefApp();
            System.Console.WriteLine("New DemoCefApp completed");


            
            System.Console.WriteLine("Before executing process");
            // This is where the code path divereges for child processes.
            if (CefRuntime.ExecuteProcess(cefMainArgs, cefApp, System.IntPtr.Zero) != -1)
            {
                System.Console.WriteLine("Could not the secondary process");
                System.Console.Error.WriteLine("Could not the secondary process.");
            }
            System.Console.WriteLine("After executing process");
            
            
            
            System.Console.WriteLine("Before new CEF-settings");
            // Settings for all of CEF (e.g. process management and control).
            CefSettings cefSettings = new CefSettings
            {
                // From v68 SingleProcess is no longer supported and it has to be published. 
                // So debugging may be a tough situation in that regard unless your had a subprocess. 
                // SingleProcess = false, // https://github.com/chromelyapps/Chromely/issues/74 
                 MultiThreadedMessageLoop = true
                ,NoSandbox=true 
                ,WindowlessRenderingEnabled = true
                ,IgnoreCertificateErrors= true
                ,CommandLineArgsDisabled= true
            };            
            System.Console.WriteLine("After new CEF-settings");
            
            
            
            System.Console.WriteLine("Before CEF initialize");
            // Start the browser process (a child process).
            // runtime files to /usr/share/dotnet
            CefRuntime.Initialize(cefMainArgs, cefSettings, cefApp, System.IntPtr.Zero);
            System.Console.WriteLine("After CEF initialize");
            
            

            System.Console.WriteLine("Before CEF Window Create");
            // Instruct CEF to not render to a window at all.
            CefWindowInfo cefWindowInfo = CefWindowInfo.Create();
            System.Console.WriteLine("After CEF Window Create");
            
            // cefWindowInfo.SetAsOffScreen(IntPtr.Zero);
            cefWindowInfo.WindowlessRenderingEnabled = true;
            cefWindowInfo.SetAsWindowless(System.IntPtr.Zero, true);
            

            System.Console.WriteLine("New CefBrowserSettings");
            // Settings for the browser window itself (e.g. enable JavaScript?).
            CefBrowserSettings cefBrowserSettings = new CefBrowserSettings();
            System.Console.WriteLine("After New CefBrowserSettings");
            
            
            
            cefBrowserSettings.WebGL = CefState.Disabled;
            cefBrowserSettings.WindowlessFrameRate = 1;
            cefBrowserSettings.Plugins = CefState.Disabled;
            cefBrowserSettings.DefaultEncoding = System.Text.Encoding.UTF8.WebName;
            cefBrowserSettings.JavaScriptCloseWindows = CefState.Disabled;
            cefBrowserSettings.JavaScriptAccessClipboard = CefState.Disabled;
            cefBrowserSettings.JavaScriptDomPaste = CefState.Disabled;
            cefBrowserSettings.JavaScript = CefState.Enabled;


            // CefRuntime.RunMessageLoop();

            
            System.Console.WriteLine("Before new DemoClient");
            

            // Initialize some the cust interactions with the browser process.
            // The browser window will be 1280 x 720 (pixels).
            DemoCefClient cefClient = new DemoCefClient(1280, 720);

            System.Console.WriteLine("After new DemoClient");
            
            
            System.Console.WriteLine("Before CreateBrowser");
            // Start up the browser instance.
            CefBrowserHost.CreateBrowser(
                cefWindowInfo,
                cefClient,
                cefBrowserSettings,
                "http://www.reddit.com/");
            System.Console.WriteLine("After CreateBrowser");


            // Hang, to let the browser to do its work.
            System.Console.WriteLine(System.Environment.NewLine);
            System.Console.WriteLine(" --- Press a key at any time to end the program. --- ");
            System.Console.ReadKey();

            
            System.Console.WriteLine("Before CefShutdown");
            
            // Clean up CEF.
            CefRuntime.Shutdown();
            System.Console.WriteLine("After CefShutdown");
        } // End Sub Main 


    } // End Class Program 


} // End Namespace PdfGlue 

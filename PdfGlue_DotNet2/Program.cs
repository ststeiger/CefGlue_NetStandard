
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
        [System.STAThread]
        internal static void Main(string[] args)
        {
#if false
	            Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new Form1());
#endif

            // CefFiles.Cleanup(); return;

            // Load CEF. This checks for the correct CEF version.
            CefRuntime.Load();

            // Start the secondary CEF process.
            string[] argv = args;

            if (CefRuntime.Platform != CefRuntimePlatform.Windows)
            {
                argv = new string[args.Length + 1];
                System.Array.Copy(args, 0, argv, 1, args.Length);
                argv[0] = "-";
            }

            CefMainArgs cefMainArgs = new CefMainArgs(argv);
            DemoCefApp cefApp = new DemoCefApp();



            // This is where the code path divereges for child processes.
            if (CefRuntime.ExecuteProcess(cefMainArgs, cefApp, System.IntPtr.Zero) != -1)
            {
                System.Console.Error.WriteLine("Could not the secondary process.");
            }

            // Settings for all of CEF (e.g. process management and control).
            CefSettings cefSettings = new CefSettings
            {
                //  From v68 SingleProcess is no longer supported and it has to be published. 
                // So debugging may be a tough situation in that regard unless your had a subprocess. 
                // SingleProcess = false, // https://github.com/chromelyapps/Chromely/issues/74 
                MultiThreadedMessageLoop = true
                ,
                NoSandbox = true
                ,
                WindowlessRenderingEnabled = true
                ,
                IgnoreCertificateErrors = true
            };

            // Start the browser process (a child process).
            // runtime files to /usr/share/dotnet
            CefRuntime.Initialize(cefMainArgs, cefSettings, cefApp, System.IntPtr.Zero);


            // Instruct CEF to not render to a window at all.
            CefWindowInfo cefWindowInfo = CefWindowInfo.Create();
            // cefWindowInfo.SetAsOffScreen(IntPtr.Zero);
            cefWindowInfo.WindowlessRenderingEnabled = true;
            cefWindowInfo.SetAsWindowless(System.IntPtr.Zero, true);



            // Settings for the browser window itself (e.g. enable JavaScript?).
            CefBrowserSettings cefBrowserSettings = new CefBrowserSettings();


            cefBrowserSettings.WebGL = CefState.Disabled;
            cefBrowserSettings.WindowlessFrameRate = 30;



            // Initialize some the cust interactions with the browser process.
            // The browser window will be 1280 x 720 (pixels).
            DemoCefClient cefClient = new DemoCefClient(1280, 720);

            // Start up the browser instance.
            CefBrowserHost.CreateBrowser(
                cefWindowInfo,
                cefClient,
                cefBrowserSettings,
                "http://www.reddit.com/");


            // Hang, to let the browser to do its work.
            System.Console.WriteLine(System.Environment.NewLine);
            System.Console.WriteLine(" --- Press a key at any time to end the program. --- ");
            System.Console.ReadKey();

            // Clean up CEF.
            CefRuntime.Shutdown();
        } // End Sub Main 


    } // End Class Program 


} // End Namespace PdfGlue 

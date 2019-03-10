
using Xilium.CefGlue;


namespace PdfGlue
{


    // https://gitlab.com/xiliumhq/chromiumembedded/cefglue
    // https://bitbucket.org/chromiumfx/chromiumfx
    // https://github.com/cefsharp/CefSharp
    // https://github.com/mono/CppSharp

    // Note to self: 
    // Used CEF-Version in AnyCefGlue\Interop\version.g.cs 
    class Program
    {
        
        
        public static void DeleteFiles(string dir, string[] destinationArray)
        {
            for(int i = 0; i < destinationArray.Length; ++i)
            {
                string file = System.IO.Path.Combine(dir, destinationArray[i]);
                if(System.IO.File.Exists(file))
                    System.IO.File.Delete(file);
            } // Next i 
            
            string shader = System.IO.Path.Combine(dir, "swiftshader");
            string locales = System.IO.Path.Combine(dir, "locales");
            
            if(System.IO.Directory.Exists(shader))
                System.IO.Directory.Delete(shader);
            
            if(System.IO.Directory.Exists(locales))
                System.IO.Directory.Delete(locales);
        } // End Sub DeleteFiles 
        
        
        internal static void CleanupCefFiles()
        {
            string basePath = "/root/Downloads/cef/cef_binary_3.3578.1870.gc974488_linux64/";
            string releaseDirectory = System.IO.Path.Combine(basePath, "Release");
            string resourcesDirectory = System.IO.Path.Combine(basePath, "Resources");
            
            releaseDirectory +=  System.IO.Path.DirectorySeparatorChar.ToString();
            resourcesDirectory +=  System.IO.Path.DirectorySeparatorChar.ToString();
            
            
            string[] releaseFiles = System.IO.Directory.GetFiles(releaseDirectory, "*.*", System.IO.SearchOption.AllDirectories);
            string[] resourceFiles = System.IO.Directory.GetFiles(resourcesDirectory, "*.*", System.IO.SearchOption.AllDirectories);
            
            string[] allCefFiles = new string[releaseFiles.Length + resourceFiles.Length];
            System.Array.Copy(releaseFiles, allCefFiles, releaseFiles.Length);
            System.Array.Copy(resourceFiles, 0, allCefFiles, releaseFiles.Length, resourceFiles.Length);
            
            
            for(int i = 0; i < allCefFiles.Length; ++i)
            {
                allCefFiles[i] = allCefFiles[i].Replace(releaseDirectory, "");
                allCefFiles[i] = allCefFiles[i].Replace(resourcesDirectory, "");
            } // Next i 
            
            DeleteFiles("/usr/bin", allCefFiles); // /usr/bin/mono
            DeleteFiles("/usr/share/dotnet", allCefFiles); // /usr/share/dotnet/dotnet
            System.Console.WriteLine("CEF-files removed.");
        } // End Sub CleanupCefFiles
        
        
        // https://www.joelverhagen.com/blog/2013/12/headless-chromium-in-c-with-cefglue/
        // http://opensource.spotify.com/cefbuilds/index.html
        internal static void Main(string[] args)
        {
            // CleanupCefFiles(); return;
            
            // Load CEF. This checks for the correct CEF version.
            CefRuntime.Load();
            
            // Start the secondary CEF process.
            CefMainArgs cefMainArgs = new CefMainArgs(new string[0]);
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
                ,NoSandbox=true 
                ,WindowlessRenderingEnabled = true
                ,IgnoreCertificateErrors= true
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
        }

// Copy release and resources
        // End Sub Main 


    } // End Class Program 


} // End Namespace PdfGlue 

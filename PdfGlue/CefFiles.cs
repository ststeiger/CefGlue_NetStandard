
namespace PdfGlue
{


    public class CefFiles
    {


        private static void DeleteFiles(string dir, string[] destinationArray)
        {
            for (int i = 0; i < destinationArray.Length; ++i)
            {
                string file = System.IO.Path.Combine(dir, destinationArray[i]);
                if (System.IO.File.Exists(file))
                    System.IO.File.Delete(file);
            } // Next i 

            string shader = System.IO.Path.Combine(dir, "swiftshader");
            string locales = System.IO.Path.Combine(dir, "locales");

            if (System.IO.Directory.Exists(shader))
                System.IO.Directory.Delete(shader);

            if (System.IO.Directory.Exists(locales))
                System.IO.Directory.Delete(locales);
        } // End Sub DeleteFiles 


        public static void Cleanup()
        {
            string basePath = "/root/Downloads/cef/cef_binary_3.3578.1870.gc974488_linux64/";
            string releaseDirectory = System.IO.Path.Combine(basePath, "Release");
            string resourcesDirectory = System.IO.Path.Combine(basePath, "Resources");

            releaseDirectory += System.IO.Path.DirectorySeparatorChar.ToString();
            resourcesDirectory += System.IO.Path.DirectorySeparatorChar.ToString();


            string[] releaseFiles = System.IO.Directory.GetFiles(releaseDirectory, "*.*", System.IO.SearchOption.AllDirectories);
            string[] resourceFiles = System.IO.Directory.GetFiles(resourcesDirectory, "*.*", System.IO.SearchOption.AllDirectories);

            string[] allCefFiles = new string[releaseFiles.Length + resourceFiles.Length];
            System.Array.Copy(releaseFiles, allCefFiles, releaseFiles.Length);
            System.Array.Copy(resourceFiles, 0, allCefFiles, releaseFiles.Length, resourceFiles.Length);


            for (int i = 0; i < allCefFiles.Length; ++i)
            {
                allCefFiles[i] = allCefFiles[i].Replace(releaseDirectory, "");
                allCefFiles[i] = allCefFiles[i].Replace(resourcesDirectory, "");
            } // Next i 

            DeleteFiles("/usr/bin", allCefFiles); // /usr/bin/mono
            DeleteFiles("/usr/share/dotnet", allCefFiles); // /usr/share/dotnet/dotnet
            System.Console.WriteLine("CEF-files removed.");
        } // End Sub Cleanup


    } // End Class CefFiles 


} // End Namespace PdfGlue 


namespace PdfGlue
{


    public class CefFiles
    {


        public static bool DownloadCefForPlatform(string fileName)
        {
            SpotifyOpenSource cefIndex = new SpotifyOpenSource();
            cefIndex.RequiredVersion = "3.3626.1895.g7001d56";

            // System.Console.WriteLine(cefIndex.CefSource);
            // System.Console.WriteLine(cefIndex.ChromiumSource);

            //System.Console.WriteLine(cefIndex.MinimalDistribution);
            //System.Console.WriteLine(cefIndex.MinimalDistributionHash);

            //System.Console.WriteLine(cefIndex.StandardDistribution);
            //System.Console.WriteLine(cefIndex.StandardDistributionHash);

            //System.Console.WriteLine(cefIndex.DebugSymbols);
            //System.Console.WriteLine(cefIndex.DebugSymbolsHash);

            //System.Console.WriteLine(cefIndex.ReleaseSymbols);
            //System.Console.WriteLine(cefIndex.ReleaseSymbolsHash);

            //System.Console.WriteLine(cefIndex.Client);
            //System.Console.WriteLine(cefIndex.ClientHash);

            cefIndex.DownloadStream(cefIndex.MinimalDistribution, fileName);
            string myhash = Sha1(fileName);
            bool isUncorrupted = myhash.Equals(cefIndex.MinimalDistributionHash, System.StringComparison.InvariantCultureIgnoreCase);

            UnTarBz2(fileName);
            // ExtractTarByEntry(fileName, @"D:\inetpub\CefFiles", true);

            return isUncorrupted;
        }

        public static string Sha1(string fileName)
        {
            string returnValue = null;

            using (System.IO.FileStream fs = System.IO.File.OpenRead(fileName))
            {
                using (System.IO.BufferedStream bs = new System.IO.BufferedStream(fs))
                {
                    using (System.Security.Cryptography.SHA1 sha1 = System.Security.Cryptography.SHA1.Create())
                    {
                        byte[] hash = sha1.ComputeHash(bs);
                        System.Text.StringBuilder formatted = new System.Text.StringBuilder(2 * hash.Length);

                        for (int i = 0; i < hash.Length; ++i)
                        {
                            formatted.AppendFormat(System.Globalization.CultureInfo.InvariantCulture, "{0:x2}", hash[i]);
                        } // Next b 

                        returnValue = formatted.ToString();
                        formatted.Length = 0;
                        formatted = null;
                    } // End Sha1 

                } // End Using bs 

            } // End Using fs 

            return returnValue;
        } // End Function Sha1 


        public static void UnTarBz2(string fileName)
        {
            string tempTarFile = System.IO.Path.GetTempFileName();

            using (System.IO.FileStream dest = System.IO.File.OpenWrite(tempTarFile))
            {

                using (System.IO.FileStream fs = System.IO.File.OpenRead(fileName))
                {
                    byte[] buffer = new byte[4096];

                    using (ICSharpCode.SharpZipLib.BZip2.BZip2InputStream bz2 = new ICSharpCode.SharpZipLib.BZip2.BZip2InputStream(fs))
                    {

                        int bytesRead;
                        while ((bytesRead = bz2.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            dest.Write(buffer, 0, bytesRead);
                            dest.Flush();
                        } // Whend 

                    } // End using bz2 

                    fs.Flush();
                } // End using fs 

            } // End Using dest 

            // https://stackoverflow.com/questions/8863875/decompress-tar-files-using-c-sharp
            // https://code.google.com/archive/p/tar-cs/

            string trash = CreateTemporaryDirectory();

            using (System.IO.FileStream fs = System.IO.File.OpenRead(tempTarFile))
            {
                using (ICSharpCode.SharpZipLib.Tar.TarArchive tarArchive = ICSharpCode.SharpZipLib.Tar.TarArchive.CreateInputTarArchive(fs))
                {
                    tarArchive.ExtractContents(trash);
                    tarArchive.Close();
                } // End Using tarArchive 

                // using (ICSharpCode.SharpZipLib.Tar.TarInputStream tar = new ICSharpCode.SharpZipLib.Tar.TarInputStream(fs)) { }

            } // End Using fs 

            System.IO.File.Delete(tempTarFile);
        } // End Sub UnTarBz2 


        public static string CreateTemporaryDirectory()
        {
            string tempDirectory = System.IO.Path.Combine(System.IO.Path.GetTempPath(), System.IO.Path.GetRandomFileName());
            System.IO.Directory.CreateDirectory(tempDirectory);

            return tempDirectory;
        }


        // https://github.com/icsharpcode/SharpZipLib/wiki/GZip-and-Tar-Samples
        public static void ExtractTarByEntry(string tarFileName, string targetDir, bool asciiTranslate)
        {
            using (System.IO.FileStream fsIn = new System.IO.FileStream(tarFileName, System.IO.FileMode.Open, System.IO.FileAccess.Read))
            {
                ICSharpCode.SharpZipLib.Tar.TarInputStream tarIn = new ICSharpCode.SharpZipLib.Tar.TarInputStream(fsIn);
                ICSharpCode.SharpZipLib.Tar.TarEntry tarEntry;

                while ((tarEntry = tarIn.GetNextEntry()) != null)
                {
                    if (tarEntry.IsDirectory)
                    {
                        System.Console.WriteLine(tarEntry.Name);

                        // ICSharpCode.SharpZipLib.Tar.TarEntry[] entries = tarEntry.GetDirectoryEntries();
                        // System.Console.WriteLine(entries);

                        continue;
                    }
                    // Converts the unix forward slashes in the filenames to windows backslashes
                    string name = tarEntry.Name.Replace('/', System.IO.Path.DirectorySeparatorChar);

                    // Remove any root e.g. '\' because a PathRooted filename defeats Path.Combine
                    if (System.IO.Path.IsPathRooted(name))
                        name = name.Substring(System.IO.Path.GetPathRoot(name).Length);

                    // Apply further name transformations here as necessary
                    string outName = System.IO.Path.Combine(targetDir, name);

                    string directoryName = System.IO.Path.GetDirectoryName(outName);

                    // Does nothing if directory exists
                    System.IO.Directory.CreateDirectory(directoryName);

                    System.IO.FileStream outStr = new System.IO.FileStream(outName, System.IO.FileMode.Create);

                    if (asciiTranslate)
                        CopyWithAsciiTranslate(tarIn, outStr);
                    else
                        tarIn.CopyEntryContents(outStr);

                    outStr.Close();

                    // Set the modification date/time. This approach seems to solve timezone issues.
                    System.DateTime myDt = System.DateTime.SpecifyKind(tarEntry.ModTime, System.DateTimeKind.Utc);
                    System.IO.File.SetLastWriteTime(outName, myDt);
                }

                tarIn.Close();
            }
        }

        private static void CopyWithAsciiTranslate(ICSharpCode.SharpZipLib.Tar.TarInputStream tarIn, System.IO.Stream outStream)
        {
            byte[] buffer = new byte[4096];
            bool isAscii = true;
            bool cr = false;

            int numRead = tarIn.Read(buffer, 0, buffer.Length);
            int maxCheck = System.Math.Min(200, numRead);
            for (int i = 0; i < maxCheck; i++)
            {
                byte b = buffer[i];
                if (b < 8 || (b > 13 && b < 32) || b == 255)
                {
                    isAscii = false;
                    break;
                }
            }

            while (numRead > 0)
            {
                if (isAscii)
                {
                    // Convert LF without CR to CRLF. Handle CRLF split over buffers.
                    for (int i = 0; i < numRead; i++)
                    {
                        byte b = buffer[i];     // assuming plain Ascii and not UTF-16
                        if (b == 10 && !cr)     // LF without CR
                            outStream.WriteByte(13);
                        cr = (b == 13);

                        outStream.WriteByte(b);
                    }
                }
                else
                    outStream.Write(buffer, 0, numRead);

                numRead = tarIn.Read(buffer, 0, buffer.Length);
            }
        }



        public static void CopyFiles(string sourcePath, string destinationPath)
        {
            //Now Create all of the directories
            foreach (string dirPath in System.IO.Directory.GetDirectories(sourcePath, "*", System.IO.SearchOption.AllDirectories))
                System.IO.Directory.CreateDirectory(dirPath.Replace(sourcePath, destinationPath));

            //Copy all the files & Replaces any files with the same name
            foreach (string newPath in System.IO.Directory.GetFiles(sourcePath, "*.*", System.IO.SearchOption.AllDirectories))
                System.IO.File.Copy(newPath, newPath.Replace(sourcePath, destinationPath), true);

            // System.IO.Directory.Delete("folderPath", true); // recursive delete
        } // End Sub CopyFiles 


        // Permits error handling 
        public static void CopyAll(System.IO.DirectoryInfo source, System.IO.DirectoryInfo target)
        {
            System.IO.Directory.CreateDirectory(target.FullName);

            // Copy each file into the new directory.
            foreach (System.IO.FileInfo fi in source.GetFiles())
            {
                System.Console.WriteLine(@"Copying {0}\{1}", target.FullName, fi.Name);
                fi.CopyTo(System.IO.Path.Combine(target.FullName, fi.Name), true);
            }

            // Copy each subdirectory using recursion.
            foreach (System.IO.DirectoryInfo diSourceSubDir in source.GetDirectories())
            {
                System.IO.DirectoryInfo nextTargetSubDir =
                    target.CreateSubdirectory(diSourceSubDir.Name);
                CopyAll(diSourceSubDir, nextTargetSubDir);
            }
        } // End Sub CopyAll 


        public static void Copy(string sourceDirectory, string targetDirectory)
        {
            System.IO.DirectoryInfo diSource = new System.IO.DirectoryInfo(sourceDirectory);
            System.IO.DirectoryInfo diTarget = new System.IO.DirectoryInfo(targetDirectory);

            CopyAll(diSource, diTarget);
        } // End Sub Copy 


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

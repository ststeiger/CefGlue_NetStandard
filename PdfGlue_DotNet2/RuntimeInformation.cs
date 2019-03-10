
namespace System.Runtime.InteropServices
{


    public enum OSPlatform
    {
         Windows
        ,OSX
        ,Linux
    }


    // Hack, so .NET-Core source-code doesn't need to be modified with inferior crap 
    public class RuntimeInformation
    {


        public static bool IsOSPlatform(OSPlatform plat)
        {
            if (plat == OSPlatform.Windows && System.Environment.OSVersion.Platform != PlatformID.Unix)
                return true;

            // https://stackoverflow.com/questions/5116977/how-to-check-the-os-version-at-runtime-e-g-windows-or-linux-without-using-a-con/5117005
            // newer versions of .NET distinguished between Unix and MacOS X, 
            // introducing yet another value 6 for MacOS X.
            int p = (int)Environment.OSVersion.Platform;

            if (plat == OSPlatform.OSX && System.Environment.OSVersion.Platform == PlatformID.Unix)
                return p == 6; 
            
            if (plat == OSPlatform.Linux && System.Environment.OSVersion.Platform == PlatformID.Unix)
                return p != 6;

            return false;
        }


    }


}

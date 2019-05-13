
namespace PdfGlue
{


    public class SpotifyOpenSource
    {

        public string Canonical = "http://opensource.spotify.com/cefbuilds/";
        public string RequiredVersion;


        private static void InitiateSSLTrust()
        {
            try
            {
                //Change SSL checks so that all checks pass
                System.Net.ServicePointManager.ServerCertificateValidationCallback =
                   new System.Net.Security.RemoteCertificateValidationCallback(
                           delegate (object sender, System.Security.Cryptography.X509Certificates.X509Certificate certificate
                           , System.Security.Cryptography.X509Certificates.X509Chain chain
                           , System.Net.Security.SslPolicyErrors errors)
                           {
                               return true;
                           }
                    );
            }
            catch (System.Exception ex)
            {
                System.Console.WriteLine(ex.Message);
            }
        }

        
        public string DownloadString(string url)
        {
            string data = null;

            InitiateSSLTrust();

            using (var wc = new System.Net.WebClient())
            {
                data = wc.DownloadString(url);
            }

            return data;
        }

        public byte[] DownloadFile(string url)
        {
            byte[] data = null;
            InitiateSSLTrust();

            using (var wc = new System.Net.WebClient())
            {
                // http://opensource.spotify.com/cefbuilds/index.html

                data = wc.DownloadData(url);
            }

            return data;
        }


        public System.IO.Stream DownloadStream1(string url)
        {
            byte[] data = this.DownloadFile(url);

            System.IO.MemoryStream ms = new System.IO.MemoryStream(data);
            ms.Position = 0;

            return ms;
        }

        public System.IO.Stream DownloadStream(string url)
        {
            InitiateSSLTrust();

            //Create a WebRequest to get the file
            System.Net.HttpWebRequest fileReq = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(url);

            //Create a response for this request
            System.Net.HttpWebResponse fileResp = (System.Net.HttpWebResponse)fileReq.GetResponse();

            // if (fileReq.ContentLength > 0) fileResp.ContentLength = fileReq.ContentLength;

            //Get the Stream returned from the response
            System.IO.Stream strm = fileResp.GetResponseStream();
            return strm;
        }


        // TODO: Resume download https://github.com/markodt/SGet
        public void DownloadStream(string url, string fileName)
        {
            //Get the Stream returned from the response
            System.IO.Stream strm = this.DownloadStream(url);

            byte[] buffer = new byte[4096];
            int bytesRead;

            using (System.IO.FileStream dest = System.IO.File.OpenWrite(fileName))
            {
                while ((bytesRead = strm.Read(buffer, 0, buffer.Length)) > 0)
                {
                    dest.Write(buffer, 0, bytesRead);
                    dest.Flush();
                } // Whend 
            } // End Using dest 

        } // End Sub DownloadStream 


        protected HtmlAgilityPack.HtmlDocument m_document;
        

        protected HtmlAgilityPack.HtmlDocument Document
        {
            get
            {
                if (this.m_document != null)
                    return this.m_document;

                string data = null;
                string sourceLocation = "../../../source.htm";
                
                if (true)
                {
                    // http://opensource.spotify.com/cefbuilds/index.html
                    data = this.DownloadString(this.Canonical + "index.html");
                    sourceLocation = "source.htm";
                    System.IO.File.WriteAllText(sourceLocation, data, System.Text.Encoding.UTF8);
                }
                
                data = System.IO.File.ReadAllText(sourceLocation, System.Text.Encoding.UTF8);
                
                
                HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();

                doc.OptionCheckSyntax = true;
                doc.OptionOutputUpperCase = false;
                doc.OptionOutputAsXml = true;
                doc.OptionFixNestedTags = true;
                doc.OptionWriteEmptyNodes = true;
                doc.OptionAutoCloseOnEnd = true;


                // doc.Load("source.htm");
                doc.LoadHtml(data);

                this.m_document = doc;
                return this.m_document;
            }
        }

        public string Prettiy(string fileName)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            using (System.IO.TextWriter swr = new System.IO.StringWriter(sb))
            {
                this.Document.Save(swr);
            } // End Using swr 


            System.Xml.XmlWriterSettings settings = new System.Xml.XmlWriterSettings();
            settings.Encoding = System.Text.Encoding.UTF8;
            settings.OmitXmlDeclaration = false;
            settings.Indent = true;
            settings.IndentChars = "  ";
            settings.ConformanceLevel = System.Xml.ConformanceLevel.Document;
            settings.NewLineChars = System.Environment.NewLine;
            settings.NewLineHandling = System.Xml.NewLineHandling.Replace;
            // settings.NewLineOnAttributes = true;


            System.Xml.XmlDocument xdoc = new System.Xml.XmlDocument();
            xdoc.LoadXml(sb.ToString());
            using (System.Xml.XmlWriter xw = System.Xml.XmlTextWriter.Create(fileName, settings))
            {
                xdoc.Save(xw);
            } // End Using xw 

            xdoc.Load(fileName);

            return xdoc.OuterXml;
        }

        // <table id="linux32" class="list" width="825">
        // <table id="linux64" class="list" width="825">
        // <table id="windows32" class="list" width="825">
        // <table id="windows64" class="list" width="825">
        // <table id="macosx64" class="list" width="825">

        protected string m_tableId;

        protected string TableId
        {
            get
            {
                if (this.m_tableId != null)
                    return this.m_tableId;

                string tableId = (System.IntPtr.Size * 8).ToString(System.Globalization.CultureInfo.InvariantCulture);

                if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Windows))
                    tableId = "windows" + tableId;
                else if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.OSX))
                    tableId = "macosx" + tableId;
                else if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Linux))
                    tableId = "linux" + tableId;
                else
                    tableId = "linux" + tableId; // Unix or undetected Linux

                this.m_tableId = tableId;
                return this.m_tableId;
            }
        }


        protected HtmlAgilityPack.HtmlNode m_table;

        protected HtmlAgilityPack.HtmlNode Table
        {
            get
            {
                // The running os is constant, so we can cache this 
                if (this.m_table != null)
                    return this.m_table;

                this.m_table = this.Document.DocumentNode.SelectSingleNode("//table[@id='" + this.TableId + "']");
                return this.m_table;
            }
        }


        protected HtmlAgilityPack.HtmlNode Version
        {
            get
            {
                // <tr class="toprow" data-version="3.3626.1895.g7001d56" style="background: rgb(255, 255, 255);">
                HtmlAgilityPack.HtmlNode version = this.Table.SelectSingleNode(".//tr[@data-version='" + this.RequiredVersion + "']");
                return version;
            }
        }


        protected string GetLink(HtmlAgilityPack.HtmlNode node)
        {
            string value = node.Attributes["href"].Value;

            if (value.ToLowerInvariant().StartsWith("http"))
                return value;

            value = this.Canonical + value;
            return value;
        }


        protected string GetHash(HtmlAgilityPack.HtmlNode node)
        {
            string value = this.DownloadString(GetLink(node));

            value = value.Trim(' ', '\t', '\v', '\r', '\n', '\f');

            return value;
        }
        

        public string CefSource
        {
            get
            {
                // <a href="https://bitbucket.org/chromiumembedded/cef/get/3626.tar.bz2">CEF source</a>
                HtmlAgilityPack.HtmlNode cefSource = this.Version.SelectSingleNode(".//a[.='CEF source']");

                return GetLink(cefSource);
            }
        }


        public string ChromiumSource
        {
            get
            {
                // <a href="https://gsdview.appspot.com/chromium-browser-official/chromium-72.0.3626.121.tar.xz">Chromium source</a>
                HtmlAgilityPack.HtmlNode chromiumSource = this.Version.SelectSingleNode(".//a[.='Chromium source']");

                return GetLink(chromiumSource);
            }
        }


        public string StandardDistribution
        {
            get
            {
                // <a href="cef_binary_3.3626.1895.g7001d56_windows32.tar.bz2">cef_binary_3.3626.1895.g7001d56_windows32.tar.bz2</a>
                // HtmlAgilityPack.HtmlNode standardDistribution = this.Version.SelectSingleNode(".//a[starts-with(., 'cef_binary_') and '_" + this.TableId + ".tar.bz2' = substring(., string-length(.) - string-length('_" + this.TableId + ".tar.bz2') +1)]");
                HtmlAgilityPack.HtmlNode standardDistribution = this.Version.SelectSingleNode(".//a[.='cef_binary_" + this.RequiredVersion + "_" + this.TableId + ".tar.bz2']");

                return GetLink(standardDistribution);
            }
        }

        public string StandardDistributionHash
        {
            get
            {

                // <a href="cef_binary_3.3626.1895.g7001d56_macosx64.tar.bz2.sha1">sha1</a>
                // HtmlAgilityPack.HtmlNode standardDistributionSha1 = this.Version.SelectSingleNode(".//a[starts-with(@href, 'cef_binary_') and '_" + this.TableId + ".tar.bz2.sha1' = substring(@href, string-length(@href) - string-length('_" + this.TableId + ".tar.bz2.sha1') +1)]");
                HtmlAgilityPack.HtmlNode standardDistributionSha1 = this.Version.SelectSingleNode(".//a[@href='cef_binary_" + this.RequiredVersion + "_" + this.TableId + ".tar.bz2.sha1']");

                return this.GetHash(standardDistributionSha1);
            }
        }


        public string MinimalDistribution
        {
            get
            {
                // <a href="cef_binary_3.3626.1895.g7001d56_windows64_minimal.tar.bz2">cef_binary_3.3626.1895.g7001d56_windows64_minimal.tar.bz2</a>
                // HtmlAgilityPack.HtmlNode minimalDistribution = this.Version.SelectSingleNode(".//a[starts-with(., 'cef_binary_') and '_" + this.TableId + "_minimal.tar.bz2' = substring(., string-length(.) - string-length('_" + this.TableId + "_minimal.tar.bz2') +1)]");
                HtmlAgilityPack.HtmlNode minimalDistribution = this.Version.SelectSingleNode(".//a[.='cef_binary_" + this.RequiredVersion + "_" + this.TableId + "_minimal.tar.bz2']");

                return GetLink(minimalDistribution);
            }
        }

        public string MinimalDistributionHash
        {
            get
            {
                // HtmlAgilityPack.HtmlNode minimalDistributionSha1 = this.Version.SelectSingleNode(".//a[starts-with(@href, 'cef_binary_') and '_" + this.TableId + "_minimal.tar.bz2.sha1' = substring(@href, string-length(@href) - string-length('_" + this.TableId + "_minimal.tar.bz2.sha1') +1)]");
                HtmlAgilityPack.HtmlNode minimalDistributionSha1 = this.Version.SelectSingleNode(".//a[@href='cef_binary_" + this.RequiredVersion + "_" + this.TableId + "_minimal.tar.bz2.sha1']");

                return this.GetHash(minimalDistributionSha1);
            }
        }


        public string DebugSymbols
        {
            get
            {
                // <a href="cef_binary_3.3626.1895.g7001d56_windows32_debug_symbols.tar.bz2">cef_binary_3.3626.1895.g7001d56_windows32_debug_symbols.tar.bz2</a>
                // HtmlAgilityPack.HtmlNode debugSymbols = this.Version.SelectSingleNode(".//a[starts-with(., 'cef_binary_') and '_" + this.TableId + "_debug_symbols.tar.bz2' = substring(., string-length(.) - string-length('_" + this.TableId + "_debug_symbols.tar.bz2') +1)]");
                // string selector = "//a[.='cef_binary_" + requiredVersion + "_" + tableId + "_debug_symbols.tar.bz2']";
                HtmlAgilityPack.HtmlNode debugSymbols = this.Version.SelectSingleNode(".//a[.='cef_binary_" + this.RequiredVersion + "_" + this.TableId + "_debug_symbols.tar.bz2']");

                return GetLink(debugSymbols);
            }
        }


        public string DebugSymbolsHash
        {
            get
            {
                // HtmlAgilityPack.HtmlNode debugSymbolsSha1 = this.Version.SelectSingleNode(".//a[starts-with(@href, 'cef_binary_') and '_" + this.TableId + "_debug_symbols.tar.bz2.sha1' = substring(@href, string-length(@href) - string-length('_" + this.TableId + "_debug_symbols.tar.bz2.sha1') +1)]");
                HtmlAgilityPack.HtmlNode debugSymbolsSha1 = this.Version.SelectSingleNode(".//a[@href='cef_binary_" + this.RequiredVersion + "_" + this.TableId + "_debug_symbols.tar.bz2.sha1']");

                return this.GetHash(debugSymbolsSha1);
            }
        }



        public string ReleaseSymbols
        {
            get
            {
                // <a href="cef_binary_3.3626.1895.g7001d56_windows32_release_symbols.tar.bz2">cef_binary_3.3626.1895.g7001d56_windows32_release_symbols.tar.bz2</a>
                // HtmlAgilityPack.HtmlNode releaseSymbols = this.Version.SelectSingleNode(".//a[starts-with(., 'cef_binary_') and '_" + this.TableId + "_release_symbols.tar.bz2' = substring(., string-length(.) - string-length('_" + this.TableId + "_release_symbols.tar.bz2') +1)]");
                HtmlAgilityPack.HtmlNode releaseSymbols = this.Version.SelectSingleNode(".//a[.='cef_binary_" + this.RequiredVersion + "_" + this.TableId + "_release_symbols.tar.bz2']");

                return GetLink(releaseSymbols);
            }
        }


        public string ReleaseSymbolsHash
        {
            get
            {
                //HtmlAgilityPack.HtmlNode releaseSymbolsSha1 = this.Version.SelectSingleNode(".//a[starts-with(@href, 'cef_binary_') and '_" + this.TableId + "_release_symbols.tar.bz2.sha1' = substring(@href, string-length(@href) - string-length('_" + this.TableId + "_release_symbols.tar.bz2.sha1') +1)]");
                HtmlAgilityPack.HtmlNode releaseSymbolsSha1 = this.Version.SelectSingleNode(".//a[@href='cef_binary_" + this.RequiredVersion + "_" + this.TableId + "_release_symbols.tar.bz2.sha1']");

                return this.GetHash(releaseSymbolsSha1);
            }
        }


        public string Client
        {
            get
            {
                // <a href="cef_binary_3.3626.1895.g7001d56_windows32_client.tar.bz2">cef_binary_3.3626.1895.g7001d56_windows32_client.tar.bz2</a>
                // HtmlAgilityPack.HtmlNode client = this.Version.SelectSingleNode(".//a[starts-with(., 'cef_binary_') and '_" + this.TableId + "_client.tar.bz2' = substring(., string-length(.) - string-length('_" + this.TableId + "_client.tar.bz2') +1)]");
                HtmlAgilityPack.HtmlNode client = this.Version.SelectSingleNode(".//a[.='cef_binary_" + this.RequiredVersion + "_" + this.TableId + "_client.tar.bz2']");

                return GetLink(client);
            }
        }


        public string ClientHash
        {
            get
            {
                //HtmlAgilityPack.HtmlNode clientSha1 = this.Version.SelectSingleNode(".//a[starts-with(@href, 'cef_binary_') and '_" + this.TableId + "_client.tar.bz2.sha1' = substring(@href, string-length(@href) - string-length('_" + this.TableId + "_client.tar.bz2.sha1') +1)]");
                HtmlAgilityPack.HtmlNode clientSha1 = this.Version.SelectSingleNode(".//a[@href='cef_binary_" + this.RequiredVersion + "_" + this.TableId + "_client.tar.bz2.sha1']");

                return this.GetHash(clientSha1);
            }
        }


    }
}

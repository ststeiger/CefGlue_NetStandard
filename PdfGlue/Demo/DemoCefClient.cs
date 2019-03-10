
using Xilium.CefGlue;


namespace PdfGlue
{

    internal class DemoCefClient
        : CefClient
    {
        private readonly DemoCefLoadHandler _loadHandler;
        private readonly DemoCefRenderHandler _renderHandler;


        public DemoCefClient(int windowWidth, int windowHeight)
        {
            _renderHandler = new DemoCefRenderHandler(windowWidth, windowHeight);
            _loadHandler = new DemoCefLoadHandler();
        } // End Constructor 


        protected override CefRenderHandler GetRenderHandler()
        {
            return _renderHandler;
        } // End Function GetRenderHandler 


        protected override CefLoadHandler GetLoadHandler()
        {
            return _loadHandler;
        } // End Function GetLoadHandler 


    } // End Class DemoCefClient 


} // End Namespace PdfGlue 

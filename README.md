# PdfGlue



## How to build CefGlue myself?

* Download CEF binaries from http://opensource.spotify.com/cefbuilds/index.html and unpack the archive
* Copy `include` folder from CEF into `CefGlue.Interop.Gen\include`. Manually remove `cef_thread.h` and `cef_waitable_event.h` - these two files should be excluded.
* Run `gen-cef3.cmd` within `CefGlue.Interop.Gen` folder. Note that you need Python 2.7 installed. In case you need to adjust path to Python binaries you can do it in `gen-cef3.cmd` file. This step will generate multiple C# files in `CefGlue` project.
* Build CefGlue binaries - e.g. by running `build-net40.cmd` in the root of the project
* If you just upgraded to a new version of CEF may see compilation errors - most typical fix is to add *new* generated files into `CefGlue` VS project

### License

TODO





###### cefglue\CefGlue.Interop.Gen

cef_parser.py
  * 'float*': ['float*', 'NULL'],

schema_cef3.py
  * 'CefAudioHandler': { 'role': ROLE_HANDLER },

using BoardFlow.Converters.GerberToBfg;

namespace ConsoleApp;

public static class MacroTest {
    public static void MacroAmTest() {
        //var di = new DirectoryInfo(Directory.GetCurrentDirectory() + @"\test_files\gerber\03");
        var fi = new FileInfo(Directory.GetCurrentDirectory() + @"\test_files\gerber\AM_EXAMPLES\AM_test.GTS");
        var gl = Program.ReadGerber(fi);
        var svg = GerberToSpvConverter.Convert(gl);
        Program.WriteSvg(svg);
    }
}
using BoardFlow.Formats.Bfg.Entities;

namespace ConsoleApp;

public static class PointsAccuracyHashing {
    public static void HashPoints() {
        var p1 = new Point(10, 25);
        var p2 = new Point(10.01, 24.96);
        var map = new Dictionary<Point, string> { { p1, "fffttt" } };
        Console.WriteLine(map[p2]);
    }
   
}
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using BoardFlow.Converters.BfgToSvg;
using BoardFlow.Converters.GerberToBfg;
using BoardFlow.Formats.Bfg.Entities;
using BoardFlow.Formats.Bfg.Entities.GraphicElements;
using BoardFlow.Formats.Bfg.Handling;
using BoardFlow.Formats.Gerber.Entities;
using BoardFlow.Formats.Gerber.Reading;
using BoardFlow.Formats.Svg.Entities;
using BoardFlow.Formats.Svg.Writing;

using Geometry = BoardFlow.Formats.Bfg.Handling.Geometry;

namespace ConsoleApp;

public static class Program {

    static Contour DrawCircle(double centerX, double centerY, double radius) {
        var p = new Painter<Contour>(centerX - radius, centerY);
        p.ArcToInc(2*radius, 0, radius, RotationDirection.Clockwise, true);
        p.ArcToInc(-2*radius, 0, radius, RotationDirection.Clockwise, true);
        return p.Root;
    }

    static void BuildTestPrintFile() {

        var spv = new StrxLayer {
            Bounds = new Bounds(0, 0, 1800, 1800)
        };
        
        var radius = 800d;
        while (radius > 10d) {
            var shape = new Shape();
            shape.OuterContours.Add(DrawCircle(900,900, radius));
            radius -= 7;
            shape.InnerContours.Add(DrawCircle(900,900, radius));
            spv.GraphicElements.Add(shape);
            radius -= 7;
        }

        // var svg = new SvgLayer();
        // svg.ViewBox = new Bounds(0, 0, 1800, 1800);
        // svg.Width = 1800;
        // svg.Height = 1800;
        var svg = SpvToSvgConverter.Convert(spv);
        SvgWriter.Write(svg,"D:\\tt1.svg");

    }


    
    [SuppressMessage("ReSharper.DPA", "DPA0001: Memory allocation issues")]
    public static void Main(string[] args) {
        


// // Или с расширенными цветами
//         var layer = new StrxLayer();
// // ... добавление элементов ...
//
//         var bounds = CarExample.CalculateTotalBounds(layer);
//         var advancedRasterizer = new AdvancedRasterizer(800, 600, bounds);
//
//         var bitmap = advancedRasterizer.RasterizeLayerWithColors(layer, SKColors.LightGray);
//         advancedRasterizer.SaveToFile(bitmap, "colored_car.png");
        
        
        JsonTest.Test();
        //BuildTestPrintFile();
        //
        // var p1 = new Painter<Contour>(10, 10);
        // p1.LineToInc(0, 60);
        // p1.LineToInc(40, 0);
        // p1.LineToInc(0, -60);
        // p1.LineToInc(-40, 0);
        // var c1 = p1.Root;
        //
        // var p2 = new Painter<Contour>(40, 30);
        // p2.LineToInc(20, 20);
        // p2.LineToInc(-30, 30);
        // p2.LineToInc(50,0);
        // p2.LineToInc(0,-50);
        // p2.LineToInc(-40,0);
        // var c2 = p2.Root;
        // var t1 = System.DateTime.Now;
        // for (var i = 0; i < 1_000_000; i++) {
        //     var mc1 = Contours.Union(c1, c2);
        //     if (i % 100_000 == 0)
        //         Console.WriteLine(mc1.OuterContour.Curves.Count);
        // }
        //
        // var t2 = System.DateTime.Now;
        // Console.WriteLine("Mss:"+(t2-t1));
        // return;
        
        //PointsAccuracyHashing.HashPoints();
        
        CalculateResult();
        //MacroTest.MacroAmTest();
        return;
        TestGeometry();
        
        double d1 = 128;
        double d2 = 100;
        var d3 = d1 / d2;
        var d4 = Math.Round(d3, 2, MidpointRounding.ToZero);
        var d5 = 0.32;
        var s1 = d5.ToString("R");
        
        TestGerberToSvg();
        //ExpressionsTest.Test("2+3*4*5/(6+7)");
        //CalculateResult();
        // ExpressionsTest.Test();
        // Console.WriteLine("ok");
        // while (true) {
        //     var line = Console.ReadLine();
        //     if (line == null) {
        //         return;
        //     }
        //     ExpressionsTest.Test(line);
        // }
        
    }

    static void TestGeometry() {
        
        //upRight
        //  cp  ep
        //  sp
        
        
        // var sp = new Point(1, 1);
        // var ep = new Point(2, 2);
        // var cp = new Point(1, 2);
        // var aw = Geometry.ArcWay(sp, ep, cp, AxisLayout.YUpXRight);
        // if(aw.IsLarge)
        //     throw new Exception("TestGeometry: large arc way");
        // if(aw.RotationDirection != RotationDirection.CounterClockwise)
        //     throw new Exception("TestGeometry: ClockWise Arc Way");
        //
        // aw = Geometry.ArcWay(sp, ep, cp, AxisLayout.YDownXRight);
        //
        // if(aw.IsLarge)
        //     throw new Exception("TestGeometry: large arc way");
        // if(aw.RotationDirection != RotationDirection.ClockWise)
        //     throw new Exception("TestGeometry: CounterClockwise Arc Way");
        
        //downRight
        //  cp  ep
        //  sp
        
        var aw = Geometry.ArcWay(new Point(1,2), new Point(2,1), new Point(1,1));
        Console.WriteLine("d");
    }

    static string GetText(string text, int charsCount) {
        if (text.Length < charsCount) {
            var d = charsCount - text.Length;
            for (var i = 0; i < d; i++)
                text += " ";
            return text;
        } else {
            return text[..charsCount];
        }
    }
    
    static void CalculateResult() {
        var callsCountPerDay = 50m;
        var avgSellPrice = 20000m;
        var clientOrdersPerDay = 0.02m;
        var resultChance = 0.05m;
        var clientsCount = 0m;
        var chanceLowingPerDay = 0.000001m;
        
        
        var nfi = new NumberFormatInfo {
            NumberGroupSizes = [3],
            NumberDecimalDigits = 3,
            NumberGroupSeparator = " ",
            NumberDecimalSeparator = ","
        };

        var perMonth = 0m;
        for (int d = 1; d < 365; d++) {
            var newClients = Math.Floor((decimal)Random.Shared.NextDouble()*2 * callsCountPerDay * resultChance);
            clientsCount+=newClients;
            var orders = Math.Floor((decimal)Random.Shared.NextDouble()*2*clientsCount * clientOrdersPerDay);
            var sum = (decimal)Random.Shared.NextDouble()*2* orders * avgSellPrice;

            // Console.WriteLine(
            //     GetText($"Day: {d}", 10) +
            //     GetText($"Clients: {clientsCount.ToString("N", nfi)}", 18) +
            //     GetText($"Sum: {sum.ToString("N", nfi)}", 18) +
            //     GetText($"PerMonth: {(sum * 21m).ToString("N", nfi)}", 24)
            // );

            Console.WriteLine(sum.ToString("N", nfi));
            perMonth += sum;
            if (d!=0 && d % 20 == 0) {
                Console.WriteLine("Всего: "+perMonth.ToString("N",nfi));
                perMonth = 0;
            }

            if (resultChance > chanceLowingPerDay) {
                resultChance -= chanceLowingPerDay;
            }
        }
    }

    public static GerberLayer ReadGerber(FileInfo fileInfo) {
        // try {
            var p = GerberReader.Instance.Read(fileInfo);
            if (p.Item2.Errors.Count > 0) {
                Console.WriteLine("  ...ERROR");
                Console.WriteLine("Обнаружены ошибки:");
                foreach (var error in p.Item2.Errors) {
                    Console.WriteLine(error);
                }
                Console.WriteLine();
            }
            
            if (p.Item2.Warnings.Count > 0) {
                Console.WriteLine("Обнаружены предупреждения:");
                foreach (var warning in p.Item2.Warnings) {
                    Console.WriteLine(warning);
                }
            }

            if (p.Item2.Errors.Count == 0 && p.Item2.Warnings.Count == 0) {
                Console.WriteLine("  ...OK");
            }
            return p.Item1;
    }


    
    private static void TestGerberToSvg() {
        var di = new DirectoryInfo(Directory.GetCurrentDirectory() + @"\test_files\gerber\03");
        var files = di.GetFiles("*");
        var i = 0;
        foreach (var fi in files) {
            // if(!fi.Name.Contains("COMPMASK"))
            //     continue;
            // if(i>3)
            //     break;
            // i++;
            if(fi.Extension.ToUpper() is ".DRL" or ".TC" or ".TOL")
                continue;
            Console.WriteLine("-----");
            Console.WriteLine("Обработка файла: " + fi.Name);

            var gl = ReadGerber(fi);
            var svg = GerberToSpvConverter.Convert(gl);
            WriteSvg(svg);
            Console.WriteLine("-----");
            Console.WriteLine("");
        }
    }
    
    public static void WriteSvg(SvgLayer layer) {
        var di = new DirectoryInfo(Directory.GetCurrentDirectory() + @"\test_files\svg");
        var files = di.GetFiles("svg_test_*");
        var num = files.Select(efi => efi.Name.Split(".")[0].Split("_").Last()).Select(int.Parse).Prepend(0).Max();
        num++;

        // var doc = new SvgLayer();
        // for (var i = 0; i < 10; i++) {
        //     var p = new Path {
        //         StartPoint = new Point((double)i * (1 + i) / 2 + i, 0)
        //     };
        //
        //     var w = i + 1;
        //
        //     p.Parts.Add(new LinePathPart {
        //         EndPoint = new Point(p.StartPoint.X, p.StartPoint.Y + w)
        //     });
        //     p.Parts.Add(new LinePathPart {
        //         EndPoint = new Point(p.StartPoint.X + w, p.StartPoint.Y + w)
        //     });
        //     p.Parts.Add(new LinePathPart {
        //         EndPoint = new Point(p.StartPoint.X + w, p.StartPoint.Y)
        //     });
        //
        //
        //     doc.Paths.Add(p);
        // }

        SvgWriter.Write(layer, Directory.GetCurrentDirectory() + @"\test_files\svg\svg_test_" + num.ToString("D4") + ".svg");
    }

}
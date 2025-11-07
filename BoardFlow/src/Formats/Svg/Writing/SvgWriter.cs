using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using BoardFlow.Formats.Bfg.Entities;
using BoardFlow.Formats.Bfg.Entities.GraphicElements;
using BoardFlow.Formats.Bfg.Entities.GraphicElements.Curves;
using BoardFlow.Formats.Bfg.Handling;
using BoardFlow.Formats.Svg.Entities;
using GraphicElements_Path = BoardFlow.Formats.Bfg.Entities.GraphicElements.Path;

namespace BoardFlow.Formats.Svg.Writing;

[SuppressMessage("ReSharper", "ForeachCanBeConvertedToQueryUsingAnotherGetEnumerator")]
public static class SvgWriter {

    static SvgWriter() {
        CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("en-US");
    }


    
    


    private static Bounds CalculateViewBox(SvgLayer doc) {
        // var leftTop = new Point(double.MaxValue, double.MaxValue);
        // var rightBottom = new Point(double.MinValue, double.MinValue);

        var result = new Bounds(double.MaxValue, double.MaxValue, double.MinValue, double.MinValue);
        
        foreach (var e in doc.Elements) {
            result = result.ExtendBounds(e.Bounds);
            // switch (e) {
            //     case Path p:
            //         foreach (var pp in p.Curves) {
            //             result = ExtendBounds(pp.Bounds, result);
            //             // result = ExtendBounds(result, pp.PointFrom);
            //             // result = ExtendBounds(result, pp.PointTo);
            //         }
            //
            //         break;
            //     case Shape shape:
            //
            //         result = ExtendBounds(result, shape.OuterContour.Curves[0].PointFrom);
            //         foreach (var pp in shape.OuterContour.Curves) {
            //             result = ExtendBounds(result, pp.PointFrom);
            //             result = ExtendBounds(result, pp.PointTo);
            //         }
            //             
            //
            //         foreach (var ic in shape.InnerContours) {
            //
            //             foreach (var pp in ic.Curves) {
            //                 result = ExtendBounds(result, ic.Curves[0].PointFrom);
            //                 result = ExtendBounds(result, pp.PointTo);
            //             }
            //                 
            //         }
            //
            //         break;
            //     case Contour contour:
            //
            //         foreach (var pp in contour.Curves) {
            //             result = ExtendBounds(result, pp.PointTo);
            //             result = ExtendBounds(result, contour.Curves[0].PointFrom);
            //         }
            //             
            //         break;
            //     case Dot dot:
            //         result = ExtendBounds(result, dot.CenterPoint);
            //         break;
            //     default: throw new NotImplementedException();
            // }
        }


        var field = result.GetWidth() > result.GetHeight() ? result.GetWidth() * 0.04 : result.GetHeight() * 0.04;
        result = new Bounds(
            result.MinPoint.X - field,
            result.MinPoint.Y - field,
            result.MaxPoint.X + field,
            result.MaxPoint.Y + field
        );

        return result;
    }

    private static List<Bounds> _pathPartsBounds = [];
    static void WritePathPart(StreamWriter writer, ICurve curve) {
        switch (curve) {
            case Line l:
                writer.Write("L " + Math.Round(l.PointTo.X, 6) + " " + Math.Round(l.PointTo.Y, 6) + " ");
                break;
            case Arc a:
                writer.Write("A " +
                             Math.Round(a.Radius, 8) + " " +
                             Math.Round(a.Radius, 8) + " " +
                             "0 " +
                             (a.IsLargeArc ? "1 " : "0 ") +
                             (a.RotationDirection == RotationDirection.Clockwise ? "0 " : "1 ") +
                             Math.Round(a.PointTo.X, 8) + " " +
                             Math.Round(a.PointTo.Y, 8) + " ");
                break;
        }
        _pathPartsBounds.Add(curve.Bounds);
    }

    static void WritePath(StreamWriter writer, GraphicElements_Path path) {
        //var sp = path.StartPoint;
        if(path.Curves.Count==0)
            return;
        var firstPart = path.Curves[0];
        
        writer.Write("\n<path d=\"M  " + Math.Round(firstPart.PointFrom.X, 6) + " " + Math.Round(firstPart.PointFrom.Y, 6) + " ");
        foreach (var pp in path.Curves) 
            WritePathPart(writer, pp);
        if (path.StrokeWidth > 0.00000001) {
            writer.Write("\" stroke-width=\""+Math.Round(path.StrokeWidth,8)+"\"");
        }
        writer.Write("/>");
    }

    static void WriteContour(StreamWriter writer, Contour contour) {
        if(contour.Curves.Count==0)
            return;
        var sp = contour.Curves[0].PointFrom;
        writer.Write("\n<path d=\"M  " + Math.Round(sp.X, 6) + " " + Math.Round(sp.Y, 6) + " ");
        foreach (var pp in contour.Curves) 
            WritePathPart(writer, pp);
        writer.Write("Z\" fill=\"black\"/>");
    }

    static void WriteShape(StreamWriter writer, Shape shape) {
        if (shape.OuterContours.Count == 0)
            return;
        writer.Write("\n<path d=\"");
        foreach (var oc in shape.OuterContours) {
            if (Contours.GetRotationDirection(oc) != RotationDirection.Clockwise) {
                oc.Reverse();
            }

            var sp = oc.Curves[0].PointFrom;
            writer.Write("M " + Math.Round(sp.X, 6) + " " + Math.Round(sp.Y, 6) + " ");
            foreach (var pp in oc.Curves)
                WritePathPart(writer, pp);
            writer.Write("Z");
        }

        foreach (var ic in shape.InnerContours) {
            if (Contours.GetRotationDirection(ic) != RotationDirection.CounterClockwise) {
                ic.Reverse();
            }
            var sp = ic.Curves[0].PointFrom;
            writer.Write("M " + Math.Round(sp.X, 6) + " " + Math.Round(sp.Y, 6) + " ");
            foreach (var pp in ic.Curves)
                WritePathPart(writer, pp);
            writer.Write("Z");
        }
        
        writer.Write("\" fill=\"black\"/>");

    }

    static void WriteDot(StreamWriter writer, Dot dot) {
        writer.Write("<circle cx=\"" + Math.Round(dot.CenterPoint.X, 6) + "\" cy=\"" + Math.Round(dot.CenterPoint.Y, 6) + "\" r=\"" + Math.Round(dot.Diameter / 2, 6) + "\" fill=\"red\"/>");
    }

    public static void Write(SvgLayer doc, string fileName) {
        using var swr = new StreamWriter(fileName);
        _pathPartsBounds.Clear();
        var vbr = doc.ViewBox ?? CalculateViewBox(doc);

        swr.Write("<svg xmlns=\"http://www.w3.org/2000/svg\" " +
                  $"width = \"{vbr.MaxX - vbr.MinX}\" " +
                  $"height = \"{vbr.MaxY - vbr.MinY}\" " +
                  "viewBox=\"" +
                  Math.Round(vbr.MinPoint.X, 6) + " " +
                  Math.Round(vbr.MinPoint.Y, 6) + " " +
                  Math.Round(vbr.GetWidth(), 6) + " " +
                  Math.Round(vbr.GetHeight(), 6) + "\">");
        swr.Write("\n<g fill=\"none\" stroke-width=\"0\">");
        foreach (var e in doc.Elements) {
            switch (e) {
                case GraphicElements_Path p:
                    WritePath(swr, p);
                    break;
                case Contour c:
                    WriteContour(swr, c);
                    break;
                case Shape sh:
                    WriteShape(swr, sh);
                    break;
                case Dot dot:
                    WriteDot(swr, dot);
                    break;
            }
            //AddBoundsRect(swr,e.Bounds,"yellow");
        }

        // foreach (var b in _pathPartsBounds) {
        //     AddBoundsRect(swr,b,"orange");
        // }
        
        swr.Write("\n</g>");
        swr.Write("\n</svg>");
        swr.Flush();
    }

    static void AddBoundsRect(StreamWriter writer, Bounds b, string color) {
        writer.Write("\n<rect x=\""+Math.Round(b.MinX,5)+"\" y=\""+Math.Round(b.MinY,5)+"\" width=\""+Math.Round(b.GetWidth(),5)+"\" height=\""+Math.Round(b.GetHeight(),5)+"\" fill=\"none\" stroke=\""+color+"\" stroke-width=\"0.05px\"/>");
    }
}
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

    static string Format(double value, int digits = 6)
        => Math.Round(value, digits).ToString(CultureInfo.InvariantCulture);

    
    private static Bounds CalculateViewBox(Entities.SvgDocument doc) {
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
                writer.Write("L " + Format(l.PointTo.X, 6) + " " + Format(l.PointTo.Y, 6) + " ");
                break;
            case Arc a:
                writer.Write("A " +
                             Format(a.Radius, 8) + " " +
                             Format(a.Radius, 8) + " " +
                             "0 " +
                             (a.IsLargeArc ? "1 " : "0 ") +
                             (a.RotationDirection == RotationDirection.Clockwise ? "0 " : "1 ") +
                             Format(a.PointTo.X, 8) + " " +
                             Format(a.PointTo.Y, 8) + " ");
                break;
        }
        _pathPartsBounds.Add(curve.Bounds);
    }

    static void WritePath(StreamWriter writer, GraphicElements_Path path) {
        //var sp = path.StartPoint;
        if(path.Curves.Count==0)
            return;
        var firstPart = path.Curves[0];
        
        writer.Write("\n<path d=\"M  " + Format(firstPart.PointFrom.X, 6) + " " + Format(firstPart.PointFrom.Y, 6) + " ");
        foreach (var pp in path.Curves) 
            WritePathPart(writer, pp);
        if (path.StrokeWidth > Geometry.Accuracy) {
            writer.Write("\" stroke-width=\""+Format(path.StrokeWidth,8)+"\"");
        }
        writer.Write("/>");
    }

    static void WriteContour(StreamWriter writer, Contour contour) {
        if(contour.Curves.Count==0)
            return;
        var sp = contour.Curves[0].PointFrom;
        writer.Write("\n<path d=\"M  " + Format(sp.X, 6) + " " + Format(sp.Y, 6) + " ");
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
            writer.Write("M " + Format(sp.X, 6) + " " + Format(sp.Y, 6) + " ");
            foreach (var pp in oc.Curves)
                WritePathPart(writer, pp);
            writer.Write("Z");
        }

        foreach (var ic in shape.InnerContours) {
            if (Contours.GetRotationDirection(ic) != RotationDirection.CounterClockwise) {
                ic.Reverse();
            }
            var sp = ic.Curves[0].PointFrom;
            writer.Write("M " + Format(sp.X, 6) + " " + Format(sp.Y, 6) + " ");
            foreach (var pp in ic.Curves)
                WritePathPart(writer, pp);
            writer.Write("Z");
        }
        
        writer.Write("\" fill=\"black\"/>");

    }

    static void WriteDot(StreamWriter writer, Dot dot) {
        writer.Write("<circle cx=\"" + Format(dot.CenterPoint.X, 6) + "\" cy=\"" + Format(dot.CenterPoint.Y, 6) + "\" r=\"" + Format(dot.Diameter / 2, 6) + "\" fill=\"red\"/>");
    }

    public static void Write(SvgDocument doc, string fileName) {
        using var swr = new StreamWriter(fileName);
        _pathPartsBounds.Clear();
        var vbr = doc.ViewBox ?? CalculateViewBox(doc);

        swr.Write("<svg xmlns=\"http://www.w3.org/2000/svg\" " +
                  $"width = \"{vbr.MaxX - vbr.MinX}\" " +
                  $"height = \"{vbr.MaxY - vbr.MinY}\" " +
                  "viewBox=\"" +
                  Format(vbr.MinPoint.X, 6) + " " +
                  Format(vbr.MinPoint.Y, 6) + " " +
                  Format(vbr.GetWidth(), 6) + " " +
                  Format(vbr.GetHeight(), 6) + "\">");
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
        writer.Write("\n<rect x=\""+Format(b.MinX,5)+"\" y=\""+Format(b.MinY,5)+"\" width=\""+Format(b.GetWidth(),5)+"\" height=\""+Format(b.GetHeight(),5)+"\" fill=\"none\" stroke=\""+color+"\" stroke-width=\"0.05px\"/>");
    }
}
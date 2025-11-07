using BoardFlow.Formats.Bfg.Entities;
using BoardFlow.Formats.Bfg.Entities.GraphicElements;
using BoardFlow.Formats.Bfg.Entities.GraphicElements.Curves;
using BoardFlow.Formats.Svg.Entities;
using BoardFlow.Formats.Svg.Writing;

namespace BoardFlow.Converters.BfgToSvg;

public static class SpvToSvgConverter {
    public static SvgLayer Convert(StrxLayer strxLayer) {
        var result = new SvgLayer {
            ViewBox = strxLayer.Bounds
        };
        result.Elements.AddRange(strxLayer.GraphicElements);
        InvertAxis(result);
        return result;
    }

    public static void WriteContour(Contour contour, string filename) {
        var area = new StrxLayer();
        area.GraphicElements.Add(contour);
        var layer = Convert(area);
        //InvertAxis(layer);
        SvgWriter.Write(layer, filename);
    }
    
    static void InvertAxis(SvgLayer layer) {
        foreach (var e in layer.Elements) {
            switch (e) {
    
                case CurvesOwner ctr:
                    InvertAxis(ctr);
                    break;
                case Shape shape:
                    InvertAxis(shape);
                    break;
                case Dot dot:
                    dot.CenterPoint = new Point(dot.CenterPoint.X, -dot.CenterPoint.Y);
                    break;
                default:
                    throw new Exception("GerberToSvgConverter: InvertAxis");
            }
        }
    }
    
    static ICurve InvertAxis(ICurve pathPart) {
        return pathPart switch {
            Line line => new Line {
                PointTo = line.PointTo with { Y = -line.PointTo.Y }, 
                PointFrom = line.PointFrom with { Y = -line.PointFrom.Y },
            },
            Arc arc => new Arc {
                PointTo = arc.PointTo with { Y = -arc.PointTo.Y },
                PointFrom = arc.PointFrom with { Y = -arc.PointFrom.Y },
                IsLargeArc = arc.IsLargeArc,
                RotationDirection = arc.RotationDirection.Invert(),
                Radius = arc.Radius,
            },
            _ => throw new Exception("GerberToSvgConverter: InvertAxis")
        };
    }
    
    
    static void InvertAxis(Shape shape) {
        foreach (var p in shape.OuterContours) {
            InvertAxis(p);
        }
        foreach (var p in shape.InnerContours) {
            InvertAxis(p);
        }
    }
    static void InvertAxis(CurvesOwner ctx) {
        foreach (var p in ctx.Curves) {
            InvertAxis(p);
        }
    }
}
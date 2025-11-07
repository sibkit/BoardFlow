using BoardFlow.Formats.Bfg.Entities;
using BoardFlow.Formats.Bfg.Entities.GraphicElements;
using BoardFlow.Formats.Bfg.Entities.GraphicElements.Curves;
using BoardFlow.Formats.Bfg.Handling;
using BoardFlow.Formats.Gerber.Entities;
using BoardFlow.Formats.Svg.Entities;
using GerberArcPart = BoardFlow.Formats.Gerber.Entities.ArcPathPart;
using GerberLinePart = BoardFlow.Formats.Gerber.Entities.LinePathPart;
using GraphicElements_Path = BoardFlow.Formats.Bfg.Entities.GraphicElements.Path;
using Path = BoardFlow.Formats.Bfg.Entities.GraphicElements.Path;

namespace BoardFlow.Converters.GerberToBfg;

public static class GerberToSpvConverter {
    
    public static SvgDocument Convert(GerberDocument document) {

        var result = new SvgDocument();
        var apertureConverter = new ApertureConverter(document);
        foreach(var operation in document.Operations)
        {
            switch (operation) {
                case PathPaintOperation path:
                    result.Elements.Add(ConvertPath(path));
                    break;
                case FlashOperation flash:
                    var aperture = document.Apertures[flash.ApertureCode];
                    result.Elements.AddRange(apertureConverter.ConvertAperture(flash.Point, aperture));
                    break;
                default:
                    throw new Exception("GerberToSvgConverter: Convert");
            }
        }
        //InvertAxis(result);
        return result;
    }
    
    static GraphicElements_Path ConvertPath(PathPaintOperation operation) {
        var result = new GraphicElements_Path {
            StrokeWidth = operation.Aperture.Diameter,
            //StartPoint = operation.StartPoint,
        };

        var startPartPoint = operation.StartPoint;
        foreach (var op in operation.Parts) {
            switch (op) {
                case LinePathPart line:
                    result.Curves.Add(new Line {
                        PointFrom = startPartPoint,
                        PointTo = line.EndPoint
                    });
                    break;
                case ArcPathPart arc:
                    result.Curves.AddRange(ConvertArcPath(startPartPoint, arc, result));
                    startPartPoint = arc.EndPoint;
                    break;
                default:
                    throw new Exception("GerberToSvgConverter: ConvertPath");
            }
        }
        return result;
    }
    
    static List<Arc> ConvertArcPath(Point gsp, ArcPathPart gap, CurvesOwner owner) {
        var result = new List<Arc>();
        //var cx = gsp.X + gap.IOffset;
        //var cy = gsp.Y - gap.JOffset;
        var cx = gap.EndPoint.X < gsp.X ? gsp.X - gap.IOffset : gsp.X + gap.IOffset;
        var cy = gap.EndPoint.Y < gsp.Y ? gsp.Y - gap.JOffset : gsp.Y + gap.JOffset;
        
        var r1 = Math.Sqrt(
            Math.Pow(cx - gsp.X, 2) +
            Math.Pow(cy - gsp.Y, 2));
        var r2 = Math.Sqrt(
            Math.Pow(cx - gap.EndPoint.X, 2) +
            Math.Pow(cy - gap.EndPoint.Y, 2));
        var tr = (r2 + r1) / 2; //true radius
        var arcWay = Geometry.ArcWay(gsp, gap.EndPoint, new Point(cx, cy));
        if (gsp == gap.EndPoint) {
            var mpx = cx + (cx - gsp.X);
            var mpy = cy + (cy - gsp.Y);
            var part1 = new Arc {
                RotationDirection = arcWay.RotationDirection,
                Radius = tr,
                IsLargeArc = false,
                PointFrom = gsp,
                PointTo = new Point(mpx, mpy),
            };
            var part2 = new Arc {
                RotationDirection = arcWay.RotationDirection,
                Radius = tr,
                IsLargeArc = true,
                PointFrom = new Point(mpx, mpy),
                PointTo = gsp,
            };
            result.Add(part1);
            result.Add(part2);
        } else {
            
            var part = new Arc {
                RotationDirection = arcWay.RotationDirection, //Invert, because gerber and svg has different axis layout
                PointTo = gap.EndPoint,
                Radius = tr,
                IsLargeArc = arcWay.IsLarge,
                PointFrom = gsp,
            };
            result.Add(part);
        }
        return result;
    }


}


using BoardFlow.Formats.Bfg.Entities;
using BoardFlow.Formats.Common;
using BoardFlow.Formats.Common.Reading;
using BoardFlow.Formats.Gerber.Entities;

namespace BoardFlow.Formats.Gerber.Reading;

public class GerberReadingContext : ReadingContext {
    public Point? CurCoordinate { get; set; }
    public int? CurApertureCode { get; set; }
    public PathPaintOperation? CurPathPaintOperation { get; set; }
    
    public NumberFormat? NumberFormat { get; set; }
    public CoordinatesMode? CoordinatesMode { get; set; }
    
    public LcMode? LcMode { get; set; }
} 
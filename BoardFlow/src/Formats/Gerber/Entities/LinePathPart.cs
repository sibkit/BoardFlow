using BoardFlow.Formats.Bfg.Entities;

namespace BoardFlow.Formats.Gerber.Entities;

public class LinePathPart: IPathPart {
    public LinePathPart(Point endPoint) {
        EndPoint = endPoint;
    }
    public Point EndPoint { get; set; }
}
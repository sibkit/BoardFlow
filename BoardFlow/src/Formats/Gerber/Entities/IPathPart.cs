using BoardFlow.Formats.Bfg.Entities;

namespace BoardFlow.Formats.Gerber.Entities;

public interface IPathPart {
    public Point EndPoint { get; }
}
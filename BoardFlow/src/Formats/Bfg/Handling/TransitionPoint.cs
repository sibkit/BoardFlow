using BoardFlow.Formats.Bfg.Entities.GraphicElements;

namespace BoardFlow.Formats.Bfg.Handling;



public class TransitionPoint {
    public ICurve InCurve { get; init; }
    public ICurve OutCurve { get; init; }
    public Contour Contour { get; init; }
}


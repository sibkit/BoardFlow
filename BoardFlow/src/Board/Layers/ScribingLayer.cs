using BoardFlow.Formats.Bfg.Entities.GraphicElements.Curves;

namespace BoardFlow.Board.Layers;

public class ScribingLayer: IBoardLayer {
    public required string Name { get; set; }
    public List<Line> Lines { get; } = [];
}
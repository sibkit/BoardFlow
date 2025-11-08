using BoardFlow.Formats.Bfg.Entities.GraphicElements;
using BoardFlow.Formats.Bfg.Entities.GraphicElements.Curves;

namespace BoardFlow.Formats.Bfg.Entities;

public class BfgDocument {
    public List<IGraphicElement> GraphicElements { get; } = [];
    public Bounds? Bounds { get; set; }
}
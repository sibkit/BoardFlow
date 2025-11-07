using BoardFlow.Formats.Bfg.Entities;

namespace BoardFlow.Formats.Svg.Entities;

public class SvgDocument {
    public Bounds? ViewBox { get; set; }
    public double? Width { get; set; }
    public double? Height { get; set; }
    public List<IGraphicElement> Elements { get; } = [];
}
using BoardFlow.Formats.Common;
using BoardFlow.Formats.Gerber.Entities.Apertures.Macro;

namespace BoardFlow.Formats.Gerber.Entities;

public class GerberLayer
{
    public List<IPaintOperation> Operations { get; } = [];
    public Uom? Uom {get; set;} = null;
    public Dictionary<int,IAperture> Apertures { get; } = new();
    public Dictionary<string, MacroApertureTemplate> MacroApertureTemplates { get; } = new();
}


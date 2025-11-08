using BoardFlow.Formats.Bfg.Entities;

namespace BoardFlow.Board.Layers;

public class CopperLayer: IStackLayer {
    public required BfgDocument Image { get; init; }

    public required double BaseThickness { get; init; }
    public double? PlatingThickness { get; init; }

    public double Thickness => BaseThickness + PlatingThickness??0;
    
    public required string Name { get; set; }
}
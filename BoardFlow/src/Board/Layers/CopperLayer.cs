using BoardFlow.Formats.Bfg.Entities;

namespace BoardFlow.Board.Layers;

public class CopperLayer: IStackLayer {
    public required StrxLayer Image { get; init; }

    public required double BaseThickness { get; init; }
    public double? PlatingThickness { get; init; }

    public double Thickness => BaseThickness + PlatingThickness??0;

    // public CopperLayer(StrxLayer image, double baseThickness) {
    //     Image = image;
    //     BaseThickness = baseThickness;
    // }
    public required string Name { get; set; }
}
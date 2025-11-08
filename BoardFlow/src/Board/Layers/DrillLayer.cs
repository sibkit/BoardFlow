using BoardFlow.Formats.Bfg.Entities;

namespace BoardFlow.Board.Layers;

public class DrillLayer: IBoardLayer {
    public required string Name { get; set; }
    public BfgDocument Image { get; }
    public bool IsMetallization { get; }
    public IStackLayer? FromLayer { get; set; }
    public IStackLayer? ToLayer { get; set; }
    public DrillLayer(BfgDocument image, bool isMetallization) {
        Image = image;
        IsMetallization = isMetallization;
    }
}
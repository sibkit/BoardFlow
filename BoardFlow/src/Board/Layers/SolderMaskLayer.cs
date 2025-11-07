using BoardFlow.Formats.Bfg.Entities;

namespace BoardFlow.Board.Layers;

public enum SolderMaskColor {
    Any,
    White,
    Green,
    Black,
    MatteWhite,
    MatteBlack
}

public class SolderMaskLayer: IBoardLayer {
    public required string Name { get; set; }
    public SolderMaskLayer(SolderMaskColor color, Bfg image) {
        Color = color;
        Image = image;
    }
    public SolderMaskColor Color { get; }
    public Bfg Image { get; }
}
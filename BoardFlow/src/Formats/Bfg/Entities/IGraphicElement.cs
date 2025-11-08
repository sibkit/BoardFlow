namespace BoardFlow.Formats.Bfg.Entities;

public interface IGraphicElement {
    
    
    Bounds Bounds { get; }
    //void UpdateBounds();
    
    void Move(double dx, double dy);
    
} 
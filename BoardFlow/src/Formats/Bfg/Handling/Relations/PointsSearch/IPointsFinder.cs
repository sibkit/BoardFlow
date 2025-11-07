using BoardFlow.Formats.Bfg.Entities;
using BoardFlow.Formats.Bfg.Entities.GraphicElements;

namespace BoardFlow.Formats.Bfg.Handling.Relations.PointsSearch;

public interface IPointsFinder<in TF, in TS> 
    where TF : ICurve
    where TS: ICurve {
    (List<Point> points, bool isMatch) FindContactPoints(TF curve1, TS curve2);
}
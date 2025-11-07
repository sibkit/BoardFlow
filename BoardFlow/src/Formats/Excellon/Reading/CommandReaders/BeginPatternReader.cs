using BoardFlow.Formats.Bfg.Entities;
using BoardFlow.Formats.Common.Reading;
using BoardFlow.Formats.Excellon.Entities;
using ApplicationException = System.ApplicationException;

namespace BoardFlow.Formats.Excellon.Reading.CommandReaders;

public class BeginPatternReader: ICommandReader<ExcellonCommandType, ExcellonReadingContext, ExcellonLayer> {
    public ExcellonCommandType[] GetNextLikelyTypes() {
        return [ExcellonCommandType.DrillOperation];
    }
    public bool Match(ExcellonReadingContext ctx) {
        return ctx.CurLine == "M25";
    }
    public void WriteToProgram(ExcellonReadingContext ctx, ExcellonLayer layer) {
        if (ctx.CurPattern == null || ctx.CurPattern.State == PatternState.Closed) {
            var coordinate = new Point(0,0);
            if (layer.Operations.Count != 0) {
                var lo = layer.Operations.Last();
                coordinate = lo.StartPoint;
            }
            
            ctx.CurPattern = new Pattern(coordinate);
            
        } else {
            throw new ApplicationException("Команда открытия шаблона при уже открытом шаблоне.");
        }
    }
}
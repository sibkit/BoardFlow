using BoardFlow.Formats.Common.Reading;
using BoardFlow.Formats.Excellon.Entities;

namespace BoardFlow.Formats.Excellon.CommandReaders;

public class RoutOperationReader: ICommandReader<ExcellonCommandType, ExcellonReadingContext, ExcellonLayer> {
    public ExcellonCommandType[] GetNextLikelyTypes() {
        return [ExcellonCommandType.StartMill];
    }
    public bool Match(ExcellonReadingContext ctx) {
        return ctx.CurLine.StartsWith("G00");
    }
    public void WriteToProgram(ExcellonReadingContext ctx, ExcellonLayer layer) {
        if (ctx.CurLine == "G00") {
            return;
        }
        var sc = ctx.CurLine[3..];
        var coordinate = ExcellonCoordinates.ReadCoordinate(sc, ctx);
        if (coordinate == null) {
            ctx.WriteError("Invalid coordinate: "+ctx.CurLine);
        } else {
            ctx.CurPoint = coordinate.Value;
        }
        
    }
}
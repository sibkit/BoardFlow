using BoardFlow.Formats.Bfg.Entities;
using BoardFlow.Formats.Common.Reading;
using BoardFlow.Formats.Excellon.Entities;

namespace BoardFlow.Formats.Excellon.CommandReaders;

public class DrillingOperationReader: ICommandReader<ExcellonCommandType, ExcellonReadingContext, ExcellonLayer> {
    
    public ExcellonCommandType[] GetNextLikelyTypes() {
        return[ExcellonCommandType.DrillOperation, ExcellonCommandType.SetTool];
    }
    public bool Match(ExcellonReadingContext ctx) {
        return ExcellonCoordinates.IsCoordinate(ctx.CurLine);
    }
    
    public void WriteToProgram(ExcellonReadingContext ctx, ExcellonLayer layer) {
        int? toolNumber;
        Point? coordinate;
        
        if (ctx.CurToolNumber != null) 
            toolNumber = ctx.CurToolNumber.Value;
        else {
            throw new ApplicationException("Tool not defined");
        }
        var readedCoordinate = ExcellonCoordinates.ReadCoordinate(ctx.CurLine, ctx) ?? throw new Exception( "DrillingOperationHandler: WriteToProgram (line matched, not readed)");
        switch (ctx.CoordinatesMode) {
            case CoordinatesMode.Incremental:
                if (layer.Operations.Count != 0) {
                    var lastOperation = layer.Operations.Last();
                    coordinate = lastOperation.StartPoint + readedCoordinate;
                } else {
                    coordinate = readedCoordinate;
                }
                break;
            case CoordinatesMode.Absolute:
                coordinate = readedCoordinate;
                break;
            default:
                throw new Exception("DrillingOperationHandler: WriteToProgram (Unknown Coordinates mode)");
        }
        ctx.CurPoint = coordinate.Value;
        var result = new DrillOperation(coordinate!.Value, toolNumber!.Value);
        if (ctx.CurPattern is { State: PatternState.Opened }) {
            ctx.CurPattern.MachiningOperations.Add(result);
        }
        layer.Operations.Add(result);
    }
}
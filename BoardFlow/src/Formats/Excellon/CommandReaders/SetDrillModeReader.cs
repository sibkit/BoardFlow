using BoardFlow.Formats.Common.Reading;
using BoardFlow.Formats.Excellon.Entities;

namespace BoardFlow.Formats.Excellon.CommandReaders;

public class SetDrillModeReader: ICommandReader<ExcellonCommandType, ExcellonReadingContext, ExcellonLayer> {
    public ExcellonCommandType[] GetNextLikelyTypes() {
        return [ExcellonCommandType.DrillOperation];
    }
    public bool Match(ExcellonReadingContext ctx) {
        return ctx.CurLine == "G05";
    }
    public void WriteToProgram(ExcellonReadingContext ctx, ExcellonLayer layer) {
        //ctx.CurOperationType = MachiningOperationType.Drill;
    }
}
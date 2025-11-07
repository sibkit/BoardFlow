using BoardFlow.Formats.Common.Reading;
using BoardFlow.Formats.Excellon.Entities;

namespace BoardFlow.Formats.Excellon.Reading.CommandReaders;

public class EndHeaderReader: ICommandReader<ExcellonCommandType, ExcellonReadingContext, ExcellonLayer> {
    public ExcellonCommandType[] GetNextLikelyTypes() {
        return [];
    }
    public bool Match(ExcellonReadingContext ctx) {
        var line = ctx.CurLine;
        return line.Equals("%") || line.Equals("M95");
    }
    public void WriteToProgram(ExcellonReadingContext ctx, ExcellonLayer layer) {
        //do nothing
    }
}
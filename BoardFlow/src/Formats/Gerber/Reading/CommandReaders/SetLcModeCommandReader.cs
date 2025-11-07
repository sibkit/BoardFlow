using BoardFlow.Formats.Common.Reading;
using BoardFlow.Formats.Gerber.Entities;

namespace BoardFlow.Formats.Gerber.Reading.CommandReaders;

public class SetLcModeCommandReader: ICommandReader<GerberCommandType, GerberReadingContext, GerberLayer> {
    public GerberCommandType[] GetNextLikelyTypes() {
        return[];
    }
    public bool Match(GerberReadingContext ctx) {
        return ctx.CurLine is "G01*" or "G02*" or "G03*";
    }
    public void WriteToProgram(GerberReadingContext ctx, GerberLayer layer) {
        ctx.LcMode = ctx.CurLine switch {
            "G01*" => LcMode.Linear,
            "G02*" => LcMode.Clockwise,
            "G03*" => LcMode.Counterclockwise,
            _ => throw new Exception("Unknown LC mode")
        };
    }
}
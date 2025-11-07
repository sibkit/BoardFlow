using BoardFlow.Formats.Common.Reading;
using BoardFlow.Formats.Gerber.Entities;

namespace BoardFlow.Formats.Gerber.Reading.CommandReaders;

public class IgnoredCommandReader: ICommandReader<GerberCommandType, GerberReadingContext, GerberDocument> {
    public GerberCommandType[] GetNextLikelyTypes() {
        return [];
    }
    public bool Match(GerberReadingContext ctx) {
        return ctx.CurLine switch {
            "G75*" => true, //This command must be issued before the first circular plotting operation, for compatibility with older Gerber versions
            "G74*" => true, //This command must be issued before the first circular plotting operation, for compatibility with older Gerber versions
            "M00*" => true, //Program stop
            _ => false
        };
    }
    public void WriteToProgram(GerberReadingContext ctx, GerberDocument document) {
        if (ctx.CurLine is "G74*" or "G75*") {
            ctx.WriteWarning("Устаревшая команда: "+ctx.CurLine);
        }
        ctx.WriteInfo("Пропущенная команда: " + ctx.CurLine);
    }
}
using System.Text.RegularExpressions;
using BoardFlow.Formats.Common.Reading;
using BoardFlow.Formats.Gerber.Entities;

namespace BoardFlow.Formats.Gerber.Reading.CommandReaders;

public partial class SetApertureCommandReader: ICommandReader<GerberCommandType, GerberReadingContext, GerberLayer> {

    [GeneratedRegex("^(?:G54)*D([0-9]+)\\*$")]
    private static partial Regex MyRegex();
    
    public GerberCommandType[] GetNextLikelyTypes() {
        return [];
    }
    public bool Match(GerberReadingContext ctx) {
        return MyRegex().IsMatch(ctx.CurLine);
    }
    public void WriteToProgram(GerberReadingContext ctx, GerberLayer layer) {
        var m = MyRegex().Match(ctx.CurLine);
        var apCode = int.Parse(m.Groups[1].Value);
        if (layer.Apertures.ContainsKey(apCode)) {
            ctx.CurApertureCode = apCode;
        } else {
            ctx.WriteError("Не найдена аппертура с кодом: "+apCode);
        }
    }
}
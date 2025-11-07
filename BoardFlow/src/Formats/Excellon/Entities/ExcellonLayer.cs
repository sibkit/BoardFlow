using BoardFlow.Formats.Common;

namespace BoardFlow.Formats.Excellon.Entities;

public class ExcellonLayer
{
    public List<IMachiningOperation> Operations { get; } = [];
    public Uom? Uom {get; set;} = null;
    public Dictionary<int,decimal> ToolsMap { get; } = new Dictionary<int, decimal>();
}


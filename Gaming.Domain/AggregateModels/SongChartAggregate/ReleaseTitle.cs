using CleanCode.Patterns.DataStructures;

namespace Gaming.Domain.AggregateModels.SongChartAggregate;

public abstract class ReleaseTitle : Enumeration
{
    protected ReleaseTitle(int id, string name, bool isDlc) : base(id, name)
    {
        IsDlc = isDlc;
    }

    public bool IsDlc { get; }
}

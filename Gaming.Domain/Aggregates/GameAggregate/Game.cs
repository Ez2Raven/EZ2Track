using Gaming.Domain.SeedWork;

namespace Gaming.Domain.Aggregates.GameAggregate;

public interface IGame
{
    string Title { get; set; }
    bool IsDlc { get; set; }
}

public class Game : Entity, IGame
{
    public string Title { get; set; } = string.Empty;
    public bool IsDlc { get; set; }

    public override string ToString()
    {
        return $"{nameof(Title)}: {Title}, {nameof(IsDlc)}: {IsDlc}";
    }
}

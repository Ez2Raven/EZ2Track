using Gaming.Domain.SeedWork;

namespace Gaming.Domain.Aggregates.GameAggregate;



public class Game : Entity, IGame
{
    public string Title { get; set; } = string.Empty;
    public bool IsDlc { get; set; }

    public override string ToString()
    {
        return $"{nameof(Title)}: {Title}, {nameof(IsDlc)}: {IsDlc}";
    }
}

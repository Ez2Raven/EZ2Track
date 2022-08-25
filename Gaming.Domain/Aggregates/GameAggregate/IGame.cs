namespace Gaming.Domain.Aggregates.GameAggregate;

public interface IGame
{
    string Title { get; set; }
    bool IsDlc { get; set; }
}

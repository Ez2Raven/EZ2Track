using MusicGames.Domain.SeedWork;

namespace MusicGames.Domain.AggregatesModels.GameAggregate
{
    public class Game : Entity
    {
        public string Title { get; set; }
        public bool IsDlc { get; set; }

        public override string ToString()
        {
            return $"{nameof(Title)}: {Title}, {nameof(IsDlc)}: {IsDlc}";
        }
    }
}
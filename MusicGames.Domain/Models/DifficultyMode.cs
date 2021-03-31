namespace MusicGames.Domain.Models
{
    public class DifficultyMode
    {
        public DifficultyMode()
        {
        }

        public DifficultyCategory Category { get; set; } = DifficultyCategory.None;
        public int Level { get; set; }

        public override string ToString()
        {
            return $"{nameof(Category)}: {Category}, {nameof(Level)}: {Level}";
        }
    }
}
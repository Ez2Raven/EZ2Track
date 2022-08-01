namespace Gaming.Domain.Aggregates.GameTrackAggregate
{
    public class DifficultyMode
    {
        public DifficultyCategory Category { get; set; } = DifficultyCategory.None;
        public int Level { get; set; }

        public override string ToString()
        {
            return $"{nameof(Category)}: {Category}, {nameof(Level)}: {Level}";
        }
    }
}
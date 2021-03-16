using CleanCode.Patterns.Specifications;
using CleanCode.Patterns.Validations;

namespace MusicGames.Domain.Specifications
{
    public class SongTitleProvidedSpec:Validatable, ISpecification<Song>
    {
        public const string ValidationMessage = "Song title must not be empty and does not contains only whitespace";
        public bool IsSatisfiedBy(Song entity)
        {
            var isSatisfied = !string.IsNullOrWhiteSpace(entity.Title);
            if (!isSatisfied)
            {
                BroadcastValidationMessage(ValidationMessage);
            }
            
            return isSatisfied;
        }
    }
}
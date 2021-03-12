using CleanCode.Patterns.Specifications;
using CleanCode.Patterns.Validations;

namespace MusicGames.Domain.Specifications
{
    public class IsSongTitleProvided:Validatable, ISpecification<Song>
    {
        public bool IsSatisfiedBy(Song entity)
        {
            var isSatisfied = !string.IsNullOrWhiteSpace(entity.Title);
            if (!isSatisfied)
            {
                BroadcastValidationMessage("Song title must not be empty and does not contains only whitespace");
            }
            
            return isSatisfied;
        }
    }
}
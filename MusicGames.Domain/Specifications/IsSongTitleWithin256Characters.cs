using CleanCode.Patterns.Specifications;
using CleanCode.Patterns.Validations;

namespace MusicGames.Domain.Specifications
{
    public class IsSongTitleWithin256Characters:Validatable,ISpecification<Song>
    {
        private const int MaxAllowedLength = 256;
        public bool IsSatisfiedBy(Song entity)
        {
            var isSatisfied = entity.Title.Length <= MaxAllowedLength;
            if (!isSatisfied)
            {
                BroadcastValidationMessage("Song title must be within 256 characters");
            }

            return isSatisfied;
        }
    }
}
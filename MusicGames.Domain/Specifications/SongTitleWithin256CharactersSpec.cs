using CleanCode.Patterns.Specifications;
using CleanCode.Patterns.Validations;

namespace MusicGames.Domain.Specifications
{
    public class SongTitleWithin256CharactersSpec:Validatable,ISpecification<Song>
    {
        private const int MaxAllowedLength = 256;
        public const string ValidationMessage = "Song title must be within 256 characters";
        public bool IsSatisfiedBy(Song entity)
        {
            var isSatisfied = entity.Title.Length <= MaxAllowedLength;
            if (!isSatisfied)
            {
                BroadcastValidationMessage(ValidationMessage);
            }

            return isSatisfied;
        }
    }
}
using CleanCode.Patterns.Specifications;
using CleanCode.Patterns.Validations;

namespace MusicGames.Domain.Specifications
{
    public class SongComposerWithin256CharactersSpec : Validatable, ISpecification<Song>
    {
        public bool IsSatisfiedBy(Song entity)
        {
            var isSatisfied = entity.Title.Length <= MaxAllowedLength;
            if (!isSatisfied)
            {
                BroadcastValidationMessage(ValidationMessage);
            }

            return isSatisfied;
        }
        private const int MaxAllowedLength = 256;
        public const string ValidationMessage = "Song composer must be within 256 characters";
    }
}
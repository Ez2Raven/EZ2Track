using Xunit;

namespace CleanCode.Patterns.XUnit
{
    public class NullAndWhitespaceTheoryData:TheoryData<string>
    {
        public NullAndWhitespaceTheoryData()
        {
            Add(null);
            Add(string.Empty);
            Add(new(' ', 20));
            Add("  \t   ");
            Add(new('\u2000', 10));
        }
    }
}
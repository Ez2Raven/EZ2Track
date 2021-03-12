using CleanCode.Patterns.Validations;

namespace CleanCode.Patterns.Specifications
{
    public interface ISpecification<T>
    {
        bool IsSatisfiedBy(T entity);
    }
}
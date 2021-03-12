using System;
using CleanCode.Patterns.Validations;

namespace CleanCode.Patterns.Specifications
{
    public class NotSpecification<T>:Validatable, ISpecification<T>
    {
        private readonly ISpecification<T> _specification;

        public NotSpecification(ISpecification<T> specification)
        {
            _specification = specification ?? throw new ArgumentNullException(nameof(specification));
        }
        
        public bool IsSatisfiedBy(T entity)
        {
            return !_specification.IsSatisfiedBy(entity);
        }
    }
}
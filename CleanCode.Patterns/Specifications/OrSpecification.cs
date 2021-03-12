using System;
using CleanCode.Patterns.Validations;

namespace CleanCode.Patterns.Specifications
{
    public class OrSpecification<T>:Validatable, ISpecification<T>
    {
        private readonly ISpecification<T> _specification1;
        private readonly ISpecification<T> _specification2;

        public OrSpecification(ISpecification<T> specification1, ISpecification<T> specification2)
        {
            _specification1 = specification1 ?? throw new ArgumentNullException(nameof(specification1));
            _specification2 = specification2 ?? throw new ArgumentNullException(nameof(specification2));
        }
        
        public bool IsSatisfiedBy(T entity)
        {
            return _specification1.IsSatisfiedBy(entity) || _specification2.IsSatisfiedBy(entity);
        }
    }
}
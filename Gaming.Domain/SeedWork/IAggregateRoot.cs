namespace Gaming.Domain.SeedWork;

/// <summary>
///     This is a marker interface to identify domain classes that encapsulates multiple classes.
///     i.e. You can manipulate the whole hierarchy only through the main object
/// </summary>
public interface IAggregateRoot
{
    // A marker interface is sometimes considered as an anti-pattern;
    // however, it is also a clean way to mark a class, especially when that interface might be evolving.
    // An attribute could be the other choice for the marker, but it is quicker to see the base class (Entity)
    // next to the IAggregate interface instead of putting an Aggregate attribute marker above the class.
    // It is a matter of preferences, in any case.
}

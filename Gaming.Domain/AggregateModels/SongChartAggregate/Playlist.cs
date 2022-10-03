using System;
using System.Collections.Generic;
using Gaming.Domain.SeedWork;

namespace Gaming.Domain.AggregateModels.SongChartAggregate;

public class Playlist<T> : IAggregateRoot
{
    public string Name { get; set; } = string.Empty;
    public DateTime DateTimeCreated { get; set; }
    public DateTime DateTimeModified { get; set; }

    public IList<T> Songs { get; set; } = new List<T>();
}

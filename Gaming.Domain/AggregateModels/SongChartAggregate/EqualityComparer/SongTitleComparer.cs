using System;
using System.Collections.Generic;

namespace Gaming.Domain.AggregateModels.SongChartAggregate.EqualityComparer;

public class SongTitleComparer:IEqualityComparer<ISongChart>
{
    public bool Equals(ISongChart? x, ISongChart? y)
    {
        if (object.ReferenceEquals(x, y)) return true;

        if (object.ReferenceEquals(x, null) || object.ReferenceEquals(y, null))
            return false;

        return string.Equals(x.Song.Title, y.Song.Title, StringComparison.CurrentCultureIgnoreCase);
    }

    public int GetHashCode(ISongChart chart)
    {
        if (object.ReferenceEquals(chart, null)) return 0;
        int songTitleHash = chart.Song.Title == string.Empty ? 0 : chart.Song.Title.GetHashCode();
        return songTitleHash;
    }
}

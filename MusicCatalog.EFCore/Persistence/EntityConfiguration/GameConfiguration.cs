using Gaming.Domain.AggregateModels.SongChartAggregate;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MusicCatalog.EFCore.Persistence.EntityConfiguration;

public sealed class GameConfiguration : SeedWorkTptConfiguration<ReleaseTitle>
{
    public override void Configure(EntityTypeBuilder<ReleaseTitle> builder)
    {
        builder
            .Property(game => game.Name)
            .HasMaxLength(250)
            .IsRequired();
        builder
            .Property(game => game.IsDlc)
            .IsRequired();

        base.Configure(builder);
    }
}

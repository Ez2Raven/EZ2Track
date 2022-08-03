using Gaming.Domain.Aggregates.GameAggregate;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MusicCatalog.EFCore.Persistence.EntityConfiguration;

public sealed class GameConfiguration : SeedWorkTptConfiguration<Game>
{
    public override void Configure(EntityTypeBuilder<Game> builder)
    {
        builder
            .Property(game => game.Title)
            .HasMaxLength(250)
            .IsRequired();
        builder
            .Property(game => game.IsDlc)
            .IsRequired();

        base.Configure(builder);
    }
}

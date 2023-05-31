using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Teams.Infrastructure.Entities;

namespace Teams.Infrastructure.Configurations
{
    public class CompetitionConfiguration : IEntityTypeConfiguration<Competition>
    {
        public void Configure(EntityTypeBuilder<Competition> builder)
        {
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Name).IsRequired();
            builder.Property(c => c.Code).IsRequired();
            builder.Property(c => c.Area).IsRequired();
            builder.Property(c => c.Type).HasConversion<string>();

            builder.HasMany(c => c.Teams)
                .WithMany(t => t.Competitions)
                .UsingEntity("CompetitionsTeams");
        }
    }
}
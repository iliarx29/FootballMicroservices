using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Teams.Infrastructure.Entities;

namespace Teams.Infrastructure.Configurations
{
    public class TeamConfiguration : IEntityTypeConfiguration<Team>
    {
        public void Configure(EntityTypeBuilder<Team> builder)
        {
            builder.HasKey(t => t.Id);

            builder.Property(t => t.Name).IsRequired();
            builder.Property(t => t.Code).HasMaxLength(3);
            builder.Property(t => t.CountryName).IsRequired();
            builder.Property(t => t.City).IsRequired();
            builder.Property(t => t.Stadium).IsRequired();

            builder.HasOne(t => t.League)
                .WithMany(l => l.Teams)
                .HasForeignKey(t => t.LeagueId);
        }
    }
}
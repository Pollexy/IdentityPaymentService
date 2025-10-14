using Domain.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class UserConfiguration: IEntityTypeConfiguration<UserAggregate>
{
    public void Configure(EntityTypeBuilder<UserAggregate> builder)
    {
        builder.ToTable("user");
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Id).HasColumnName("id").HasColumnType("uuid");
        builder.Property(x => x.FirstName).HasColumnName("first_name").HasColumnType("varchar");
        builder.Property(x => x.LastName).HasColumnName("last_name").HasColumnType("varchar");
        builder.Property(x => x.FullName).HasColumnName("full_name").HasColumnType("varchar");
        builder.Property(x => x.Email).HasColumnName("email").HasColumnType("varchar");
        builder.Property(x => x.Phone).HasColumnName("phone").HasColumnType("varchar");
        builder.Property(x=>x.Bio).HasColumnName("bio").HasColumnType("varchar");
        builder.Property(x=>x.ProfileImageUrl).HasColumnName("profile_image_url").HasColumnType("varchar");
        builder.Property(x => x.Weight).HasColumnName("weight").HasColumnType("numeric(5,2)");
        builder.Property(x => x.Height).HasColumnName("height").HasColumnType("numeric(5,2)");
        builder.Property(x=>x.BirthDate).HasColumnName("birth_date").HasColumnType("timestamptz");
        builder.Property(e => e.Gender)
            .HasColumnName("gender")
            .HasColumnType("varchar(1)")
            .HasConversion(
                v => v == GenderType.Woman ? "F" :
                    v == GenderType.Man ? "M" : "U", 
                v => v == "F" ? GenderType.Woman :
                    v == "M" ? GenderType.Man :
                    GenderType.Unknown 
            );
    }
}
using Domain.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class IdentityConfigurations : IEntityTypeConfiguration<IdentityAggregate>
    {
        public void Configure(EntityTypeBuilder<IdentityAggregate> builder)
        {
            builder.HasKey(x => x.Id);

            builder.ToTable("identities");
            builder.Property(x => x.UserId).HasColumnName("user_id").HasColumnType("uuid");
            builder.Property(x => x.Email).HasColumnName("email").HasColumnType("varchar(256)").IsRequired(); 
            builder.Property(x => x.Phone).HasColumnName("phone").HasColumnType("varchar(15)").IsRequired(false);
            builder.Property(x => x.PasswordHash).HasColumnName("password_hash").HasColumnType("varchar(256)").IsRequired();
            builder.Property(x => x.LastFailedLoginDate).HasColumnName("last_failed_login_date").HasColumnType("timestamptz");
            builder.Property(x => x.FailedLoginCount).HasColumnName("failed_login_count").HasColumnType("integer")
                .HasDefaultValue(0).IsRequired();
            builder.Property(x => x.IsEmailVerified).HasColumnName("is_email_verified").HasColumnType("boolean")
                .HasDefaultValue(false).IsRequired();
            builder.Property(x => x.IsPhoneVerified).HasColumnName("is_phone_verified").HasColumnType("boolean")
                .HasDefaultValue(false).IsRequired();
            builder.Property(x => x.Status)
                .HasColumnName("status")
                .HasConversion<string>()
                .HasMaxLength(100)
                .IsRequired();
            builder.HasIndex(x => x.Email).IsUnique();
            builder.HasOne(i => i.User)
                .WithMany(u => u.Identities)
                .HasForeignKey(i => i.UserId)   
                .HasPrincipalKey(u => u.Id)    
                .OnDelete(DeleteBehavior.Cascade);

            builder.Navigation(i => i.User).AutoInclude(false);  
        
        }
    }
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Yarnique.Modules.UsersManagement.Domain.Users;

namespace Yarnique.Modules.UsersManagement.Infrastructure.Domain.Users
{
    internal class UsersEntityTypeConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users", "users");

            builder.HasKey(x => x.Id);

            builder.Property<string>("_userName").HasColumnName("UserName");
            builder.Property<string>("_firstName").HasColumnName("FirstName");
            builder.Property<string>("_lastName").HasColumnName("LastName");
            builder.Property<string>("_email").HasColumnName("Email");
            builder.Property<string>("_password").HasColumnName("Password");
            builder.Property<string>("_passwordSalt").HasColumnName("PasswordSalt");
            builder.Property<bool>("_isActive").HasColumnName("IsActive");
            builder.OwnsOne<ApplicationRole>("_role", b =>
            {
                b.Property(p => p.Value).HasColumnName("Role");
            });
        }
    }
}

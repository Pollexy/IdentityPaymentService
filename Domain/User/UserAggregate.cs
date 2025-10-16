using System.ComponentModel.DataAnnotations;

using Domain.Common.BaseModels;
using Domain.Identity;

namespace Domain.User;

public enum GenderType
{
    [Display(Name = "U")] Unknown = 0,

    [Display(Name = "F")] Woman = 1,

    [Display(Name = "M")] Man = 2
}

public class UserAggregate : BaseAggregate
{
    public UserAggregate()
    {
        //only use db
    }

    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string FullName { get; set; }
    public string? Phone { get; set; }
    public required string Email { get; set; }
    public string? Bio { get; private set; }
    public string? ProfileImageUrl { get; private set; }
    public decimal? Weight { get; set; }
    public decimal? Height { get; set; }
    public DateOnly? BirthDate { get; set; } = new DateOnly(2000, 1, 1);
    public GenderType Gender { get; set; } = GenderType.Unknown;

    public ICollection<IdentityAggregate> Identities { get; set; } = new List<IdentityAggregate>();

    private UserAggregate(string firstName, string lastName, DateOnly birthDate, string email,
        string? phone = null, string? bio = null, string? profileImageUrl = null)
    {
        FirstName = firstName.Trim();
        LastName = lastName.Trim();
        FullName = firstName + " " + lastName;
        Phone = phone?.Trim() ?? string.Empty;
        Email = email.Trim();
        BirthDate = birthDate;
        Bio = bio;
        ProfileImageUrl = profileImageUrl;
    }


    public static UserAggregate Create(string firstName, string lastName, string email,
        string phone)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new ArgumentException("Ad boş olamaz.", nameof(firstName));

        if (string.IsNullOrWhiteSpace(lastName))
            throw new ArgumentException("Soyad boş olamaz.", nameof(lastName));

        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email boş olamaz.", nameof(email));

        if (string.IsNullOrWhiteSpace(phone))
            throw new ArgumentException("Telefon boş olamaz.", nameof(phone));

        var user = new UserAggregate
        {
            Phone = phone.Trim(),
            FirstName = firstName.Trim(),
            LastName = lastName.Trim(),
            Email = email.Trim(),
            FullName = firstName + " " + lastName,
        };

        user.SetAsCreated();
        return user;
    }

    public void UpdateProfileInfos(string? bio = null, string? profileImageUrl = null, GenderType? genderType = null,
        decimal? weight = null, decimal? height = null, DateOnly? birthDate = null)
    {
        Bio = string.IsNullOrWhiteSpace(bio) ? null : bio.Trim();
        ProfileImageUrl = string.IsNullOrWhiteSpace(profileImageUrl) ? null : profileImageUrl.Trim();
        Weight = weight;
        Height = height;
        if (birthDate.HasValue)
            BirthDate = birthDate.Value;
        if (genderType.HasValue)
            Gender = genderType.Value;
        SetAsModified(Id);
    }

    public void UpdateName(string firstName, string lastName)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new ArgumentException("Ad boş olamaz.", nameof(firstName));

        if (string.IsNullOrWhiteSpace(lastName))
            throw new ArgumentException("Soyad boş olamaz.", nameof(lastName));

        FirstName = firstName.Trim();
        LastName = lastName.Trim();
        FullName = firstName + " " + lastName;
        SetAsModified();
    }

    public IdentityAggregate AddIdentity(string email, string hashedPassword, string? username = null,
        string? phone = null)
    {
        if (string.IsNullOrWhiteSpace(email) || !IdentityAggregate.IsValidEmail(email))
            throw new ArgumentException("Geçersiz e-posta formatı.", nameof(email));

        if (string.IsNullOrWhiteSpace(hashedPassword))
            throw new ArgumentException("Parola boş olamaz.", nameof(hashedPassword));

        var identity = new IdentityAggregate
        {
            UserId = Id,
            User = this,
            Phone = string.IsNullOrWhiteSpace(phone) ? string.Empty : phone.Trim(),
            Email = email.Trim().ToLowerInvariant(),
            PasswordHash = hashedPassword,
            Status = IdentityStatus.Passive
        };

        identity.SetAsCreated();
        Identities.Add(identity);

        return identity;
    }
}

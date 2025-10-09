using Domain.Common.BaseModels;
using Domain.User;

namespace Domain.Identity;

public enum IdentityStatus
{
    Passive,
    Active,
    Locked,
    Deactivated
}

public class IdentityAggregate : BaseAggregate
{
    public IdentityAggregate()
    {
        //only use db
    }

    private const int MaxFailedLoginAttempts = 5;
    public Guid UserId { get; set; }
    public required string Email { get; set; }
    public string? Phone { get; set; }
    public required string PasswordHash { get; set; }
    public DateTime? LastFailedLoginDate { get; private set; }
    public int FailedLoginCount { get; set; } = 0;
    public bool IsEmailVerified { get; set; }
    public bool IsPhoneVerified { get; set; }
    public IdentityStatus Status { get; set; }
    public UserAggregate User { get; set; } = null!;

    private IdentityAggregate(Guid userId, string email,  string phone, string passwordHash,
        DateTime lastFailedLoginDate, int failedLoginCount, bool isEmailVerified, bool isPhoneVerified,
        IdentityStatus identityStatus)
    {
        UserId = userId;
        Email = email.Trim().ToLowerInvariant();
        Phone = string.IsNullOrWhiteSpace(phone) ? null : phone.Trim();
        PasswordHash = passwordHash;
        LastFailedLoginDate = lastFailedLoginDate;
        FailedLoginCount = failedLoginCount;
        IsEmailVerified = isEmailVerified;
        IsPhoneVerified = isPhoneVerified;
        Status = identityStatus;
    }

    public void ChangeEmail(string newEmail)
    {
        if (string.IsNullOrWhiteSpace(newEmail) || !IsValidEmail(newEmail))
            throw new ArgumentException("Geçersiz e-posta formatı.", nameof(newEmail));
        if (!Email.Equals(newEmail, StringComparison.OrdinalIgnoreCase))
        {
            Email = newEmail;
            IsEmailVerified = false;
            Status = IdentityStatus.Passive;
            SetAsModified();
        }
    }

    public void Deactivate()
    {
        Status = IdentityStatus.Deactivated;
        SetAsModified();
    }

    public void RecordFailedLoginAttempt()
    {
        if (Status == IdentityStatus.Locked) return;

        FailedLoginCount++;
        LastFailedLoginDate = DateTime.UtcNow;

        if (FailedLoginCount >= MaxFailedLoginAttempts)
        {
            Status = IdentityStatus.Locked;
        }

        SetAsModified();
    }

    public void VerifyEmailAndActivate()
    {
        if (IsEmailVerified) return;

        IsEmailVerified = true;

        if (Status == IdentityStatus.Passive)
        {
            Status = IdentityStatus.Active;
        }

        SetAsModified();
    }

    public void VerifyPhoneAndActivate()
    {
        if (IsPhoneVerified) return;
        IsPhoneVerified = true;
        if (Status == IdentityStatus.Passive)
        {
            Status = IdentityStatus.Active;
        }

        SetAsModified();
    }

    public void ChangePassword(string newPasswordHash)
    {
        if (string.IsNullOrWhiteSpace(newPasswordHash))
            throw new ArgumentNullException(nameof(newPasswordHash));

        PasswordHash = newPasswordHash;

        ResetFailedLoginAttempts();

        if (Status == IdentityStatus.Locked)
        {
            Status = IdentityStatus.Active;
        }

        SetAsModified();
    }

    private void ResetFailedLoginAttempts()
    {
        if (FailedLoginCount == 0 && LastFailedLoginDate == null) return;

        FailedLoginCount = 0;
        LastFailedLoginDate = null;
        SetAsModified();
    }

    public void Unlock()
    {
        if (Status != IdentityStatus.Locked) return;

        ResetFailedLoginAttempts();
        Status = IdentityStatus.Active;
        SetAsModified();
    }

    public static bool IsValidEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email)) return false;

        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }

    public void AttachUser(Guid userId)
    {
        if (userId == Guid.Empty) throw new ArgumentException("Geçersiz UserId");
        if (UserId != Guid.Empty) throw new InvalidOperationException("Identity zaten bir kullanıcıya bağlı");

        UserId = userId;
        SetAsModified();
    }
}
using Application.Features.User.Commands;

using Domain.User;

namespace IdentityPaymentService.Models.User.Request
{
    public class UpdateUserAccountDetailRequest
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Fullname => $"{FirstName} {LastName}".Trim();
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Bio { get; set; }
        public GenderType? Gender { get; set; }
        public decimal? Weight { get; set; }
        public decimal? Height { get; set; }
        public string? ProfileImageUrl { get; set; }

        public UpdateUserAccountDetailCommand ToCommand(Guid id)
        {
            return new UpdateUserAccountDetailCommand(id, FirstName, LastName, Phone, Email, Gender, Weight, Height, Bio,
                ProfileImageUrl);
        }
    }
}

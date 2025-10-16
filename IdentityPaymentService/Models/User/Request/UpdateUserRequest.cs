using Application.Features.User.Commands;
using Domain.User;

namespace IdentityPaymentService.Models.User.Request
{
    public class UpdateUserRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Fullname { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public GenderType? Gender { get; set; }
        public decimal? Weight { get; set; }
        public decimal? Height { get; set; }
        public UpdateUserCommand ToCommand(Guid id)
        {
            return new UpdateUserCommand(id, FirstName, LastName, Fullname, Email, Phone, Gender, Weight,Height);
        }
    }
}

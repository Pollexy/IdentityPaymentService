using Application.Common.Interfaces;
using Domain.User;
using MediatR;

namespace Application.Features.User.Commands
{
    public class UpdateUserAccountDetailCommand : IRequest
    {
        public UpdateUserAccountDetailCommand(Guid id, string firstName, string lastName, string phone,
            string email, GenderType? genderType, decimal? weight, decimal? height, string? bio = null,
            string? profileImageUrl = null)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Phone = phone;
            Gender = genderType;
            Weight = weight;
            Height = height;
            Bio = bio;
            ProfileImageUrl = profileImageUrl;
        }

        public Guid Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string Fullname => $"{FirstName} {LastName}".Trim();
        public string Email { get; set; }
        public string Phone { get; set; }
        public GenderType? Gender { get; set; }
        public decimal? Weight { get; set; }
        public decimal? Height { get; set; }
        public string? Bio { get; set; }
        public string? ProfileImageUrl { get; set; }

        public class Handler : IRequestHandler<UpdateUserAccountDetailCommand>
        {
            private readonly IUserRepository _userRepository;

            public Handler(IUserRepository userRepository)
            {
                _userRepository = userRepository;
            }

            public async Task Handle(UpdateUserAccountDetailCommand request, CancellationToken cancellationToken)
            {
                var user = await _userRepository.GetByIdAsync(request.Id, cancellationToken);
                user.UpdateProfileInfos(request.Bio, request.ProfileImageUrl, request.Gender);

                await _userRepository.UpdateAsync(user, cancellationToken);
            }
        }
    }
}

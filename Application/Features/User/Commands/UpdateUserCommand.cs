using Application.Common.Interfaces;

using Domain.User;

using MediatR;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.User.Commands
{
    public class UpdateUserCommand : IRequest
    {
        public UpdateUserCommand(Guid id, string firstName, string lastName, string phone,
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

        public class Handler : IRequestHandler<UpdateUserCommand>
        {
            private readonly IUserRepository _userRepository;

            public Handler(IUserRepository userRepository)
            {
                _userRepository = userRepository;
            }

            public async Task Handle(UpdateUserCommand request, CancellationToken cancellationToken)
            {
                var user = await _userRepository.GetByIdAsync(request.Id, cancellationToken);
                user.UpdateProfileInfos(request.Bio, request.ProfileImageUrl, request.Gender);

                await _userRepository.UpdateAsync(user, cancellationToken);
            }
        }
    }
}

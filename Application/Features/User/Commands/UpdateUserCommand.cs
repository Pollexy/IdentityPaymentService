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
    public class UpdateUserCommand: IRequest
    {
        public UpdateUserCommand(Guid id, string firstName, string lastName,string fullname, string phone,string email,GenderType? genderType=null,decimal? weight,decimal? height)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Fullname = fullname;
            Email = email;
            Phone = phone;
            Gender = genderType;
            Weight = weight;
            Height = height;
        }
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Fullname { get; set; }
        public string Email { get; set; }  
        public string Phone { get; set; }
        public GenderType? Gender { get; set; }
        public decimal? Weight { get; set;} 
        public decimal? Height { get; set; }

        public class Handler : IRequestHandler<UpdateUserCommand>
        {
            private readonly IUserRepository _userRepository;
            public Handler(IUserRepository userRepository)
            {
                _userRepository = userRepository;
            }

            public async Task Handle(UpdateUserCommand request, CancellationToken cancellationToken)
            {
               //TODO var user = await _userRepository.GetByIdAsync(request.Id, cancellationToken);

                user.FirstName = request.FirstName;
                user.LastName = request.LastName;
                user.Email = request.Email;
                user.UpdateProfileInfos(request.Bio, request.ProfileImageUrl, request.Gender);

                await _userRepository.UpdateAsync(user, cancellationToken);
            }
        }

    }
}

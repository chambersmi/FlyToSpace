using API.Application.DTOs;
using API.Domain.Entities;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<RegisterUserDto, ApplicationUser>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email));

            CreateMap<ApplicationUser, UserDto>()
                .ForMember(dest => dest.State, opt => opt.MapFrom(src => src.State.ToString()));

            CreateMap<UpdateUserDto, ApplicationUser>();
        }
    }
}



// Saving if ever needed
//.ForMember(dest => dest.State, opt => opt.MapFrom(src => src.State));
//.ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))  

//.ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
//.ForMember(dest => dest.MiddleName, opt => opt.MapFrom(src => src.MiddleName))
//.ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
//.ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.DateOfBirth))
//.ForMember(dest => dest.StreetAddress1, opt => opt.MapFrom(src => src.StreetAddress1))
//.ForMember(dest => dest.StreetAddress2, opt => opt.MapFrom(src => src.StreetAddress2))
//.ForMember(dest => dest.City, opt => opt.MapFrom(src => src.City))
//.ForMember(dest => dest.ZipCode
//, opt => opt.MapFrom(src => src.ZipCode));
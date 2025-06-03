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

            // Tours
            CreateMap<CreateTourDto, Tour>();
            CreateMap<UpdateTourDto, Tour>();
            CreateMap<Tour, TourDto>();

            // Bookings
            CreateMap<CreateItineraryDto, Itinerary>();
            CreateMap<UpdateItineraryDto, Itinerary>();

            CreateMap<Itinerary, BookingDto>()
                .ForMember(dest => dest.TourName, opt => opt.MapFrom(src => src.Tour.TourName));

            CreateMap<BookingDto, Itinerary>()
                .ForMember(dest => dest.Tour, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore());
        }
    }
}
using System.Linq;
using AutoMapper;
using Udemy.API.DTOs;
using Udemy.API.Models;
namespace Udemy.API.Helpers {
    public class AutoMapperProfiles : Profile {
        public AutoMapperProfiles () {
            CreateMap<User, UserForListDto> ()
                .ForMember (dest => dest.PhotoUrl, opt => opt.MapFrom (src => src.Photos.FirstOrDefault (p => p.IsMain).Url))
                .ForMember (dest => dest.Age, opt => opt.MapFrom (src => src.DateOfBirth.CalculateAge ()));

            CreateMap<User, UserForDetailsDto> ()
                .ForMember (dest => dest.PhotoUrl, opt => opt.MapFrom (src => src.Photos.FirstOrDefault (p => p.IsMain).Url))
                .ForMember (dest => dest.Age, opt => opt.MapFrom (src => src.DateOfBirth.CalculateAge ()));

            CreateMap<UserForUpdateDto, User> ();
            CreateMap<Photo, UserPhotoDto> ();
            CreateMap<Photo, PhotoForReturnDto>();
            CreateMap<PhotoForCreationDto, Photo>();
        }
    }
}
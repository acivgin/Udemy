using AutoMapper;
using Udemy.API.DTOs;
using Udemy.API.Models;

namespace Udemy.API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<User, UserForListDto>();
            CreateMap<User, UserForDetailsDto>();
            CreateMap<Photo, UserPhotoDto>();
        }
    }
}
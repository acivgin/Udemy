using Microsoft.AspNetCore.Http;
using System;

namespace Udemy.API.DTOs
{
    public class PhotoForCreationDto
    {
        public PhotoForCreationDto()
        {
            Added = DateTime.Now;
        }
        public string Url { get; set; }
        public IFormFile File { get; set; }
        public string Description { get; set; }
        public DateTime Added { get; set; }
        public string PublicId { get; set; }
    }
}
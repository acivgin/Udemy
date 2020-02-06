using System;

namespace Udemy.API.DTOs
{
    public class UserPhotoDto
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public string Descriptioon { get; set; }
        public DateTime Added { get; set; }
        public bool IsMain { get; set; }
    }
}
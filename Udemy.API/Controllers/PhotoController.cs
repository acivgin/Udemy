using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Udemy.API.Data;
using Udemy.API.DTOs;
using Udemy.API.Helpers;
using Udemy.API.Models;

namespace Udemy.API.Controllers
{

    [Authorize]
    [Route ("api/users/{userId}/photos")]
    [ApiController]
    public class PhotoController : ControllerBase {
        private readonly IDatingRepository repo;
        private readonly IMapper _mapper;
        private readonly IOptions<CloudinarySettings> _cloudinaryConfig;
        private Cloudinary _cloudinary;

        public PhotoController (IDatingRepository repo, IMapper mapper, IOptions<CloudinarySettings> cloudinaryConfig) {
            this._cloudinaryConfig = cloudinaryConfig;
            this._mapper = mapper;
            this.repo = repo;
            Account account = new Account (
                _cloudinaryConfig.Value.CloudName,
                _cloudinaryConfig.Value.ApiKey,
                _cloudinaryConfig.Value.ApiSecret
            );

            _cloudinary = new Cloudinary (account);
        }

        [HttpGet ("{id}", Name = "GetPhoto")]
        public async Task<IActionResult> GetPhoto (int id) {

            var photoFromRepo = await repo.GetPhoto (id);
            var photo = _mapper.Map<PhotoForReturnDto> (photoFromRepo);
            return Ok (photo);
        }

        [HttpPost ()]
        public async Task<IActionResult> AddPhotoForUser (int userId, [FromForm] PhotoForCreationDto photoModel) {

            //Check the id of logged in user
            if (userId != int.Parse (User.FindFirst (ClaimTypes.NameIdentifier).Value))
                return Unauthorized ();

            //Get the user to store the photo
            var user = await repo.GetUser (userId);

            var file = photoModel.File;
            var uploadResult = new ImageUploadResult ();

            if (file.Length <= 0) {
                return BadRequest ("There is no File");
            }

            //If there is a file, create some options and upload to cloudinary
            using (var stream = file.OpenReadStream ()) {
                var uploadParams = new ImageUploadParams {
                File = new FileDescription (file.Name, stream),
                Transformation = new Transformation ().Width (500).Height (500).Crop ("fill").Gravity ("face")
                };

                uploadResult = _cloudinary.Upload (uploadParams);
            }

            //Populate our model
            photoModel.Url = uploadResult.Uri.ToString ();
            photoModel.PublicId = uploadResult.PublicId;

            //Map to the Photo
            var photo = _mapper.Map<Photo> (photoModel);

            //First image should be Main photo
            if (!user.Photos.Any (u => u.IsMain)) {
                photo.IsMain = true;
            }

            //Add to the user's Photos collection
            user.Photos.Add (photo);

            if (await repo.SaveAll ()) {
                //If photo uploaded successfuly create a location for redirection
                var photoToReturn = _mapper.Map<PhotoForReturnDto> (photo);
                return CreatedAtRoute ("GetPhoto", new { userId = userId, id = photo.Id }, photoToReturn);
            }
            return BadRequest ("Could not add the photo");
        }

        [HttpPost ("{id}/setMain")]
        public async Task<IActionResult> SetMainPhotoForUser (int userId, int id) {

            //Check the id of logged in user
            if (userId != int.Parse (User.FindFirst (ClaimTypes.NameIdentifier).Value))
                return Unauthorized ();

            //Get the user to store the photo
            var user = await repo.GetUser (userId);

            //Photo with this Id doesn't exist
            if (!user.Photos.Any (p => p.Id == id)) {
                return Unauthorized ();
            }

            //Photo that user wants to mark as Main photo
            var photoFromRepo = await repo.GetPhoto (id);

            //Get main photo for the user and unset main flag
            var mainPhoto = await repo.GetMainPhoto (userId);
            if (mainPhoto != null)
                mainPhoto.IsMain = false;

            //Set as Main Photo
            photoFromRepo.IsMain = true;

            if (await repo.SaveAll ())
                return NoContent ();

            return BadRequest ("Could not set photo to main");
        }

        [HttpDelete ("{id}")]
        public async Task<IActionResult> DeletePhoto (int userId, int id) {

            //Check the id of logged in user
            if (userId != int.Parse (User.FindFirst (ClaimTypes.NameIdentifier).Value))
                return Unauthorized ();

            //Get the user to store the photo
            var user = await repo.GetUser (userId);

            //Photo with this Id doesn't exist
            if (!user.Photos.Any (p => p.Id == id)) {
                return Unauthorized ();
            }

            //Photo that user wants to mark as Main photo
            var photoFromRepo = await repo.GetPhoto (id);

            if (photoFromRepo.IsMain)
                return BadRequest ("You cann't delete the Main photo");

            //If we want to delete photo from Cloudinary
            if (photoFromRepo.PublicId != null) {

                var result = _cloudinary.Destroy (new DeletionParams (photoFromRepo.PublicId));
                if (result.Result == "ok") {
                    repo.Delete (photoFromRepo);
                }
            }

            //If we want to delete photo from random user API for images
            if (photoFromRepo.PublicId == null) {
                repo.Delete (photoFromRepo);
            }

            if (await repo.SaveAll ())
                return Ok ();

            return BadRequest ("Failed to delete the photo");
        }
    }
}
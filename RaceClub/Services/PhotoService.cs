using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Options;
using RaceClub.Helper;

namespace RaceClub.Services
{
    public class PhotoService : IPhotoService
    {
        private readonly Cloudinary _cloudinary;
        private readonly IConfiguration _config;
        private readonly IConfiguration _con;

        public PhotoService(IOptions<CloudinarySettings> config, IConfiguration con)
        {
            var acc = new Account
            {
                ApiKey = config.Value.ApiKey,
                ApiSecret = config.Value.ApiSecret,
                Cloud = config.Value.CloudName,
            };
            _cloudinary = new Cloudinary(acc);
        }
           
        
        public async Task<ImageUploadResult> AddPhotoAsync(IFormFile formFile)
        {
            var uploadResult = new ImageUploadResult();
            
            if(formFile.Length > 0)
            {
                using var stream =  formFile.OpenReadStream();
                var UploadParams = new ImageUploadParams
                {
                    File = new FileDescription(formFile.FileName, stream),
                    Transformation = new Transformation().Height(500).Width(300).Crop("fill").Gravity("face")
                };
                uploadResult = await _cloudinary.UploadAsync(UploadParams);         
            }
            return uploadResult;



        }

        public Task<DeletionResult> DeletePhotoAsync(string publicId)
        {
            var deleteParams = new DeletionParams(publicId);
            var result = _cloudinary.DestroyAsync(deleteParams);

            return result;
        }
    }
}

using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using VehicleDealer.API.Controllers.Resources;
using VehicleDealer.API.Core;
using VehicleDealer.API.Core.Models;

namespace VehicleDealer.API.Controllers
{
    // /api/vehicles/1/photos
    [Route("/api/vehicles/{vehicleId}/photos")]
    public class PhotosController : Controller
    {
        private readonly PhotoSettings photoSettings;
        private readonly IHostingEnvironment _host;
        private readonly IVehicleRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public PhotosController(IHostingEnvironment host, IVehicleRepository repository, IUnitOfWork unitOfWork, IMapper mapper, IOptionsSnapshot<PhotoSettings> options)
        {
            this.photoSettings = options.Value;
            this._mapper = mapper;
            this._unitOfWork = unitOfWork;
            this._repository = repository;
            this._host = host;

        }

        [HttpPost]
        public async Task<IActionResult> Upload(int vehicleId, IFormFile file)
        {
            var vehicle = await _repository.GetVehicle(vehicleId, includeRelated: false);
            if (vehicle == null)
                return NotFound();

            if (file == null)
                return BadRequest("Null file");

            if (file.Length == 0)
                return BadRequest("Empty file");

            if (file.Length > photoSettings.MaxBytes)
                return BadRequest("Maximum file size exceeded");

            if (!photoSettings.IsSupported(file.FileName))
                return BadRequest("Invalid file type");

            if (string.IsNullOrWhiteSpace(_host.WebRootPath))
            {
                _host.WebRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            }

            var uploadsFolderPath = Path.Combine(_host.WebRootPath, "Uploads");
            if (!Directory.Exists(uploadsFolderPath))
                Directory.CreateDirectory(uploadsFolderPath);

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(uploadsFolderPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var photo = new Photo { FileName = fileName };
            vehicle.Photos.Add(photo);

            await _unitOfWork.CompleteAsync();

            return Ok(_mapper.Map<Photo, PhotoResource>(photo));
        }
    }
}
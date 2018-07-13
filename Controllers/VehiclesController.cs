using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VehicleDealer.API.Controllers.Resources;
using VehicleDealer.API.Models;
using VehicleDealer.API.Persistance;

namespace VehicleDealer.API.Controllers
{
    [Route("/api/vehicles")]
    public class VehiclesController : Controller
    {
        private readonly IMapper _mapper;
        private readonly VegaDbContext _context;
        public VehiclesController(IMapper mapper, VegaDbContext context)
        {
            this._context = context;
            this._mapper = mapper;

        }

        [HttpPost]
        public async Task<IActionResult> CreateVehicle([FromBody]VehicleResource vehicleResource)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var model = await _context.Models.FindAsync(vehicleResource.ModelId);
            if (model == null)
            {
                ModelState.AddModelError("ModelId", "Invalid modelId.");
                return BadRequest(ModelState);
            }

            // if (true)
            // {
            //     ModelState.AddModelError("key", "error");
            //     return BadRequest(ModelState);
            // }

            var vehicle = _mapper.Map<VehicleResource, Vehicle>(vehicleResource);
            vehicle.LastUpdate = DateTime.Now;

            _context.Vehicles.Add(vehicle);
            await _context.SaveChangesAsync();

            var result = _mapper.Map<Vehicle, VehicleResource>(vehicle);

            return Ok(result);
        }
    }
}
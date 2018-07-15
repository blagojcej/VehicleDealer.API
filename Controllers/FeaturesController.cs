using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VehicleDealer.API.Controllers.Resources;
using VehicleDealer.API.Core.Models;
using VehicleDealer.API.Persistance;

namespace VehicleDealer.API.Controllers
{
    public class FeaturesController : Controller
    {
        private readonly VegaDbContext _context;
        private readonly IMapper _mapper;
        public FeaturesController(VegaDbContext context, IMapper mapper)
        {
            this._mapper = mapper;
            this._context = context;
        }

        [HttpGet("/api/features")]
        public async Task<IEnumerable<KeyValuePairResource>> GetFeatures()
        {
            var features= await _context.Features.ToListAsync();

            return _mapper.Map<List<Feature>, List<KeyValuePairResource>>(features);
        }
    }
}
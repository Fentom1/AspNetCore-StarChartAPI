using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StarChart.Data;
using StarChart.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace StarChart.Controllers
{
    [Route("")]
    [ApiController]
    public class CelestialObjectController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CelestialObjectController(ApplicationDbContext context)
        {
            this._context = context;
        }

        // is called by 'https://localhost:44360/5'
        [HttpGet("{id:int}", Name ="GetById")]
        public IActionResult GetById(int id)
        {
            var celesticalObject = _context.CelestialObjects.Find(id);
            if (celesticalObject == null)
            {
                return NotFound();
            }
            celesticalObject.Satellites = _context.CelestialObjects.Where(co => co.OrbitedObjectId == celesticalObject.Id).ToList();
            return Ok(celesticalObject);
        }

        // is called by 'https://localhost:44360/Sun'
        [HttpGet("{name}")]
        public IActionResult GetByName(string name)
        {
            var celesticalsObjects = _context.CelestialObjects.Where(co => co.Name == name).ToList();
            if (celesticalsObjects.Count == 0)
            {
                return NotFound();
            }

            foreach (var celesticalObject in celesticalsObjects)
            {
                celesticalObject.Satellites = _context.CelestialObjects.Where(co => co.OrbitedObjectId == celesticalObject.Id).ToList();
            }

            return Ok(celesticalsObjects);
        }

        // is called by 'https://localhost:44360/'
        [HttpGet]
        public IActionResult GetAll()
        {
            var celesticalsObjects = _context.CelestialObjects.ToList();

            foreach (var celesticalObject in celesticalsObjects)
            {
                celesticalObject.Satellites = _context.CelestialObjects.Where(co => co.OrbitedObjectId == celesticalObject.Id).ToList();
            }

            return Ok(celesticalsObjects);
        }
    }
}

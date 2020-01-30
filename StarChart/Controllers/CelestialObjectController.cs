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

        [HttpPost]
        public IActionResult Create([FromBody]CelestialObject newObject)
        {
            _context.Add(newObject);
            _context.SaveChanges();
            return CreatedAtRoute("GetById", new { id = newObject.Id}, newObject);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, CelestialObject objToUpdate)
        {
            var orgObject = _context.CelestialObjects.Find(id);
            if (orgObject == null)
            {
                return NotFound();
            }

            orgObject.Name = objToUpdate.Name;
            orgObject.OrbitedObjectId = objToUpdate.OrbitedObjectId;
            orgObject.OrbitalPeriod = objToUpdate.OrbitalPeriod;
            
            _context.Update(orgObject);
            _context.SaveChanges();

            return NoContent();
        }

        [HttpPatch("{id}/{name}")]
        public IActionResult RenameObject(int id, string name)
        {
            var orgObject = _context.CelestialObjects.Find(id);
            if (orgObject == null)
            {
                return NotFound();
            }

            orgObject.Name = name;

            _context.Update(orgObject);
            _context.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var orgObject = _context.CelestialObjects.Find(id);
            if (orgObject == null)
            {
                return NotFound();
            }

            _context.Remove(orgObject);
            _context.SaveChanges();

            return NoContent();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using SPA_Angular_CRUD.Data;
using SPA_Angular_CRUD.Models;

namespace SPA_Angular_CRUD.Controllers
{
    public class PositionsController : ApiController
    {
        private AppDbContext db = new AppDbContext();

        // GET: api/Positions
        public IQueryable<Position> Get()
        {
            return db.Positions;
        }

        // GET: api/Positions/5
        [ResponseType(typeof(Position))]
        public IHttpActionResult Get(int id)
        {
            Position position = db.Positions.Find(id);
            if (position == null)
            {
                return NotFound();
            }

            return Ok(position);
        }

        // PUT: api/Positions/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Put(int id, Position position)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != position.Id)
            {
                return BadRequest();
            }

            db.Entry(position).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PositionExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Positions
        [ResponseType(typeof(Position))]
        public IHttpActionResult Post(Position position)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Positions.Add(position);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = position.Id }, position);
        }

        // DELETE: api/Positions/5
        [ResponseType(typeof(Position))]
        public IHttpActionResult Delete(int id)
        {
            Position position = db.Positions.Find(id);
            if (position == null)
            {
                return NotFound();
            }

            db.Positions.Remove(position);
            db.SaveChanges();

            return Ok(position);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PositionExists(int id)
        {
            return db.Positions.Count(e => e.Id == id) > 0;
        }
    }
}
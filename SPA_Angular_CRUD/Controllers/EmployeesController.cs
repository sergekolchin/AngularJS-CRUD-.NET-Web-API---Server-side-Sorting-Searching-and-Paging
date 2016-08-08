using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Dynamic;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using SPA_Angular_CRUD.Data;
using SPA_Angular_CRUD.Models;
using System.Globalization;
using System.Collections.Generic;

namespace SPA_Angular_CRUD.Controllers
{
    public class EmployeesController : ApiController
    {
        private AppDbContext db = new AppDbContext();

        // GET: api/Employees
        public DataWrapper<Employee> Get(
            int currentPage = 1,
            int pageSize = 10,
            string sortBy = "Id",
            bool reverse = false,
            string search = null)
        {
            TextInfo ti = CultureInfo.CurrentCulture.TextInfo;
            IQueryable<Employee> employees = null;
            
            // searching
            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.ToLower();
                employees = db.Employees.Include(x => x.Position).Where(x =>
                x.FirstName.ToLower().Contains(search) ||
                x.LastName.ToLower().Contains(search) ||
                x.Email.ToLower().Contains(search) ||
                x.Position.Name.ToLower().Contains(search));
            } else
            {
                employees = db.Employees.Include(x => x.Position);
            }

            var totalItems = employees.Count();
            // sorting (done with the System.Linq.Dynamic NuGet library)
            employees = employees.OrderBy(ti.ToTitleCase(sortBy + (reverse ? " descending" : "")));


            //paging
            employees = employees.Skip((currentPage - 1) * pageSize).Take(pageSize);

            return new DataWrapper<Employee> { TotalItems = totalItems, Data = employees };
        }

        // GET: api/Employees/5
        [ResponseType(typeof(Employee))]
        public IHttpActionResult Get(int id)
        {
            Employee employee = db.Employees.Include(x => x.Position).SingleOrDefault(x => x.Id == id);
            if (employee == null)
            {
                return NotFound();
            }

            return Ok(employee);
        }

        // PUT: api/Employees/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Put(int id, Employee employee)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != employee.Id)
            {
                return BadRequest();
            }

            db.Entry(employee).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeExists(id))
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

        // POST: api/Employees
        [ResponseType(typeof(Employee))]
        public IHttpActionResult Post(Employee employee)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Employees.Add(employee);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = employee.Id }, employee);
        }

        // DELETE: api/Employees/5
        [ResponseType(typeof(Employee))]
        public IHttpActionResult Delete(int id)
        {
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return NotFound();
            }

            db.Employees.Remove(employee);
            db.SaveChanges();

            return Ok(employee);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool EmployeeExists(int id)
        {
            return db.Employees.Count(e => e.Id == id) > 0;
        }
    }
}
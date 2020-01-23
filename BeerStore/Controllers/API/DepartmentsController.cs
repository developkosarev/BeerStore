using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Authorization;

using BeerStore.Helpers;
using BeerStore.DAL.EF;
using BeerStore.Models.Entities;

namespace BeerStore.Controllers.Api
{
    [Authorize]
    [EnableCors("AllowAllOrigin")]
    [Produces("application/json")]
    [Route("api/[controller]")]    
    public class DepartmentsController : ControllerBase
    {
        private readonly StoreContext _context;

        public DepartmentsController(StoreContext context)
        {
            _context = context;
        }

        // GET: api/Departments
        [Authorize(Roles = "Admins, AdminsApi, Users")]
        [HttpGet]
        public PagesListResult<Department> GetDepartments(string filter, int? pageNumber, int? pageSize, bool showIsMark = false)
        {
            var itemsPerPage = pageSize.HasValue ? pageSize.Value : 50;
            var currentPageNum = pageNumber.HasValue ? pageNumber.Value : 1;
            
            var qry = _context.Departments.AsNoTracking().AsQueryable();
            
            if (!string.IsNullOrWhiteSpace(filter))
            {
                qry = qry.Where(c => c.Descr.StartsWith(filter));
            }

            if (!showIsMark)
            {
                qry = qry.Where(c => c.IsMark == false);
            }

            qry = qry.OrderBy(c => c.Descr);

            return PagesListResult<Department>.Create(qry, currentPageNum, itemsPerPage);
        }

        // GET: api/Departments/5
        [Authorize(Roles = "Admins, AdminsApi, Users")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDepartment([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var department = await _context.Departments.FindAsync(id);

            if (department == null)
            {
                return NotFound();
            }

            return Ok(department);
        }

        // POST: api/Departments
        [Authorize(Roles = "Admins, AdminsApi")]
        [HttpPost]
        public async Task<IActionResult> PostDepartment([FromBody] Department department)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }            

            Department departmentdb = _context.Departments.FirstOrDefault(c => c.Code == department.Code);
            if (departmentdb != null)
            {
                departmentdb.Descr = department.Descr;
                departmentdb.Version = department.Version;
                //departmentdb.IsMark = department.IsMark;

                await _context.SaveChangesAsync();

                return CreatedAtAction("GetDepartment", new { id = departmentdb.DepartmentId }, departmentdb);
            }
            else
            {
                _context.Departments.Add(department);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetDepartment", new { id = department.DepartmentId }, department);
            }            
        }
                
    }
}
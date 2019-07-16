using JwtAuthApp.Server.Contexts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace JwtAuthApp.Server.Controllers
{
    [Authorize(Policy = "ApiUser")]
    [Route("api/[controller]/[action]")]
    public class DashBoardController : Controller
    {
        private readonly ClaimsPrincipal _caller;
        private readonly ApplicationDbContext _dbContext;

        public DashBoardController(ApplicationDbContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            _caller = httpContextAccessor.HttpContext.User;
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> Dashboard()
        {
            var userId = _caller.Claims.Single(c => c.Type == "id");
            var customer = await _dbContext.Customers.Include(c => c.Identity).SingleAsync(c => c.IdentityId == userId.Value);

            return new OkObjectResult(new
            {
                Message = "success.",
                customer.Identity.FirstName,
                customer.Identity.LastName,
                customer.Location,
                customer.Locale,
                customer.Gender
            });
        }
    }
}
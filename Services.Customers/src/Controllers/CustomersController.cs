using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Services.Customers.Data;
using Swashbuckle.AspNetCore.Annotations;

namespace Services.Customers.Controllers
{
    [Route("api/customers")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly CustomerDBContext _dbContext;

        public CustomersController(CustomerDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get Customer By Id")]
        public async Task<ActionResult> Get(Guid id)
        {
            return Ok(await _dbContext.Customers.FindAsync(id));
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Get All Customers")]
        public async Task<ActionResult> Get()
        {
            return Ok(await _dbContext.Customers.ToListAsync());
        }
    }
}
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Services.Customers.Data;
using Swashbuckle.AspNetCore.Annotations;

namespace Services.Orders.Controllers
{
    [Route("api/orders")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly OrderDBContext _dbContext;

        public OrdersController(OrderDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get Order By Id")]
        public async Task<ActionResult> Get(Guid id)
        {
            return Ok(await _dbContext.Orders.Include(i => i.Items).FirstOrDefaultAsync(s => s.Id == id));
        }

        [HttpGet("customerId/{id}")]
        [SwaggerOperation(Summary = "Get Orders By Customer Id")]
        public async Task<ActionResult> GetOrdersByCustomerId(Guid id)
        {
            return Ok(await _dbContext.Orders.Where(s => s.CustomerId == id).ToListAsync());
        }
    }
}

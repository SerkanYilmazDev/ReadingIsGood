using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Services.Customers.Commands;
using Services.Customers.Data;

namespace Services.Customers.Controllers
{
    [Route("api/baskets")]
    [ApiController]
    public class BasketsController : ControllerBase
    {
        private readonly CustomerDBContext _dbContext;
        private readonly IMediator _mediator;

        public BasketsController(CustomerDBContext dbContext,
            IMediator mediator)
        {
            _dbContext = dbContext;
            _mediator = mediator;
        }

        [HttpGet("{customerId}")]
        public async Task<ActionResult> Get(Guid customerId)
        {
            return Ok(await _dbContext.Baskets
                            .Include(i => i.Items)
                            .AsNoTracking()
                            .FirstOrDefaultAsync(s => s.CustomerId == customerId));
        }

        [HttpPost]
        public async Task<ActionResult> AddBookToBasket(AddBookToBasketCommand command)
        {
            return Ok(await _mediator.Send(command));
        }
    }
}
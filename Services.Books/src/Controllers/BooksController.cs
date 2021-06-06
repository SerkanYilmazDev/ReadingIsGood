using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Services.Customers.Data;
using Services.Books.Commands;
using Swashbuckle.AspNetCore.Annotations;

namespace Services.Books.Controllers
{
    [Route("api/books")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly BookDBContext _dbContext;
        private readonly IMediator _mediator;

        public BooksController(BookDBContext dbContext,
            IMediator mediator)
        {
            _dbContext = dbContext;
            _mediator = mediator;
        }

        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get Book By Id")]
        public async Task<ActionResult> Get(Guid id)
        {
            return Ok(await _dbContext.Books.FindAsync(id));
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Get All Books")]
        public async Task<ActionResult> GetList()
        {
            return Ok(await _dbContext.Books.ToListAsync());
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Create Book")]
        public async Task<ActionResult> CreateBook(CreateBookCommand command)
        {
            return Ok(await _mediator.Send(command));
        }
    }
}

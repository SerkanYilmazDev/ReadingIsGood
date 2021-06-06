using Shared.RabbitMq;
using Microsoft.AspNetCore.Mvc;
using Api.Controllers.Dtos;

namespace Api.Controllers
{
    [ApiController]
    public class BaseController : ControllerBase
    {
        protected readonly IBusPublisher BusPublisher;
        public BaseController(IBusPublisher busPublisher)
        {
            BusPublisher = busPublisher;
        }

        protected UserClaimDto CurrentUser
        {
            get
            {
                return HttpContext.Items["CurrentUser"] != null ?
                    HttpContext.Items["CurrentUser"] as UserClaimDto : null;
            }
        }
    }
}
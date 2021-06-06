using System;
using System.Threading.Tasks;
using Services.Orders.Dtos;

namespace Services.Orders.HttpServices
{
    public interface ICustomerHttpService
    {
        Task<BasketDto> GetBasket(Guid CustomerId);
    }
}
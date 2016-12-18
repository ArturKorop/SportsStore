using SportsStore.Domain.Entities;

namespace SportsStore.Domain.Interfaces
{
    public interface IOrderProcessor
    {
        void ProcessOrder(Cart cart, ShippingDetails shippingDetails);
    }
}
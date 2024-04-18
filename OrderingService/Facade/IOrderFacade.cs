namespace OrderingService.Facade
{
    public interface IOrderFacade
    {
        public OrderResponse PlaceOrder(OrderRequest order);
    }
}

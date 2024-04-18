namespace OrderingService.Database
{
    public interface IOrderRepository
    {
        public int PlaceOrder(OrderRequest orderRequest);
    }
}

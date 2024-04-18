namespace OrderingService.Database
{
    public class OrderRepository : IOrderRepository
    {
        public OrderRepository() { }

        public int PlaceOrder(OrderRequest orderRequest)
        {
            try
            {
                throw new NotImplementedException("Should build a DB on start up.");
            } catch
            {
                throw new Exception("Custom DB exceptions will be thrown depending on the type of error for context");
            }
        }
    }
}

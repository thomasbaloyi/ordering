using Docker.DotNet;

namespace OrderingService.Database
{
    public class OrderRepository : IOrderRepository
    {
        private DockerClient _dockerClient;
        private ILogger<OrderRepository> _logger;

        public OrderRepository(ILogger<OrderRepository> logger)
        {
            _logger = logger;
            _dockerClient = new DockerClientConfiguration().CreateClient();
        }

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

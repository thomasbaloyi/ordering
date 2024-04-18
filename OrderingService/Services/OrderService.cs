using Grpc.Core;
using OrderingService.Facade;

namespace OrderingService.Services
{
    public class OrderService : Ordering.OrderingBase
    {
        private readonly ILogger<OrderService> _logger;
        private readonly IOrderFacade _orderFacade;

        public OrderService(ILogger<OrderService> logger, IOrderFacade orderFacade)
        {
            _logger = logger;
            _orderFacade = orderFacade;
        }

        public override Task<OrderResponse> PlaceOrder(OrderRequest order, ServerCallContext context)
        {
            _logger.LogInformation("Processing the following order: " + order.ToString() + "\n");
            try
            {
                return Task.FromResult(_orderFacade.PlaceOrder(order));
            } catch (Exception ex)
            {
                return Task.FromResult(new OrderResponse { Message = $"Failed to place an order for {order.Description}. Error: {ex.Message}"});
            }            
        }
    }
}

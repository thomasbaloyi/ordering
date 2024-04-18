using OrderingService.Database;
using OrderingService.Models;

namespace OrderingService.Facade
{
    public class OrderFacade : IOrderFacade
    {
        private IOrderRepository _orderRepository;
        private ILogger<OrderFacade> _logger;

        public OrderFacade(IOrderRepository orderRepository, ILogger<OrderFacade> logger) 
        {
            _orderRepository = orderRepository;
            _logger = logger;
        }

        public OrderResponse PlaceOrder(OrderRequest order)
        {

            int orderNumber;
            
            try
            {
                orderNumber = _orderRepository.PlaceOrder(order);
            } catch (Exception ex)
            {
                string message = $"Something went wrong in the DB: {ex.Message}.\n";
                _logger.LogError(message);
                throw new Exception(message);
            }

            bool isSuccessful = PublishToKafka(new OrderEvent());

            if (!isSuccessful)
            {
                string message = "Something went wrong pushing to kafka.\n";
                _logger.LogCritical(message);
                throw new Exception();
            }

            return new OrderResponse { Message = "Successfully created order" };
        }

        private bool PublishToKafka(OrderEvent orderEvent)
        {
            throw new NotImplementedException();
        }


    }
}

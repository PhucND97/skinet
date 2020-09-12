using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Core.Entities.OrderAggregate;
using Core.Interfaces;
using Core.Specifications;

namespace Infrastructure.Services
{
    public class OrderSerivce : IOrderService
    {
        private readonly IBasketRepo _basketRepo;
        private readonly IUnitOfWork _unitOfWork;
        public OrderSerivce(IBasketRepo basketRepo, IUnitOfWork unitOfWork, IPaymentService paymentService)
        {
            _paymentService = paymentService;
            _unitOfWork = unitOfWork;
            _basketRepo = basketRepo;
        }
        private readonly IPaymentService _paymentService;

        public async Task<Order> CreateOrderAsync(string buyerEmail, int deliveryMethod, string basketId, Address shippingAddress)
        {
            // get basket
            var basket = await _basketRepo.GetBasketAsync(basketId);
            // get products 
            var items = new List<OrderItem>();
            foreach (var item in basket.Items)
            {
                var productItem = await _unitOfWork.Repository<Product>().GetByIdAsync(item.Id);
                var itemOrdered = new ProductItemOrdered(productItem.Id, productItem.Name, productItem.PictureUrl);
                var orderItem = new OrderItem(itemOrdered, productItem.Price, item.Quantity);
                items.Add(orderItem);
            }

            // get delivery method
            var delivery = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(deliveryMethod);

            // calc subtotal
            decimal subtotal = items.Sum(item => item.Quantity * item.Price);

            // check if order with the same paymentIntentId exist
            var spec = new OrderByPaymentIntentIdSpecification(basket.PaymentIntentId);
            var orderToCheck = await _unitOfWork.Repository<Order>().GetBySpecification(spec);

            if (!(orderToCheck is null))
            {
                _unitOfWork.Repository<Order>().Delete(orderToCheck);
                // update to make sure order is accurate
                await _paymentService.CreateOrUpdatePaymentIntent(basket.PaymentIntentId);
            }

            // create order
            var order = new Order(items, buyerEmail, shippingAddress, delivery, subtotal, basket.PaymentIntentId);
            _unitOfWork.Repository<Order>().Add(order);

            // save to db
            var result = await _unitOfWork.Complete();

            if (result <= 0)
            {
                return null;
            }

            // delete basket
            // await _basketRepo.DeleteBasketAsync(basketId);

            return order;
        }

        public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync()
        {
            return (IReadOnlyList<DeliveryMethod>)await _unitOfWork.Repository<DeliveryMethod>().GetAllAsync();
        }

        public async Task<Order> GetOrderByIdAsync(int id, string buyerEmail)
        {
            var spec = new OrderWithOrderItemAndDeliveryMethodSpecification(id, buyerEmail);
            var order = await _unitOfWork.Repository<Order>().GetBySpecification(spec);

            return order;
        }

        public async Task<IReadOnlyList<Order>> GetOrdersAsync(string buyerEmail)
        {
            var spec = new OrderWithOrderItemAndDeliveryMethodSpecification(buyerEmail);
            var orders = await _unitOfWork.Repository<Order>().GetAllBySpecAsync(spec);

            return (IReadOnlyList<Order>)orders;
        }
    }
}
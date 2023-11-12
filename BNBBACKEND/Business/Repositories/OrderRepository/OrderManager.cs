using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Repositories.OrderRepository;
using Entities.Concrete;
using Business.Aspects.Secured;
using Core.Aspects.Validation;
using Core.Aspects.Caching;
using Core.Aspects.Performance;
using Business.Repositories.OrderRepository.Validation;
using Business.Repositories.OrderRepository.Constants;
using Core.Utilities.Result.Abstract;
using Core.Utilities.Result.Concrete;
using DataAccess.Repositories.OrderRepository;
using Entities.Dtos;
using Business.Repositories.OrderDetailRepository;
using Business.Repositories.BasketRepository;

namespace Business.Repositories.OrderRepository
{
    public class OrderManager : IOrderService
    {
        private readonly IOrderDal _orderDal;
        private readonly IOrderDetailService _orderdetailservice;
        private readonly IBasketService _basketservicel;

        public OrderManager(IOrderDal orderDal, IOrderDetailService orderdetailservice, IBasketService basketservicel)
        {
            _orderDal = orderDal;
            _orderdetailservice = orderdetailservice;
            _basketservicel = basketservicel;
        }

        //[SecuredAspect()]
        [ValidationAspect(typeof(OrderValidator))]
        [RemoveCacheAspect("IOrderService.Get")]

        public async Task<IResult> Add(int customerid)
        {
            var baskets =await _basketservicel.GetListByCustomerId(customerid);
            //mevcut sipariþ için yeni numara aldým
            string newordernumber = _orderDal.GetOrderNumber();
            //Siparii oluþturdum
            Order order = new()
            {  
                Id = 0,
                CustomerId = baskets.Data[0].CustomerId,
                OrderNumber = newordernumber,
                Status = "Onay Bekliyor",
                Date = DateTime.Now,
            };

          await  _orderDal.Add(order);

            //Sipariþ Kalemleri oluþturuldu
            foreach(var basket in baskets.Data)
            {
                OrderDetail orderDetail = new()
                {
                    Id = 0,
                    OrderId = order.Id,
                    Price = basket.Price,
                    ProductId = basket.ProductId,
                    Quantity = basket.Quantity,
                };
                await _orderdetailservice.Add(orderDetail);
                //MEVCUT SEPETTEKÝ ÜRÜNÜ  SÝLMEM GEREKÝYO
                Basket basketentity = new()
                {
                    Id = basket.Id,
                    CustomerId = basket.CustomerId,
                    Price = basket.Price,
                    Quantity = basket.Quantity,
                    ProductId = basket.ProductId,
                };
                await _basketservicel.Delete(basketentity);
            }
      
            return new SuccessResult(OrderMessages.Added);
        }

        [SecuredAspect()]
        [ValidationAspect(typeof(OrderValidator))]
        [RemoveCacheAspect("IOrderService.Get")]

        public async Task<IResult> Update(Order order)
        {
            await _orderDal.Update(order);
            return new SuccessResult(OrderMessages.Updated);
        }

        //[SecuredAspect()]
        [RemoveCacheAspect("IOrderService.Get")]

        public async Task<IResult> Delete(Order order)
        {
            //Sipariþin Detayýný Buluyorum
            var details = await _orderdetailservice.GetList(order.Id);

            foreach (var detail in details.Data)
            {
              await  _orderdetailservice.Delete(detail);

            }
            await _orderDal.Delete(order);
            return new SuccessResult(OrderMessages.Deleted);
        }

        [SecuredAspect()]
        [CacheAspect()]
        [PerformanceAspect()]
        public async Task<IDataResult<List<Order>>> GetList()
        {
            return new SuccessDataResult<List<Order>>(await _orderDal.GetAll());
        }

        [SecuredAspect()]
        public async Task<IDataResult<Order>> GetById(int id)
        {
            return new SuccessDataResult<Order>(await _orderDal.Get(p => p.Id == id));
        }

        public async Task<IDataResult<List<Order>>> GetListByCustomerId(int customerId)
        {
            return new SuccessDataResult<List<Order>>(await _orderDal.GetAll(p=>p.CustomerId==customerId));
        }

        [SecuredAspect()]
        [CacheAspect()]
        [PerformanceAspect()]
        public async Task<IDataResult<List<OrderDto>>> GetListDto()
        {
            return new SuccessDataResult<List<OrderDto>>(await _orderDal.GetListDto());
        }
    }
}

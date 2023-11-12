using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DataAccess.EntityFramework;
using Entities.Concrete;
using DataAccess.Repositories.OrderRepository;
using DataAccess.Context.EntityFramework;
using Entities.Dtos;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories.OrderRepository
{
    public class EfOrderDal : EfEntityRepositoryBase<Order, SimpleContextDb>, IOrderDal
    {
        public async Task<List<OrderDto>> GetListDto()
        {
            using (var context = new SimpleContextDb())
            {
                var result = from order in context.Orders
                             join customer in context.Customers on order.CustomerId
                             equals customer.Id
                             select new OrderDto
                             {
                                 Id = order.Id,
                                 CustomerId = order.CustomerId,
                                 CustomeName = customer.Name,
                                 Date = order.Date,
                                 OrderNumber = order.OrderNumber,
                                 Status = order.Status,
                                 Quantity = context.OrderDetails.Where(x => x.OrderId == order.Id).Sum(s => s.Quantity),
                                 Total = context.OrderDetails.Where(x => x.OrderId == order.Id).Sum(s => s.Price) * context.OrderDetails.Where(x => x.OrderId == order.Id).Sum(s => s.Quantity),
                             };
                return await result.OrderByDescending(x=>x.Id).ToListAsync();
            }
        }

        public string GetOrderNumber()
        {
            using(var context = new SimpleContextDb())
            {
                var finlastorder = context.Orders.OrderByDescending(x=>x.Id).LastOrDefault();
              
                if (finlastorder == null)
                {
                    //16 HANELÝ OLUR
                    return "SP00000000000001";
                }
                string findlastordernumber = finlastorder.OrderNumber;
                findlastordernumber = findlastordernumber.Substring(2,14);
                int ordernumber = Convert.ToInt16(findlastordernumber);
                ordernumber++;
                string newordernumber = ordernumber.ToString();
                //Numarayý aldým Hanesini Saydýrýcagým
                //For Döngüsü ile
                for(int i = newordernumber.Length; i < 15; i++)
                {
                    newordernumber = "0"+ newordernumber;
                }
                newordernumber = "SP" + newordernumber;
                return newordernumber;
            }
        }
    }
}

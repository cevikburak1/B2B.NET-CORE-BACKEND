using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DataAccess.EntityFramework;
using Entities.Concrete;
using DataAccess.Repositories.CustomerRepository;
using DataAccess.Context.EntityFramework;
using Entities.Dtos;
using Castle.Core.Resource;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories.CustomerRepository
{
    public class EfCustomerDal : EfEntityRepositoryBase<Customer, SimpleContextDb>, ICustomerDal
    {
        public async Task<List<CustomerDto>> GetListDto()
        {
            using(var context = new SimpleContextDb())
            {
                var result = from customer in context.Customers
                             select new CustomerDto
                             {
                                 Id = customer.Id,
                                 Name = customer.Name,
                                 Email = customer.Email,
                                 PasswordHash = customer.PasswordHash,
                                 PasswordSalt = customer.PasswordSalt,
                                 Discount = (context.CustomerRelationShipses.Where(x => x.CustomerId == customer.Id) != null
                                 ? context.CustomerRelationShipses.Where(x => x.CustomerId == customer.Id).Select(s => s.Discount).FirstOrDefault() : 0),

                                 PrticeListId = (context.CustomerRelationShipses.Where(x => x.CustomerId == customer.Id) != null
                                 ? context.CustomerRelationShipses.Where(x => x.CustomerId == customer.Id).Select(s => s.PriceListId).FirstOrDefault() : 0),

                                 PrticeListName = (context.CustomerRelationShipses.Where(x => x.CustomerId == customer.Id) != null
                                 ? context.PriceLists.Where(p => p.Id == (context.CustomerRelationShipses.Where(x => x.CustomerId == customer.Id).Select(s=>s.PriceListId).FirstOrDefault())).Select(s=>s.Name).FirstOrDefault()
                                

                                 : "")
                                 
                             };
                return await result.OrderBy(p => p.Name).ToListAsync();
            }
        }

        public async Task<CustomerDto> GetDto(int id)
        {
            using (var context = new SimpleContextDb())
            {
                var result = from customer in context.Customers.Where(p=>p.Id==id)
                             select new CustomerDto
                             {
                                 Id = customer.Id, 
                                 Name = customer.Name,
                                 Email = customer.Email,
                                 PasswordHash = customer.PasswordHash,
                                 PasswordSalt = customer.PasswordSalt,
                                 Discount = (context.CustomerRelationShipses.Where(x => x.CustomerId == customer.Id) != null
                                 ? context.CustomerRelationShipses.Where(x => x.CustomerId == customer.Id).Select(s => s.Discount).FirstOrDefault() : 0),

                                 PrticeListId = (context.CustomerRelationShipses.Where(x => x.CustomerId == customer.Id) != null
                                 ? context.CustomerRelationShipses.Where(x => x.CustomerId == customer.Id).Select(s => s.PriceListId).FirstOrDefault() : 0),

                                 PrticeListName = (context.CustomerRelationShipses.Where(x => x.CustomerId == customer.Id) != null
                                 ? context.PriceLists.Where(p => p.Id == (context.CustomerRelationShipses.Where(x => x.CustomerId == customer.Id).Select(s => s.PriceListId).FirstOrDefault())).Select(s => s.Name).FirstOrDefault()


                                 : "")

                             };
                return await result.FirstOrDefaultAsync();
            }
        }
    }
}

//context.PriceLists.Where(p => p.Id == (context.CustomerRelationShipses.Where(x => x.CustomerId == customer.Id).Select(s => s.PriceListId).FirstOrDefault()).Select(s => s.Name).FirstOrDefault()

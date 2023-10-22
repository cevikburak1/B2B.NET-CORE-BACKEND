using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DataAccess.EntityFramework;
using Entities.Concrete;
using DataAccess.Repositories.PriceListDetailRepository;
using DataAccess.Context.EntityFramework;
using Entities.Dtos;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories.PriceListDetailRepository
{
    public class EfPriceListDetailDal : EfEntityRepositoryBase<PriceListDetail, SimpleContextDb>, IPriceListDetailDal
    {
        public async Task<List<PriceListDetailDto>> GetListDto(int pricelistId)
        {
           using(var context = new SimpleContextDb())
            {
                var result = from priceListDetail in context.PriceListDetails.Where(x => x.PriceListId == pricelistId)
                             join product in context.Products on priceListDetail.ProductId equals product.Id
                             select new PriceListDetailDto
                             {
                                 Id = priceListDetail.Id,
                                 Price = priceListDetail.Price,
                                 PriceListId = priceListDetail.PriceListId,
                                 ProductId = priceListDetail.ProductId,
                                 ProductName = product.Name
                             };
                return await result.OrderBy(p => p.ProductName).ToListAsync();
            }
        }
    }
}

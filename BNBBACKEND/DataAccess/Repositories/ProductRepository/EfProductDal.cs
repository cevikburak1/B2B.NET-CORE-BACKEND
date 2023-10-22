using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DataAccess.EntityFramework;
using Entities.Concrete;
using DataAccess.Repositories.ProductRepository;
using DataAccess.Context.EntityFramework;
using Entities.Dtos;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories.ProductRepository
{
    public class EfProductDal : EfEntityRepositoryBase<Product, SimpleContextDb>, IProductDal
    {
        public async Task<List<ProductListDto>> GetProductList(int customerId)
        {
            using (var context = new SimpleContextDb())
            {
                var customerrelationship = context.CustomerRelationShipses.Where(p => p.CustomerId == customerId).SingleOrDefault();
                var reslt = from x in context.Products
                            select new ProductListDto
                            {
                                Id = x.Id,
                                Name = x.Name,
                                Discount = customerrelationship.Discount,
                                Price = context.PriceListDetails.Where(x => x.PriceListId == customerrelationship.PriceListId && x.ProductId == x.Id).Count() > 0
                                ? context.PriceListDetails.Where(x => x.PriceListId == customerrelationship.PriceListId && x.ProductId == x.Id).Select(s => s.Price).FirstOrDefault()
                                : 0,
                                MainImageUrl = (context.ProductImages.Where(p => p.ProductId == x.Id && p.IsMainImage == true).Count() > 0
                               ? context.ProductImages.Where(p => p.ProductId == x.Id && p.IsMainImage == true).Select(s => s.ImageUrl).FirstOrDefault()
                               : ""),
                                Images = context.ProductImages.Where(y => y.ProductId == x.Id).Select(s=>s.ImageUrl).ToList()
                            };
                return await reslt.OrderBy(p => p.Name).ToListAsync();
            }
               
        }
        public async Task<List<ProductListDto>> GetList()
        {
            using (var context = new SimpleContextDb())
            {
       
                var reslt = from x in context.Products
                            select new ProductListDto
                            {
                                Id = x.Id,
                                Name = x.Name,
                               
                                MainImageUrl = (context.ProductImages.Where(p => p.ProductId == x.Id && p.IsMainImage == true).Count() > 0
                               ? context.ProductImages.Where(p => p.ProductId == x.Id && p.IsMainImage == true).Select(s => s.ImageUrl).FirstOrDefault()
                               : "")
                                
                            };
                return await reslt.OrderBy(p => p.Name).ToListAsync();
            }

        }
    }
}

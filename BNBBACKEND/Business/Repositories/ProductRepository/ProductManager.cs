using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Repositories.ProductRepository;
using Entities.Concrete;
using Business.Aspects.Secured;
using Core.Aspects.Validation;
using Core.Aspects.Caching;
using Core.Aspects.Performance;
using Business.Repositories.ProductRepository.Validation;
using Business.Repositories.ProductRepository.Constants;
using Core.Utilities.Result.Abstract;
using Core.Utilities.Result.Concrete;
using DataAccess.Repositories.ProductRepository;
using Entities.Dtos;
using Business.Repositories.ProductImageRepository;
using Business.Repositories.PriceListDetailRepository;
using Core.Utilities.Business;
using Business.Repositories.BasketRepository;
using Business.Repositories.OrderDetailRepository;

namespace Business.Repositories.ProductRepository
{
    public class ProductManager : IProductService
    {
        private readonly IProductDal _productDal;
        private readonly IProductImageService _productImageService;
        private readonly IPriceListDetailService _priceListDetail;
        private readonly IBasketService _basketService;
        private readonly IOrderDetailService _orderdetailService;

        public ProductManager(IProductDal productDal, IProductImageService productImageService, IPriceListDetailService priceListDetail, IBasketService basketService, IOrderDetailService orderdetailService)
        {
            _productDal = productDal;
            _productImageService = productImageService;
            _priceListDetail = priceListDetail;
            _basketService = basketService;
            _orderdetailService = orderdetailService;
        }

        //[SecuredAspect("admin,product.add")]
        [ValidationAspect(typeof(ProductValidator))]
        [RemoveCacheAspect("IProductService.Get")]

        public async Task<IResult> Add(Product product)
        {
            await _productDal.Add(product);
            return new SuccessResult(ProductMessages.Added);
        }

        //[SecuredAspect("admin,product.update")]
        [ValidationAspect(typeof(ProductValidator))]
        [RemoveCacheAspect("IProductService.Get")]

        public async Task<IResult> Update(Product product)
        {
            await _productDal.Update(product);
            return new SuccessResult(ProductMessages.Updated);
        }

        //[SecuredAspect("admin,product.delete")]
        [RemoveCacheAspect("IProductService.Get")]

        public async Task<IResult> Delete(Product product)
        {
            IResult result = BusinessRules.Run
                (
                   await ChechIfProductExisttoBaskets(product.Id),
                   await ChechIfProductExisttoOrderDetail(product.Id)

                );
            if (result != null)
            {
                return result;
            }

            var images =await _productImageService.GetListByProductId(product.Id);
            foreach(var x in images)
            {
              await  _productImageService.Delete(x);
            }

            var pricelistproducts = await _priceListDetail.GetListByProductId(product.Id);
            foreach(var y in pricelistproducts)
            {
                await _priceListDetail.Delete(y);
            }

            await _productDal.Delete(product);
            return new SuccessResult(ProductMessages.Deleted);
        }

        //[SecuredAspect("admin,product.get")]
        [CacheAspect()]
        [PerformanceAspect()]
        public async Task<IDataResult<List<ProductListDto>>> GetList()
        {
            return new SuccessDataResult<List<ProductListDto>>(await _productDal.GetList());
        }

        //[SecuredAspect("admin,product.get")]
        public async Task<IDataResult<Product>> GetById(int id)
        {
            return new SuccessDataResult<Product>(await _productDal.Get(p => p.Id == id));
        }

       

        public async Task<IResult> ChechIfProductExisttoBaskets(int productId)
        {
            var result = await _basketService.GetListByProductId(productId);
            if (result.Count() > 0)
            {
                return new ErrorResult("Ürün Sepette bulundugundan silinemez");
            }

            else
            {
                return new SuccessResult("");
            }
        }

        public async Task<IResult> ChechIfProductExisttoOrderDetail(int productId)
        {
            var result = await _orderdetailService.GetListByProductId(productId);
            if (result.Count() > 0)
            {
                return new ErrorResult("Ürünün Sipariþi Oldugundan Dolayý silinemez");
            }

            else
            {
                return new SuccessResult("");
            }
        }

        public Task<IDataResult<List<ProductListDto>>> GetProductList(int customerId)
        {
            throw new NotImplementedException();
        }
    }
}

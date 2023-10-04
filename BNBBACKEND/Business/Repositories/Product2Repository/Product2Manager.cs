using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Repositories.Product2Repository;
using Entities.Concrete;
using Business.Aspects.Secured;
using Core.Aspects.Validation;
using Core.Aspects.Caching;
using Core.Aspects.Performance;
using Business.Repositories.Product2Repository.Validation;
using Business.Repositories.Product2Repository.Constants;
using Core.Utilities.Result.Abstract;
using Core.Utilities.Result.Concrete;
using DataAccess.Repositories.Product2Repository;

namespace Business.Repositories.Product2Repository
{
    public class Product2Manager : IProduct2Service
    {
        private readonly IProduct2Dal _product2Dal;

        public Product2Manager(IProduct2Dal product2Dal)
        {
            _product2Dal = product2Dal;
        }

        [SecuredAspect()]
        [ValidationAspect(typeof(Product2Validator))]
        [RemoveCacheAspect("IProduct2Service.Get")]

        public async Task<IResult> Add(Product2 product2)
        {
            await _product2Dal.Add(product2);
            return new SuccessResult(Product2Messages.Added);
        }

        [SecuredAspect()]
        [ValidationAspect(typeof(Product2Validator))]
        [RemoveCacheAspect("IProduct2Service.Get")]

        public async Task<IResult> Update(Product2 product2)
        {
            await _product2Dal.Update(product2);
            return new SuccessResult(Product2Messages.Updated);
        }

        [SecuredAspect()]
        [RemoveCacheAspect("IProduct2Service.Get")]

        public async Task<IResult> Delete(Product2 product2)
        {
            await _product2Dal.Delete(product2);
            return new SuccessResult(Product2Messages.Deleted);
        }

        [SecuredAspect()]
        [CacheAspect()]
        [PerformanceAspect()]
        public async Task<IDataResult<List<Product2>>> GetList()
        {
            return new SuccessDataResult<List<Product2>>(await _product2Dal.GetAll());
        }

        [SecuredAspect()]
        public async Task<IDataResult<Product2>> GetById(int id)
        {
            return new SuccessDataResult<Product2>(await _product2Dal.Get(p => p.Id == id));
        }

    }
}

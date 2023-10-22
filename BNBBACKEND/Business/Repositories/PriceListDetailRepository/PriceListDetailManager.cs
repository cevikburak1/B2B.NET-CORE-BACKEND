using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Repositories.PriceListDetailRepository;
using Entities.Concrete;
using Business.Aspects.Secured;
using Core.Aspects.Validation;
using Core.Aspects.Caching;
using Core.Aspects.Performance;
using Business.Repositories.PriceListDetailRepository.Validation;
using Business.Repositories.PriceListDetailRepository.Constants;
using Core.Utilities.Result.Abstract;
using Core.Utilities.Result.Concrete;
using DataAccess.Repositories.PriceListDetailRepository;
using Entities.Dtos;
using Core.Utilities.Business;

namespace Business.Repositories.PriceListDetailRepository
{
    public class PriceListDetailManager : IPriceListDetailService
    {
        private readonly IPriceListDetailDal _priceListDetailDal;

        public PriceListDetailManager(IPriceListDetailDal priceListDetailDal)
        {
            _priceListDetailDal = priceListDetailDal;
        }

        //[SecuredAspect()]
        [ValidationAspect(typeof(PriceListDetailValidator))]
        [RemoveCacheAspect("IPriceListDetailService.Get")]

        public async Task<IResult> Add(PriceListDetail priceListDetail)
        {
            IResult result = BusinessRules.Run(await CheckIfProductExist(priceListDetail));
            if (result != null)
            {
                return result;
            }
            await _priceListDetailDal.Add(priceListDetail);
            return new SuccessResult(PriceListDetailMessages.Added);
        }

         public  async Task<IResult> CheckIfProductExist(PriceListDetail priceListDetail)
        {
            
            var result =await _priceListDetailDal.Get(p => p.PriceListId == priceListDetail.PriceListId && p.ProductId == priceListDetail.ProductId);
            if(result!=null) 
            {
                return new ErrorResult("Bu Ürün Daha Önce Eklenmiþ Var Olan ürünü Ekleymessiniz ");
            }
            return new SuccessResult();
        }

        [SecuredAspect()]
        [ValidationAspect(typeof(PriceListDetailValidator))]
        [RemoveCacheAspect("IPriceListDetailService.Get")]

        public async Task<IResult> Update(PriceListDetail priceListDetail)
        {
            await _priceListDetailDal.Update(priceListDetail);
            return new SuccessResult(PriceListDetailMessages.Updated);
        }

        //[SecuredAspect()]
        [RemoveCacheAspect("IPriceListDetailService.Get")]

        public async Task<IResult> Delete(PriceListDetail priceListDetail)
        {
            await _priceListDetailDal.Delete(priceListDetail);
            return new SuccessResult(PriceListDetailMessages.Deleted);
        }

        [SecuredAspect()]
        [CacheAspect()]
        [PerformanceAspect()]
        public async Task<IDataResult<List<PriceListDetail>>> GetList()
        {
            return new SuccessDataResult<List<PriceListDetail>>(await _priceListDetailDal.GetAll());
        }

        [SecuredAspect()]
        public async Task<IDataResult<PriceListDetail>> GetById(int id)
        {
            return new SuccessDataResult<PriceListDetail>(await _priceListDetailDal.Get(p => p.Id == id));
        }

        public async Task<List<PriceListDetail>> GetListByProductId(int id)
        {
            return await _priceListDetailDal.GetAll(p => p.Id == id);
        }

        [SecuredAspect()]
        [CacheAspect()]
        [PerformanceAspect()]
        public async Task<IDataResult<List<PriceListDetailDto>>> GetListDto(int pricelistId)
        {
            return new SuccessDataResult<List<PriceListDetailDto>>(await _priceListDetailDal.GetListDto(pricelistId));
        }
    }
}

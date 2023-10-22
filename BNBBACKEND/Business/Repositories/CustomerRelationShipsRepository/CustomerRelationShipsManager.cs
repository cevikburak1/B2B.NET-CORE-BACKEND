using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Repositories.CustomerRelationShipsRepository;
using Entities.Concrete;
using Business.Aspects.Secured;
using Core.Aspects.Validation;
using Core.Aspects.Caching;
using Core.Aspects.Performance;
using Business.Repositories.CustomerRelationShipsRepository.Validation;
using Business.Repositories.CustomerRelationShipsRepository.Constants;
using Core.Utilities.Result.Abstract;
using Core.Utilities.Result.Concrete;
using DataAccess.Repositories.CustomerRelationShipsRepository;

namespace Business.Repositories.CustomerRelationShipsRepository
{
    public class CustomerRelationShipsManager : ICustomerRelationShipsService
    {
        private readonly ICustomerRelationShipsDal _customerRelationShipsDal;

        public CustomerRelationShipsManager(ICustomerRelationShipsDal customerRelationShipsDal)
        {
            _customerRelationShipsDal = customerRelationShipsDal;
        }

        //[SecuredAspect()]
        [ValidationAspect(typeof(CustomerRelationShipsValidator))]
        [RemoveCacheAspect("ICustomerRelationShipsService.Get")]

        public async Task<IResult> Add(CustomerRelationShips customerRelationShips)
        {
            await _customerRelationShipsDal.Add(customerRelationShips);
            return new SuccessResult(CustomerRelationShipsMessages.Added);
        }

        [SecuredAspect()]
        [ValidationAspect(typeof(CustomerRelationShipsValidator))]
        [RemoveCacheAspect("ICustomerRelationShipsService.Get")]

        public async Task<IResult> Update(CustomerRelationShips customerRelationShips)
        {
            await _customerRelationShipsDal.Update(customerRelationShips);
            return new SuccessResult(CustomerRelationShipsMessages.Updated);
        }

        [SecuredAspect()]
        [RemoveCacheAspect("ICustomerRelationShipsService.Get")]

        public async Task<IResult> Delete(CustomerRelationShips customerRelationShips)
        {
            await _customerRelationShipsDal.Delete(customerRelationShips);
            return new SuccessResult(CustomerRelationShipsMessages.Deleted);
        }

        [SecuredAspect()]
        [CacheAspect()]
        [PerformanceAspect()]
        public async Task<IDataResult<List<CustomerRelationShips>>> GetList()
        {
            return new SuccessDataResult<List<CustomerRelationShips>>(await _customerRelationShipsDal.GetAll());
        }

        [SecuredAspect()]
        public async Task<IDataResult<CustomerRelationShips>> GetById(int id)
        {
            return new SuccessDataResult<CustomerRelationShips>(await _customerRelationShipsDal.Get(p => p.Id == id));
        }
        [SecuredAspect()]
        public async Task<IDataResult<CustomerRelationShips>> GetByCustomerId(int customerId)
        {
            return new SuccessDataResult<CustomerRelationShips>(await _customerRelationShipsDal.Get(p => p.CustomerId == customerId));
        }

    }
}

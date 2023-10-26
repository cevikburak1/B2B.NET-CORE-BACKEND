using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Repositories.CustomerRepository;
using Entities.Concrete;
using Business.Aspects.Secured;
using Core.Aspects.Validation;
using Core.Aspects.Caching;
using Core.Aspects.Performance;
using Business.Repositories.CustomerRepository.Validation;
using Business.Repositories.CustomerRepository.Constants;
using Core.Utilities.Result.Abstract;
using Core.Utilities.Result.Concrete;
using DataAccess.Repositories.CustomerRepository;
using Entities.Dtos;
using Core.Utilities.Hashing;
using Business.Repositories.UserRepository;
using Core.Utilities.Business;
using Core.Utilities.Security.JWT;
using DataAccess.Repositories.CustomerRelationShipsRepository;
using Business.Repositories.CustomerRelationShipsRepository;
using Business.Repositories.OrderRepository;

namespace Business.Repositories.CustomerRepository
{
    public class CustomerManager : ICustomerService
    {
        private readonly ICustomerDal _customerDal;
        private readonly ICustomerRelationShipsService _customerRelationShipsService;
        private readonly IOrderService _orderService;
        private readonly ITokenHandler _tokenHandler;


        public CustomerManager(ICustomerDal customerDal, ITokenHandler tokenHandler, ICustomerRelationShipsService customerRelationShipsService, IOrderService orderService)
        {
            _customerRelationShipsService = customerRelationShipsService;
            _customerDal = customerDal;
            _orderService = orderService;
            _tokenHandler = tokenHandler;
        }

        //[SecuredAspect()]
        [ValidationAspect(typeof(CustomerValidator))]
        [RemoveCacheAspect("ICustomerService.Get")]

        public async Task<IResult> Add(CustomerRegisterDto customerRegisterDto)
        {
            IResult result = BusinessRules.Run(
         await CheckIfEmailExists(customerRegisterDto.Email)
        
         );

            if (result != null)
            {
                return result;
            }

            byte[] passwordhash, passwordsalt;
            HashingHelper.CreatePassword(customerRegisterDto.Password, out passwordhash, out passwordsalt);
            Customer customer = new Customer()
            {
                Id = 0,
                Email = customerRegisterDto.Email,
                Name = customerRegisterDto.Name,
                PasswordHash = passwordhash,
                PasswordSalt = passwordsalt,
            };
            await _customerDal.Add(customer);
            return new SuccessResult(CustomerMessages.Added);
        }

       

        [SecuredAspect()]
        [ValidationAspect(typeof(CustomerValidator))]
        [RemoveCacheAspect("ICustomerService.Get")]

        public async Task<IResult> Update(Customer customer)
        {
            await _customerDal.Update(customer);
            return new SuccessResult(CustomerMessages.Updated);
        }

        [SecuredAspect()]
        [RemoveCacheAspect("ICustomerService.Get")]

        public async Task<IResult> Delete(Customer customer)
        {
            IResult result = BusinessRules.Run(await CheckIfCustomerOrderExists(customer.Id));
            if (result != null)
            {
                return result;
            }
            var customerRelatishonship =await _customerRelationShipsService.GetByCustomerId(customer.Id);
            if (customerRelatishonship.Data != null)
            {
                await _customerRelationShipsService.Delete(customerRelatishonship.Data);
            }
            await _customerDal.Delete(customer);
            return new SuccessResult(CustomerMessages.Deleted);
        }


        public async Task<IResult> CheckIfCustomerOrderExists(int customerId)
        {
            var result = await _orderService.GetListByCustomerId(customerId);
            if (result.Data.Count > 0)
            {
                return new ErrorResult("Sipariþi Bulunan Müþteri Kaydý Silinemez");
            }
            return new  SuccessResult();
        }



        [SecuredAspect()]
        [CacheAspect()]
        [PerformanceAspect()]
        public async Task<IDataResult<List<CustomerDto>>> GetList()
        {
            return new SuccessDataResult<List<CustomerDto>>(await _customerDal.GetListDto());
        }

        [SecuredAspect()]
        public async Task<IDataResult<Customer>> GetById(int id)
        {
            return new SuccessDataResult<Customer>(await _customerDal.Get(p => p.Id == id));
        }

        public async Task<Customer> GetByEmail(string email)
        {
            var result = await _customerDal.Get(p => p.Email == email);
            return result;
        }

        private async Task<IResult> CheckIfEmailExists(string email)
        {
            var list = await GetByEmail(email);
            if (list != null)
            {
                return new ErrorResult("Bu mail adresi daha önce kullanýlmýþ");
            }
            return new SuccessResult();
        }

        [SecuredAspect()]
        public async Task<IResult> ChangePasswordByAdmin(CustomerChangePasswordByAdminDto customerDto)
        {
            byte[] passwordHash,passwordSalt;
            HashingHelper.CreatePassword(customerDto.Password,out passwordHash,out passwordSalt);
            var customer = await _customerDal.Get(x => x.Id == customerDto.Id);
            customer.PasswordHash = passwordHash;
            customer.PasswordSalt = passwordSalt;
            await _customerDal.Update(customer);
            return new SuccessResult(CustomerMessages.Updated);
        }
    }
}

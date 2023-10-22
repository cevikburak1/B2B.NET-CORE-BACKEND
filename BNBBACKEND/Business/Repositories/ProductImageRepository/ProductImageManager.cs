using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Repositories.ProductImageRepository;
using Entities.Concrete;
using Business.Aspects.Secured;
using Core.Aspects.Validation;
using Core.Aspects.Caching;
using Core.Aspects.Performance;
using Business.Repositories.ProductImageRepository.Validation;
using Business.Repositories.ProductImageRepository.Constants;
using Core.Utilities.Result.Abstract;
using Core.Utilities.Result.Concrete;
using DataAccess.Repositories.ProductImageRepository;
using Entities.Dtos;
using Business.Concrete;
using Business.Abstract;
using Core.Utilities.Business;
using Core.Aspects.Transaction;

namespace Business.Repositories.ProductImageRepository
{
    public class ProductImageManager : IProductImageService
    {
        private readonly IProductImageDal _productImageDal;
        private readonly IFileService _fileManager;

        public ProductImageManager(IProductImageDal productImageDal, IFileService fileManager)
        {
           
            _productImageDal = productImageDal;
            _fileManager = fileManager;
        }

        [SecuredAspect()]
        [ValidationAspect(typeof(ProductImageValidator))]
        [RemoveCacheAspect("IProductImageService.Get")]

        public async Task<IResult> Add(ProductImageAddDto productImageAddDto)
        {

            foreach(var image in productImageAddDto.Images)
            {
                IResult result = BusinessRules.Run(
         CheckIfImageExtesionsAllow(image.FileName),
         CheckIfImageSizeIsLessThanOneMb(productImageAddDto.Images.Length)
         );

                if (result == null)
                {
                    string filename = _fileManager.FileSaveToServer(image, "C:/business-to-business/src/assets/img/");

                    ProductImage productImage = new ()
                    {
                        Id = 0,
                        ImageUrl = filename,
                        ProductId = productImageAddDto.ProductId,
                        IsMainImage = false
                    };
                    await _productImageDal.Add(productImage);
                }
             
            }
            return new SuccessResult(ProductImageMessages.Added);
        }

        [SecuredAspect()]
        [ValidationAspect(typeof(ProductImageValidator))]
        [RemoveCacheAspect("IProductImageService.Get")]

        public async Task<IResult> Update(ProductImageUpdateDto productImageUpdateDto)
        {
            IResult result = BusinessRules.Run(
           CheckIfImageExtesionsAllow(productImageUpdateDto.Image.FileName),
           CheckIfImageSizeIsLessThanOneMb(productImageUpdateDto.Image.Length)
           );

            if (result != null)
            {
                return result;
            }
            string path = @"./Content/img/" + productImageUpdateDto.ImageUrl;
            _fileManager.FileDeleteToServer(path);
            try
            {
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
            }
            catch (Exception)
            {

            }

            string filename = _fileManager.FileSaveToServer(productImageUpdateDto.Image, "Content/img/");

            ProductImage productImage = new ()
            {
                Id = productImageUpdateDto.Id,
                ImageUrl = filename,
                ProductId = productImageUpdateDto.ProductId,
                IsMainImage = productImageUpdateDto.IsMainImage
            };

            await _productImageDal.Update(productImage);
            return new SuccessResult(ProductImageMessages.Updated);
        }

        [SecuredAspect()]
        [RemoveCacheAspect("IProductImageService.Get")]

        public async Task<IResult> Delete(ProductImage productImage)
        {
            string path = @"C:/business-to-business/src/assets/img/" + productImage.ImageUrl;
            _fileManager.FileDeleteToServer(path);

            await _productImageDal.Delete(productImage);
            return new SuccessResult(ProductImageMessages.Deleted);
        }

        [SecuredAspect]
        [CacheAspect()]
        [PerformanceAspect()]
        public async Task<IDataResult<List<ProductImage>>> GetList()
        {
            return new SuccessDataResult<List<ProductImage>>(await _productImageDal.GetAll());
        }

        [SecuredAspect]
        [CacheAspect()]
        [PerformanceAspect()]
        public async Task<IDataResult<List<ProductImage>>> GetListByProductId(int productid)
        {
            return new SuccessDataResult<List<ProductImage>>(await _productImageDal.GetAll(p=>p.ProductId== productid));
        }


        [SecuredAspect()]
        public async Task<IDataResult<ProductImage>> GetById(int id)
        {
            return new SuccessDataResult<ProductImage>(await _productImageDal.Get(p => p.Id == id));
        }

        private IResult CheckIfImageSizeIsLessThanOneMb(long imgSize)
        {
            decimal imgMbSize = Convert.ToDecimal(imgSize * 0.000001);
            if (imgMbSize > 5)
            {
                return new ErrorResult("Yüklediðiniz resmi boyutu en fazla 5mb olmalýdýr");
            }
            return new SuccessResult();
        }

        private IResult CheckIfImageExtesionsAllow(string fileName)
        {
            var ext = fileName.Substring(fileName.LastIndexOf('.'));
            var extension = ext.ToLower();
            List<string> AllowFileExtensions = new List<string> { ".jpg", ".jpeg", ".gif", ".png" };
            if (!AllowFileExtensions.Contains(extension))
            {
                return new ErrorResult("Eklediðiniz resim .jpg, .jpeg, .gif, .png türlerinden biri olmalýdýr!");
            }
            return new SuccessResult();
        }
        //[TransactionAspect]
        [SecuredAspect()]
        [RemoveCacheAspect("IProductImageService.Get")]
        [RemoveCacheAspect("IProductService.Get")]
        public async Task<IResult> SetMainImage(int id)
        {
            var productImage =await _productImageDal.Get(x => x.Id == id);
            var productImages =await _productImageDal.GetAll(x => x.ProductId == productImage.ProductId);
            foreach(var item in productImages)
            {
                item.IsMainImage = false;
               await _productImageDal.Update(item);
            }
            productImage.IsMainImage = true;
            await _productImageDal.Update(productImage);
            return new SuccessResult(ProductImageMessages.MainIsUpdated);
        }

    }
}

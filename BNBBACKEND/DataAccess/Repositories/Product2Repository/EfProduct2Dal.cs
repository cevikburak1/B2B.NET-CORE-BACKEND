using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DataAccess.EntityFramework;
using Entities.Concrete;
using DataAccess.Repositories.Product2Repository;
using DataAccess.Context.EntityFramework;

namespace DataAccess.Repositories.Product2Repository
{
    public class EfProduct2Dal : EfEntityRepositoryBase<Product2, SimpleContextDb>, IProduct2Dal
    {
    }
}

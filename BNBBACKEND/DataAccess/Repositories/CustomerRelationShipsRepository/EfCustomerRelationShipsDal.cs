using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DataAccess.EntityFramework;
using Entities.Concrete;
using DataAccess.Repositories.CustomerRelationShipsRepository;
using DataAccess.Context.EntityFramework;

namespace DataAccess.Repositories.CustomerRelationShipsRepository
{
    public class EfCustomerRelationShipsDal : EfEntityRepositoryBase<CustomerRelationShips, SimpleContextDb>, ICustomerRelationShipsDal
    {
    }
}

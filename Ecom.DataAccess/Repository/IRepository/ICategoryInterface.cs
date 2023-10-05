using Ecom.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.DataAccess.Repository.IRepository
{
    public interface ICategoryInterface :IRepository<Category>
    {
        void Update(Category obj);

      
    }
}

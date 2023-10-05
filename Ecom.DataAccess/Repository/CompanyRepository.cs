using Ecom.DataAccess.Data;
using Ecom.DataAccess.Repository.IRepository;
using Ecom.Models.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.DataAccess.Repository
{
    public class CategoryRepository :Repository<Category>, ICategoryInterface
    {
        private readonly ApplicationDbContext _db;
     
        public CategoryRepository(ApplicationDbContext db):base(db) 
        {
            _db = db;
                 
        }


        public void Update(Category category)
        {
           _db.Categories.Update(category);
        }

       
    }
}

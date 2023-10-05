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
    public class AuthorRepository : Repository<Author>, IAuthorRepository
    {
        private readonly ApplicationDbContext _db;
     
        public AuthorRepository(ApplicationDbContext db):base(db) 
        {
            _db = db;
                 
        }


        public void Update(Author obj)
        {
           _db.Authors.Update(obj);
        }

       
    }
}

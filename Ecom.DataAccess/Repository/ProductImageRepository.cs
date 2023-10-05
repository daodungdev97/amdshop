﻿using Ecom.DataAccess.Data;
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
    public class ProductImageRepository : Repository<ProductImage>,IProductImageRepository
    {
        private readonly ApplicationDbContext _db;
     
        public ProductImageRepository(ApplicationDbContext db):base(db) 
        {
            _db = db;
                 
        }


        public void Update(ProductImage obj)
        {
           _db.ProductImages.Update(obj);
        }

       
    }
}

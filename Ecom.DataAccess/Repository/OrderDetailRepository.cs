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
    public class OrderDetailRepository : Repository<OrderDetail>, IOrderDetailRepository
	{
        private readonly ApplicationDbContext _db;
     
        public OrderDetailRepository(ApplicationDbContext db):base(db) 
        {
            _db = db;
                 
        }


        public void Update(OrderDetail obj)
        {
           _db.OrderDetails.Update(obj);
        }

       
    }
}
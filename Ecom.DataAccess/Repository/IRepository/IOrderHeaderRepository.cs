﻿using Ecom.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.DataAccess.Repository.IRepository
{
    public interface IOrderHeaderRepository : IRepository<OrderHeader>
    {
        void Update(OrderHeader obj);

        void UpdateStatus(Guid id, string orderStatus, string? paymentStatus = null);
        void UpdateStripePaymentID(Guid id, string sessionId, string paymentIntentId);


    }
}

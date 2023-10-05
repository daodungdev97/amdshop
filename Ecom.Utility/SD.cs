using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Utility
{
    public class SD
    {

        public const string Role_Customer = "Customer";
        public const string Role_CompanyStaff = "CompanyStaff";//người của công ty, không cần thanh toán ngay  ApprovedForDelayedPayment
        public const string Role_Admin = "Admin";
        public const string Role_Employee = "Employee";

        public const string StatusPending = "Pending";//chưa giải quyết
        public const string StatusApproved = "Approved";
        public const string StatusInProcess = "Processing";
        public const string StatusShipped = "Shipped";
        public const string StatusCancelled = "Cancelled";
        public const string StatusRefunded = "Refunded";

        public const string PaymentStatusPending = "Pending";
        public const string PaymentStatusApproved = "Approved";
        public const string PaymentStatusDelayedPayment = "ApprovedForDelayedPayment";
        public const string PaymentStatusRejected = "Rejected";


        public const string SessionCart = "SessionShoppingCart";
    }
}

using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Models.Models
{
	public class OrderDetail
	{
		public Guid Id { get; set; }
		[Required]
		public Guid OrderHeaderId { get; set; }
		[ForeignKey("OrderHeaderId")]
		[ValidateNever]
		public OrderHeader OrderHeader { get; set; }


		[Required]
		public Guid ProductId { get; set; }
		[ForeignKey("ProductId")]
		[ValidateNever]
		public Product Product { get; set; }

		public int Count { get; set; }
		public double Price { get; set; }
	}
}

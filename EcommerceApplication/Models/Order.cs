using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace EcommerceApplication.Models
{
	public class Order
	{
		public int OrderId { get; set; }

		public string UserId { get; set; }

		[JsonIgnore]
		public ApplicationUser User { get; set; }

		public DateTime OrderDate { get; set; }

		public decimal TotalAmount { get; set; }

		public List<OrderItem> Items { get; set; } = new List<OrderItem>();
	}
}
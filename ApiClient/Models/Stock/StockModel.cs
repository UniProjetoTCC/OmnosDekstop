using System;

namespace Omnos.Desktop.ApiClient.Models.Stock
{
    public class StockModel
    {
        public Guid Id { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string Sku { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public string Location { get; set; } = string.Empty;
        public DateTime? ExpiresAt { get; set; }
    }
}

using System;

namespace Reusables.Models
{
    public class AddProductModel
    {
        public string Name { get; set; }
        public string ProductNumber { get; set; }
        public short SafetyStockLevel { get; set; }
        public short ReorderPoint { get; set; }
        public decimal StandartCost { get; set; }
        public decimal ListPrice { get; set; }
        public string Class { get; set; }
        public string Style { get; set; }
        public int DaysToManifacture { get; set; }
        public DateTime SellStartDate { get; set; }
    }
}

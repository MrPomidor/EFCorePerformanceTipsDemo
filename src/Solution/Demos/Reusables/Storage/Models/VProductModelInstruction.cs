using System;
using System.Collections.Generic;

namespace Reusables.Storage
{
    /// <summary>
    /// Displays the content from each element in the xml column Instructions for each product in the Production.ProductModel table that has manufacturing instructions.
    /// </summary>
    public partial class VProductModelInstruction
    {
        public int ProductModelId { get; set; }
        public string Name { get; set; }
        public string Instructions { get; set; }
        public int? LocationId { get; set; }
        public decimal? SetupHours { get; set; }
        public decimal? MachineHours { get; set; }
        public decimal? LaborHours { get; set; }
        public int? LotSize { get; set; }
        public string Step { get; set; }
        public Guid Rowguid { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}

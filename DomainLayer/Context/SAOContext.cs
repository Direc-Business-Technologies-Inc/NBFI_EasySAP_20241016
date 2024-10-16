using DomainLayer.Models;
using System.Data.Entity;

namespace DomainLayer
{
    public class SAOContext : DbContext
    {
        public SAOContext() : base("name=SAPSql")
        { }

        public DbSet<InventoryCounting> InventoryCounting { get; set; }
        public DbSet<InventoryCountingRow> InventoryCountingRow { get; set; }
        public DbSet<DTInventoryCounting> DTInventoryCounting { get; set; }
        public DbSet<DTInventoryCountingRow> DTInventoryCountingRow { get; set; }
        public DbSet<DTheader> dtheader { get; set; }
        public DbSet<DTrow> dtrows { get; set; }
        public DbSet<MarketingDocumentHeaders> DocumentHeaders { get; set; }
        public DbSet<MarketingDocumentLines> DocumentLines { get; set; }
    }
}
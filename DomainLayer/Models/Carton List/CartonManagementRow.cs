namespace DirecLayer
{
    public class CartonManagementRow
    {
        public int Id { get; set; }
        public int Index { get; set; }
        public string CartonNo { get; set; }
        public string VendorCode { get; set; }
        public string ItemNo { get; set; }
        public string Description { get; set; }
        public string Quantity { get; set; }
        public string QtyPerInnerBox { get; set; }
        public string BasedDocEntry { get; set; }
        public string BasedDocType { get; set; }
    }
}

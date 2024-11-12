namespace Yarnique.BackgroundService.Models
{
    public class DueOrderInformation
    {
        public string DesignName { get; set; }
        public string DesignPartName { get; set; }
        public string SellerName {  get; set; }
        public string ToEmail { get; set; }
        public DateTime DueDate { get; set; }
        public string Status { get; set; }
    }
}

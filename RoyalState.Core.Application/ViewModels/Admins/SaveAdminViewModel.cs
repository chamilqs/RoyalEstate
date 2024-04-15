namespace RoyalState.Core.Application.ViewModels.Admins
{
    public class SaveAdminViewModel
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Identification { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}

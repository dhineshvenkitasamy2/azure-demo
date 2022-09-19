namespace AzureDemo.Models
{
    public class User
    {
        public string EmailId { get; set; }=String.Empty;
        public string Name { get; set; } = String.Empty;
        public string Password { get; set; } = String.Empty;
        public List<User> AddUsers()
        {
            List<User> users = new List<User>();
            users.Add(new Models.User
            {
                EmailId = "d.venkitasamy@nfp.com",
                Name = "Dhinesh Venkitasamy",
                Password = "Dhinu@123"
            });
            users.Add(new Models.User
            {
                EmailId = "b.ramesh@nfp.com",
                Name = "BalajiPrasad Ramesh",
                Password = "Balaji@123"
            });
            users.Add(new Models.User
            {
                EmailId = "karthick.karthick@nfp.com",
                Name = "Karthick M",
                Password = "Balaji@123"
            });
            users.Add(new Models.User
            {
                EmailId = "subhapradha.s@nfp.com",
                Name = "Subhapradha S",
                Password = "Subha@123"
            });
            users.Add(new Models.User
            {
                EmailId = "n.balakrishnan@nfp.com",
                Name = "Nageshwaran BalaKrishnan",
                Password = "Nagesh@123"
            });
            return users;
        }
    }
}

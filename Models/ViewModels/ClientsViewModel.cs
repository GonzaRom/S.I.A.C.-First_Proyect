using System.Text;

namespace S.I.A.C.Models
{
    public class ClientsViewModel
    {
        public int keyClient { get; set; }
        public string nameClient { get; set; }
        public string lastNameClient { get; set; }
        public string addressClient { get; set; }

        public string GetClientFullViewModel()
        {
            var stringClient = new StringBuilder();
            stringClient.Append(nameClient);
            stringClient.Append(" ");
            stringClient.Append(lastNameClient);
            stringClient.Append(" | ");
            stringClient.Append(addressClient.Trim());
            return stringClient.ToString();
        }
    }
}
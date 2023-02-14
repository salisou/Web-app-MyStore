using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace Web_app_MyStore.Pages.Clients
{
    public class IndexModel : PageModel
    {
        public List<ClientInfo> listClients = new List<ClientInfo>();   
        public void OnGet()
        {
            try
            {
                // Stringa della connezione al Database SQl Server locale 
                string connectionString = "Data Source=.\\sqlexpress;Initial Catalog=mystore;Integrated Security=True";

                // Connezione SQl 
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    // Apro la connezione SQl
                    con.Open();

                    // Query Select della table Clients
                    string Query = "Select * From clients";

                    // Command SQL che prende la query select e la connezione al DB
                    using (SqlCommand command = new SqlCommand(Query, con))
                    {
                        // Esecuzione del command SQl
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ClientInfo info = new ClientInfo();
                                info.id = "" + reader.GetInt32(0);
                                info.name= reader.GetString(1);
                                info.email = reader.GetString(2);
                                info.phone= reader.GetString(3);
                                info.address = reader.GetString(4);
                                info.created_at = reader.GetDateTime(5).ToString();

                                listClients.Add(info);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: ", ex.Message);
            }
        }
    }

    public class ClientInfo
    {
        public String id;
        public String name;
        public String email;
        public String phone;
        public String address;
        public String created_at;
    }
}

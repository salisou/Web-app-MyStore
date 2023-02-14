using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace Web_app_MyStore.Pages.Clients
{
    public class EditModel : PageModel
    {

        public ClientInfo clientInfo = new ClientInfo();
        public string errorMessage = "";
        public string succesMessage = "";

        public void OnGet()
        {
            String id = Request.Query["id"];

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
                    String sql = "Select * From clients WHERE id=@id";

                    // Command SQL che prende la query select e la connezione al DB
                    using (SqlCommand command = new SqlCommand(sql, con))
                    {
                        // Esecuzione del command SQl
                        command.Parameters.AddWithValue("@id", id);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {

                            if (reader.Read())
                            {
                                clientInfo.id = "" + reader.GetInt32(0);
                                clientInfo.name = reader.GetString(1);
                                clientInfo.email = reader.GetString(2);
                                clientInfo.phone = reader.GetString(3);
                                clientInfo.address = reader.GetString(4);
                            }
                        }
                    }


                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }
        }

        public void OnPost() 
        { 
            clientInfo.id = Request.Form["id"];
            clientInfo.name = Request.Form["name"];
            clientInfo.email = Request.Form["email"];
            clientInfo.phone = Request.Form["phone"];
            clientInfo.address = Request.Form["address"];

            // verrifica campi vuoti
            if (clientInfo.id.Length == 0 || 
                clientInfo.name.Length == 0 ||
                clientInfo.email.Length == 0 ||
                clientInfo.phone.Length == 0 || 
                clientInfo.address.Length == 0)
            {
                errorMessage = "Attenzione: i campi devono essere compilati prima";
                return;
            }

            ConnectionDB();
        }

        private void ConnectionDB()
        {
            try
            {
                string connectionString = "Data Source=.\\sqlexpress;Initial Catalog=mystore;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Update della table clients
                    String sql = "UPDATE clients " +
                                 "SET name=@name, email=@email, phone=@phone, address=@address " +
                                 "WHERE id=@id";

                    // SQL Commande 
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@name", clientInfo.name);
                        command.Parameters.AddWithValue("@email", clientInfo.email);
                        command.Parameters.AddWithValue("@phone", clientInfo.phone);
                        command.Parameters.AddWithValue("@address", clientInfo.address);
                        command.Parameters.AddWithValue("@id", clientInfo.id);

                        // Esecuzione SQL Query
                        command.ExecuteNonQuery();
                    }

                }
            }
            catch (Exception ex)
            {
                 errorMessage = ex.Message;
                 return;
            }

            Response.Redirect("/Clients/Index");
        }
    }
}

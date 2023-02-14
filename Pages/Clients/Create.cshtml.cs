using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace Web_app_MyStore.Pages.Clients
{
    public class CreateModel : PageModel
    {
        public ClientInfo clientInfo = new ClientInfo();

        // Golbal Error message
        public string errorMessage = "";
        public string succesMessage = "";

        public void OnGet()
        {
        }

        public void OnPost()
        {
            clientInfo.name = Request.Form["name"];
            clientInfo.email = Request.Form["email"];
            clientInfo.phone = Request.Form["phone"];
            clientInfo.address = Request.Form["address"];

            // Verrifica se i campi è vuoto
            if (clientInfo.name.Length == 0 || clientInfo.email.Length == 0 || 
                clientInfo.phone.Length == 0 || clientInfo.address.Length == 0)
            {
                errorMessage = "Attenzione: il campo deve non deve essere vuoto";
                return;
            }

            // Salvataggio del cliente sul DB
            try
            {
                string connectionString = "Data Source=.\\sqlexpress;Initial Catalog=mystore;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Inserimento al DB
                    string sql = "INSERT INTO clients" +
                        "(name, email, phone, address) VALUES" +
                        "(@name, @email, @phone, @address);";

                    // SQL Commande 
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@name", clientInfo.name);
                        command.Parameters.AddWithValue("@email", clientInfo.email);
                        command.Parameters.AddWithValue("@phone", clientInfo.phone);
                        command.Parameters.AddWithValue("@address", clientInfo.address);

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


            Cleare();
            succesMessage = "Il salvataggio è andata a buon fine";

            // Nome del file Utilizzato pre vedere il risultato
            Response.Redirect("/Clients/Index");
        }

        private void Cleare()
        {
            clientInfo.name = "";
            clientInfo.email = "";
            clientInfo.phone = "";
            clientInfo.address = "";
        }
    }
}

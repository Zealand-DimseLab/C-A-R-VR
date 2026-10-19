using CARDataLib;
using cardataapi;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Data.SqlClient;
using BCrypt.Net;
namespace CarDataTest;

[TestClass]
public sealed class Test1
{
    
    [TestMethod]
    public void InsertAdmin(){
        LoginUser toadd = new LoginUser(545454, "nikolajqm");
        string connectionstring = "Server=localhost;Database=CARData;Trusted_Connection=True;TrustServerCertificate=True;";
        SqlConnection connection = new SqlConnection(connectionstring);
        using(connection){
            connection.Open();
            string sql = "INSERT INTO (id, password) LoginUser VALUES (@id, @password);";

            SqlCommand command = new SqlCommand(sql, connection);

            command.Parameters.AddWithValue("@id", toadd.Id);
            command.Parameters.AddWithValue("@password", toadd.Password);
            command.ExecuteNonQuery();
        }


    }
}

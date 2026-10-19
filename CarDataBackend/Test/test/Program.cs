namespace test;
using Microsoft.Data.SqlClient;
using BCrypt.Net;
using cardataapi;

class Program
{
    static void Main(string[] args)
    {
        try{
        Console.WriteLine("Hello, World!");
        LoginUser toadd = new LoginUser(545454, "nikolajqm");
        string? hashedpassword = BCrypt.EnhancedHashPassword(toadd.Password, 12);
        string connectionstring = "Server=localhost;Database=CARData;Trusted_Connection=True;TrustServerCertificate=True;";
        SqlConnection connection = new SqlConnection(connectionstring);
        using(connection){
            connection.Open();
            string sql = "INSERT INTO LoginUser VALUES (@id, @password);";

            SqlCommand command = new SqlCommand(sql, connection);

            command.Parameters.AddWithValue("@id", toadd.Id);
            command.Parameters.AddWithValue("@password", hashedpassword);
            command.ExecuteNonQuery();
}
}catch(Exception e){           
    Console.WriteLine(e.Message);
}
}
}

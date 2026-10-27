namespace cardataapi;
using System.Data;
using System.Configuration.Assemblies;
using System.Data.Common;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Collections;
using Microsoft.Data.SqlClient;
using CARDataLib;
public class CarDataMssqlRepository{
    private readonly string connectionString;
    public CarDataMssqlRepository(string connectionString)
    {
        this.connectionString = connectionString; 
    }
    // public List<User> GetUsers()
    // {
    //     List<User> users = new List<User>();
    //     try
    //     {
    //         SqlConnection connection = HelperMethods.NewConnection(connectionString);
    //         using (connection)
    //         {
    //             connection.Open();
    //
    //             string sql = "SELECT TestPersonNumber, Age, Gender FROM Users";
    //
    //             SqlCommand command = HelperMethods.NewCommand(sql, connection);
    //
    //             // HelperMethods.BindValueInt(command, "@testpersonnumber", id);
    //
    //             SqlDataReader reader = command.ExecuteReader();
    //             while (reader.Read())
    //             {
    //                 User user = new User(reader.GetInt32(0), reader.GetInt32(1), reader.GetString(2));
    //                 users.Add(user);
    //             }
    //             return users;
    //         }
    //     }
    //     catch(SqlException e)
    //     {
    //         throw new Exception(e.Message);
    //     }
    // }

    // public Session GetOneSession(int id){
    //     try{
    //         SqlConnection connection = HelperMethods.NewConnection(connectionString);
    //         Session session;
    //         using(connection){
    //             connection.Open();
    //             string sql = "SELECT Id FROM Session Where Id = @id";
    //
    //             SqlCommand command = HelperMethods.NewCommand(sql, connection);
    //
    //             HelperMethods.BindValueInt(command, "@id", id);
    //
    //             SqlDataReader reader = command.ExecuteReader();
    //
    //             if(reader.Read()){
    //                 session = new Session(){
    //                     Id = reader.GetInt32(0)
    //                 };
    //                 return session;
    //             }
    //             return null;
    //         }
    //     }
    //     catch(SqlException e){
    //         throw new Exception(e.Message);
    //         }
    // }
    // public User GetOneUser(int id){
    //     try{
    //         SqlConnection connection = HelperMethods.NewConnection(connectionString);
    //         User user;
    //         using(connection){
    //             connection.Open();
    //
    //             string sql = "SELECT TestPersonNumber, Age, Gender FROM Users Where TestPersonNumber = 16128;";
    //
    //             SqlCommand command = HelperMethods.NewCommand(sql, connection);
    //
    //             HelperMethods.BindValueInt(command, "@id", id);
    //
    //             SqlDataReader reader = command.ExecuteReader();
    //
    //             if(reader.Read()){
    //                 user = new User(reader.GetInt32(0), reader.GetInt32(1), reader.GetString(2));
    //                 return user; 
    //             }
    //             return null;
    //         }
    //     }
    //     catch(SqlException e){
    //         throw new Exception(e.Message);
    //         }
    // }
    public User InsertQuaterOne(User user)
    {
        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string sql = "INSERT INTO Users (TestPersonNumber, Name, Gender, Age, Q1, Q1Comment, Q2, Q3, Q4) VALUES (@testpersonnumber, @name, @gender, @age, @q1, @q1comment, @q2, @q3, @q4)";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    HelperMethods.BindValueInt(command, "@testpersonnumber", user.TestPersonNumber);
                    HelperMethods.BindValueString(command, "@name", user.Name);
                    HelperMethods.BindValueString(command, "@gender", user.Gender);
                    HelperMethods.BindValueString(command, "@age", user.Age);
                    HelperMethods.BindValueString(command, "@q1", user.Q1);
                    HelperMethods.BindValueString(command, "@q1comment", user.Q1Comment);
                    HelperMethods.BindValueString(command, "@q2", user.Q2);
                    HelperMethods.BindValueString(command, "@q3", user.Q3);
                    HelperMethods.BindValueString(command, "@q4", user.Q4);
                    command.ExecuteNonQuery();
                }
            }
            return user;
        }
        catch (SqlException e)
        {
            throw new Exception(e.Message);
        }
    }

    public User InsertQuaterTwo(User user)
    {
        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string sql = "INSERT INTO Users (Q5, Q6, Q7, Q8, OtherRemarks) VALUES (@q5, @q6, @q7, @q8, @otherremarks)";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    HelperMethods.BindValueString(command, "@q5", user.Q5);
                    HelperMethods.BindValueString(command, "@q6", user.Q6);
                    HelperMethods.BindValueString(command, "@q7", user.Q7);
                    HelperMethods.BindValueString(command, "@q8", user.Q8);
                    command.ExecuteNonQuery();
                }
            }
            return user;
        }
        catch (SqlException e)
        {
            throw new Exception(e.Message);
        }
    }
    // public Session AddSession(int userId)
    // {
    //     try
    //     {
    //         SqlConnection connection = HelperMethods.NewConnection(connectionString);
    //         int id;
    //         Session newSess;
    //         using (connection)
    //         {
    //             connection.Open();
    //
    //             string sql = "INSERT INTO Session (UserId) VALUES (@userid); SELECT CAST(SCOPE_IDENTITY() AS INT)";
    //
    //             SqlCommand command = HelperMethods.NewCommand(sql, connection);
    //
    //             HelperMethods.BindValueInt(command, "@userid", userId);
    //             SqlDataReader reader = command.ExecuteReader();
    //             if(reader.Read()){
    //                 id = reader.GetInt32(0);
    //                 if(id != 0){
    //                     newSess = GetOneSession(id);
    //                     return newSess;
    //                 }
    //             }
    //         }
    //         return null;
    //     }
    //     catch (SqlException e)
    //     {
    //         throw new Exception(e.Message);
    //     }
    // }
    // public Scenario AddScenario(Scenario scenario, int sessionId)
    // {
    //     try
    //     {
    //         SqlConnection connection = HelperMethods.NewConnection(connectionString);
    //         using (connection)
    //         {
    //             connection.Open();
    //
    //             string sql = "INSERT INTO Scenario (SessionId, ScenarioName, CycleToCarDistance, ScenarioStart, ScenarioEnd) VALUES (@sessionId, @scenarioName, @cycleToCarDistance, @scenarioStart, @scenarioEnd)";
    //
    //             SqlCommand command = HelperMethods.NewCommand(sql, connection);
    //
    //             HelperMethods.BindValueInt(command, "@sessionId", sessionId);
    //             HelperMethods.BindValueString(command, "@scenarioName", scenario.ScenarioName);
    //             HelperMethods.BindValueDouble(command, "@cycleToCarDistance", scenario.CycleToCarDistance);
    //             HelperMethods.BindValueDateTime(command, "@scenarioStart", scenario.ScenarioStart);
    //             HelperMethods.BindValueDateTime(command, "@scenarioEnd", scenario.ScenarioEnd);
    //             command.ExecuteNonQuery();
    //         }
    //         return scenario;
    //     }
    //     catch (SqlException e)
    //     {
    //         throw new Exception(e.Message);
    //     }
    // }
    // public PulseData AddPulseData(PulseData pulseData, int userId)
    // {
    //     try
    //     {
    //         SqlConnection connection = HelperMethods.NewConnection(connectionString);
    //         using (connection)
    //         {
    //             connection.Open();
    //
    //             string sql = "INSERT INTO PulseData (UserId, Pulse) VALUES (@userid, @pulse)";
    //
    //             SqlCommand command = HelperMethods.NewCommand(sql, connection);
    //
    //             HelperMethods.BindValueInt(command, "@userId", userId);
    //             HelperMethods.BindValueDouble(command, "@pulse", pulseData.Pulse);
    //             command.ExecuteNonQuery();
    //         }
    //         return pulseData;
    //     }
    //     catch (SqlException e)
    //     {
    //         throw new Exception(e.Message);
    //     }
    // }
    // public TimeCheck AddTimeCheck(TimeCheck timeCheck)
    // {
    //     try
    //     {
    //         SqlConnection connection = HelperMethods.NewConnection(connectionString);
    //         using (connection)
    //         {
    //             connection.Open();
    //
    //             string sql = "INSERT INTO TimeCheck (Time) VALUES (@timeCheck)";
    //
    //             SqlCommand command = HelperMethods.NewCommand(sql, connection);
    //
    //             HelperMethods.BindValueDouble(command, "@timeCheck", timeCheck.Time);
    //             command.ExecuteNonQuery();
    //         }
    //         return timeCheck;
    //     }
    //     catch (SqlException e)
    //     {
    //         throw new Exception(e.Message);
    //     }
    // }
    // public BikeData AddBikeData(BikeData bikeData, int userId)
    // {
    //     try
    //     {
    //         SqlConnection connection = HelperMethods.NewConnection(connectionString);
    //         using (connection)
    //         {
    //             connection.Open();
    //
    //             string sql = "INSERT INTO BikeData (UserId, HandleRotationY, DistanceCurbside, Speed) VALUES (@userId, @handleRotationY, @distanceCurbside, @speed)";
    //
    //             SqlCommand command = HelperMethods.NewCommand(sql, connection);
    //             HelperMethods.BindValueInt(command, "@userId", userId);
    //             HelperMethods.BindValueDouble(command, "@handleRotationY", bikeData.HandleRotationY);
    //             HelperMethods.BindValueDouble(command, "@distanceCurbside", bikeData.DistanceCurbSide);
    //             HelperMethods.BindValueDouble(command, "@speed", bikeData.Speed);
    //             command.ExecuteNonQuery();
    //         }
    //         return bikeData;
    //     }
    //     catch (SqlException e)
    //     {
    //         throw new Exception(e.Message);
    //     }
    // }
    // public HeadTransform AddHeadTransform(HeadTransform headTransform, int userId)
    // {
    //     try
    //     {
    //         SqlConnection connection = HelperMethods.NewConnection(connectionString);
    //         using (connection)
    //         {
    //             connection.Open();
    //
    //             string sql = "INSERT INTO HeadTransForm (UserId, RotW, RotZ, RotX, RotY, PosX, PosY, PosZ) VALUES (@userId, @rotW, @rotZ, @rotX, @rotY, @posX, @posY, @posZ)";
    //
    //             SqlCommand command = HelperMethods.NewCommand(sql, connection);
    //             HelperMethods.BindValueInt(command, "@userId", userId);
    //             HelperMethods.BindValueDouble(command, "@rotW", headTransform.RotW);
    //             HelperMethods.BindValueDouble(command, "@rotZ", headTransform.RotZ);
    //             HelperMethods.BindValueDouble(command, "@rotY", headTransform.RotY);
    //             HelperMethods.BindValueDouble(command, "@rotX", headTransform.RotX);
    //             HelperMethods.BindValueDouble(command, "@posX", headTransform.PosX);
    //             HelperMethods.BindValueDouble(command, "@posY", headTransform.PosY);
    //             HelperMethods.BindValueDouble(command, "@posZ", headTransform.PosZ);
    //             command.ExecuteNonQuery();
    //         }
    //         return headTransform;
    //     }
    //     catch (SqlException e)
    //     {
    //         throw new Exception(e.Message);
    //     }
    // }
    // public async void AddScenarios(List<Scenario> scenarios, User user)
    // {
    //     try
    //     {
    //         SqlConnection connection = HelperMethods.NewConnection(connectionString);
    //         using (connection)
    //         {
    //             connection.Open();
    //             DataTable table = new DataTable();
    //             SqlBulkCopy bulk = new SqlBulkCopy(connection)
    //             {
    //                 DestinationTableName = "dbo.Scenario",
    //                 BatchSize = 1000,
    //                 //Antal sekunder før timeout
    //                 BulkCopyTimeout = 60,
    //                 //Skal testes og evt fjernes hvis det ligefyldigt.
    //                 //Det sørger for at streaming til db kan forsætte uden problemer.
    //                 EnableStreaming = true
    //             };
    //             //ColumnMappings er mapningen af rækkernes navne i Db
    //             //De skal passe med navnet i db.
    //             //De bytter table navne ud hvis de evt ikke skulle passe.
    //             bulk.ColumnMappings.Add("UserId", "UserId");
    //             bulk.ColumnMappings.Add("ScenarioStart", "ScenarioStart");
    //             bulk.ColumnMappings.Add("ScenarioEnd", "ScenarioEnd");
    //             //Column her er tilføjelsen af kolonner til table. 
    //             //Med datatype
    //             table.Columns.Add("UserId", typeof(int));
    //             table.Columns.Add("ScenarioStart", typeof(DateTime));
    //             table.Columns.Add("ScenarioEnd", typeof(DateTime));
    //             foreach (Scenario scenario in scenarios)
    //             {
    //                 //Tilføjelse af rækker i db.
    //                 table.Rows.Add(user.TestPersonNumber, scenario.ScenarioStart, scenario.ScenarioEnd);
    //             }
    //             //Skrivning af hele bulken til det (table) vi har mappet til.
    //             await bulk.WriteToServerAsync(table);
    //         }
    //     }
    //     catch (SqlException e)
    //     {
    //         throw new Exception(e.Message);
    //     }
    // }
}


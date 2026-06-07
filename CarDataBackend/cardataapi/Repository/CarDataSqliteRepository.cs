using CARDataLib;
using Microsoft.Data.Sqlite;
namespace cardataapi;

public class CarDataSqliteRepository{
    private string connectionString;

    public CarDataSqliteRepository(string connectionString){
        this.connectionString = connectionString;
    }
    // private void ExecuteQuery(SqliteCommand command){
    //     command.ExecuteNonQuery();
    // }
    public void FirstInstanceOfUser(User user){
        SqliteConnection connection = new SqliteConnection(connectionString);
        using(connection){
            connection.Open();
            string sql = "insert into Users values(@id, @age, @gender)";

            SqliteCommand comm = new SqliteCommand(sql, connection);
            HelperMethods.BindSqliteValueInt(comm, "@id", user.TestPersonNumber);
            HelperMethods.BindSqliteValueInt(comm, "@age", user.Age);
            HelperMethods.BindSqliteValueString(comm, "@gender", user.Gender);
            comm.ExecuteNonQuery();
        }
    }
    public User GetUser(int id){
        SqliteConnection connection = HelperMethods.NewSqliteConnection(connectionString);
        using(connection){
            connection.Open();
            string sql = "Select * from Users where (TestPersonNumber = @id)";
            SqliteCommand comm = new SqliteCommand(sql, connection);
            HelperMethods.BindSqliteValueInt(comm, "@id", id);
            SqliteDataReader reader = comm.ExecuteReader();
            if(reader.Read()){
                User u = new User(reader.GetInt32(0), reader.GetInt32(1), reader.GetString(2));
                return u;
            }
            return null;
        }
    }
    // public void AddBikeData(BikeData bikeData, int userId){
    //     SqliteConnection connection = new SqliteConnection(connectionString);
    //     using(connection){
    //
    //         connection.Open();
    //         string sql = "insert into bikedata values(null, @userid, @handleY, @curbSide, @speed";
    //         SqliteCommand comd = new SqliteCommand(sql, connection);
    //         HelperMethods.BindSqliteValueDouble(comd, "@handleY", bikeData.HandleRotationY);
    //         HelperMethods.BindSqliteValueDouble(comd, "@curbSide", bikeData.DistanceCurbSide);
    //         HelperMethods.BindSqliteValueDouble(comd, "@speed", bikeData.Speed);
    //         ExecuteQuery(comd);
    //     }
    // }
    // public void AddPulseData(PulseData pulseData, int userId){
    //     SqliteConnection connection = new SqliteConnection(connectionString);
    //     using(connection){
    //
    //         connection.Open();
    //         string sql = $"insert into pulseData values(null, @userid, @pulse)";
    //         SqliteCommand comd = new SqliteCommand(sql, connection);
    //         HelperMethods.BindSqliteValueInt(comd, "@userid", userId);
    //         HelperMethods.BindSqliteValueInt(comd, "@pulse", pulseData.Pulse);
    //         ExecuteQuery(comd);
    //     }
    // }
    // public void AddHeadTransform(HeadTransform headTransform, int userId){
    //     SqliteConnection connection = new SqliteConnection(connectionString);
    //     using(connection){
    //         connection.Open();
    //         string sql = "insert into headtransform values (null, @userid, @rotw, @rotz, @rotx, @roty, @posx, @posy, @posz)";
    //         SqliteCommand command = new SqliteCommand(sql, connection);
    //         HelperMethods.BindSqliteValueInt(command, "@userId", userId);
    //         HelperMethods.BindSqliteValueDouble(command, "@rotW", headTransform.RotW);
    //         HelperMethods.BindSqliteValueDouble(command, "@rotZ", headTransform.RotZ);
    //         HelperMethods.BindSqliteValueDouble(command, "@rotY", headTransform.RotY);
    //         HelperMethods.BindSqliteValueDouble(command, "@rotX", headTransform.RotX);
    //         HelperMethods.BindSqliteValueDouble(command, "@posX", headTransform.PosX);
    //         HelperMethods.BindSqliteValueDouble(command, "@posY", headTransform.PosY);
    //         HelperMethods.BindSqliteValueDouble(command, "@posZ", headTransform.PosZ);
    //         ExecuteQuery(command);
    //     }
    // }
    // public void AddTimeCheck(TimeCheck time){
    //     try
    //     {
    //         SqliteConnection connection = new SqliteConnection(connectionString);
    //         using (connection)
    //         {
    //             connection.Open();
    //
    //             string sql = "INSERT INTO TimeCheck VALUES (null, @timeCheck)";
    //
    //             SqliteCommand command = new SqliteCommand(sql, connection);
    //
    //             HelperMethods.BindSqliteValueDouble(command, "@timeCheck", time.Time);
    //             ExecuteQuery(command);
    //         }
    //     }
    //     catch (SqliteException e)
    //     {
    //         throw new Exception(e.Message);
    //     }
    // }
    // public void AddScenario(Scenario scenario, int sessionId)
    // {
    //     try
    //     {
    //         SqliteConnection connection = new SqliteConnection(connectionString);
    //         using (connection)
    //         {
    //             connection.Open();
    //
    //             string sql = "INSERT INTO Scenario VALUES (null, @sessionId, @scenarioName, @cycleToCarDistance, @scenarioStart, @scenarioEnd)";
    //
    //             SqliteCommand command = new SqliteCommand(sql, connection);
    //
    //             HelperMethods.BindSqliteValueInt(command, "@sessionId", sessionId);
    //             HelperMethods.BindSqliteValueString(command, "@scenarioName", scenario.ScenarioName);
    //             HelperMethods.BindSqliteValueDouble(command, "@cycleToCarDistance", scenario.CycleToCarDistance);
    //             HelperMethods.BindSqliteValueDateTime(command, "@scenarioStart", scenario.ScenarioStart);
    //             HelperMethods.BindSqliteValueDateTime(command, "@scenarioEnd", scenario.ScenarioEnd);
    //             ExecuteQuery(command);
    //         }
    //     }
    //     catch (SqliteException e)
    //     {
    //         throw new Exception(e.Message);
    //     }
    // }
    // public void AddSession(int userId)
    // {
    //     try
    //     {
    //         SqliteConnection connection = new SqliteConnection(connectionString);
    //         using (connection)
    //         {
    //             connection.Open();
    //
    //             string sql = "INSERT INTO Session VALUES (@userid);";
    //
    //             SqliteCommand command = new SqliteCommand(sql, connection);
    //
    //             HelperMethods.BindSqliteValueInt(command, "@userid", userId);
    //             ExecuteQuery(command);
    //         }
    //     }
    //     catch (SqliteException e)
    //     {
    //         throw new Exception(e.Message);
    //     }
    // }
}

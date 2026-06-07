namespace cardataapi;
using System.Data;
using CARDataLib;
using Microsoft.Data.Sqlite;

public class CarDataSqliteChunkRepository{

    private string connectionString; 
    public CarDataSqliteChunkRepository(string connectionString){
        this.connectionString = connectionString;
    }
    public List<PulseData> GetAll(){
        try{
            using SqliteConnection connection = HelperMethods.NewSqliteConnection(connectionString);
                connection.Open();
                return null;
        }
        catch(Exception e){
            throw new Exception(e.Message);
        }
    }
    public async Task AddScenarios(List<Scenario> scenarios, int userId)
    {
        try
        {
            SqliteConnection connection = new SqliteConnection(connectionString);
            using (connection)
            {
                connection.Open();
                string sql = "insert into Scenario (UserId, ScenarioName, CycleToCarDistance, ScenarioStart, ScenarioEnd) values (@userId, @scenarioname, @cycletocardis, @scenariostart, @scenarioend)";
                SqliteCommand command = connection.CreateCommand(); 
                command.CommandText = sql;
                SqliteParameter userIdParam = HelperMethods.CreateParam(command, "@userId");
                SqliteParameter scenarionameParam = HelperMethods.CreateParam(command, "@scenarioname");
                SqliteParameter cycletocardisParam = HelperMethods.CreateParam(command, "@cycletocardis");
                SqliteParameter scenariostartParam = HelperMethods.CreateParam(command, "@scenariostart");
                SqliteParameter scenarioendParam = HelperMethods.CreateParam(command, "@scenarioend");
                foreach(Scenario scenario in scenarios){
                    userIdParam.Value = userId;
                    scenarionameParam.Value = scenario.ScenarioName;
                    cycletocardisParam.Value = scenario.CycleToCarDistance;
                    scenariostartParam.Value = scenario.ScenarioStart;
                    scenarioendParam.Value = scenario.ScenarioEnd;
                    await command.ExecuteNonQueryAsync();
                }

            }
        }
        catch (SqliteException e)
        {
            throw new Exception(e.Message);
        }
    }
    public async Task AddPulseData(List<PulseData> pulseData, int userId)
    {
        try
        {
            SqliteConnection connection = HelperMethods.NewSqliteConnection(connectionString);
            using (connection)
            {
                connection.Open();
                string sql = "insert into PulseData (UserId, Pulse, PulseTime) values (@userId, @pulse, @pulseTime)";
                SqliteCommand command = connection.CreateCommand(); 
                command.CommandText = sql;
                SqliteParameter userIdParam = HelperMethods.CreateParam(command, "@userId");
                SqliteParameter pulseParam = HelperMethods.CreateParam(command, "@pulse");
                SqliteParameter pulseTimeParam = HelperMethods.CreateParam(command, "@pulseTime");
                foreach(PulseData pulse in pulseData){
                    userIdParam.Value = userId;
                    pulseParam.Value = pulse.Pulse;
                    pulseTimeParam.Value = pulse.PulseTime;
                     await command.ExecuteNonQueryAsync();
                }
            }
        }
        catch (SqliteException e)
        {
            throw new Exception(e.Message);
        }
    }
    public async Task AddBikeData(List<BikeData> bikeData, int userId)
    {
        try
        {
            SqliteConnection connection = new SqliteConnection(connectionString);
            using(connection){
                connection.Open();
                string sql = "insert into BikeData (UserId, HandleRotationY, DistanceCurbSide, Speed) values (@userId, @handley, @discurb, @speed)";
                SqliteCommand command = connection.CreateCommand(); 
                command.CommandText = sql;
                SqliteParameter userIdParam = HelperMethods.CreateParam(command, "@userId");
                SqliteParameter handleyParam = HelperMethods.CreateParam(command, "@handley");
                SqliteParameter cubsideParam = HelperMethods.CreateParam(command, "@discurb");
                SqliteParameter speedParam = HelperMethods.CreateParam(command, "@speed");
                foreach(BikeData bikdD in bikeData){
                    userIdParam.Value = userId;
                    handleyParam.Value = bikdD.HandleRotationY;
                    cubsideParam.Value = bikdD.DistanceCurbSide;
                    speedParam.Value = bikdD.Speed;
                    await command.ExecuteNonQueryAsync();
                }
            }
        }
        catch (SqliteException e)
        {
            throw new Exception(e.Message);
        }
    }
    public async Task AddHeadTransform(List<HeadTransform> headTransforms, int userId)
    { 
        try
        {
            SqliteConnection connection = new SqliteConnection(connectionString);
            using(connection){
                connection.Open();
                string sql = "insert into HeadTransform (UserId, RotW, RotZ, RotY, RotX, PosX, PosY, PosZ) values (@userId, @rotW, @rotZ, @rotY, @rotX, @posX, @posY, @posZ)";

                SqliteCommand command = connection.CreateCommand();
                command.CommandText = sql;
                SqliteParameter userIdParam = HelperMethods.CreateParam(command, "@userId");
                SqliteParameter rWParam = HelperMethods.CreateParam(command, "@rotW");
                SqliteParameter rZParam = HelperMethods.CreateParam(command, "@rotZ");
                SqliteParameter rYParam = HelperMethods.CreateParam(command, "@rotY");
                SqliteParameter rXParam = HelperMethods.CreateParam(command, "@rotX");
                SqliteParameter pXParam = HelperMethods.CreateParam(command, "@posX");
                SqliteParameter pYParam = HelperMethods.CreateParam(command, "@posY");
                SqliteParameter pZParam = HelperMethods.CreateParam(command, "@posZ");
                foreach(HeadTransform htf in headTransforms){
                    userIdParam.Value = userId;
                    rWParam.Value = htf.RotW;
                    rZParam.Value = htf.RotZ;
                    rYParam.Value = htf.RotY;
                    rXParam.Value = htf.RotZ;
                    pXParam.Value = htf.PosX;
                    pYParam.Value = htf.PosY;
                    pZParam.Value = htf.PosZ;
                    await command.ExecuteNonQueryAsync();
                }
            }
        }
        catch (SqliteException e)
        {
            throw new Exception(e.Message);
        }
    }
    public async Task AddLeftBrake(List<LeftBrake> leftBrakes, int userid){
        try{
            SqliteConnection connection = new SqliteConnection(connectionString);
            using(connection){
                connection.Open();
                string sql = "insert into LeftBrake (UserId, LeftBrake, BrakeTime) values (@userId, @brake, @datetime)";
                SqliteCommand cmd1 = connection.CreateCommand();
                cmd1.CommandText = sql;
                SqliteParameter userIdParam = HelperMethods.CreateParam(cmd1, "@userId");
                SqliteParameter brakeParam1 = HelperMethods.CreateParam(cmd1, "@brake");
                SqliteParameter dateParam1 = HelperMethods.CreateParam(cmd1, "@datetime");
                foreach(LeftBrake lb in leftBrakes){
                    userIdParam.Value = userid;
                    brakeParam1.Value = lb.LeftBraking;
                    dateParam1.Value = lb.BrakeTime;
                    await cmd1.ExecuteNonQueryAsync();
                }
            }
        }
        catch(SqliteException e){
            throw new Exception(e.Message);
        }
    }
    public async Task AddRigthBrake(List<RightBrake> rightBrakes, int userid){
        try{
            SqliteConnection connection = new SqliteConnection(connectionString);
            using(connection){
                string sql2 = "insert into RightBrake (UserId, RightBrake, BrakeTime) values (@userId, @brake, @datetime)";
                SqliteCommand cmd2 = connection.CreateCommand();
                cmd2.CommandText = sql2;
                SqliteParameter userIdParam2 = HelperMethods.CreateParam(cmd2, "@userId");
                SqliteParameter brakeParam2 = HelperMethods.CreateParam(cmd2, "@brake");
                SqliteParameter dateParam2 = HelperMethods.CreateParam(cmd2, "@datetime");
                foreach(RightBrake rb in rightBrakes){
                    userIdParam2.Value = userid;
                    brakeParam2.Value = rb.RightBraking;
                    dateParam2.Value = rb.BrakeTime;
                    await cmd2.ExecuteNonQueryAsync();
                }
            }
        }
        catch(SqliteException e){
            throw new Exception(e.Message);
        }
    }

    // public async Task AddTimeCheck(List<TimeCheck> timeChecks)
    // {
    //     try
    //     {
    //     }
    //     catch (SqliteException e)
    //     {
    //         throw new Exception(e.Message);
    //     }
    // }
}

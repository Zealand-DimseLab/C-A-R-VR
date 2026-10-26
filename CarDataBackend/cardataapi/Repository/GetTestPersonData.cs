namespace cardataapi;
using CARDataLib;
using Microsoft.Data.SqlClient;

public class GetTestPersonData{
    private readonly string connectionString;
    public GetTestPersonData(string connectionString){
        this.connectionString = connectionString;
    }
    public async Task<List<TestPersonData?>> GetTPD(int? tpid){
        try{
            if(tpid == null){
            throw new ArgumentNullException("Id is null");
        }
        List<TestPersonData>? testPersonDatas = new List<TestPersonData>();
        SqlConnection connection = new SqlConnection(connectionString);
        using(connection){
            connection.Open();
            string sql = "SELECT s.ScenarioName, s.ScenarioStart, s.ScenarioEnd, s.CycleToCarDistance, ht.LookRotation, bd.Speed, bd.HandleRotationY, bd.DistanceCurbSide, lb.Brake, lb.BrakeTime, rb.Brake, rb.BrakeTime, pd.Pulse FROM Scenario s LEFT JOIN HeadTransform ht ON s.UserId = ht.UserId LEFT JOIN BikeData bd ON s.UserId = bd.UserId LEFT JOIN LeftBrake lb ON s.UserId = lb.UserId LEFT JOIN RightBrake rb ON s.UserId = rb.UserId LEFT JOIN PulseData pd ON s.UserId = pd.UserId WHERE s.UserId = @id";

            SqlCommand cmd = new SqlCommand(sql, connection);
            using(cmd){
            cmd.Parameters.AddWithValue("@id", tpid);
            SqlDataReader reader = cmd.ExecuteReader();
            using(reader){
            while(reader.Read()){
                TestPersonData tpd = new TestPersonData(
                        reader.IsDBNull(0) ? null : reader.GetString(0),
                        reader.IsDBNull(1) ? null : reader.GetDateTime(1),
                        reader.IsDBNull(2) ? null : reader.GetDateTime(2),
                        reader.IsDBNull(3) ? (float?)null : (float)reader.GetDouble(3),
                        reader.IsDBNull(4)  ? (float?)null : (float)reader.GetDouble(4),
                        reader.IsDBNull(5)  ? (float?)null : (float)reader.GetDouble(5),
                        reader.IsDBNull(6)  ? (float?)null : (float)reader.GetDouble(6),
                        reader.IsDBNull(7)  ? (float?)null : (float)reader.GetDouble(7),
                        reader.IsDBNull(8) ? (bool?)null : reader.GetBoolean(8),
                        reader.IsDBNull(9) ? null : reader.GetDateTime(9),
                        reader.IsDBNull(10) ? (bool?)null : reader.GetBoolean(10),
                        reader.IsDBNull(11) ? null : reader.GetDateTime(11),
                        reader.IsDBNull(12) ? (int?)null : reader.GetInt32(12)
                        );
                testPersonDatas.Add(tpd);
            }
            }
            }
        }
        return testPersonDatas;
        }

        catch(IndexOutOfRangeException e){
            throw new Exception(e.Message);
        }
        catch(InvalidCastException e){
            throw new Exception(e.Message);
        }
    }
}

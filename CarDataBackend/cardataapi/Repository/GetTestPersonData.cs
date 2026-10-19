namespace cardataapi;
using CARDataLib;
using Microsoft.Data.SqlClient;

public class GetTestPersonData{
    private readonly string connectionString;
    public GetTestPersonData(string connectionString){
        this.connectionString = connectionString;
    }
    public TestPersonData? GetTPD(int? tpid){
        if(tpid == null){
            throw new ArgumentNullException("Id is null");
        }
        SqlConnection connection = new SqlConnection(connectionString);
        using(connection){
            connection.Open();
            string sql = "SELECT s.ScenarioName, s.ScenarioStart, s.ScenarioEnd, s.CycleToCarDistance, ht.LookRotation, bd.Speed, bd.HandleRotationY, bd.DistanceCurbSide, lb.Brake, lb.BrakeTime, rb.Brake, rb.BrakeTime, pd.Pulse FROM Scenario s LEFT JOIN HeadTransform ht ON s.UserId = ht.UserId LEFT JOIN BikeData bd ON s.UserId = bd.UserId LEFT JOIN LeftBrake lb ON s.UserId = lb.UserId LEFT JOIN RightBrake rb ON s.UserId = rb.UserId LEFT JOIN PulseData pd ON s.UserId = pd.UserId WHERE s.UserId = @id";

            SqlCommand cmd = new SqlCommand(sql, connection);
            cmd.Parameters.AddWithValue("@id", tpid);
            SqlDataReader reader = cmd.ExecuteReader();
            if(reader.Read()){
                return new TestPersonData(
                        reader.IsDBNull(0) ? null : reader.GetString(0),
                        reader.IsDBNull(1) ? null : reader.GetDateTime(1),
                        reader.IsDBNull(2) ? null : reader.GetDateTime(2),
                        reader.IsDBNull(3) ? (float?)null : (float)reader.GetDouble(3),
                        reader.IsDBNull(4) ? (float?)null : reader.GetFloat(4),
                        reader.IsDBNull(5) ? (float?)null : reader.GetFloat(5),
                        reader.IsDBNull(6) ? (float?)null : reader.GetFloat(6),
                        reader.IsDBNull(7) ? (float?)null : reader.GetFloat(7),
                        reader.IsDBNull(8) ? (float?)null : reader.GetFloat(8),
                        reader.IsDBNull(9) ? (float?)null : reader.GetFloat(9),
                        reader.IsDBNull(10) ? (float?)null : reader.GetFloat(10),
                        reader.IsDBNull(11) ? (float?)null : reader.GetFloat(11),
                        reader.IsDBNull(12) ? (float?)null : reader.GetFloat(12),
                        reader.IsDBNull(13) ? (float?)null : reader.GetFloat(13),
                        reader.IsDBNull(14) ? (bool?)null : reader.GetBoolean(14),
                        reader.IsDBNull(15) ? null : reader.GetDateTime(15),
                        reader.IsDBNull(16) ? (bool?)null : reader.GetBoolean(16),
                        reader.IsDBNull(17) ? null : reader.GetDateTime(17),
                        reader.IsDBNull(18) ? (int?)null : reader.GetInt32(18)
                        );
            }
        }
        return null;
    }
}

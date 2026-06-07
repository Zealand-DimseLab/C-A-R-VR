using CARDataLib;
using System.Text;
namespace cardataapi;

public class StringHandlerService{
    private CarDataSqliteChunkRepository carDataSqliteChunkRepository;

    public StringHandlerService(CarDataSqliteChunkRepository carDataSqliteChunkRepository){
        this.carDataSqliteChunkRepository = carDataSqliteChunkRepository;
    }
    public async Task AddBikeData(string incomingText){
        ValidateIncomingText(incomingText);
        string readText = ReadIncomingText(incomingText);
        string[] splitText = readText.Split("|");
        string firstLine = splitText[0];
        int.TryParse(firstLine, out int id);
        var chunks = splitText.Skip(1).Chunk(3);
        List<BikeData> bikeDatas = new List<BikeData>();
        foreach(var chunk in chunks){
            if(chunk.Length < 3) throw new ArgumentOutOfRangeException("Chunk skal minimum være 3 lang");
            double yRot = double.Parse(chunk[0]);
            double curb = double.Parse(chunk[1]);
            double speed = double.Parse(chunk[2]);
            BikeData b = new BikeData();
            b.HandleRotationY = yRot;
            b.DistanceCurbSide = curb;
            b.Speed = speed;
            bikeDatas.Add(b);
        }
        await carDataSqliteChunkRepository.AddBikeData(bikeDatas, id);
    }
    public async void AddArduino(string incomingText){
        ValidateIncomingText(incomingText);

        string readText = ReadIncomingText(incomingText);
        string[] splitText = readText.Split("|");
        int id = SameProcedureAsLastMethodJames(splitText);
        var chunks = splitText.Skip(1).Chunk(4);
        List<LeftBrake> leftBrakes = new List<LeftBrake>();
        List<RightBrake> rightBrakes = new List<RightBrake>();
        foreach(var chunk in chunks){
            bool left = bool.Parse(chunk[0]);
            bool right = bool.Parse(chunk[1]);
            DateTime dT = DateTime.Parse(chunk[2]); 
            RightBrake rb = new RightBrake();
            LeftBrake lb = new LeftBrake();
            lb.LeftBraking = left;
            lb.BrakeTime = dT;
            rb.RightBraking = right;
            rb.BrakeTime = dT;
            leftBrakes.Add(lb);
            rightBrakes.Add(rb);
        }
        await carDataSqliteChunkRepository.AddLeftBrake(leftBrakes, id);
        await carDataSqliteChunkRepository.AddRigthBrake(rightBrakes, id);

    }
    private void ValidateIncomingText(string incomingText){
        if(string.IsNullOrEmpty(incomingText)){
            throw new ArgumentOutOfRangeException("Fejl i teksen");
        }
    }
    private string ReadIncomingText(string incomingText){
        string readText = File.ReadAllText(incomingText);
        return readText;
    }
    private int SameProcedureAsLastMethodJames(string[] splitText){
        string firstLine = splitText[0];
        int.TryParse(firstLine, out int id);
        return id;
    }
}

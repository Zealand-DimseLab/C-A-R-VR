namespace cardataapi;
using System.Globalization;
using CARDataLib;
using System.Text;

public class ByteHandlerService {
    private CarDataSqliteChunkRepository carDataChunkRepository;
    public ByteHandlerService(CarDataSqliteChunkRepository carDataChunkRepository){
        this.carDataChunkRepository = carDataChunkRepository;
    }
    public async Task ByteScenarios(byte[] unityarray){
        ValidateByteArray(unityarray);

        string fromByteToString = Encoding.UTF8.GetString(unityarray);
        string[] splitString = fromByteToString.Split("|");
        string firstLine = splitString[0];
        int.TryParse(firstLine, out int id);
        var chunks = splitString.Skip(1).Chunk(3);
        List<Scenario> scenarios = new List<Scenario>();
        foreach(var chunk in chunks){
        if(chunk.Length <= 0) continue;
            if(chunk.Length < 3) throw new ArgumentOutOfRangeException("Chunk skal minimum være 3 lang");
            string scename = chunk[0].ToString();
            double cyclToDis = double.Parse(chunk[1]);
            DateTime start = DateTime.Parse(chunk[2]);
            DateTime end = DateTime.Parse(chunk[3]);
            Scenario sce = new Scenario();
            sce.ScenarioName = scename;
            sce.CycleToCarDistance = cyclToDis;
            sce.ScenarioStart = start;
            sce.ScenarioEnd = end;
            scenarios.Add(sce);
        }
        await carDataChunkRepository.AddScenarios(scenarios, id);
    }
    public async Task ByteBikeData(byte[] unityarray){
        ValidateByteArray(unityarray);
        string fromByteToString = Encoding.UTF8.GetString(unityarray);
        string[] splitString = fromByteToString.Split("|");
        string firstLine = splitString[0];
        int.TryParse(firstLine, out int id);
        var chunks = splitString.Skip(1).Chunk(3);
        List<BikeData> bikeDatas = new List<BikeData>();
        foreach(var chunk in chunks){
        if(chunk.Length <= 0) continue;
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
        await carDataChunkRepository.AddBikeData(bikeDatas, id);
    }
    public async Task ByteHeadTransform(byte[] unityarray){
        ValidateByteArray(unityarray);
        string fromByteToString = Encoding.UTF8.GetString(unityarray);
        string[] splitString = fromByteToString.Split("|");
        string firstLine = splitString[0];
        int.TryParse(firstLine, out int id);
        var chunks = splitString.Skip(1).Chunk(7);
        List<HeadTransform> hTfs = new List<HeadTransform>();
        foreach(var chunk in chunks){
            if(chunk.Length <= 0) continue;
            if(chunk.Length < 7) throw new ArgumentOutOfRangeException("Chunk skal være minimum 7");
            double rw = double.Parse(chunk[0]);
            double rz = double.Parse(chunk[1]);
            double rx = double.Parse(chunk[2]);
            double ry = double.Parse(chunk[3]);
            double pz = double.Parse(chunk[4]);
            double px = double.Parse(chunk[5]);
            double py = double.Parse(chunk[6]);
            HeadTransform htf = new HeadTransform();
            htf.RotW = rw;
            htf.RotZ = rz;
            htf.RotX = rx;
            htf.RotY = ry;
            htf.PosZ = pz;
            htf.PosX = px;
            htf.PosY = py;
            hTfs.Add(htf);
        }
        await carDataChunkRepository.AddHeadTransform(hTfs, id);
    }
    public async Task ByteArduino(byte[] unityarray){
        ValidateByteArray(unityarray);
        string fromByteToString = Encoding.UTF8.GetString(unityarray);
        string[] splitString = fromByteToString.Split("|");
        string firstLine = splitString[0];
        int.TryParse(firstLine, out int id);
        var chunks = splitString.Skip(1).Chunk(4);
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
        await carDataChunkRepository.AddLeftBrake(leftBrakes, id);
        await carDataChunkRepository.AddRigthBrake(rightBrakes, id);
    }
    public async Task BytePulseData(byte[] unityarray){
        ValidateByteArray(unityarray);
        string fromByteToString = Encoding.UTF8.GetString(unityarray);
        string[] splitString = fromByteToString.Split("|");
        string firstLine = splitString[0];
        int.TryParse(firstLine, out int id);
        var chunks = splitString.Skip(1).Chunk(2);
        List<PulseData> pulseDatas = new List<PulseData>();
        foreach(var chunk in chunks){
            int pulse = int.Parse(chunk[0]);
            DateTime dateTime = DateTime.Parse(chunk[1]);
            PulseData p = new PulseData();
            p.Pulse = pulse;
            p.PulseTime = dateTime;
            pulseDatas.Add(p);
        }
        await carDataChunkRepository.AddPulseData(pulseDatas, id);
    }
    private void ValidateByteArray(byte[] tovalidate){ 
        if(tovalidate.Length == 0 || tovalidate == null){
            throw new ArgumentNullException("Intet i array"); 
        } 
    }
}

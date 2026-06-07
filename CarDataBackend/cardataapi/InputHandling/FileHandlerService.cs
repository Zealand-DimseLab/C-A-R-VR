namespace cardataapi;
using System.Globalization;
using CARDataLib;

public class FileHandlerService{
    private CarDataSqliteChunkRepository carDataSqliteChunkRepository;
    private CarDataChunkRepository carDataChunkRepository;
    public FileHandlerService(CarDataSqliteChunkRepository carDataSqliteChunkRepository, CarDataChunkRepository carDataChunkRepository){
        this.carDataSqliteChunkRepository = carDataSqliteChunkRepository;
        this.carDataChunkRepository = carDataChunkRepository;
    }
    public async Task HandleBikeData(IncomingFile incomingFile){
        if(incomingFile.newTestFile == null || incomingFile.newTestFile.Length == 0){
            throw new ArgumentNullException("Fil fejl");
        }
        var folderpath = Path.Combine(Directory.GetCurrentDirectory(), "userBikeData");
        var filepath = Path.Combine("userBikeData", incomingFile.newTestFile.FileName);

        Directory.CreateDirectory(folderpath);
        FileStream stream = new FileStream(filepath, FileMode.Create);
        using(stream){
            await incomingFile.newTestFile.CopyToAsync(stream);
        }
        string fileContent = System.IO.File.ReadAllText(filepath);
        if(fileContent.Length == 0 && fileContent == null)
            throw new ArgumentNullException("Læse fejl");
        var stringSplit = fileContent.Split(new[] {"\r\n", "\r", "\n"}, StringSplitOptions.RemoveEmptyEntries);
        string firstLine = stringSplit[0];
        int.TryParse(firstLine, out int id);

        var chunkSplit = stringSplit.Skip(1);

        var chunks = chunkSplit.Chunk(3);

        List<BikeData> bikeDatas = new List<BikeData>();

        foreach (string[] chunk in chunks){
            if(chunk.Length < 3) {
                throw new ArgumentOutOfRangeException("Chunk skal minimum være 3");
            };
            double yRot = double.Parse(chunk[0], CultureInfo.InvariantCulture);
            double curbSide = double.Parse(chunk[1], CultureInfo.InvariantCulture);
            double spee = double.Parse(chunk[2], CultureInfo.InvariantCulture);
            BikeData p = new BikeData();
            p.HandleRotationY = yRot;
            p.DistanceCurbSide = curbSide;
            p.Speed = spee;
            bikeDatas.Add(p);
        }
        await carDataSqliteChunkRepository.AddBikeData(bikeDatas, id);
        await carDataChunkRepository.AddBikeData(bikeDatas, id);
    }
    public async Task HandleHeadTrans(IncomingFile incomingFile){
        if(incomingFile == null || incomingFile.newTestFile.Length == 0){
            throw new ArgumentNullException("File fejl");
        }
        var folderpath = Path.Combine(Directory.GetCurrentDirectory(), "userHeadTransform");
        var filepath = Path.Combine("userHeadTransform", incomingFile.newTestFile.FileName);

        Directory.CreateDirectory(folderpath);
        FileStream stream = new FileStream(filepath, FileMode.Create);
        using(stream){
            await incomingFile.newTestFile.CopyToAsync(stream);
        }
        string fileContent = System.IO.File.ReadAllText(filepath);
        if(string.IsNullOrEmpty(fileContent)){
            throw new ArgumentNullException("Intet indhold");
        }
        var contentSplit = fileContent.Split(new[] {"\r\n", "\n", "\r"}, StringSplitOptions.RemoveEmptyEntries);
        string firstLine = contentSplit[0];
        int.TryParse(firstLine, out int id);
        var chunkSplit = contentSplit.Skip(1);

        var chunks = chunkSplit.Chunk(7);

        List<HeadTransform> hTfs = new List<HeadTransform>();

        foreach(string[] chunk in chunks){
            if(chunk.Length < 7)
                throw new ArgumentOutOfRangeException("Chunk skal minimum være 7");
            double rw = double.Parse(chunk[0], CultureInfo.InvariantCulture);
            double rz = double.Parse(chunk[1], CultureInfo.InvariantCulture);
            double rx = double.Parse(chunk[2], CultureInfo.InvariantCulture);
            double ry = double.Parse(chunk[3], CultureInfo.InvariantCulture);
            double px = double.Parse(chunk[4], CultureInfo.InvariantCulture);
            double py = double.Parse(chunk[5], CultureInfo.InvariantCulture);
            double pz = double.Parse(chunk[6], CultureInfo.InvariantCulture);
            // double.TryParse(chunk[0], out double rw, CultureInfo.InvariantCulture);
            // double.TryParse(chunk[1], out double rz);
            // double.TryParse(chunk[2], out double rx);
            // double.TryParse(chunk[3], out double ry);
            // double.TryParse(chunk[4], out double pz);
            // double.TryParse(chunk[5], out double px);
            // double.TryParse(chunk[6], out double py);
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
        await carDataSqliteChunkRepository.AddHeadTransform(hTfs, id);
        await carDataChunkRepository.AddHeadTransform(hTfs, id);
    }
    public async Task HandleScenarios(IncomingFile incomingFile){
        ValidateFile(incomingFile);
        var folderpath = CreateFolderPath("userScenarios");
        var filepath = CreateFilePath(incomingFile, "userScenarios");

        CreateDirectory(folderpath);

        FileStream stream = new FileStream(filepath, FileMode.Create);
        using(stream){
            await incomingFile.newTestFile.CopyToAsync(stream);
        }
        string fileContent = ReadFile(filepath);
        if(fileContent.Length == 0 || fileContent == null){
            throw new ArgumentNullException("Intet indhold");
        }
        var splitContent = fileContent.Split(new[] {"\r\n", "\n", "\r"}, StringSplitOptions.RemoveEmptyEntries);

        string firstLine = splitContent[0];
        int.TryParse(firstLine, out int id);

        var chunkSplit = splitContent.Skip(1);
        var chunks = chunkSplit.Chunk(4);

        List<Scenario> scenarios = new List<Scenario>();

        foreach(string[] chunk in chunks){
            if(chunk.Length < 4) throw new ArgumentOutOfRangeException("Chunk er for lille");
            Scenario sce = new Scenario();
            string sceName = chunk[0].ToString();
            double cycToCarDist = double.Parse(chunk[1], CultureInfo.InvariantCulture);
            DateTime start = DateTime.Parse(chunk[2]);
            DateTime end = DateTime.Parse(chunk[3]);
            sce.ScenarioName = sceName;
            sce.CycleToCarDistance = cycToCarDist;
            sce.ScenarioStart = start;
            sce.ScenarioEnd = end;
            scenarios.Add(sce);
        }
        await carDataSqliteChunkRepository.AddScenarios(scenarios, id);
        await carDataChunkRepository.AddScenarios(scenarios, id);
    }
    // public async Task HandleTimeCheck(IncomingFile incomingFile){
    //     ValidateFile(incomingFile);
    //     var folderpath = CreateFolderPath("userTimeCheck");
    //     var filepath = CreateFilePath(incomingFile, "userTimeCheck");
    //     CreateDirectory(folderpath);
    //
    //     FileStream stream = new FileStream(filepath, FileMode.Create);
    //     using(stream){
    //         await incomingFile.newTestFile.CopyToAsync(stream);
    //     }
    //     string fileContent = ReadFile(filepath);
    //     if(fileContent.Length == 0 || fileContent == null){
    //         throw new ArgumentNullException("Intet indhold");
    //     }
    //     var fileSplit = fileContent.Split("\n");
    //
    //     List<TimeCheck> timeChecks = new List<TimeCheck>();
    //
    //     foreach(string line in fileSplit){
    //         int time = int.Parse(line);
    //         TimeCheck tC = new TimeCheck();
    //         tC.Time = time;
    //         timeChecks.Add(tC);
    //     }
    //     await carDataSqliteChunkRepository.AddTimeCheck(timeChecks);
    // }
    public async Task HandleBraking(IncomingFile incomingFile){
        ValidateFile(incomingFile);
        string folderpath = CreateFolderPath("breakingdata");
        string filepath = CreateFilePath(incomingFile, "breakingdata");

        CreateDirectory(folderpath);

        string fileContent = ReadFile(filepath);
        ValidateContent(fileContent);
        var splitString = fileContent.Split(new[] {"\n", "\n", "\n"}, StringSplitOptions.None);
        string firstLine = splitString[0];
        int.TryParse(firstLine, out int userid);
        if(firstLine.Length == 0 || firstLine == null){
            throw new ArgumentOutOfRangeException("User Id er 0");
        }
        var chunkSplit = splitString.Skip(1);
        var chunks = chunkSplit.Chunk(3);
        List<LeftBrake> leftBrakes = new List<LeftBrake>();
        List<RightBrake> rightBrakes = new List<RightBrake>();
        foreach(var chunk in chunks){
            if(chunk.Length < 3)
                throw new ArgumentOutOfRangeException("Chunk skal minimum være 3");
            LeftBrake lB = new LeftBrake();
            RightBrake rB = new RightBrake();
            rB.RightBraking = bool.Parse(chunk[0]);
            rB.BrakeTime = DateTime.Parse(chunk[2]);
            lB.LeftBraking = bool.Parse(chunk[1]);
            lB.BrakeTime = DateTime.Parse(chunk[2]);
            leftBrakes.Add(lB);
            rightBrakes.Add(rB);
            await carDataSqliteChunkRepository.AddRigthBrake(rightBrakes, userid);
            await carDataSqliteChunkRepository.AddLeftBrake(leftBrakes, userid);
            await carDataSqliteChunkRepository.AddRigthBrake(rightBrakes, userid);
            await carDataSqliteChunkRepository.AddLeftBrake(leftBrakes, userid);
        }
    }
    // public async void HandlePulseData(IncomingFile incomingFile){ if(incomingFile == null || incomingFile.newTestFile.Length == 0){ throw new ArgumentNullException("File fejl");
    //     }
    //     var folderpath = Path.Combine(Directory.GetCurrentDirectory(), "userPulseData");
    //     var filepath = Path.Combine("userPulseData", incomingFile.newTestFile.FileName);
    //
    //     Directory.CreateDirectory(folderpath);
    //     FileStream stream = new FileStream(filepath, FileMode.Create);
    //     using(stream){
    //         await incomingFile.newTestFile.CopyToAsync(stream);
    //     }
    //     string fileContent = System.IO.File.ReadAllText(filepath);
    //     if(fileContent.Length == 0 || fileContent == null){
    //         throw new ArgumentNullException("Intet indhold");
    //     }
    //     string[] contentSplit = fileContent.Split("\n");
    //
    //     List<PulseData> pulseDatas = new List<PulseData>();
    //
    //     foreach(string line in contentSplit){
    //         int puls = int.Parse(line);
    //         PulseData pulseObject = new PulseData();
    //         pulseObject.Pulse = puls;
    //         pulseDatas.Add(pulseObject);
    //     }
    //     carDataSqliteChunkRepository.AddPulseData(pulseDatas, userId);
    // }
    private void ValidateFile(IncomingFile incomingFile){
        if(incomingFile.newTestFile.Length == 0 || incomingFile == null){
            throw new ArgumentNullException("File fejl");
        }
    }
    private string CreateFolderPath(string folderName){
        string folderpath = Path.Combine(Directory.GetCurrentDirectory(), folderName);
        return folderpath;
    }
    private string CreateFilePath(IncomingFile incomingFile, string folderpath){
        string filepath = Path.Combine(folderpath, incomingFile.newTestFile.FileName);
        return filepath;

    }
    private void CreateDirectory(string folderpath){
        Directory.CreateDirectory(folderpath);
    }
    private void ValidateContent(string fileContent){
        if(fileContent.Length == 0 || fileContent == null){
            throw new ArgumentNullException("Intet indhold");
        }
    }
    private string ReadFile(string filepath){
        string fileContent = System.IO.File.ReadAllText(filepath);
        return fileContent;
    }
}

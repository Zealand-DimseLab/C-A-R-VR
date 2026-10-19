namespace cardataapi;

public class TestPersonData
{
    public string? ScenarioName { get; set; }
    public DateTime? ScenarioStart { get; set; }
    public DateTime? ScenarioEnd { get; set; }
    public float? CycleToCarDistance { get; set; }

    public float? PosX { get; set; }
    public float? PosY { get; set; }
    public float? PosZ { get; set; }
    public float? RotW { get; set; }
    public float? RotX { get; set; }
    public float? RotY { get; set; }
    public float? RotZ { get; set; }

    public float? Speed { get; set; }
    public float? HandleRotationY { get; set; }
    public float? DistanceCurbSide { get; set; }

    public bool? LeftBraking { get; set; }
    public DateTime? LeftBrakeTime { get; set; }

    public bool? RightBraking { get; set; }
    public DateTime? RightBrakeTime { get; set; }

    public int? Pulse { get; set; }

    public TestPersonData(string? scenarioName, DateTime? scenarioStart, DateTime? scenarioEnd,
    float? cycleToCarDistance,
    float? posX, float? posY, float? posZ, float? rotW, float? rotX, float? rotY, float? rotZ,
    float? speed, float? handleRotationY, float? distanceCurbSide,
    bool? leftBraking, DateTime? leftBrakeTime,
    bool? rightBraking, DateTime? rightBrakeTime,
    int? pulse)
{
    ScenarioName = scenarioName;
    ScenarioStart = scenarioStart;
    ScenarioEnd = scenarioEnd;
    CycleToCarDistance = cycleToCarDistance;
    PosX = posX;
    PosY = posY;
    PosZ = posZ;
    RotW = rotW;
    RotX = rotX;
    RotY = rotY;
    RotZ = rotZ;
    Speed = speed;
    HandleRotationY = handleRotationY;
    DistanceCurbSide = distanceCurbSide;
    LeftBraking = leftBraking;
    LeftBrakeTime = leftBrakeTime;
    RightBraking = rightBraking;
    RightBrakeTime = rightBrakeTime;
    Pulse = pulse;
}
}

namespace cardataapi;

public class TestPersonData
{
    public string? ScenarioName { get; set; }
    public DateTime? ScenarioStart { get; set; }
    public DateTime? ScenarioEnd { get; set; }
    public float? CycleToCarDistance { get; set; }

    public float? LookRotation { get; set; }

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
    float? lookRotation,
    float? speed, float? handleRotationY, float? distanceCurbSide,
    bool? leftBraking, DateTime? leftBrakeTime,
    bool? rightBraking, DateTime? rightBrakeTime,
    int? pulse)
{
    ScenarioName = scenarioName;
    ScenarioStart = scenarioStart;
    ScenarioEnd = scenarioEnd;
    CycleToCarDistance = cycleToCarDistance;
    LookRotation = lookRotation;
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

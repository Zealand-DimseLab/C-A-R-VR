using System;

//be able to write to disk
[System.Serializable]
public struct PulseData{
    //8 bytes
    public DateTime dateTime;
    //4 bytes
    public int pulse;
    //4 bytes
    public float distanceToCar;
}

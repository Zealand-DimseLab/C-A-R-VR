//be able to write to disk
[System.Serializable]
public struct CycleData{
    //4 bytes
    public float speed;
    //4 bytes
    public float steeringYRot;
    //4 bytes
    public float distanceToCurbside;
    //4 bytes. To hit the sweetspot of 16 bytes
    private float dummy;
}

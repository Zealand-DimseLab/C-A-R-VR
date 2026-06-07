using UnityEngine;

public class HeadsetController : MonoBehaviour{
    [SerializeField] private Transform bikeTransform;
    [SerializeField] private Transform recenterTarget;

    void Update(){
        transform.forward = bikeTransform.forward;
        if(OVRInput.Get(OVRInput.Button.One))
            TryRecenter();
    }

    public void TryRecenter(){
        OVRManager.display.RecenterPose();
    }
}

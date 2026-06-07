using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class RecenterPlayer : MonoBehaviour {
    public Transform bikeSeatAnchor; 
    public Transform centerEyeAnchor; // Assign TrackingSpace/CenterEyeAnchor here in Inspector

    private bool _isRecentering = false;

    [SerializeField] private OVRManager oVRManager;

    void Start() {
        // StartCoroutine(RecenterRoutine());
        oVRManager.AllowRecenter = true;
    }

    public void GetCalibrateInput(InputAction.CallbackContext context) {
        // Use 'started' to ensure it only triggers once per press
        if (context.started && !_isRecentering) {
            StartCoroutine(RecenterRoutine());
        }
    }

    public void TryReCen(){
        Debug.Log("Trying to recenter");
        if(OVRManager.isHmdPresent){
            OVRManager.instance.trackingOriginType = OVRManager.TrackingOrigin.Stationary;
            OVRManager.display.RecenterPose();
        }
    }

    private IEnumerator RecenterRoutine() {
        _isRecentering = true;

        // 1. Tell the Meta OS to reset
        OVRManager.display.RecenterPose();

        // 2. WAIT for one frame so the CenterEyeAnchor coordinates update
        yield return null;

        // 3. Now calculate the offset with FRESH data
        Vector3 offset = bikeSeatAnchor.position - centerEyeAnchor.position;
        offset.y = 0; // Keep the floor height consistent

        transform.position += offset;

        // 4. Match Rotation (Yaw only)
        float rotationAngleY = bikeSeatAnchor.eulerAngles.y - centerEyeAnchor.eulerAngles.y;
        transform.Rotate(0, rotationAngleY, 0);

        _isRecentering = false;
    }
}

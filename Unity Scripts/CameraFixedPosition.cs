using UnityEngine;

public class CameraFixedPosition : MonoBehaviour {
    [SerializeField] Transform controller;

    void Update() {
        transform.rotation = controller.rotation;
    }
}

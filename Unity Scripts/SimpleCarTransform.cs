using UnityEngine;

public class SimpleCarTransform : MonoBehaviour{
    [SerializeField] private Transform questController;
    public CharacterController characterController;
    public float speedMult = 0;

    void Update(){
        // transform.position += new Vector3(0,0,10) * speedMult * Time.deltaTime;
        characterController.Move(-questController.up * speedMult * Time.deltaTime);
    }
}

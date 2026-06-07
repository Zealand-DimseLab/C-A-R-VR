using UnityEngine;

public class MarkFieldPlacer : MonoBehaviour{
    [SerializeField] private Material mat;
    [SerializeField] private Mesh meshLOD0;

    public ComputeShader cullingShader;
    public GrassDataAsset bakedData;

    public Vector2 gridSize;
    public float gridWidth;
    public float gridLength;
    
    [ContextMenu("Place Crops")]
    public void PlaceCrops(){
        for(int i = 0; i < gridWidth; i++){
            for(int l = 0; l < gridLength; l++){
            }
        }
    }

    void OnDrawGizmos(){
        Gizmos.color = Color.yellow;
        Vector3 gridCenter = new(
                transform.position.x + (gridSize.x * gridWidth * 0.5f) - gridSize.x * 0.5f,
                0,
                transform.position.z + (gridSize.y * gridLength * 0.5f) - gridSize.y * 0.5f
                );
        Gizmos.DrawWireCube(gridCenter, new Vector3(gridWidth * gridSize.x, 0.1f, gridLength * gridSize.y));
    }
}

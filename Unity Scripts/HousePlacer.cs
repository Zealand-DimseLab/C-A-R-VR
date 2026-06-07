using UnityEngine;

public class HousePlacer : MonoBehaviour{
    [SerializeField] private GameObject houseToPlace;

    public bool drawGizmos;
    public Vector2 regionSize;

    public LayerMask groundLayer;

    [ContextMenu("Place Houses")]
    public void PlaceHouses(){
        for(int i = 0; i < regionSize.x; i++){
            for(int l = 0; l < regionSize.y; l++){
                RaycastHit hit;
                Physics.Raycast(
                        new Vector3(transform.position.x + houseToPlace.transform.localScale.x + i,
                            250f,
                            transform.position.z + houseToPlace.transform.localScale.z + l),
                        Vector3.down,
                        out hit,
                        500f,
                        groundLayer
                        );

                GameObject inst =
                    Instantiate(houseToPlace,
                            new Vector3(transform.position.x + houseToPlace.transform.localScale.x + i,hit.point.y + (houseToPlace.transform.localScale.y / 2),transform.position.z + houseToPlace.transform.localScale.z + l),
                            Quaternion.identity
                            );

                inst.transform.SetParent(transform);
            }
        }
    }
    [ContextMenu("ClearChilds")]
    public void ClearChilds(){
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
#if UNITY_EDITOR
            DestroyImmediate(transform.GetChild(i).gameObject);
#else
            Destroy(transform.GetChild(i).gameObject);
#endif
        }
    }

    void OnDrawGizmos(){
        if (!drawGizmos) return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position + new Vector3(regionSize.x / 2, 0 , regionSize.y / 2), new Vector3(regionSize.x, 0.5f, regionSize.y));
    }
}

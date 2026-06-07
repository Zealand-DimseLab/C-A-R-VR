using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class PoissonIndirectPlacer_Bake_DOD : MonoBehaviour {
    [SerializeField] private PoissonDOD poissonDOD = new();

    [Header("Resources")]
    public GameObject gameObject1;
    public GameObject gameObject2;
    public GameObject[] allGameObjects;

    private string[] outputBuffers;

    public Material material;
    public ComputeShader cullingShader;
    public GrassDataAssetDOD bakedData; // Træk din gemte fil herind

    [Header("LOD & Settings")]
    public float lod0Distance = 30f;
    public float lod1Distance = 30f;
    public float lod2Distance = 30f;
    public float maxDistance = 150f;
    public Vector2 regionSize = new Vector2(100, 100);
    public float minDistance = 1.5f;

    private ComputeBuffer grassDataBuffer;
    private MaterialPropertyBlock propBlock;

    private bool initialized = false;
    private int instanceCount = 0;

    [Header("Layer Masks")]
    public LayerMask groundMask;
    public LayerMask obstacleMask;
    public float roadClearanceRadius = 1.0f;

    [Header("Variationer")]
    public Vector2 scaleRange;

    private Camera mainCam;

    private List<GrassDataAsset> allGrass = new();
    private List<ComputeBuffer> allPositionBuffers = new();
    private int counter;
    public string newFileName;
    public string folderName;

    public bool showGizmos;

    [ContextMenu("Bage Grid")]
    public void BakeGrid(){
#if UNITY_EDITOR
        List<Vector3> positions = new();
        List<float> scales = new();
        List<float> yaws = new();

        for(float i = 0; i < regionSize.x; i = i + 0.3f){
            for(float l = 0; l < regionSize.y; l = l + 0.6f){
                Vector3 rayStart = new Vector3(
                        this.transform.position.x - this.regionSize.x * 0.5f + i,
                        this.transform.position.y + 500f,
                        this.transform.position.z - this.regionSize.y * 0.5f + l
                        );

                if (Physics.Raycast(rayStart, Vector3.down, 1000f,this.obstacleMask)) 
                    continue;

                float fScale = Random.Range(this.scaleRange.x,this.scaleRange.y);
                scales.Add(fScale);
                if (Physics.Raycast(rayStart, Vector3.down, out RaycastHit hit, 1000f)) {
                    if (Physics.CheckSphere(hit.point + Vector3.up * 0.5f, fScale, this.obstacleMask))
                        continue;

                    Vector3 scale = new Vector3(Random.Range(this.scaleRange.x,this.scaleRange.y),0.7f,Random.Range(this.scaleRange.x,this.scaleRange.y));
                    float fYaw = Random.Range(0,360);

                    positions.Add(hit.point);
                    yaws.Add(fYaw);
                }

            }
        }

        GrassDataAssetDOD asset = ScriptableObject.CreateInstance<GrassDataAssetDOD>();
        asset.positions = positions.ToArray();
        asset.yaws = yaws.ToArray();
        asset.scales = scales.ToArray();

        if(newFileName.Length <= 2) this.newFileName = transform.name;

        string path = $"Assets/{folderName}/BakedGrassData_{this.newFileName}.asset";
        UnityEditor.AssetDatabase.CreateAsset(asset, path);
        UnityEditor.AssetDatabase.SaveAssets();

        this.bakedData = asset;

        EditorUtility.SetDirty(asset);
        EditorUtility.SetDirty(this);

        if(PrefabUtility.IsPartOfPrefabAsset(this)){
            PrefabUtility.RecordPrefabInstancePropertyModifications(this);
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log("Bagning færdig! Gemt til: " + path + " - Antal: " + asset.positions.Length);
#else
        Debug.Log("Baking can only be done in the Unity Editor!");
#endif
    }

    // --- BAGE FUNKTION (KØRES KUN I EDITOR) ---
    [ContextMenu("Bage Græs Data")]
    public void BakeGrass() {
#if UNITY_EDITOR
        System.Diagnostics.Stopwatch stopwatch = new();
        poissonDOD.scaleRange = scaleRange;
        poissonDOD.minDistance = minDistance;
        poissonDOD.obstacleMask = obstacleMask;
        poissonDOD.regionSize = regionSize;
        poissonDOD.newFileName = newFileName;
        poissonDOD.folderName = folderName;
        poissonDOD.transform = transform;

        stopwatch.Start();
        GrassDataAssetDOD baked = poissonDOD.BakeGrass();
        this.bakedData = baked;
        stopwatch.Stop();
        Debug.Log(stopwatch.Elapsed);

        EditorUtility.SetDirty(baked);
        EditorUtility.SetDirty(this);

        if(PrefabUtility.IsPartOfPrefabAsset(this)){
            PrefabUtility.RecordPrefabInstancePropertyModifications(this);
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
#else
        Debug.Log("BakeGrass is only available in the Editor");
#endif
    }

    RenderVariant[] allRenderVariants;
    RenderVariant gameObject1Variant;
    RenderVariant gameObject2Variant;

    void Start() {
        System.Diagnostics.Stopwatch stopwatch = new();
        stopwatch.Start();

        this.mainCam = Camera.main;
        if (this.bakedData == null) {
            Debug.LogError("Ingen bage-data fundet! Højreklik på komponenten og vælg 'Bage Græs Data'.");
            return;
        }
        this.instanceCount = this.bakedData.positions.Length;

        LODGroup gameObjectLODGroup = gameObject1.GetComponent<LODGroup>();
        LOD[] gameObjectLODs = gameObjectLODGroup.GetLODs();
        Mesh[] meshes = new Mesh[gameObjectLODs.Length];
        gameObject1Variant.lodArgsBuffers = new ComputeBuffer[gameObjectLODs.Length];

        for(int i = 0; i < gameObjectLODs.Length; i++){
            Renderer[] mesh1Renderers = gameObjectLODs[i].renderers;

            foreach(Renderer r in mesh1Renderers){
                Mesh m = r.GetComponent<MeshFilter>().sharedMesh;
                meshes[i] = m;

                gameObject1Variant.lodArgsBuffers[i] = new ComputeBuffer(1,5 * sizeof(uint), ComputeBufferType.IndirectArguments);
                gameObject1Variant.lodArgsBuffers[i].SetData(new uint[] {m.GetIndexCount(0),0,m.GetIndexStart(0),m.GetBaseVertex(0),0});
            }
        }

        gameObject1Variant.lodMeshes = meshes;

        outputBuffers = new string[gameObject1Variant.lodMeshes.Length];
        for(int i = 0; i < gameObject1Variant.lodMeshes.Length; i++){
            outputBuffers[i] = "_OutputBufferLOD"+i;
        }

        GrassData[] toGPU = new GrassData[this.bakedData.positions.Length];
        for(int i = 0; i < this.bakedData.positions.Length; i++){
            Quaternion rot = Quaternion.Euler(0,this.bakedData.yaws[i],0);
            float s = this.bakedData.scales[i];
            Vector3 sc = new Vector3(s,s,s);

            toGPU[i].Position = this.bakedData.positions[i];
            toGPU[i].Scale = this.bakedData.scales[i];
            toGPU[i].Yaw = this.bakedData.yaws[i];
        }

        this.grassDataBuffer = new ComputeBuffer(instanceCount, 20);
        this.grassDataBuffer.SetData(toGPU);

        gameObject1Variant.visibleBuffers = new ComputeBuffer[gameObject1Variant.lodMeshes.Length];
        for(int i = 0; i < gameObject1Variant.lodMeshes.Length; i++){
            gameObject1Variant.visibleBuffers[i] = new ComputeBuffer(instanceCount,20,ComputeBufferType.Append);
        }

        this.material.EnableKeyword("UNITY_PROCEDURAL_INSTANCING_ENABLED");
        this.propBlock = new MaterialPropertyBlock();
        this.initialized = true;

        stopwatch.Stop();
        Debug.Log(stopwatch.Elapsed);
    }

    // Update() forbliver præcis som den var før...
    void Update() {
        if (!this.initialized || this.mainCam == null) return;
        if (this.propBlock == null) this.propBlock = new MaterialPropertyBlock();

        float distSq = (this.mainCam.transform.position - this.transform.position).sqrMagnitude;
        if(distSq > (this.maxDistance + this.regionSize.x) * (this.maxDistance + this.regionSize.x)) return;

        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(mainCam);
        Vector4[] frustumPlanes = new Vector4[6];
        for(int i = 0; i < 6; i++){
            frustumPlanes[i] = new Vector4(planes[i].normal.x, planes[i].normal.y, planes[i].normal.z, planes[i].distance);
        }

        // Nulstil tællere
        ComputeBuffer[] visibleBuffers = gameObject1Variant.visibleBuffers;
        for(int i = 0; i < visibleBuffers.Length; i++){
            visibleBuffers[i].SetCounterValue(0);
        }

        // 1. Dispatch Compute Shader
        int kernel = cullingShader.FindKernel("CSMain");
        this.cullingShader.SetBuffer(kernel, "_InputBuffer", grassDataBuffer);

        for(int i = 0; i < visibleBuffers.Length; i++){
            this.cullingShader.SetBuffer(kernel,outputBuffers[i],visibleBuffers[i]);
        }

        this.cullingShader.SetVectorArray("_FrustumPlanes", frustumPlanes);

        this.cullingShader.SetVector("_CameraPosition", mainCam.transform.position);
        this.cullingShader.SetFloat("_LOD0Distance", lod0Distance);
        this.cullingShader.SetFloat("_LOD1Distance", lod1Distance);
        this.cullingShader.SetFloat("_LOD2Distance", lod2Distance);
        this.cullingShader.SetFloat("_MaxDistance", maxDistance);
        this.cullingShader.SetInt("_InstanceCount", instanceCount);

        int groups = Mathf.CeilToInt(instanceCount / 64f);
        this.cullingShader.Dispatch(kernel, groups, 1, 1);

        // 2. CopyCount
        for(int l = 0; l < visibleBuffers.Length; l++){
            ComputeBuffer.CopyCount(visibleBuffers[l], gameObject1Variant.lodArgsBuffers[l],4);
        }

        Bounds drawBounds = new Bounds(transform.position, new Vector3(regionSize.x * 2f, 2000f, regionSize.y * 2f));

        // 3. Tegn LODs
        for(int i = 0; i < gameObject1Variant.lodMeshes.Length; i++){
            this.propBlock.SetBuffer("_InstanceBuffer",visibleBuffers[i]);
            Graphics.DrawMeshInstancedIndirect(gameObject1Variant.lodMeshes[i],0,material,drawBounds,gameObject1Variant.lodArgsBuffers[i],0,propBlock);
        }
    }

    void OnDisable() {
        this.grassDataBuffer?.Release();
        for(int i = 0; i < gameObject1Variant.lodArgsBuffers.Length; i++){
            gameObject1Variant.lodArgsBuffers[i]?.Release();
        }
        for(int i = 0; i < gameObject1Variant.visibleBuffers.Length; i++){
            gameObject1Variant.visibleBuffers[i]?.Release();
        }
    }

    private void OnDrawGizmos() {
        if(!showGizmos) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, new Vector3(regionSize.x, 0.1f, regionSize.y));
    }
}

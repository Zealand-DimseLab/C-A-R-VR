using System.Collections.Generic;
using UnityEngine;

public class PoissonTreePlacer_OneFile : MonoBehaviour {
    // [Header("Area")]
    // [Tooltip("Center af spawn området i world space.")]
    // public Vector3 worldCenter = Vector3.zero;

    [Tooltip("Størrelse på området i world space (X,Z).")]
    public Vector2 regionSize = new Vector2(200, 200);

    [Header("Poisson Settings")]
    public float minDistance = 6f;
    [Range(5, 60)] public int rejectionSamples = 30;
    public int seed = 0;

    [Header("Placement Filters")]
    [Tooltip("Layer for ground/terrain (træer placeres kun hvis raycast rammer disse).")]
    public LayerMask groundMask;

    [Tooltip("Layer for roads/paths (træer må IKKE overlappe disse).")]
    public LayerMask roadMask;

    [Tooltip("Radius brugt til overlap-check mod veje (cirkel omkring træ).")]
    public float roadClearanceRadius = 1.5f;

    [Tooltip("Ekstra buffer til vejkanter.")]
    public float extraRoadBuffer = 0.0f;

    [Tooltip("Max afstand raycast må søge ned efter ground.")]
    public float groundRayLength = 500f;

    [Header("Prefabs")]
    public GameObject[] treePrefabs;
    [Range(0f, 1f)] public float spawnChance = 1f;

    [Header("Random Variation")]
    public Vector2 randomScaleRange = new Vector2(0.9f, 1.3f);
    public bool randomYRotation = true;

    [Header("Debug")]
    public bool clearPreviousChildren = true;
    public bool drawGizmos = true;

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

    [ContextMenu("Generate Trees")]
    public void GenerateTrees()
    {
        if (treePrefabs == null || treePrefabs.Length == 0)
        {
            Debug.LogWarning("Ingen treePrefabs sat.");
            return;
        }

        if (clearPreviousChildren)
        {
            for (int i = transform.childCount - 1; i >= 0; i--)
            {
#if UNITY_EDITOR
                DestroyImmediate(transform.GetChild(i).gameObject);
#else
                Destroy(transform.GetChild(i).gameObject);
#endif
            }
        }

        var points = GeneratePoissonPoints(minDistance, regionSize, rejectionSamples, seed);

        int spawned = 0;
        foreach (var p in points)
        {
            if (Random.value > spawnChance)
                continue;

            // Poisson point (0..regionSize) -> world XZ omkring worldCenter
            Vector3 rayStart = new Vector3(
                    // worldCenter.x - regionSize.x * 0.5f + p.x,
                    // worldCenter.y + 200f,
                    // worldCenter.z - regionSize.y * 0.5f + p.y
                    this.transform.position.x - this.regionSize.x * 0.5f + p.x,
                    this.transform.position.y + 500f,
                    this.transform.position.z - this.regionSize.y * 0.5f + p.y
                    );

            // Find ground
            if (!Physics.Raycast(rayStart, Vector3.down, out RaycastHit hit, groundRayLength, groundMask, QueryTriggerInteraction.Ignore))
                continue;

            Vector3 placePos = hit.point;

            // Undgå veje (overlap-sphere mod road layer)
            float checkRadius = roadClearanceRadius + extraRoadBuffer;
            bool hitsRoad = Physics.CheckSphere(placePos + Vector3.up * 0.5f, checkRadius, roadMask, QueryTriggerInteraction.Ignore);
            if (hitsRoad)
                continue;

            // Spawn
            GameObject prefab = treePrefabs[Random.Range(0, treePrefabs.Length)];
            float scale = Random.Range(randomScaleRange.x, randomScaleRange.y);
            GameObject tree = Instantiate(prefab, placePos, Quaternion.identity, transform);

            if (randomYRotation)
                tree.transform.rotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);

            float s = Random.Range(randomScaleRange.x, randomScaleRange.y);
            tree.transform.localScale = tree.transform.localScale * s;

            spawned++;
        }

        Debug.Log($"PoissonTreePlacer: Spawned {spawned} træer (fra {points.Count} punkter).");
    }

    // -------------------------
    // Poisson Disc Sampling 2D
    // -------------------------
    private static List<Vector2> GeneratePoissonPoints(float radius, Vector2 region, int rejectSamples, int seed)
    {
        if (radius <= 0f) radius = 0.01f;

        System.Random prng = (seed == 0) ? new System.Random() : new System.Random(seed);

        float cellSize = radius / Mathf.Sqrt(2f);
        int gridW = Mathf.CeilToInt(region.x / cellSize);
        int gridH = Mathf.CeilToInt(region.y / cellSize);

        int[,] grid = new int[gridW, gridH];
        for (int x = 0; x < gridW; x++)
            for (int y = 0; y < gridH; y++)
                grid[x, y] = -1;

        List<Vector2> points = new List<Vector2>();
        List<Vector2> spawnPoints = new List<Vector2>();

        Vector2 first = new Vector2(
                (float)prng.NextDouble() * region.x,
                (float)prng.NextDouble() * region.y
                );

        points.Add(first);
        spawnPoints.Add(first);
        grid[(int)(first.x / cellSize), (int)(first.y / cellSize)] = 0;

        while (spawnPoints.Count > 0)
        {
            int spawnIndex = prng.Next(0, spawnPoints.Count);
            Vector2 centre = spawnPoints[spawnIndex];
            bool accepted = false;

            for (int i = 0; i < rejectSamples; i++)
            {
                float angle = (float)prng.NextDouble() * Mathf.PI * 2f;
                Vector2 dir = new Vector2(Mathf.Sin(angle), Mathf.Cos(angle));
                float dist = radius * (1f + (float)prng.NextDouble()); // radius..2*radius
                Vector2 candidate = centre + dir * dist;

                if (IsValid(candidate, region, cellSize, radius, points, grid))
                {
                    points.Add(candidate);
                    spawnPoints.Add(candidate);
                    grid[(int)(candidate.x / cellSize), (int)(candidate.y / cellSize)] = points.Count - 1;
                    accepted = true;
                    break;
                }
            }

            if (!accepted)
                spawnPoints.RemoveAt(spawnIndex);
        }

        return points;
    }

    private static bool IsValid(Vector2 c, Vector2 region, float cellSize, float radius, List<Vector2> points, int[,] grid)
    {
        if (c.x < 0 || c.y < 0 || c.x >= region.x || c.y >= region.y)
            return false;

        int cellX = (int)(c.x / cellSize);
        int cellY = (int)(c.y / cellSize);

        int startX = Mathf.Max(0, cellX - 2);
        int endX = Mathf.Min(grid.GetLength(0) - 1, cellX + 2);
        int startY = Mathf.Max(0, cellY - 2);
        int endY = Mathf.Min(grid.GetLength(1) - 1, cellY + 2);

        float r2 = radius * radius;

        for (int x = startX; x <= endX; x++)
        {
            for (int y = startY; y <= endY; y++)
            {
                int idx = grid[x, y];
                if (idx != -1)
                {
                    Vector2 p = points[idx];
                    if ((c - p).sqrMagnitude < r2)
                        return false;
                }
            }
        }

        return true;
    }

    private void OnDrawGizmosSelected()
    {
        if (!drawGizmos) return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(this.transform.position, new Vector3(regionSize.x, 0.1f, regionSize.y));
    }
    [Header("GPU Instancing")]
    public Mesh grassMesh;
    public Material grassMaterial;
    private List<Vector4> grassData = new List<Vector4>();
    private ComputeBuffer positionBuffer;

    [ContextMenu("Generate Grass Data")]
    public void GenerateGrass()
    {
        // 1. Ryd gammel data
        grassData.Clear();

        // 2. Hent dine Poisson punkter (din eksisterende logik)
        var points = GeneratePoissonPoints(minDistance, regionSize, rejectionSamples, seed);

        foreach (var p in points)
        {
            // 3. Find ground via Raycast (din eksisterende logik)
            Vector3 rayStart = new Vector3(
                    // worldCenter.x - regionSize.x * 0.5f + p.x,
                    // worldCenter.y + 200f,
                    // worldCenter.z - regionSize.y * 0.5f + p.y
                    this.transform.position.x - regionSize.x * 0.5f + p.x,
                    this.transform.position.y + 500f,
                    this.transform.position.z - regionSize.y * 0.5f + p.y
                    );

            if (Physics.Raycast(rayStart, Vector3.down, out RaycastHit hit, groundRayLength, groundMask))
            {
                float scale = Random.Range(randomScaleRange.x, randomScaleRange.y);

                // 4. PAK DATA: x, y, z er position, w er skala
                grassData.Add(new Vector4(hit.point.x, hit.point.y, hit.point.z, scale));
            }
        }

        // 5. Send til GPU (hvis du bruger en Compute Shader løsning)
        UpdateBuffer();
        Debug.Log($"Genereret {grassData.Count} græspunkter klar til GPU.");
    }

    void UpdateBuffer()
    {
        if (grassData.Count == 0) return;

        // Ryd op hvis bufferen findes
        if (positionBuffer != null) positionBuffer.Release();

        // Opret buffer: (antal elementer) * (størrelse af en Vector4, som er 16 bytes)
        positionBuffer = new ComputeBuffer(grassData.Count, 16);
        positionBuffer.SetData(grassData.ToArray());

        // Fortæl dit materiale, hvor bufferen er
        grassMaterial.SetBuffer("_PositionBuffer", positionBuffer);
    }

    // Husk at rydde op i hukommelsen når spillet slukker
    void OnDisable() {
        if (positionBuffer != null) positionBuffer.Release();
    }
    void Update() {
        if (positionBuffer == null || grassData.Count == 0) return;

        // Tegn alle græsstrå i ét kald!
        // Vi bruger 0 som submesh index og sender vores materiale
        Graphics.DrawMeshInstancedProcedural(grassMesh,
                0,
                grassMaterial,
                new Bounds(Vector3.one, Vector3.one * 10000f),
                grassData.Count
                );
    }
}

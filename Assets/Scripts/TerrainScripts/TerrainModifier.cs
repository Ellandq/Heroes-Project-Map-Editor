using UnityEngine;

public class TerrainModifier : MonoBehaviour
{
    public KeyCode raiseKey = KeyCode.R;
    public KeyCode lowerKey = KeyCode.T;
    public float raiseHeight = 3f;
    public float lowerHeight = 3f;
    public float raiseRadius = 5f;

    public float defaultHeight = 0f;
    public int modifyRadius = 5;

    private Vector3[] prevSegments;


    private Terrain terrain;

    void Start()
    {
        terrain = GetComponent<Terrain>();
        ResetTerrain();
    }

    void Update()
    {
        if (Input.GetKey(raiseKey)){
            RaiseTerrain();
        }else if (Input.GetKey(lowerKey)){
            LowerTerrain();
        }
        // Cast a ray from the camera to the terrain
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        int layersToHit = LayerMask.GetMask("Terrain");

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layersToHit)){
            GameObject prevCircle = GameObject.Find("Circle");
            DrawCircle(hit.point + new Vector3(0, raiseHeight, 0), (modifyRadius / 2), 32, Color.red, 0.1f, prevCircle);
        }
    }


    public void ChangeSize(Vector2Int size){
        // terrain.
    }

    void RaiseTerrain()
    {
        // Cast a ray from the camera to the terrain
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        int layersToHit = LayerMask.GetMask("Terrain");

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layersToHit))
        {
            // Get the position of the terrain at the ray hit point
            Vector3 terrainPos = hit.point - terrain.transform.position;
            terrainPos.y = terrain.SampleHeight(terrainPos);

            // Raise the terrain at the ray hit point
            float[,] heights = terrain.terrainData.GetHeights(0, 0, terrain.terrainData.heightmapResolution, terrain.terrainData.heightmapResolution);
            int centerX = Mathf.RoundToInt((terrainPos.x / terrain.terrainData.size.x) * (terrain.terrainData.heightmapResolution - 1));
            int centerZ = Mathf.RoundToInt((terrainPos.z / terrain.terrainData.size.z) * (terrain.terrainData.heightmapResolution - 1));
            float[,] modifyHeights = new float[modifyRadius * 2 + 1, modifyRadius * 2 + 1];

            for (int z = 0; z <= modifyRadius * 2; z++)
            {
                for (int x = 0; x <= modifyRadius * 2; x++)
                {
                    int currentX = centerX + x - modifyRadius;
                    int currentZ = centerZ + z - modifyRadius;
                    if (currentX >= 0 && currentX < terrain.terrainData.heightmapResolution && currentZ >= 0 && currentZ < terrain.terrainData.heightmapResolution)
                    {
                        float distance = Vector2.Distance(new Vector2(centerX, centerZ), new Vector2(currentX, currentZ));
                        float normalizedDistance = Mathf.Clamp01(1f - (distance / modifyRadius));
                        modifyHeights[z, x] = normalizedDistance;
                    }
                }
            }

            for (int z = 0; z < modifyRadius * 2 + 1; z++)
            {
                for (int x = 0; x < modifyRadius * 2 + 1; x++)
                {
                    int currentX = centerX + x - modifyRadius;
                    int currentZ = centerZ + z - modifyRadius;
                    if (currentX >= 0 && currentX < terrain.terrainData.heightmapResolution && currentZ >= 0 && currentZ < terrain.terrainData.heightmapResolution)
                    {
                        heights[currentZ, currentX] += modifyHeights[z, x] * raiseHeight;
                        heights[currentZ, currentX] = Mathf.Clamp(heights[currentZ, currentX], 0f, 1f);
                    }
                }
            }

            terrain.terrainData.SetHeights(0, 0, heights);
        }
    }

    void LowerTerrain()
    {
        // Cast a ray from the camera to the terrain
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        int layersToHit = LayerMask.GetMask("Terrain");

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layersToHit))
        {
            // Get the position of the terrain at the ray hit point
            Vector3 terrainPos = hit.point - terrain.transform.position;
            terrainPos.y = terrain.SampleHeight(terrainPos);

            // Lower the terrain at the ray hit point
            float[,] heights = terrain.terrainData.GetHeights(0, 0, terrain.terrainData.heightmapResolution, terrain.terrainData.heightmapResolution);
            int centerX = Mathf.RoundToInt((terrainPos.x / terrain.terrainData.size.x) * (terrain.terrainData.heightmapResolution - 1));
            int centerZ = Mathf.RoundToInt((terrainPos.z / terrain.terrainData.size.z) * (terrain.terrainData.heightmapResolution - 1));
            float[,] modifyHeights = new float[modifyRadius * 2 + 1, modifyRadius * 2 + 1];

            for (int z = 0; z <= modifyRadius * 2; z++)
            {
                for (int x = 0; x <= modifyRadius * 2; x++)
                {
                    int currentX = centerX + x - modifyRadius;
                    int currentZ = centerZ + z - modifyRadius;
                    if (currentX >= 0 && currentX < terrain.terrainData.heightmapResolution && currentZ >= 0 && currentZ < terrain.terrainData.heightmapResolution)
                    {
                        float distance = Vector2.Distance(new Vector2(centerX, centerZ), new Vector2(currentX, currentZ));
                        float normalizedDistance = Mathf.Clamp01(1f - (distance / modifyRadius));
                        modifyHeights[z, x] = -normalizedDistance;
                    }
                }
            }

            for (int z = 0; z < modifyRadius * 2 + 1; z++)
            {
                for (int x = 0; x < modifyRadius * 2 + 1; x++)
                {
                    int currentX = centerX + x - modifyRadius;
                    int currentZ = centerZ + z - modifyRadius;
                    if (currentX >= 0 && currentX < terrain.terrainData.heightmapResolution && currentZ >= 0 && currentZ < terrain.terrainData.heightmapResolution)
                    {
                        heights[currentZ, currentX] += modifyHeights[z, x] * lowerHeight;
                        heights[currentZ, currentX] = Mathf.Clamp(heights[currentZ, currentX], 0f, 1f);
                    }
                }
            }

            terrain.terrainData.SetHeights(0, 0, heights);
        }
    }








    public void ResetTerrain()
    {
        int heightmapResolution = terrain.terrainData.heightmapResolution;
        float[,] heights = new float[heightmapResolution, heightmapResolution];

        for (int i = 0; i < heightmapResolution; i++)
        {
            for (int j = 0; j < heightmapResolution; j++)
            {
                heights[i, j] = defaultHeight;
            }
        }

        terrain.terrainData.SetHeights(0, 0, heights);
    }

    public static void DrawCircle(Vector3 center, float radius, int numSegments, Color color, float duration, GameObject prevCircle = null)
    {
        Vector3[] segments = new Vector3[numSegments];
        float angle = 0f;
        float angleDelta = (Mathf.PI * 2f) / numSegments;

        for (int i = 0; i < numSegments; i++)
        {
            float x = Mathf.Sin(angle) * radius;
            float z = Mathf.Cos(angle) * radius;
            segments[i] = new Vector3(center.x + x, center.y, center.z + z);
            angle += angleDelta;
        }

        GameObject circle = new GameObject("Circle");
        LineRenderer lineRenderer = circle.AddComponent<LineRenderer>();
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = color;
        lineRenderer.endColor = color;
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.positionCount = numSegments;
        lineRenderer.useWorldSpace = true;
        lineRenderer.SetPositions(segments);

        if (prevCircle != null)
        {
            GameObject.Destroy(prevCircle);
        }

        GameObject.Destroy(circle, duration);
    }



}

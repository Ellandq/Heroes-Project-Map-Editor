using UnityEngine;

public class TerrainModifier : MonoBehaviour
{
    public KeyCode raiseKey = KeyCode.R;
    public float raiseHeight = 3f;
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
        if (Input.GetKey(raiseKey))
        {
            RaiseTerrain();
        }
        // Cast a ray from the camera to the terrain
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        int layersToHit = LayerMask.GetMask("Terrain");

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layersToHit)){
            GameObject prevCircle = GameObject.Find("Circle");
            DrawCircle(hit.point + new Vector3(0, raiseHeight, 0), modifyRadius, 30, Color.red, 0.1f, prevCircle);
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
            Vector3 terrainPos = hit.point;
            terrainPos.y = terrain.SampleHeight(terrainPos);

            // Raise the terrain at the ray hit point
            float[,] heights = terrain.terrainData.GetHeights(0, 0, terrain.terrainData.heightmapResolution, terrain.terrainData.heightmapResolution);
            int centerX = Mathf.RoundToInt((terrainPos.x / terrain.terrainData.size.x) * (terrain.terrainData.heightmapResolution - 1));
            int centerZ = Mathf.RoundToInt((terrainPos.z / terrain.terrainData.size.z) * (terrain.terrainData.heightmapResolution - 1));

            for (int z = centerZ - modifyRadius; z <= centerZ + modifyRadius; z++)
            {
                for (int x = centerX - modifyRadius; x <= centerX + modifyRadius; x++)
                {
                    if (z >= 0 && z < terrain.terrainData.heightmapResolution && x >= 0 && x < terrain.terrainData.heightmapResolution)
                    {
                        float distance = Mathf.Sqrt((x - centerX) * (x - centerX) + (z - centerZ) * (z - centerZ));
                        float normalizedDistance = distance / modifyRadius;
                        float amountToRaise = raiseHeight * (1f - normalizedDistance);

                        heights[z, x] += amountToRaise;
                        heights[z, x] = Mathf.Clamp(heights[z, x], 0f, 1f);
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

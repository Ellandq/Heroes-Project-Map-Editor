using UnityEngine;

public class TerrainModifier : MonoBehaviour
{
    private float defaultHeight = 0.004f;

    private Terrain terrain;
    
    private void Awake()
    {
        terrain = GetComponent<Terrain>();
        ResetTerrain();
    }

    public void ChangeTerrainSize(int gridSize)
    {
        float terrainSize = gridSize * 5f;
        terrain.terrainData.size = new Vector3(terrainSize, terrain.terrainData.size.y, terrainSize);
    }


    
    public void SetGridCellTerrain(Vector2Int gridCellPosition, float heightLevel)
    {
        // Get the heightmap data for the terrain
        float[,] heights = terrain.terrainData.GetHeights(0, 0, terrain.terrainData.heightmapResolution, terrain.terrainData.heightmapResolution);

        // Calculate the size of a grid cell in the heightmap data
        int cellSize = Mathf.CeilToInt(terrain.terrainData.heightmapResolution / (float)terrain.terrainData.size.x * 5);
        int offsetY = Mathf.FloorToInt(gridCellPosition.x * (cellSize - ((float)terrain.terrainData.heightmapResolution / GameGrid.Instance.GetGridWidth())));
        int offsetX = Mathf.FloorToInt(gridCellPosition.y * (cellSize - ((float)terrain.terrainData.heightmapResolution / GameGrid.Instance.GetGridLength())));
        
        // Calculate the new height of the terrain
        int xStart = Mathf.FloorToInt(gridCellPosition.y * cellSize) - offsetX;
        int yStart = Mathf.FloorToInt(gridCellPosition.x * cellSize) - offsetY;

        float newHeight = 0;
        if (xStart >= 0 && xStart < terrain.terrainData.heightmapResolution &&
            yStart >= 0 && yStart < terrain.terrainData.heightmapResolution)
        {
            newHeight = heightLevel * defaultHeight;
        }

        // Ensure that the new height does not exceed the maximum height of the terrain
        newHeight = Mathf.Clamp(newHeight, 0, 1);

        // Raise the terrain
        for (int x = xStart; x < xStart + cellSize; x++)
        {
            for (int y = yStart; y < yStart + cellSize; y++)
            {
                if (x >= 0 && x < terrain.terrainData.heightmapResolution &&
                    y >= 0 && y < terrain.terrainData.heightmapResolution)
                {
                    heights[x, y] = newHeight;
                }
            }
        }

        // Apply the changes to the terrain
        terrain.terrainData.SetHeights(0, 0, heights);
    }

    public void CreateSlope(Vector2Int gridCellPosition, float middleHeight, SlopeType slopeType)
    {
        if (slopeType == SlopeType.None) return;
        // Get the heightmap data for the terrain
        float[,] heights = terrain.terrainData.GetHeights(0, 0, terrain.terrainData.heightmapResolution, terrain.terrainData.heightmapResolution);

        // Calculate the size of a grid cell in the heightmap data
        int cellSize = Mathf.CeilToInt(terrain.terrainData.heightmapResolution / (float)terrain.terrainData.size.x * 5);
        int offsetY = Mathf.FloorToInt(gridCellPosition.x * (cellSize - ((float)terrain.terrainData.heightmapResolution / GameGrid.Instance.GetGridWidth())));
        int offsetX = Mathf.FloorToInt(gridCellPosition.y * (cellSize - ((float)terrain.terrainData.heightmapResolution / GameGrid.Instance.GetGridLength())));

        // Calculate the new height of the terrain
        int xStart = Mathf.FloorToInt(gridCellPosition.y * cellSize) - offsetX;
        int yStart = Mathf.FloorToInt(gridCellPosition.x * cellSize) - offsetY;

        float middleHeightScaled = middleHeight * defaultHeight;
        float minHeight = Mathf.Max(0, middleHeightScaled - 0.5f * defaultHeight);
        float maxHeight = Mathf.Min(1, middleHeightScaled + 0.5f * defaultHeight);

        // Apply the slope based on the selected SlopeType
        for (int x = xStart; x < xStart + cellSize; x++)
        {
            for (int y = yStart; y < yStart + cellSize; y++)
            {
                if (x >= 0 && x < terrain.terrainData.heightmapResolution &&
                    y >= 0 && y < terrain.terrainData.heightmapResolution)
                {
                    float slopeHeight = 0f;

                    switch (slopeType)
                    {
                        case SlopeType.BottomToTop:
                            slopeHeight = minHeight + (x - xStart) * (maxHeight - minHeight) / cellSize;
                            break;
                        case SlopeType.TopToBottom:
                            slopeHeight = maxHeight - (x - xStart) * (maxHeight - minHeight) / cellSize;
                            break;
                        case SlopeType.LeftToRight:
                            slopeHeight = minHeight + (y - yStart) * (maxHeight - minHeight) / cellSize;
                            break;
                        case SlopeType.RightToLeft:
                            slopeHeight = maxHeight - (y - yStart) * (maxHeight - minHeight) / cellSize;
                            break;
                    }

                    heights[x, y] = Mathf.Clamp(slopeHeight, 0, 1);
                }
            }
        }

        // Apply the changes to the terrain
        terrain.terrainData.SetHeights(0, 0, heights);
    }

    public void CreateCornerSlope(Vector2Int gridCellPosition, float middleHeight, SlopeType slopeType)
    {
        if (slopeType == SlopeType.None) return;
        // Get the heightmap data for the terrain
        float[,] heights = terrain.terrainData.GetHeights(0, 0, terrain.terrainData.heightmapResolution, terrain.terrainData.heightmapResolution);

        // Calculate the size of a grid cell in the heightmap data
        int cellSize = Mathf.CeilToInt(terrain.terrainData.heightmapResolution / (float)terrain.terrainData.size.x * 5);
        int offsetY = Mathf.FloorToInt(gridCellPosition.x * (cellSize - ((float)terrain.terrainData.heightmapResolution / GameGrid.Instance.GetGridWidth())));
        int offsetX = Mathf.FloorToInt(gridCellPosition.y * (cellSize - ((float)terrain.terrainData.heightmapResolution / GameGrid.Instance.GetGridLength())));

        // Calculate the new height of the terrain
        int xStart = Mathf.FloorToInt(gridCellPosition.y * cellSize) - offsetX;
        int yStart = Mathf.FloorToInt(gridCellPosition.x * cellSize) - offsetY;

        float middleHeightScaled = middleHeight * defaultHeight;
        float minHeight = Mathf.Max(0, middleHeightScaled - (defaultHeight / 2));
        float maxHeight = Mathf.Min(1, middleHeightScaled + (defaultHeight / 2));

        // Apply the slope based on the selected SlopeType
        for (int x = xStart; x < xStart + cellSize; x++)
        {
            for (int y = yStart; y < yStart + cellSize; y++)
            {
                if (x >= 0 && x < terrain.terrainData.heightmapResolution &&
                    y >= 0 && y < terrain.terrainData.heightmapResolution)
                {
                    float slopeHeight = 0f;

                    switch (slopeType)
                    {
                        case SlopeType.TopRightToBottomLeft:
                            slopeHeight = Mathf.Lerp(maxHeight, minHeight, Mathf.Sqrt((x - xStart) * (x - xStart) + (y - yStart) * (y - yStart)) / cellSize);
                            break;
                        case SlopeType.BottomRightToTopLeft:
                            slopeHeight = Mathf.Lerp(maxHeight, minHeight, Mathf.Sqrt((cellSize - (x - xStart)) * (cellSize - (x - xStart)) + (y - yStart) * (y - yStart)) / cellSize);
                            break;
                        case SlopeType.TopLeftToBottomRight:
                            slopeHeight = Mathf.Lerp(maxHeight, minHeight, Mathf.Sqrt((cellSize - (y - yStart)) * (cellSize - (y - yStart)) + (x - xStart) * (x - xStart)) / cellSize);
                            break;
                        case SlopeType.BottomLeftToTopRight:
                            slopeHeight = Mathf.Lerp(maxHeight, minHeight, Mathf.Sqrt((cellSize - (y - yStart)) * (cellSize - (y - yStart)) + (cellSize - (x - xStart)) * (cellSize - (x - xStart))) / cellSize);
                            break;
                    }

                    heights[x, y] = Mathf.Clamp(slopeHeight, 0, 1);
                }
            }
        }
        // Apply the changes to the terrain
        terrain.terrainData.SetHeights(0, 0, heights);
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
}

public enum SlopeType { 
    None,
    LeftToRight, RightToLeft, BottomToTop, TopToBottom,
    BottomLeftToTopRight, BottomRightToTopLeft, TopLeftToBottomRight, TopRightToBottomLeft
}
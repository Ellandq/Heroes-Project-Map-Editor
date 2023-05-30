using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class TerrainModifier : MonoBehaviour
{
    private Terrain terrain;

    private const float defaultHeight = 0.004f;

    [Header ("Map Size")]
    private int cellSize;
    private int gridSize;
    
    private void Awake()
    {
        terrain = GetComponent<Terrain>();
    }

    public void ChangeTerrainSize(int gridSize)
    {  
        if (gridSize < 128){
            terrain.terrainData.heightmapResolution = 1025;
        }else if (gridSize < 256){
            terrain.terrainData.heightmapResolution = 2049;
        }else{
            terrain.terrainData.heightmapResolution = 4097;
            Debug.Log("Warning! HeightMap Resolution of this size can drastically impatct performance");
        }

        float terrainSize = gridSize * 5f;
        terrain.terrainData.size = new Vector3(terrainSize, terrain.terrainData.size.y, terrainSize);
        cellSize = Mathf.FloorToInt(terrain.terrainData.heightmapResolution / (float)terrain.terrainData.size.x * 5);
        this.gridSize = gridSize;

        ResetTerrain();

        Debug.Log("CellSize: " + cellSize);
    }

    public void SetGridCellTerrain(List<Vector2Int> gridCellPosition, float heightLevel, float multiplier)
    {
        int resolution = terrain.terrainData.heightmapResolution;
        float[,] heights = terrain.terrainData.GetHeights(0, 0, resolution, resolution);
        
        Parallel.For(0, gridCellPosition.Count, i =>
        {
            int offsetY = Mathf.FloorToInt(gridCellPosition[i].x * (cellSize - ((float)resolution / gridSize)));
            int offsetX = Mathf.FloorToInt(gridCellPosition[i].y * (cellSize - ((float)resolution / gridSize)));

            int xStart = Mathf.FloorToInt(gridCellPosition[i].y * cellSize) - offsetX;
            int yStart = Mathf.FloorToInt(gridCellPosition[i].x * cellSize) - offsetY;
            int xEnd = Mathf.Min(xStart + cellSize, resolution - 1);
            int yEnd = Mathf.Min(yStart + cellSize, resolution - 1);

            float newHeight = multiplier * defaultHeight;
            for (int x = xStart; x < xEnd; x++)
            {
                for (int y = yStart; y < yEnd; y++)
                {
                    heights[x, y] += newHeight;
                }
            }

        });

        for (int i = 0; i < gridCellPosition.Count; i++){
            GameGrid.Instance.GetGridCellInformation(gridCellPosition[i]).ChangeCellLevel(heightLevel);
        }
        

        terrain.terrainData.SetHeights(0, 0, heights);
    }

    public void CreateSlope(Vector2Int gridCellPosition, float middleHeight, SlopeType slopeType)
    {
        if (slopeType == SlopeType.None) return;

        // Get the heightmap data for the terrain
        int resolution = terrain.terrainData.heightmapResolution;
        float[,] heights = terrain.terrainData.GetHeights(0, 0, resolution, resolution);

        // Calculate the size of a grid cell in the heightmap data
        int offsetY = Mathf.FloorToInt(gridCellPosition.x * (cellSize - ((float)resolution / gridSize)));
        int offsetX = Mathf.FloorToInt(gridCellPosition.y * (cellSize - ((float)resolution / gridSize)));

        // Calculate the new height of the terrain
        int xStart = Mathf.FloorToInt(gridCellPosition.y * cellSize) - offsetX;
        int yStart = Mathf.FloorToInt(gridCellPosition.x * cellSize) - offsetY;

        float middleHeightScaled = middleHeight * defaultHeight;
        float minHeight = Mathf.Max(0, middleHeightScaled - 0.5f * defaultHeight);
        float maxHeight = Mathf.Min(1, middleHeightScaled + 0.5f * defaultHeight);

        // Apply the slope based on the selected SlopeType
        Parallel.For(xStart, xStart + cellSize, x =>
        {
            for (int y = yStart; y < yStart + cellSize; y++)
            {
                if (x >= 0 && x < resolution &&
                    y >= 0 && y < resolution)
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
        });

        // Apply the changes to the terrain
        terrain.terrainData.SetHeights(0, 0, heights);
    }

    public void CreateCornerSlope(Vector2Int gridCellPosition, float middleHeight, SlopeType slopeType)
    {
        if (slopeType == SlopeType.None) return;
        // Get the heightmap data for the terrain
        int resolution = terrain.terrainData.heightmapResolution;
        float[,] heights = terrain.terrainData.GetHeights(0, 0, resolution, resolution);

        // Calculate the size of a grid cell in the heightmap data
        int cellSize = Mathf.CeilToInt(resolution / (float)terrain.terrainData.size.x * 5);
        int offsetY = Mathf.FloorToInt(gridCellPosition.x * (cellSize - ((float)resolution / GameGrid.Instance.GetGridSize())));
        int offsetX = Mathf.FloorToInt(gridCellPosition.y * (cellSize - ((float)resolution / GameGrid.Instance.GetGridSize())));

        // Calculate the new height of the terrain
        int xStart = Mathf.FloorToInt(gridCellPosition.y * cellSize) - offsetX;
        int yStart = Mathf.FloorToInt(gridCellPosition.x * cellSize) - offsetY;

        float middleHeightScaled = middleHeight * defaultHeight;
        float minHeight = Mathf.Max(0, middleHeightScaled - (defaultHeight / 2));
        float maxHeight = Mathf.Min(1, middleHeightScaled + (defaultHeight / 2));

        // Apply the slope based on the selected SlopeType
        Parallel.For(xStart, xStart + cellSize, x =>
        {
            for (int y = yStart; y < yStart + cellSize; y++)
            {
                if (x >= 0 && x < resolution &&
                    y >= 0 && y < resolution)
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
        });
        // Apply the changes to the terrain
        terrain.terrainData.SetHeights(0, 0, heights);
    }

    public void RemoveSlope (Vector2Int gridCellPosition, float heightLevel){
        int resolution = terrain.terrainData.heightmapResolution;
        float[,] heights = terrain.terrainData.GetHeights(0, 0, resolution, resolution);
        
        int offsetY = Mathf.FloorToInt(gridCellPosition.x * (cellSize - ((float)resolution / gridSize)));
        int offsetX = Mathf.FloorToInt(gridCellPosition.y * (cellSize - ((float)resolution / gridSize)));

        int xStart = Mathf.FloorToInt(gridCellPosition.y * cellSize) - offsetX;
        int yStart = Mathf.FloorToInt(gridCellPosition.x * cellSize) - offsetY;
        int xEnd = Mathf.Min(xStart + cellSize, resolution - 1);
        int yEnd = Mathf.Min(yStart + cellSize, resolution - 1);

        float newHeight = 0;
        if (xStart >= 0 && xStart < resolution &&
            yStart >= 0 && yStart < resolution)
        {
            newHeight = heightLevel * defaultHeight;
        }

        newHeight = Mathf.Clamp(newHeight, 0, 1);

        for (int x = xStart; x < xEnd; x++)
        {
            for (int y = yStart; y < yEnd; y++)
            {
                heights[x, y] = newHeight;
            }
        }

        GameGrid.Instance.GetGridCellInformation(gridCellPosition).ChangeCellLevel(heightLevel);
        terrain.terrainData.SetHeights(0, 0, heights);
    }

    public void PaintTerrain(Vector3 worldPosition, float radius, TerrainType terrainType)
    {
        int alphamapWidth = terrain.terrainData.alphamapWidth;
        int alphamapHeight = terrain.terrainData.alphamapHeight;

        // Convert world position to terrain position
        Vector3 terrainPos = worldPosition - terrain.transform.position;
        Vector3 normalizedPos = new Vector3(terrainPos.x / terrain.terrainData.size.x, 0f, terrainPos.z / terrain.terrainData.size.z);

        // Calculate the brush radius in terrain space
        float brushRadiusInTerrainSpace = radius / terrain.terrainData.size.x;

        // Calculate the brush center indices
        int brushCenterX = Mathf.FloorToInt(normalizedPos.x * alphamapWidth);
        int brushCenterZ = Mathf.FloorToInt(normalizedPos.z * alphamapHeight);

        // Check if the terrain type is within the range of terrain layers
        if (terrainType < 0 || (int)terrainType >= terrain.terrainData.terrainLayers.Length)
        {
            Debug.LogWarning("Invalid terrain type!");
            return;
        }

        // Get the terrain layer based on the terrain type
        TerrainLayer terrainLayer = terrain.terrainData.terrainLayers[(int)terrainType];

        // Get the alphamaps
        float[,,] alphamaps = terrain.terrainData.GetAlphamaps(0, 0, alphamapWidth, alphamapHeight);

        // Create a modified copy of the alphamaps
        float[,,] modifiedAlphamaps = alphamaps.Clone() as float[,,];

        // Calculate the radius in pixel coordinates
        int brushRadiusInPixels = Mathf.RoundToInt(radius / terrain.terrainData.size.x * alphamapWidth);

        // Loop through the pixels within the brush circle and modify the modifiedAlphamaps
        for (int row = 0; row < alphamapHeight; row++)
        {
            for (int column = 0; column < alphamapWidth; column++)
            {
                // Calculate the distance from the brush center
                float distance = Vector2.Distance(new Vector2(column, row), new Vector2(brushCenterX, brushCenterZ));

                // Check if the pixel is within the brush radius
                if (distance <= brushRadiusInPixels)
                {
                    // Calculate the falloff value based on the distance from the brush center
                    float falloff = 1f - Mathf.Clamp01(distance / brushRadiusInPixels);

                    // Blend the existing and new values with falloff
                    for (int layer = 0; layer < terrain.terrainData.terrainLayers.Length; layer++)
                    {
                        float oldValue = modifiedAlphamaps[row, column, layer];
                        float newValue = (layer == (int)terrainType) ? 1f : 0f;
                        modifiedAlphamaps[row, column, layer] = Mathf.Lerp(oldValue, newValue, falloff);
                    }
                }
            }
        }

        // Apply the modified alphamaps to the terrain
        terrain.terrainData.SetAlphamaps(0, 0, modifiedAlphamaps);
    }

    public void ModifyTerrain(Vector3 worldPosition, float radius, int multiplier)
    {
        int heightmapWidth = terrain.terrainData.heightmapResolution;
        int heightmapHeight = terrain.terrainData.heightmapResolution;

        // Convert world position to terrain position
        Vector3 terrainPos = worldPosition - terrain.transform.position;
        Vector3 normalizedPos = new Vector3(terrainPos.x / terrain.terrainData.size.x, 0f, terrainPos.z / terrain.terrainData.size.z);

        // Calculate the brush radius in terrain space
        float brushRadiusInTerrainSpace = radius / terrain.terrainData.size.x;

        // Calculate the brush center indices
        int brushCenterX = Mathf.FloorToInt(normalizedPos.x * heightmapWidth);
        int brushCenterZ = Mathf.FloorToInt(normalizedPos.z * heightmapHeight);

        // Get the heights
        float[,] heights = terrain.terrainData.GetHeights(0, 0, heightmapWidth, heightmapHeight);

        // Create a modified copy of the heights
        float[,] modifiedHeights = heights.Clone() as float[,];

        // Calculate the radius in pixel coordinates
        int brushRadiusInPixels = Mathf.RoundToInt(radius / terrain.terrainData.size.x * heightmapWidth);

        // Loop through the pixels within the brush circle and modify the modifiedHeights
        for (int row = 0; row < heightmapHeight; row++)
        {
            for (int column = 0; column < heightmapWidth; column++)
            {
                // Calculate the distance from the brush center
                float distance = Vector2.Distance(new Vector2(column, row), new Vector2(brushCenterX, brushCenterZ));

                // Check if the pixel is within the brush radius
                if (distance <= brushRadiusInPixels)
                {
                    // Calculate the falloff value based on the distance from the brush center
                    float falloff = 1f - Mathf.Clamp01(distance / brushRadiusInPixels);

                    // Modify the height value with the multiplier and falloff
                    float newHeight = modifiedHeights[row, column] + multiplier * falloff;

                    // Clamp the new height value between 0 and 1
                    newHeight = Mathf.Clamp01(newHeight);

                    // Set the modified height value
                    modifiedHeights[row, column] = newHeight;
                }
            }
        }

        // Apply the modified heights to the terrain
        terrain.terrainData.SetHeights(0, 0, modifiedHeights);
    }

    public void ResetTerrain()
    {
        TerrainData terrainData = terrain.terrainData;
        int heightmapResolution = terrainData.heightmapResolution;
        int alphamapWidth = terrainData.alphamapWidth;
        int alphamapHeight = terrainData.alphamapHeight;

        // Reset heightmap
        float[,] heights = new float[heightmapResolution, heightmapResolution];
        for (int i = 0; i < heightmapResolution; i++)
        {
            for (int j = 0; j < heightmapResolution; j++)
            {
                heights[i, j] = defaultHeight; 
            }
        }
        terrainData.SetHeights(0, 0, heights);

        // Reset alphamap
        float[,,] alphamaps = new float[alphamapHeight, alphamapWidth, terrainData.alphamapLayers];
        for (int row = 0; row < alphamapHeight; row++)
        {
            for (int column = 0; column < alphamapWidth; column++)
            {
                for (int layer = 0; layer < terrainData.alphamapLayers; layer++)
                {
                    alphamaps[row, column, layer] = (layer == 0) ? 1f : 0f; // Set the first layer to 1, others to 0
                }
            }
        }
        terrainData.SetAlphamaps(0, 0, alphamaps);
    }

}

public enum SlopeType { 
    None,
    LeftToRight, RightToLeft, BottomToTop, TopToBottom,
    BottomLeftToTopRight, BottomRightToTopLeft, TopLeftToBottomRight, TopRightToBottomLeft
}
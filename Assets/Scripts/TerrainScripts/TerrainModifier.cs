using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class TerrainModifier : MonoBehaviour
{
    private float defaultHeight = 0.004f;

    private Terrain terrain;

    private int cellSize;
    private int gridSize;
    
    private void Awake()
    {
        terrain = GetComponent<Terrain>();
        ResetTerrain();
    }

    public void ChangeTerrainSize(int gridSize)
    {  
        float terrainSize = gridSize * 5f;
        terrain.terrainData.size = new Vector3(terrainSize, terrain.terrainData.size.y, terrainSize);
        cellSize = Mathf.CeilToInt(terrain.terrainData.heightmapResolution / (float)terrain.terrainData.size.x * 5);
        gridSize = GameGrid.Instance.GetGridSize();
    }

    public void SetGridCellTerrain(List<Vector2Int> gridCellPosition, float heightLevel)
    {
        int resolution = terrain.terrainData.heightmapResolution;
        float[,] heights = terrain.terrainData.GetHeights(0, 0, resolution, resolution);
        
        Parallel.For(0, gridCellPosition.Count, i =>
        {
            int offsetY = Mathf.FloorToInt(gridCellPosition[i].x * (cellSize - ((float)resolution / gridSize)));
            int offsetX = Mathf.FloorToInt(gridCellPosition[i].y * (cellSize - ((float)resolution / gridSize)));

            int xStart = Mathf.FloorToInt(gridCellPosition[i].y * cellSize) - offsetX;
            int yStart = Mathf.FloorToInt(gridCellPosition[i].x * cellSize) - offsetY;
            int xEnd = Mathf.Min(xStart + cellSize, resolution);
            int yEnd = Mathf.Min(yStart + cellSize, resolution);

            float newHeight = 0;
            if (xStart >= 0 && xStart < resolution &&
                yStart >= 0 && yStart < resolution)
            {
                newHeight = heightLevel * defaultHeight;
            }

            newHeight = Mathf.Clamp(newHeight, 0, 1);
            //float adjustedNewHeight;

            for (int x = xStart; x < xEnd; x++)
            {
                for (int y = yStart; y < yEnd; y++)
                {
                    // adjustedNewHeight = newHeight + (heights[x, y] - originalHeight * defaultHeight);
                    // heights[x, y] = Mathf.Clamp(adjustedNewHeight, heights[x, y] - defaultHeight, heights[x, y] + defaultHeight);
                    heights[x, y] = newHeight;
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
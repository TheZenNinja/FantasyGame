using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class PoissonDiscSampling
{
    public static List<Vector2> GeneratePoints(float radius, Vector2 size, int samplesBeforeReject = 30)
    {
        float cellSize = radius / Mathf.Sqrt(2);

        int[,] grid = new int[Mathf.CeilToInt(size.x / cellSize), Mathf.CeilToInt(size.y / cellSize)];
        List<Vector2> points = new List<Vector2>();
        List<Vector2> spawnPoints = new List<Vector2>();

        spawnPoints.Add(size / 2);
        while (spawnPoints.Count > 0)
        {
            int spawnIndex = Random.Range(0, spawnPoints.Count);
            Vector2 spawnCenter = spawnPoints[spawnIndex];
            bool accepted = false;
            for (int i = 0; i < samplesBeforeReject; i++)
            {
                float angle = Random.value * Mathf.PI * 2;
                Vector2 dir = new Vector2(Mathf.Sin(angle), Mathf.Cos(angle));
                Vector2 canidatePos = spawnCenter + dir * Random.Range(radius, 2 * radius);
                if (IsValid(canidatePos,size, cellSize, radius, points, grid))
                {
                    accepted = true;
                    points.Add(canidatePos);
                    spawnPoints.Add(canidatePos);
                    grid[(int)(canidatePos.x / cellSize), (int)(canidatePos.y / cellSize)] = points.Count;
                    break;
                }
            }
            if (!accepted)
            {
                spawnPoints.RemoveAt(spawnIndex);
            }
        }
        return points;
    }
    public static bool IsValid(Vector2 canidate, Vector2 size, float cellSize, float radius, List<Vector2> points, int[,] grid)
    {
        if (canidate.x >= 0 && canidate.x < size.x && canidate.y >= 0 && canidate.y < size.y)
        {
            int cellX = (int)(canidate.x / cellSize);
            int cellY = (int)(canidate.y / cellSize);

            int searchStartX = Mathf.Max(0, cellX - 2);
            int searchEndX = Mathf.Max(cellX + 2, grid.GetLength(0) - 1);

            int searchStartY = Mathf.Max(0, cellY - 2);
            int searchEndY = Mathf.Max(cellY + 2, grid.GetLength(1) - 1);

            for (int x = searchStartX; x < searchEndX; x++)
                for (int y = searchStartY; y < searchEndY; y++)
                {
                    int pointIndex = grid[x, y] - 1;
                    if (pointIndex != -1)
                    {
                        float sqrDist = (canidate - points[pointIndex]).sqrMagnitude;
                        if (sqrDist < radius* radius)
                            return false;
                    }
                }
            return true;
        }
        return false;
    }
}

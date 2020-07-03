using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ForestGenerator : MonoBehaviour
{
    [System.Serializable]
    private struct TreePos
    {
        public Vector2 horzPos;
        public float radius;

        public TreePos(Vector2 horzPos, float radius)
        {
            this.horzPos = horzPos;
            this.radius = radius;
        }
    }

    public Vector2 size;
    public float minRad = 1;
    public float maxRad = 5;

    [Range(0,2)]
    public float density = 1;
    public int maxTreeCount;
    public int triesPerPoint = 4;


    public int maxNewPoints = 10;
    public float delayBetweenNewPoints = 60;
    private float delayTimer = 0;
    public bool inMinutes;
    [Range(0, 0.5f)]
    public float silverwoodFrequency = 0.01f;

    //[SerializeField] BoxCollider[] paths;

    [SerializeField] List<TreePos> points;

    void Start()
    {
        GenerateAllNewPoints();
        delayTimer = delayBetweenNewPoints;
    }
    private void FixedUpdate()
    {
        if (delayTimer <= 0)
        {
            GenerateExistingPoints();
            delayTimer = delayBetweenNewPoints;
        }
        else
            delayTimer -= Time.deltaTime;
    }
    public void GeneratePoints()
    {
        GenerateAllNewPoints();
    }
    [ContextMenu("Generate New Points")]
    public void GenerateAllNewPoints()
    {
        Clear();
        points = new List<TreePos>();
        for (int i = 0; i < maxTreeCount; i++)
        {
            Vector2 newPos = Vector2.zero;
            float rad = Random.Range(minRad, maxRad);
            bool valid = true;

            for (int t = 0; t < triesPerPoint; t++)
            {
                valid = true;
                newPos = new Vector2(Random.Range(-size.x/2, size.x/2), Random.Range(-size.y/2, size.y/2));

                //foreach (BoxCollider box in paths)
                //    if (box.bounds.Contains(new Vector3(newPos.x, 0, newPos.y)) || (box.ClosestPoint(newPos) - new Vector3(newPos.x, 0, newPos.y)).sqrMagnitude <= rad)
                //        valid = false;

                if (valid)
                    if (points.Count > 0)
                        foreach (TreePos point in points)
                            if ((point.horzPos - newPos).sqrMagnitude <= Mathf.Pow(rad + point.radius, 2))
                                valid = false;
            }

            if (valid)
                points.Add(new TreePos(newPos, rad));
        }
        SpawnTrees(points);
    }
    public void GenerateExistingPoints()
    {
        List<TreePos> newPoints = new List<TreePos>();
        int numToSpawn = Mathf.Clamp(maxTreeCount - points.Count,0, maxNewPoints);
        if (numToSpawn > 0)
            for (int i = 0; i < numToSpawn; i++)
            {
                Vector2 newPos = Vector2.zero;
                float rad = Random.Range(minRad, maxRad);
                bool valid = true;

                for (int t = 0; t < triesPerPoint; t++)
                {
                    valid = true;
                    newPos = new Vector2(Random.Range(-size.x / 2, size.x / 2), Random.Range(-size.y / 2, size.y / 2));

                    //foreach (BoxCollider box in paths)
                    //    if (box.bounds.Contains(new Vector3(newPos.x, 0, newPos.y)) || (box.ClosestPoint(newPos) - new Vector3(newPos.x, 0, newPos.y)).sqrMagnitude <= rad)
                    //        valid = false;

                    if (valid)
                        if (points.Count > 0)
                            foreach (TreePos point in points)
                                if ((point.horzPos - newPos).sqrMagnitude <= Mathf.Pow(rad + point.radius, 2))
                                    valid = false;
                }

                if (valid)
                    newPoints.Add(new TreePos(newPos, rad));
            }

        SpawnTrees(newPoints);
        points.AddRange(newPoints);
    }
    private void SpawnTrees(List<TreePos> positions)
    {
        foreach (TreePos pos in positions)
        {
            GameObject g;
            PartMaterial mat;
            if (Random.value <= silverwoodFrequency)
            {
                g = Instantiate(Resources.Load<GameObject>("Silverwood Tree"), transform);
                mat = PartMaterial.silverwood;
            }
            else
            {
                g = Instantiate(Resources.Load<GameObject>("Tree"), transform);
                mat = PartMaterial.wood;
            }

            g.transform.localPosition = new Vector3(pos.horzPos.x, 0, pos.horzPos.y);
            g.transform.localEulerAngles = new Vector3(0, Random.value * 360, 0);
            g.transform.localScale = Vector3.one * pos.radius;
            g.GetComponent<HarvestableObject>().SetHarvestStats(mat, maxRad, pos.radius);
        }
    }
    [ContextMenu("Clear")]
    public void Clear()
    {
        points = new List<TreePos>();
        foreach (Transform t in transform)
        {
            if (Application.isEditor)
                DestroyImmediate(t.gameObject);
            else
                Destroy(t.gameObject);
        }
    }

    private void OnValidate()
    {
        maxTreeCount = Mathf.RoundToInt( (size.x + size.y)/2 * density);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, new Vector3(size.x, 0, size.y));
    }
}

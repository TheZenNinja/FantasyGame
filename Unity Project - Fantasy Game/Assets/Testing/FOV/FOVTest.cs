using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FOVTest : MonoBehaviour
{
    public float viewRadius;
    [Range(0,360)]
    public float viewAngle;

    public Transform forwardRef;

    private void Start()
    {
        if (forwardRef == null)
            forwardRef = transform;
    }

    public List<Entity> FindVisibleEntities()
    {
        Vector3 center = transform.position + Vector3.up;
        Collider[] cols = Physics.OverlapCapsule(center + Vector3.up * (viewRadius * 2), center + Vector3.up * (-viewRadius * 2), viewRadius);//Physics.OverlapSphere(transform.position + Vector3.up, viewRadius);

        List<Entity> entities = new List<Entity>();

        foreach (Collider c in cols)
        {
            Entity e = c.GetComponentInParent<Entity>();
            if (e != null)
            {
                if (!entities.Contains(e))
                {
                    Vector3 dir = (e.position - transform.position).normalized;
                    if (Vector3.Angle(GetHorzDir(forwardRef.forward), dir) <= viewAngle / 2)
                        if (e.IsVisible(transform.position))
                            entities.Add(e);
                }
            }
        }
        return entities;
    }

    public Vector3 DirFromAngle(float angleDeg, bool global = false)
    {
        if (!global)
            angleDeg += forwardRef.eulerAngles.y;

        return new Vector3(Mathf.Sin(angleDeg * Mathf.Deg2Rad), 0, Mathf.Cos(angleDeg * Mathf.Deg2Rad));
    }

    public Vector3 GetHorzDir(Vector3 input)
    {
        return Vector3.Scale(input, new Vector3(1, 0, 1)).normalized;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EntityFinder : MonoBehaviour
{
    [System.Serializable]
    private class Radius
    {
        public float viewRadius;
        [Range(0, 360)]
        public float viewAngle;

        public List<Entity> GetEntities(Vector3 center, Vector3 angleDir, bool testVisibility = false)
        {
            Collider[] cols = Physics.OverlapCapsule(center + Vector3.up * (viewRadius * 2),
                                                     center + Vector3.up * (-viewRadius * 2), viewRadius);

            List<Entity> entities = new List<Entity>();

            foreach (Collider c in cols)
            {
                Entity e = c.GetComponentInParent<Entity>();
                if (e != null)
                {
                    if (!entities.Contains(e))
                    {
                        Vector3 dir = (e.position - center).normalized;
                        if (Vector3.Angle(angleDir, dir) <= viewAngle / 2)
                            if (!testVisibility || e.IsVisible(center))
                                entities.Add(e);
                    }
                }
            }
            return entities;
        }
    }

    [SerializeField]
    List<Radius> radii;

    public Transform forwardRef;

    private void Start()
    {
        if (forwardRef == null)
            forwardRef = transform;
    }
    public void X()
    {
        Vector3 angleDir = GetHorzDir(forwardRef.forward);
    }
    public List<Entity> GetEntities()
    {
        List<Entity> entities = new List<Entity>();
        Vector3 angleDir = GetHorzDir(forwardRef.forward);

        for (int i = 0; i < radii.Count; i++)
        {
            entities.AddRange(radii[i].GetEntities(transform.position, angleDir));
        }
        return entities;
    }

    public List<Entity> FindVisibleEntities()
    {
        List<Entity> entities = new List<Entity>();
        Vector3 angleDir = GetHorzDir(forwardRef.forward);

        for (int i = 0; i < radii.Count; i++)
        {
            entities.AddRange(radii[i].GetEntities(transform.position, angleDir, true));
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
    private void OnDrawGizmosSelected()
    {
        Handles.color = Color.white;

        for (int i = 0; i < radii.Count; i++)
        {
            Vector3 angleLeft = DirFromAngle(-radii[i].viewAngle/2);
            Vector3 angleRight = DirFromAngle(radii[i].viewAngle / 2);

            Handles.DrawLine(transform.position + angleLeft, transform.position + angleLeft * radii[i].viewRadius);
            Handles.DrawLine(transform.position + angleRight, transform.position + angleRight * radii[i].viewRadius);

            Vector3 fwd = angleLeft;// DirFromAngle(angle / 2- angle);

            Handles.DrawWireArc(transform.position, Vector3.up, Vector3.forward, 360, radii[i].viewRadius);
        }
    }
}

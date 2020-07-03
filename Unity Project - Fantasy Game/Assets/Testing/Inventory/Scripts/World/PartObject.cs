using UnityEngine;
using System.Collections;

public class PartObject : MonoBehaviour
{
    public PartType part;

    public Transform nextPartPos;
    public PartType nextPartType;

    public Renderer[] primaryRenderers;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        if (nextPartPos)
            Gizmos.DrawSphere(nextPartPos.position, 0.025f);
    }

    public void SetPrimaryMaterial(Material mat, bool isPlayer = false)
    {
        if (primaryRenderers.Length > 0)
            foreach (Renderer r in primaryRenderers)
            {
                r.material = mat;
                if (isPlayer)
                    r.gameObject.layer = LayerMask.NameToLayer("First Person Equipment");
            }
    }
}

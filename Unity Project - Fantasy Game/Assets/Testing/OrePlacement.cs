using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrePlacement : MonoBehaviour
{
    [SerializeField] Transform[] tiers;

    [SerializeField] Transform oreParent;

    [ContextMenu("Spawn")]
    public void SpawnOre()
    {
        ClearOre();

        for (int t = 0; t < tiers.Length; t++)
        {
            List<PartMaterial> materials = new List<PartMaterial>();
            foreach (PartMaterialAtlas.MaterialData data in PartMaterialAtlas.GetMaterialsOfLevel(t+1))
                if (PartMaterialAtlas.GetResourceType(data.material) != ResourceType.wood)
                    materials.Add(data.material);

            foreach (Transform trans in tiers[t])
            {
                PartMaterial randomMat = materials[Random.Range(0, materials.Count)];

                HarvestableObject item;
                if (PartMaterialAtlas.GetResourceType(randomMat) == ResourceType.stone)
                {
                    item = Instantiate(Resources.Load<GameObject>("Stone"), trans.position, Random.rotation, oreParent).GetComponent<HarvestableObject>();
                }
                else
                {
                    item = Instantiate(Resources.Load<GameObject>("Ore"), trans.position, Random.rotation, oreParent).GetComponent<HarvestableObject>();
                }
                item.SetHarvestStats(randomMat, 2, Random.value * 2);
            }
        }

    }

    [ContextMenu("Clear")]
    public void ClearOre()
    {
        while (oreParent.childCount > 0)
        {
            foreach (Transform t in oreParent)
            {
                if (Application.isEditor)
                    DestroyImmediate(t.gameObject);
                else
                    Destroy(t.gameObject);
            }
        }
    }
    private void OnDrawGizmos()
    {
        for (int t = 0; t < tiers.Length; t++)
        {
            switch (t)
            {
                default:
                    Gizmos.color = Color.green;
                    break;
                case 1:
                    Gizmos.color = Color.blue;
                    break;
                case 2:
                    Gizmos.color = Color.cyan;
                    break;
            }
            foreach (Transform trans in tiers[t])
                Gizmos.DrawSphere(trans.position, 0.5f);
        }
    }
}

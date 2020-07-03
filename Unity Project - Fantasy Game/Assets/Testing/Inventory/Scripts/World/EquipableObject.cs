using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EquipableObject : MonoBehaviour
{
    public ModularItem item;

    public GameObject mesh;

    public bool isCurrentItem;

    public bool isPlayer = true;

    public void Assemble()
    {
        if (mesh)
        {
            if (Application.isEditor)
                DestroyImmediate(mesh);
            else
                Destroy(mesh);
        }
        if (item == null)
            return;

        mesh = new GameObject();//Instantiate(new GameObject(), transform);
        mesh.name = "New " + item.equipmentType.ToString();
        mesh.transform.SetParent(transform);
        mesh.transform.localPosition = Vector3.zero;
        mesh.transform.localEulerAngles = Vector3.zero;

        List<Part> constructs = PartAtlas.SortPartsForConstruction(item.GetPartList());

        //Debug.Log(constructs.Count);

        PartObject obj = null;

        for (int i = 0; i < constructs.Count; i++)
        {
            //Debug.Log(constructs[i].ToString());

            GameObject part = PartAtlas.GetPartMesh(item.equipmentType, constructs[i].type, constructs[i].subType);
            if (part != null)
            {
                if (obj == null)
                    part = Instantiate(part, mesh.transform);
                else
                {
                    part = Instantiate(part, obj.nextPartPos.position, obj.nextPartPos.rotation, mesh.transform);
                }

                obj = part.GetComponent<PartObject>();
                part.layer = LayerMask.NameToLayer("First Person Equipment");

                Material mat = PartMaterialAtlas.GetMaterialShader(constructs[i].material);
                obj.SetPrimaryMaterial(mat, isPlayer);
            }
        }
    }

    public void Equip()
    {
        if (!mesh)
            Assemble();
        mesh.SetActive(true);
    }

    public void Unequip()
    {
        mesh.SetActive(false);
    }
}



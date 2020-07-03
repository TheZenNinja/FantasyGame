using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarvestScript : MonoBehaviour
{
    public Transform cam;
    public float distance = 3;
    public Animator anim;
    public EquipmentManager equipment;
    private void Update()
    {

        /*if (equipment.GetCurrentTool() != null)
        {
            anim.SetInteger("Tool", equipment.GetCurrentTool().typ);
            anim.SetBool("Swinging", Input.GetKey(KeyCode.Mouse0) && !InventoryManager.instance.inUI);
        }
        else
            anim.SetBool("Swinging", false);*/
    }

    public void Harvest()
    {
        Debug.Log("Swing");
        RaycastHit hit;
        if (Physics.Raycast(cam.position, cam.forward, out hit, distance))
        {
            HarvestableObject harvestable = hit.collider.gameObject.GetComponentInParent<HarvestableObject>();
            Debug.Log(harvestable);

            //if (harvestable)
              //  harvestable.Harvest(FindObjectOfType<EquipmentMenu>().GetCurrentTool().GetSubType());
        }
    }
    private void OnDrawGizmos()
    {
        if (cam)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(cam.position, distance);
        }
    }
}

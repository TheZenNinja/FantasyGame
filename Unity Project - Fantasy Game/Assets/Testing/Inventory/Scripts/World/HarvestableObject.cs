using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarvestableObject : MonoBehaviour, IDamagableObject
{
    public int health = 20;
    [Tooltip("pick=0,axe=1,hammer=2")]
    public int requiredTool;
    public PartMaterial material;
    public ResourceType resourceType;
    public float itemMulti;
    public float velocity = 4;

    public Renderer oreMetalRenderer;

    public Vector3 spawnOffset;
    public AudioClip sound;

    private void Start()
    {
        //sound = GetComponentInChildren<AudioSource>();
    }


    public void DamageObj(int dmg, EntityStatus sender)
    {
        health -= dmg;
        if (health <= 0)
            Harvest();

        AudioSource.PlayClipAtPoint(sound, transform.position);

        //if (sound.isPlaying)
        //    sound.Stop();
        //sound.Play();
    }

    public void Harvest()
    {
        Debug.Log("Hit Harvest");

        /*if (toolType != requiredTool)
        {
            Debug.Log("Incorrect tool");
            return;
        }*/

        WorldItem item = Instantiate(Resources.Load("World Item") as GameObject, transform.position + transform.TransformVector(spawnOffset), Quaternion.identity).GetComponent<WorldItem>();

        item.SetResourceData(resourceType, material, Mathf.RoundToInt((1 + Random.value) * itemMulti));

        item.GetComponent<Rigidbody>().velocity = new Vector3(Random.Range(-velocity, velocity), velocity, Random.Range(-velocity, velocity));

        Destroy(gameObject);
    }

    private void OnValidate()
    {
        SetHarvestStats(material, 5, transform.localScale.magnitude);
    }

    public void SetHarvestStats(PartMaterial material, float maxScale, float sizeMulti = 1)
    {
        if (PartMaterialAtlas.GetResourceType(material, true) == ResourceType.ore)
            oreMetalRenderer.material = PartMaterialAtlas.GetMaterialShader(material);
        else if (PartMaterialAtlas.GetResourceType(material) == ResourceType.stone)
            GetComponent<Renderer>().material = PartMaterialAtlas.GetMaterialShader(material);

        this.material = material;

        // health calc
        // hp = maxScale * (scale-1) ^ 2 + 20

        health = Mathf.RoundToInt(maxScale * Mathf.Pow((sizeMulti - 1), 2) + 20);
        itemMulti = System.Convert.ToInt32(48 * 1 / (1 + System.Math.Pow(System.Math.E, -2 * sizeMulti)) - 23);
    }
}

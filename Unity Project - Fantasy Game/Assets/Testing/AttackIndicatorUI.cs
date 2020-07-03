using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//convert this to be a rune spell?
public class AttackIndicatorUI : MonoBehaviour
{
    public Transform player;
    private EntityStatus playerStatus;
    public float detectRadius = 5;

    public float offset = 22.5f;

    public float indicatorSpeed = 10;

    public List<EntityStatus> entities;
    public List<Transform> indicators;

    public GameObject indicatorPrefab;

    private void Start()
    {
        playerStatus = player.GetComponent<EntityStatus>();
    }

    void Update()
    {
        if (entities.Count > 0)
        {
            while (entities.Count != indicators.Count)
            {
                if (entities.Count > indicators.Count)
                    indicators.Add(Instantiate(indicatorPrefab.transform, transform));
                else if (entities.Count < indicators.Count)
                {
                    Destroy(indicators[indicators.Count].gameObject);
                    indicators.TrimExcess();
                }
                else
                    break;
            }

            for (int i = 0; i < entities.Count; i++)
            {
                if (!indicators[i].gameObject.activeSelf)
                    indicators[i].gameObject.SetActive(true);

                Vector3 dir = (entities[i].transform.position - player.position).normalized;
                //dir.y = 0;
                indicators[i].localEulerAngles = new Vector3(0, 0, GetAngleFromDir(dir));
            }
        }
        else if (indicators.Count > 0)
        {
            foreach (Transform t in indicators)
            {
                Destroy(t.gameObject);
            }
            indicators.Clear();
        }
    }


    private void FixedUpdate()
    {
        entities.Clear();
        Collider[] cols = Physics.OverlapSphere(player.position, detectRadius);

        foreach (Collider c in cols)
        {
            if (c.GetComponentInParent<EntityStatus>())
            {
                EntityStatus e = c.GetComponentInParent<EntityStatus>();

                if (e != playerStatus && !entities.Contains(e))
                    entities.Add(e);
            }
        }
    }

    private float GetAngleFromDir(Vector3 dir)
    {
        dir = player.InverseTransformDirection(dir);
        dir.x *= -1;
        dir = player.TransformDirection(dir);

        return Vector3.SignedAngle(player.forward, dir, player.up) + offset;
    }
}

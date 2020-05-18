using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(CharacterController))]
public class Entity : MonoBehaviour
{
    public enum EntityType
    {
        none,
        enemy,
        friendly,
        neutral,
    }
    [HideInInspector]
    public Vector3 position
    {
        get
        {
            return transform.position;

        }
        set
        {
            transform.position = value;
        }

    }
    public EntityType type = EntityType.none;
    public List<Transform> raycastPoints;
    CharacterController cc;

    public float gravForce = 20;
    public Vector3 velocity;
    public bool useGrav = true;



    private void Awake()
    {
        cc = GetComponent<CharacterController>();

        if (raycastPoints.Count < 1)
            raycastPoints.Add(transform);
    }
    private void Update()
    {

        if (cc.isGrounded)
        {
            if (useGrav && velocity.y < 0.1f)
                velocity.y = 0;
        }
        else
        {
            if (useGrav)
                velocity.y -= gravForce * Time.deltaTime;
        }

        cc.Move(velocity * Time.deltaTime);
    }



    public bool IsVisible(Vector3 rayStartPos, float maxDistance = 0)
    {
        for (int i = 0; i < raycastPoints.Count; i++)
        {
            Vector3 dir = (transform.position - rayStartPos).normalized;
            RaycastHit hit;
            if (maxDistance == 0)
                maxDistance = Vector3.Distance(rayStartPos, raycastPoints[i].position);
            if (Physics.Raycast(rayStartPos, dir, out hit, maxDistance))
            {
                if (hit.collider.GetComponentInParent<Entity>() == this)
                    return true;
            }
        }
        return false;
    }
    IEnumerator gravStop(bool zeroVel = true)
    {
        useGrav = false;
        velocity.y = 0;
        yield return new WaitForSeconds(1f);
        useGrav = true;
    }
    public void Launch(Vector3 force)
    {
        StopCoroutine(gravStop());
        StartCoroutine(gravStop());
        velocity = force;
    }

    public void Hit(int dmg = 0)
    {
        Debug.Log(gameObject.name + " got hit");

        if (cc.isGrounded)
        {

        }
        else
        {
            StopCoroutine(gravStop());
            StartCoroutine(gravStop());
        }
    }
}

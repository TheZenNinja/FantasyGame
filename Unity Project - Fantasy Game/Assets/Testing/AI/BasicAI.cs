using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEditor;
[System.Serializable]
public struct FOV
{
    public float distance;
    [Range(1, 360)]
    public float angle;

    public FOV(float distance, float angle)
    {
        this.distance = distance;
        this.angle = angle;
    }
}
public class BasicAI : MonoBehaviour
{
    public Faction faction = Faction.enemy;
    NavMeshAgent agent;
    public Transform target;
    //public bool follow;

    public bool canMove;
    public bool canAttack;

    public FOV[] fovLayers = new FOV[1] { new FOV(6,90)};
    public float loseTargetDistance = 30;

    public GameObject deathFX;

    public float attackDistance;
    public float attackCooldown = 2;

    HitboxManager hitbox;
    EquipableObject weapon;
    Animator anim;
    [SerializeField] AudioSource source;
    void Start()
    {
        canMove = true;
        canAttack = true;
        hitbox = GetComponent<HitboxManager>();
        weapon = GetComponentInChildren<EquipableObject>();
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        Health h;
        if (TryGetComponent(out h))
        {
            h.Hurt += OnHit;
            h.Die += Die;
        }

    }

    void Update()
    {
        if (!agent.isOnNavMesh)
            canMove = false;

        if (canMove && target)
        {
            agent.SetDestination(target.position);
            if (canAttack && attackDistance * attackDistance > sqrDistanceToTarget())
                Attack();
        }
    }
    private void FixedUpdate()
    {
        if (!target)
        {
            foreach (FOV fov in fovLayers)
            {
                if (target)
                    break;

                Collider[] cols = Physics.OverlapSphere(transform.position, fov.distance);

                List<EntityStatus> entities = new List<EntityStatus>();
                for (int i = 0; i < cols.Length; i++)
                {
                    EntityStatus entity;
                    if (cols[i].TryGetComponent<EntityStatus>(out entity))
                        if (!entities.Contains(entity) && entity != GetComponent<EntityStatus>() && /*just for targeting players*/ entity.GetType() == typeof(PlayerStatus))
                            target = entity.transform;//entities.Add(entity);

                }

                /*foreach (EntityStatus entity in entities)
                {
                    Vector3 dir = (entity.transform.position - transform.position).normalized;
                    if (!Physics.Raycast(transform.position + dir * (agent.radius + 0.1f), dir, fov.distance))
                    {
                        target = entity.transform;
                        break;
                    }
                }*/
            }
        }
        else
        {
            if ((transform.position - target.position).sqrMagnitude > loseTargetDistance * loseTargetDistance)
                target = null;
        }
    }

    public void OnHit(int dmg)
    {
        DamagePopup.Create(dmg, transform.position + transform.up * (agent.height), 0.5f);
        StartCoroutine(PauseMove(0.75f));
        source.Play();
    }
    public void Attack()
    {
        StartCoroutine(AttackCooldown(1.5f));
        StartCoroutine(PauseMove(1f));
        anim.SetTrigger("Attack");
    }
    public void AttackAnimEvent()
    {
        hitbox.Hit(weapon.item.attackDmg);
    }
    IEnumerator PauseMove(float time)
    {
        canMove = false;
        yield return new WaitForSeconds(time);
        canMove = true;
    }
    IEnumerator AttackCooldown(float time)
    {
        canAttack = false;
        yield return new WaitForSeconds(time);
        canAttack = true;
    }

    public void Die()
    {
        Instantiate(deathFX, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
    public float sqrDistanceToTarget()
    {
        if (target)
            return (transform.position - target.position).sqrMagnitude;
        throw new System.Exception(gameObject.name + " has no target");
    }
    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal = false)
    {
        if (!angleIsGlobal)
            angleInDegrees += transform.eulerAngles.y;

        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}

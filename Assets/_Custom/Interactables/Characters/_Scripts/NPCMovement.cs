using Pathfinding;
using UnityEngine;

public class NPCMovement : MonoBehaviour
{
    IAstarAI astar;
    public Animator animator;
    CharacterStats characterStats;
    HateManager hateManager;
    NPCFocus npcFocus;

    Vector3 spawnPosition;
    Quaternion spawnRotation;

    public float despawnTimer = 10f;
    public float respawnTimer;
    public bool despawned;
    public float distanceToTarget;

    private void Start()
    {
        astar = GetComponent<IAstarAI>();
        characterStats = GetComponent<CharacterStats>();
        hateManager = GetComponent<HateManager>();
        npcFocus = GetComponent<NPCFocus>();

        //find my location
        spawnPosition = transform.position;
        spawnRotation = transform.rotation;

        respawnTimer = characterStats.behaviorSO.respawnTimer;
    }

    public void Despawn()
    {
        if (characterStats.dead && !despawned)
        {
            despawnTimer -= Time.deltaTime;
        }

        if (despawnTimer <= 0)
        {
            transform.GetChild(0).gameObject.SetActive(false);
            transform.GetComponent<AIPath>().enabled = false;
            despawned = true;
            hateManager.hateList.Clear();
            hateManager.target = null;

            despawnTimer = 10f;
        }
    }

    public void Respawn()
    {
        if (despawned && characterStats.dead)
        {
            respawnTimer -= Time.deltaTime;
        }

        if (respawnTimer <= 0)
        {
            despawned = false;
            transform.GetChild(0).gameObject.SetActive(true);
            transform.GetComponent<AIPath>().enabled = true;
            astar.destination = transform.position;
            characterStats.currentHitPoints = characterStats.maxHitpoints;
            characterStats.dead = false;
            animator.SetBool("Dead", false);
            //characterStats.gaveXP = false;

            respawnTimer = characterStats.behaviorSO.respawnTimer;
        }
    }

    public void ResetPosition()
    {
        if (despawned)
        {
            //Debug.Log(transform.name + " resetting position to " + spawnPosition + " from " + gameObject.transform.position);
            gameObject.transform.SetPositionAndRotation(spawnPosition, spawnRotation);
        }
    }

    public void ResponseToBeingTargeted()
    {
        if (npcFocus.playersTargetingMe.Count > 0 && !characterStats.dead && hateManager.target == null)
        {
            //there are players targeting me get top item in list and face them
            Transform targetPlayer = npcFocus.playersTargetingMe[0].transform;
            FaceTarget(targetPlayer.position, transform);

            //stop moving and clear astar path
            astar.destination = transform.position;
            astar.SearchPath();
            astar.maxSpeed = 0;
        }
    }

    public void Roam()
    {
        if (hateManager.target == null && !characterStats.dead && npcFocus.playersTargetingMe.Count == 0)
        {
            if (!astar.pathPending && (astar.reachedEndOfPath || !astar.hasPath)) //i need a new path
            {
                astar.SearchPath();
                astar.destination = PickRandomPoint(characterStats.behaviorSO.roamDistance, transform);
                astar.maxSpeed = characterStats.characterRace.walkSpeed;
            }
            else
            {
                astar.maxSpeed = characterStats.characterRace.walkSpeed;
            }
        }
    }

    public static Vector3 PickRandomPoint(float radius, Transform transform)
    {
        var point = Random.insideUnitSphere * radius;
        point.y = 0;
        point += transform.position;
        return point;
    }

    public void CheckAstarVelocity()
    {
        if (astar.velocity.x != 0 || astar.velocity.y != 0 || astar.velocity.z != 0)
        {
            animator.SetFloat("VelocityX", astar.maxSpeed);
        }
        else
        {
            animator.SetFloat("VelocityX", 0);
        }
        if (characterStats.dead)
        {
            astar.maxSpeed = 0;
        }
    }

    public static void FaceTarget(Vector3 targetLocation, Transform transform)
    {

        Vector3 direction = (targetLocation - transform.position).normalized;

        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }
    }

    public void ApproachTarget(Transform target)
    {

        // If we have anything on the hate list, force pursue/attack the top entry regardless of sensing or aggro radius
        if (hateManager.hateList.Count > 0 && !characterStats.dead)
        {
            var top = hateManager.hateList[0];
            if (top != null)
            {
                Debug.Log(transform.name + " pursuing hate target " + top.name);
                target = top.transform;
                // Always face and move toward the target, ignoring aggro/sense constraints
                FaceTarget(target.position, transform);
                astar.destination = target.position;
                astar.maxSpeed = characterStats.characterRace.runSpeed;

                // If within attack distance, stop movement (actual attack handled elsewhere by animation/events)
                distanceToTarget = Vector3.Distance(target.position, transform.position);
                if (distanceToTarget <= characterStats.characterRace.attackDistance)
                {
                    astar.destination = transform.position;
                    astar.maxSpeed = 0;
                }
                return; // skip roaming when we have a hate target
            }
        }
        
        if (!characterStats.dead && target != null)
        {
            distanceToTarget = Vector3.Distance(target.transform.position, transform.position); //distance to target
            if (distanceToTarget <= characterStats.characterRace.aggroRadius && distanceToTarget >= characterStats.characterRace.attackDistance)
            {
                //less than agro radius and more than attack distnace
                astar.destination = target.position;
                astar.maxSpeed = characterStats.characterRace.runSpeed;
            }
            else
            {
                //within attack distance stop moving
                astar.destination = transform.position;
                astar.maxSpeed = 0;
            }
        }
    }

    public void AnimationCheck()
    {
        animator.SetFloat("VelocityX", astar.maxSpeed);
    }
}

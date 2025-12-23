using Pathfinding;
using UnityEngine;

public class NPCMovement : MonoBehaviour
{
    IAstarAI astar;
    public Animator animator;
    CharacterStats characterStats;
    HateManager hateManager;
    CharacterFocus characterFocus;

    Vector3 spawnPosition;
    Quaternion spawnRotation;
    bool positionReset;

    public float despawnTimer;
    public float respawnTimer;
    public bool despawned;
    public float distanceToTarget;

    private void Start()
    {
        astar = GetComponent<IAstarAI>();
        characterStats = GetComponent<CharacterStats>();
        hateManager = GetComponent<HateManager>();
        characterFocus = GetComponent<CharacterFocus>();

        //find my location
        spawnPosition = transform.position;
        spawnRotation = transform.rotation;

        respawnTimer = characterStats.behaviorSO.respawnTimer;
        despawnTimer = characterStats.behaviorSO.despawnTimer;
    }

    public void DespawnCharacter()
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
            characterFocus.currentFocus = null;
            positionReset = false;

            despawnTimer = characterStats.behaviorSO.despawnTimer;
        }
    }

    public void RespawnCharacter()
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
            positionReset = false;
            //characterStats.gaveXP = false;

            respawnTimer = characterStats.behaviorSO.respawnTimer;
        }
    }

    public void ResetPosition()
    {
        if (despawned && !positionReset)
        {
            //Debug.Log(transform.name + " resetting position to " + spawnPosition + " from " + gameObject.transform.position);
            gameObject.transform.SetPositionAndRotation(spawnPosition, spawnRotation);
            positionReset = true;
        }
    }

    // ResponseToBeingTargeted moved to HateManager.ResponseToBeingTargeted()

    public void Roam()
    {
        if (characterFocus.currentFocus == null && !characterStats.dead && characterFocus.charactersTargetingMe.Count == 0)
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
        //check null
        if (transform == null)
            return;

        Vector3 direction = (targetLocation - transform.position).normalized;

        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }
    }

    public void ApproachTarget(Transform target)
    {
        if (!characterStats.dead && target != null)
        {
            distanceToTarget = Vector3.Distance(target.position, transform.position);

            // Face the target
            FaceTarget(target.position, transform);

            // Within attack distance: stop moving
            if (distanceToTarget <= characterStats.characterRace.attackDistance)
            {
                astar.destination = transform.position;
                astar.SearchPath();
                astar.maxSpeed = 0;
                astar.isStopped = true;
            }
            // Within aggro radius but outside attack distance: run toward target
            else if (distanceToTarget <= characterStats.characterRace.aggroRadius)
            {
                astar.destination = target.position;
                astar.SearchPath();
                astar.maxSpeed = characterStats.characterRace.runSpeed;
                astar.isStopped = false;
            }
            // Outside aggro radius: walk slowly toward target (or stop if you prefer)
            else if (distanceToTarget > characterStats.characterRace.attackDistance && distanceToTarget < characterStats.characterRace.viewRadius)
            {
                astar.destination = target.position;
                astar.SearchPath();
                astar.maxSpeed = characterStats.characterRace.walkSpeed;
                astar.isStopped = false;
            }
            // Outside view radius: return to spawn
            else
            {
                //clear hate list of distant targets
                hateManager.hateList.Clear();
                astar.destination = spawnPosition;
                //astar.rotation = spawnRotation;
                astar.SearchPath();
                astar.maxSpeed = characterStats.characterRace.walkSpeed;
                astar.isStopped = false;

				// If close to spawn, stop and face original rotation
				if (Vector3.Distance(transform.position, spawnPosition) <= 0.5f)
				{
					astar.destination = transform.position;
                    astar.maxSpeed = 0;
					astar.isStopped = true;
					transform.rotation = Quaternion.Slerp(transform.rotation, spawnRotation, Time.deltaTime * 5f);
				}
            }

            // If idle at spawn, ensure facing spawn rotation
            if (characterFocus.currentFocus == null && hateManager.hateList.Count == 0)
            {
                if (Vector3.Distance(transform.position, spawnPosition) <= 0.5f)
                {
                    astar.destination = transform.position;
                    astar.maxSpeed = 0;
                    astar.isStopped = true;
                    transform.rotation = Quaternion.Slerp(transform.rotation, spawnRotation, Time.deltaTime * 5f);
                }
            }
        }
    }

    public void AnimationCheck()
    {
        animator.SetFloat("VelocityX", astar.maxSpeed);
    }
}

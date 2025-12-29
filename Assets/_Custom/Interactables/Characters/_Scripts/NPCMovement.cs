using Pathfinding;
using PurrNet;
using UnityEngine;

//All NPC reuired Components
//movement and pathfinding
[RequireComponent(typeof(Seeker))]
[RequireComponent(typeof(AIPath))]
[RequireComponent(typeof(RaycastModifier))]
[RequireComponent(typeof(CharacterController))]

[RequireComponent(typeof(NPCTimers))]
[RequireComponent(typeof(CharacterStats))]
[RequireComponent(typeof(HateManager))]
[RequireComponent(typeof(FieldOfView))]

//inventory
[RequireComponent(typeof(Inventory))]
[RequireComponent(typeof(Equipment))]
[RequireComponent(typeof(SkillBar))]
[RequireComponent(typeof(NPCSkillManager))]

//Network
[RequireComponent(typeof(NetworkTransform))]
[RequireComponent(typeof(NetworkAnimator))]


public class NPCMovement : NetworkIdentity
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

    void Update()
    {
        Vector3 velocityDirection = new Vector3(astar.velocity.x, 0, astar.velocity.z);
        if (velocityDirection == Vector3.zero)
        {
            animator.SetFloat("VelocityX", 0);
        }
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
            characterFocus.target = null;
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
            characterStats.Revive();
            positionReset = false;

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

    public void Roam()
    {
        if (characterStats.dead) //i'm dead don't roam
            return;

        if (characterFocus.target != null) // i have focus, don't roam
            return;

        if (hateManager.hateList.Count > 0) //i have hate, don't roam
            return;

        if (!astar.pathPending && (astar.reachedEndOfPath || !astar.hasPath)) //i need a new path
        {
            astar.SearchPath();
            astar.destination = PickRandomPoint(characterStats.behaviorSO.roamDistance, transform);
            astar.maxSpeed = characterStats.characterRace.walkSpeed;
        }
        else //i'm still moving
        {
            astar.maxSpeed = characterStats.characterRace.walkSpeed;
        }
    }

    public void RunToTarget(Transform target)
    {
        Debug.Log(transform.name + "RunToTarget");
        if (!characterStats.dead && target != null)
        {
            distanceToTarget = Vector3.Distance(target.transform.position, transform.position); //distance to target
            if (distanceToTarget <= characterStats.characterRace.aggroRadius && distanceToTarget >= characterStats.characterRace.attackDistance)
            {//less than aggro radius and more than attack distance
                astar.destination = target.position;
                astar.maxSpeed = characterStats.characterRace.runSpeed;
            }
            else
            {
                astar.maxSpeed = 0;
                FaceTarget(target.position, transform);
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
            // Face the direction of movement when roaming
            if (characterFocus.target == null && !characterStats.dead)
            {
                Vector3 velocityDirection = new Vector3(astar.velocity.x, 0, astar.velocity.z);
                if (velocityDirection != Vector3.zero)
                {
                    Quaternion lookRotation = Quaternion.LookRotation(velocityDirection);
                    transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10f);
                }
            }
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

    //face a specified target location
    public void FaceTarget(Vector3 targetLocation, Transform transform)
    {
        Vector3 direction = (targetLocation - transform.position).normalized;
        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10f);
        }

        //RunToTarget(transform);
    }

    //check astar velocity and face that direction
    public void FaceMovementDirection()
    {
        Vector3 velocityDirection = new Vector3(astar.velocity.x, 0, astar.velocity.z);
        if (velocityDirection != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(velocityDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10f);
        }
    }

    public void ApproachTarget(Transform target)
    {
        if (!characterStats.dead && target != null)
        {
            distanceToTarget = Vector3.Distance(target.position, transform.position);

            // Within attack distance: stop moving and face target directly
            if (distanceToTarget <= characterStats.characterRace.attackDistance)
            {
                // Face the target directly when in attack range
                FaceTarget(target.position, transform);

                astar.destination = transform.position;
                astar.SearchPath();
                astar.maxSpeed = 0;
                astar.isStopped = true;
            }
            // Has hate on target: run toward target
            else if (hateManager.hateList.Count > 0 && hateManager.hateList.Contains(characterFocus.target))
            {
                // Face movement direction when running
                Vector3 velocityDirection = new Vector3(astar.velocity.x, 0, astar.velocity.z);
                if (velocityDirection != Vector3.zero)
                {
                    Quaternion lookRotation = Quaternion.LookRotation(velocityDirection);
                    transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10f);
                }

                astar.destination = target.position;
                astar.SearchPath();
                astar.maxSpeed = characterStats.characterRace.runSpeed;
                astar.isStopped = false;
            }
            // Within aggro radius but outside attack distance: run toward target
            else if (distanceToTarget <= characterStats.characterRace.aggroRadius)
            {
                // Face movement direction when running
                Vector3 velocityDirection = new Vector3(astar.velocity.x, 0, astar.velocity.z);
                if (velocityDirection != Vector3.zero)
                {
                    Quaternion lookRotation = Quaternion.LookRotation(velocityDirection);
                    transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10f);
                }

                astar.destination = target.position;
                astar.SearchPath();
                astar.maxSpeed = characterStats.characterRace.runSpeed;
                astar.isStopped = false;
            }
            // Outside aggro radius but within view: walk slowly toward target
            else if (distanceToTarget < characterStats.characterRace.viewRadius)
            {
                // Face movement direction when walking
                FaceMovementDirection();

                astar.destination = target.position;
                astar.SearchPath();
                astar.maxSpeed = characterStats.characterRace.walkSpeed;
                astar.isStopped = false;
            }
            // Outside view radius: return to spawn
            else
            {
                // No hate, return to spawn
                astar.destination = spawnPosition;
                astar.SearchPath();
                astar.maxSpeed = characterStats.characterRace.walkSpeed;
                astar.isStopped = false;

                // If close to spawn, stop and face original rotation
                if (Vector3.Distance(transform.position, spawnPosition) <= 0.5f)
                {
                    astar.destination = transform.position;
                    astar.maxSpeed = 0;
                    astar.isStopped = true;
                    transform.rotation = Quaternion.Slerp(transform.rotation, spawnRotation, Time.deltaTime * 10f);

                    // Only clear hate when back at spawn
                    hateManager.hateList.Clear();
                    characterFocus.target = null;
                }
            }

            // If idle at spawn, ensure facing spawn rotation
            if (characterFocus.target == null && hateManager.hateList.Count == 0)
            {
                if (Vector3.Distance(transform.position, spawnPosition) <= 0.5f)
                {
                    astar.destination = transform.position;
                    astar.maxSpeed = 0;
                    astar.isStopped = true;
                    transform.rotation = Quaternion.Slerp(transform.rotation, spawnRotation, Time.deltaTime * 10f);
                }
            }
        }
    }

    public void AnimationCheck()
    {
        animator.SetFloat("VelocityX", astar.maxSpeed);
    }
}

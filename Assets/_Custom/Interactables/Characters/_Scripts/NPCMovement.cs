using Pathfinding;
using UnityEngine;

public class NPCMovement : MonoBehaviour
{
    IAstarAI astar;
    public Animator animator;
    CharacterStats characterStats;
    //HateListScript hateListScript;
    public BehaviorSO behaviorSO;
    NPCFocus npcFocus;

    Vector3 spawnPosition;
    Quaternion spawnRotation;

    float despawnTimer = 10f;
    float respawnTimer;
    bool despawned;

    private void Start()
    {
        astar = GetComponent<IAstarAI>();
        characterStats = GetComponent<CharacterStats>();
        //hateListScript = GetComponent<HateListScript>();
        npcFocus = GetComponent<NPCFocus>();

        //find my location
        spawnPosition = transform.position;
        spawnRotation = transform.rotation;

        respawnTimer = behaviorSO.respawnTimer;
    }

    void Update()
    {
        if (npcFocus.playersTargetingMe.Count > 0)
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
        if (/*hateListScript.target == null &&*/ !characterStats.dead && npcFocus.playersTargetingMe.Count == 0)
        {
            if (!astar.pathPending && (astar.reachedEndOfPath || !astar.hasPath)) //i need a new path
            {
                astar.SearchPath();
                astar.destination = PickRandomPoint(behaviorSO.roamDistance, transform);
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
        //Debug.Log(transform.name + "PickRandomPoint");
        
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

    private void Despawn()
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
            //hateListScript.hateList.Clear();
            //hateListScript.target = null;

            despawnTimer = 10f;
        }
    }

    private void Respawn()
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

            respawnTimer = behaviorSO.respawnTimer;
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

    public float distanceToTarget;
    public void RunToTarget(Transform target)
    {
        //Debug.Log(transform.name + "RunToTarget");
        if (!characterStats.dead && target != null)
        {
            distanceToTarget = Vector3.Distance(target.transform.position, transform.position); //distance to target
            if (distanceToTarget <= characterStats.characterRace.aggroRadius && distanceToTarget >= characterStats.characterRace.attackDistance)
            {//less than agro radius and more than attack distnace
                astar.destination = target.position/* + new Vector3(0,0,3)*/;
                astar.maxSpeed = characterStats.characterRace.runSpeed;
            }
            else
            {
                astar.maxSpeed = 0;
            }
        }
    }
}

using Pathfinding;
using UnityEngine;

public class NPCMovement : MonoBehaviour
{
    IAstarAI astar;
    public Animator animator;
    CharacterStats characterStats;
    //HateListScript hateListScript;
    public BehaviorSO behaviorSO;

    Vector3 spawnPosition;
    Quaternion spawnRotation;

    float despawnTimer = 10f;
    float respawnTimer;
    bool despawned;

    private void Start()
    {
        astar = GetComponent<IAstarAI>();
        //animator = GetComponent<Animator>();
        characterStats = GetComponent<CharacterStats>();
        //hateListScript = GetComponent<HateListScript>();

        //find my location
        spawnPosition = transform.position;
        spawnRotation = transform.rotation;

        respawnTimer = behaviorSO.respawnTimer;
    }

    public void Roam()
    {
        if (/*hateListScript.target == null &&*/ !characterStats.dead)
        {
            //PathUtilities.IsPathPossible
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
}

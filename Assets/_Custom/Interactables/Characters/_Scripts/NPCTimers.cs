using Pathfinding;
using UnityEngine;

public class NPCTimers : MonoBehaviour
{
    //timers
    float tickPointOne = 0.1f;
    float tickOne = 1f;
    float tickTen = 10f;
    float tickOneHundred = 100f;

    //references
    FieldOfView fieldOfView;
    NPCMovement npcMovement;
    CharacterStats characterStats;
    HateManager hateList;

    private void Start()
    {
        fieldOfView = GetComponent<FieldOfView>();
        npcMovement = GetComponent<NPCMovement>();
        characterStats = GetComponent<CharacterStats>();
        hateList = GetComponent<HateManager>();
    }

    private void Update()
    {
        TickPointOne();
        TickOne();
        TickTen();
        TickOneHundred();
    }

    private void TickPointOne() //do every tenth of a second
    {
        tickPointOne -= Time.deltaTime;
        if (tickPointOne <= 0)
        {
            tickPointOne = 0.1f; //reset timer
            /* v Start code here v */
            fieldOfView.FindVisibleTargets();
            fieldOfView.FindHearableTargets();
            npcMovement.CheckAstarVelocity();
            hateList.UpdateHateList();
            hateList.AggroTarget();
            characterStats.DeathCheck();
            //npcMovement.ResponseToBeingTargeted();
        }
    }

    private void TickOne() //do every second
    {
        tickOne -= Time.deltaTime;
        if (tickOne <= 0)
        {
            tickOne = 1f; //reset timer
            /* v Start code here v */
            //npcMovement.ResetPosition();
        }
    }
    private void TickTen() //do every ten seconds
    {
        tickTen -= Time.deltaTime;
        if (tickTen <= 0)
        {
            tickTen = 10f; //reset timer
            /* v Start code here v */
            npcMovement.Roam();
            //characterStats.ConstitutionRecovery(1 + characterStats.constitutionModifier);
        }
    }
    private void TickOneHundred() //do every Hundred seconds
    {
        tickOneHundred -= Time.deltaTime;
        if (tickOneHundred <= 0)
        {
            tickOneHundred = 100f; //reset timer
            /* v Start code here v */
        }
    }
}

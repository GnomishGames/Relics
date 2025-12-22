using UnityEngine;

public class PlayerTimers : MonoBehaviour
{
    //timers
    float tickPointOne = 0.1f;
    float tickOne = 1f;
    float tickTen = 10f;
    float tickOneHundred = 100f;

    //references
    FieldOfView fieldOfView;
    CharacterStats characterStats;
    PlayerMovement playerMovement;
    public SkillBar skillbar;

    private void Start()
    {
        fieldOfView = GetComponent<FieldOfView>();
        characterStats = GetComponent<CharacterStats>();
    }

    private void Update()
    {
        TickPointOne();
        TickOne();
        TickTen();
        TickOneHundred();
    }

    private void TickPointOne()
    {
        tickPointOne -= Time.deltaTime;
        if (tickPointOne <= 0)
        {
            tickPointOne = 0.1f;
            /* v Start code here v */
            fieldOfView.FindVisibleTargets();
            fieldOfView.FindHearableTargets();
            characterStats.DeathCheck();
        }
    }

    private void TickOne()
    {
        tickOne -= Time.deltaTime;
        if (tickOne <= 0)
        {
            tickOne = 1f;
            /* v Start code here v */
            if (skillbar.autoattackOn)
            {
                skillbar.DoSkill(0, skillbar.skillTimer[0]);
            }
        }
    }

    private void TickTen()
    {
        tickTen -= Time.deltaTime;
        if (tickTen <= 0)
        {
            tickTen = 10f;
            /* v Start code here v */

            //regenerate health
            characterStats.RegenerateStats();
        }
    }

    private void TickOneHundred()
    {
        tickOneHundred -= Time.deltaTime;
        if (tickOneHundred <= 0)
        {
            tickOneHundred = 100f;
            /* v Start code here v */
        }
    }
}
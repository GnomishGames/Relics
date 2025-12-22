using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    public float viewRadius;
    public float viewAngle;

    public LayerMask targetMask; //creatures that can be seen/heard
    public LayerMask obsticleMask; //walls/objects that block sight/hearing

    public List<Interactable> visibleTargets = new List<Interactable>();
    public List<Interactable> hearableTargets = new List<Interactable>();

    //NpcStats npcStats;
    CharacterStats characterStats;

    private void Start()
    {
        //npcStats = GetComponent<NpcStats>();
        characterStats = GetComponent<CharacterStats>();
        viewRadius = characterStats.characterRace.viewRadius;
        viewAngle = characterStats.characterRace.viewAngle;
    }

    public void FindVisibleTargets()
    {
        visibleTargets.Clear();
        if (!characterStats.dead)
        {
            //all characters need the character layer to be detected
            Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, characterStats.characterRace.viewRadius, targetMask);

            for (int i = 0; i < targetsInViewRadius.Length; i++)
            {
                Interactable target = targetsInViewRadius[i].transform.GetComponent<Interactable>();
                Vector3 dirToTarget = (target.transform.position - transform.position).normalized;
                if (Vector3.Angle(transform.forward, dirToTarget) < characterStats.characterRace.viewAngle / 2)
                {
                    float disToTarget = Vector3.Distance(transform.position, target.transform.position);
                    if (!Physics.Raycast(transform.position, dirToTarget, disToTarget, obsticleMask) && !target.GetComponent<CharacterStats>().dead) //no obstacles in the way!
                    {
                        visibleTargets.Add(target);
                        visibleTargets.Remove(transform.GetComponent<Interactable>());
                    }
                }
            }
        }
    }

    public void FindHearableTargets()
    {
        hearableTargets.Clear();
        if (!characterStats.dead)
        {
            //all characters need the character layer to be detected
            Collider[] targetsInHearRadius = Physics.OverlapSphere(transform.position, characterStats.characterRace.hearRadius, targetMask);

            for (int i = 0; i < targetsInHearRadius.Length; i++)
            {
                Interactable target = targetsInHearRadius[i].transform.GetComponent<Interactable>();
                Vector3 dirToTarget = (target.transform.position - transform.position).normalized;
                float disToTarget = Vector3.Distance(transform.position, target.transform.position);
                if (!Physics.Raycast(transform.position, dirToTarget, disToTarget, obsticleMask) && !target.GetComponent<CharacterStats>().dead) //no obstacles in the way!
                {
                    hearableTargets.Add(target);
                    hearableTargets.Remove(transform.GetComponent<Interactable>());
                }
            }
        }
    }

    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal) //used for editor only
    {
        if (!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }

}

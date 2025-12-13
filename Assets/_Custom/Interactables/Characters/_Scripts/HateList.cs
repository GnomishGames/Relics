using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HateList : MonoBehaviour
{
    //anger management
    public List<Interactable> hateList = new List<Interactable>();
    public Interactable target; //this being's current target

    //references
    FieldOfView fieldOfView;
    NPCMovement nPCMovement;
    CharacterStats characterStats;
    CreatureFaction creatureFaction;

    private void Awake()
    {
        fieldOfView = GetComponent<FieldOfView>();
        nPCMovement = GetComponent<NPCMovement>();
        characterStats = GetComponent<CharacterStats>();
        creatureFaction = GetComponent<CreatureFaction>();
    }

    private void Update()
    {
        if (target != null && !characterStats.dead)
        {
            NPCMovement.FaceTarget(target.transform.position, transform);
        }
    }

    public void UpdateHateList()
    {
        if (!characterStats.dead)
        {
            foreach (Interactable item in fieldOfView.visibleTargets)
            {
                if (!hateList.Contains(item))
                {
                    if (IsHostileTo(item.GetComponent<CharacterStats>()))
                    {
                        hateList.Add(item);
                    }
                }
            }

            hateList.RemoveAll(item => item == null); //remove null items

            if (hateList.Count > 0)
            {
                target = hateList[0]; //set my target as the top of my hatelist

                foreach (Interactable interactable in hateList.ToList())
                {
                    if (interactable.GetComponent<CharacterStats>().dead)
                    {
                        hateList.Remove(interactable);
                        target = null;
                    }
                }
            }
        }
    }

    public bool IsHostileTo(CharacterStats other)
    {
        return characterStats.IsEnemy(other);
    }

    public void AggroTarget()
    {
        if ( target != null && !characterStats.dead)
        {
            float distanceToTarget = Vector3.Distance(target.transform.position, transform.position);
            if (distanceToTarget <= characterStats.characterRace.aggroRadius)
            {
                nPCMovement.RunToTarget(target.transform);
            }
            if (distanceToTarget > characterStats.characterRace.aggroRadius)
            {
                hateList.Remove(target);
            }
        }
    }
}
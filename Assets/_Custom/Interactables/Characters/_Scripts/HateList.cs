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

    private void Awake()
    {
        fieldOfView = GetComponent<FieldOfView>();
        nPCMovement = GetComponent<NPCMovement>();
        characterStats = GetComponent<CharacterStats>();
    }

    private void Update()
    {
        if (target != null && !characterStats.dead)
        {
            NPCMovement.FaceTarget(target.transform.position, transform);
        }

        UpdateHateList();
    }

    public void UpdateHateList()
    {
        if (!characterStats.dead)
        {
            foreach (Interactable item in fieldOfView.visibleTargets)
            {
                if (!hateList.Contains(item))
                {
                    //check for faction via faction system
                    CreatureFaction itemFaction = item.GetComponent<CreatureFaction>();
                    CreatureFaction myFaction = GetComponent<CreatureFaction>();
                    if (itemFaction != null && myFaction != null)
                    {
                        if (myFaction.IsEnemy(itemFaction))
                        {
                            hateList.Add(item); //add to hate list if enemy
                        }
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
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HateManager : MonoBehaviour
{
    //anger management
    public List<Interactable> hateList = new List<Interactable>();
    public Interactable target; //this beings't current target

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
            nPCMovement.FaceTarget(target.transform.position, transform);
            AggroTarget();
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
                        //add item as my target
                        target = item;
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
                    if (interactable.GetComponent<CharacterStats>().dead) //remove dead targets
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
        if (target != null)
        {
            float distanceToTarget = Vector3.Distance(target.transform.position, transform.position);
            if (distanceToTarget <= characterStats.characterRace.aggroRadius)
            {
                nPCMovement.RunToTarget(target.transform);
            }
            if (distanceToTarget > characterStats.characterRace.aggroRadius)
            {
                hateList.Remove(target);
                target = null;
            }
        }
    }

    //add specific interactable to hate list
    public void AddToHateList(Interactable interactable)
    {
        if (!hateList.Contains(interactable))
        {
            hateList.Add(interactable); //add to hate list 
        }
    }
}
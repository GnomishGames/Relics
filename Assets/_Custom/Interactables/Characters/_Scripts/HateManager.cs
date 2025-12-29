using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

public class HateManager : MonoBehaviour
{
    //anger management
    public List<Interactable> hateList = new List<Interactable>();

    //references
    FieldOfView fieldOfView;
    NPCMovement nPCMovement;
    CharacterStats characterStats;
    CharacterFocus characterFocus;

    private void Awake()
    {
        fieldOfView = GetComponent<FieldOfView>();
        nPCMovement = GetComponent<NPCMovement>();
        characterStats = GetComponent<CharacterStats>();
        characterFocus = GetComponent<CharacterFocus>();
    }

    private void Update()
    {
        if (characterFocus.target != null && !characterStats.dead)
        {
            nPCMovement.ApproachTarget(characterFocus.target.transform);
        }

        if (hateList.Count > 0 && characterFocus.target == null && !characterStats.dead)
        {
            characterFocus.target = hateList[0];
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
                        hateList.Add(item);
                    }
                }
            }

            hateList.RemoveAll(item => item == null); //remove null items

            if (hateList.Count > 0)
            {
                characterFocus.target = hateList[0]; //set my target as the top of my hatelist

                foreach (Interactable interactable in hateList.ToList())
                {
                    if (interactable.GetComponent<CharacterStats>().dead) //remove dead targets
                    {
                        hateList.Remove(interactable);
                        characterFocus.target = null;
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
        if (characterFocus.target != null)
        {
            float distanceToTarget = Vector3.Distance(characterFocus.target.transform.position, transform.position);
            if (distanceToTarget <= characterStats.characterRace.aggroRadius)
            {
                nPCMovement.RunToTarget(characterFocus.target.transform); Debug.Log("Running to Target");
            }
            
            if (distanceToTarget > characterStats.characterRace.aggroRadius || characterFocus.target.GetComponent<CharacterStats>().dead)
            {
                hateList.Remove(characterFocus.target);
                characterFocus.target = null;
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

    private void HandleBeingAttacked(Interactable attacker)
    {
        AddToHateList(attacker);
        characterFocus.target = attacker;
    }
}
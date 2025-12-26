using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HateManager : MonoBehaviour
{
    //anger management
    public List<Interactable> hateList = new List<Interactable>();

    //references
    FieldOfView fieldOfView;
    NPCMovement nPCMovement;
    CharacterStats characterStats;
    NPCSkillManager nPCSkillManager;
    CharacterFocus characterFocus;
    Pathfinding.IAstarAI astar;

    private void Awake()
    {
        fieldOfView = GetComponent<FieldOfView>();
        nPCMovement = GetComponent<NPCMovement>();
        characterStats = GetComponent<CharacterStats>();
        nPCSkillManager = GetComponent<NPCSkillManager>();
        characterFocus = GetComponent<CharacterFocus>();
        astar = GetComponent<Pathfinding.IAstarAI>();
    }

    private void Update()
    {
        AggroTarget();
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
                        characterFocus.currentFocus = item;
                        characterFocus.SetCharacterFocus(item.GetComponent<CharacterStats>());
                        hateList.Add(item);
                    }
                }
            }

            hateList.RemoveAll(item => item == null); //remove null items

            if (hateList.Count > 0)
            {
                characterFocus.currentFocus = hateList[0]; //set my target as the top of my hatelist

                foreach (Interactable interactable in hateList.ToList())
                {
                    if (interactable.GetComponent<CharacterStats>().dead)
                    {
                        hateList.Remove(interactable);
                        characterFocus.currentFocus = null;
                    }
                }
            }
        }
    }

    // When players are targeting this NPC but we don't yet have a target,
    // face the first targeter and stop movement.
    public void ResponseToBeingTargeted()
    {
        //only respond if I have a target
        if (characterFocus.currentFocus != null)
            return;

        //only respond if i am not hostile to player targeting me
        if (IsHostileTo(characterFocus.currentFocus.GetComponent<CharacterStats>()))
            return;

        if (characterFocus != null && characterFocus.charactersTargetingMe.Count > 0 && !characterStats.dead && characterFocus.currentFocus == null)
        {
            Transform targetPlayer = characterFocus.charactersTargetingMe[0].transform;
            //NPCMovement.FaceTarget(targetPlayer.position, transform);

            if (astar != null)
            {
                astar.destination = transform.position;
                astar.SearchPath();
                astar.maxSpeed = 0;
            }
        }
    }

    public bool IsHostileTo(CharacterStats other)
    {
        return characterStats.IsEnemy(other);
    }

    public void AggroTarget()
    {
        if (characterFocus.currentFocus != null && !characterStats.dead)
        {
            float distanceToTarget = Vector3.Distance(characterFocus.currentFocus.transform.position, transform.position);
            
            // Only approach if within aggro radius OR already on hate list
            if (distanceToTarget <= characterStats.characterRace.aggroRadius || hateList.Contains(characterFocus.currentFocus))
            {
                nPCMovement.ApproachTarget(characterFocus.currentFocus.transform);
            }
            else
            {
                // Outside aggro radius and not on hate list: stop pursuing
                if (astar != null)
                {
                    astar.destination = transform.position;
                    astar.SearchPath();
                    astar.maxSpeed = 0;
                    astar.isStopped = true;
                }
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
using UnityEngine;

public class Character : Interactable
{
    //references
    public Race characterRace;
    public Class characterClass;
    public BehaviorSO behaviorSO;
    public Faction faction;
    
    //vars
    public bool dead;
    public int xpToGive;
}

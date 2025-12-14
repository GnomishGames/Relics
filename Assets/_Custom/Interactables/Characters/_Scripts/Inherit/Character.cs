using UnityEngine;

public class Character : Interactable
{
    //references
    public RaceSO characterRace;
    public ClassSO characterClass;
    public BehaviorSO behaviorSO;
    public FactionSO faction;
    
    //vars
    public bool dead;
    public int xpToGive;
}

using UnityEngine;

public class Character : Interactable
{
    //references
    public RaceSO characterRace;
    public ClassSO characterClass;
    public BehaviorSO behaviorSO;
    public FactionSO faction;
    
    //status
    public bool dead;
    public bool sitting;

    //experience
    public int xpToGive;
}

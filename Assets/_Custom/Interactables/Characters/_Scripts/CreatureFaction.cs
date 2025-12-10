using UnityEngine;

public class CreatureFaction : MonoBehaviour
{
    public Faction faction;

    public bool IsEnemy(CreatureFaction other)
    {
        return FactionManager.Instance.IsHostile(faction, other.faction);
    }
}
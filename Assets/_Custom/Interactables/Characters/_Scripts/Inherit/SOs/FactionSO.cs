using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Object/Faction")]
public class FactionSO : ScriptableObject
{
    public string factionName;

    [Serializable]
    public class FactionRelationship
    {
        public FactionSO otherFaction;
        public int relationshipValue;
    }

    public List<FactionRelationship> defaultRelationships;
}
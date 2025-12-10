using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Object/Faction")]
public class Faction : ScriptableObject
{
    public string factionName;

    [Serializable]
    public class FactionRelationship
    {
        public Faction otherFaction;
        public int relationshipValue;
    }

    public List<FactionRelationship> defaultRelationships;
}
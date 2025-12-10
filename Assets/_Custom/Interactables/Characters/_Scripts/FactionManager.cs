using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO;

public class FactionManager : MonoBehaviour
{
    public static FactionManager Instance;

    [Header("List of ALL factions (drag SOs here)")]
    public List<Faction> factions;

    private Dictionary<(string, string), int> _runtimeRelationships;

    private string SavePath => Path.Combine(Application.persistentDataPath, "faction_relations.json");

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            LoadDefaultRelationships();
            LoadSavedData();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // ----------------------------------------------------
    // STEP 1: Load SO defaults into runtime dictionary
    // ----------------------------------------------------
    private void LoadDefaultRelationships()
    {
        _runtimeRelationships = new Dictionary<(string, string), int>();

        foreach (var f in factions)
        {
            foreach (var rel in f.defaultRelationships)
            {
                _runtimeRelationships[(f.factionName, rel.otherFaction.factionName)] = rel.relationshipValue;
                Debug.Log($"Loading defaults for {f.factionName}: {rel.otherFaction} = {rel.relationshipValue}");

            }
        }
    }

    // ----------------------------------------------------
    // STEP 2: Load saved data (if it exists) and override defaults
    // ----------------------------------------------------
    private void LoadSavedData()
    {
        if (!File.Exists(SavePath))
            return;

        var json = File.ReadAllText(SavePath);
        var savedList = JsonUtility.FromJson<RelationshipSaveWrapper>(json);

        foreach (var entry in savedList.entries)
        {
            _runtimeRelationships[(entry.factionA, entry.factionB)] = entry.value;
        }
    }

    // ----------------------------------------------------
    // STEP 3: Query relationships during gameplay
    // ----------------------------------------------------
    public int GetRelationship(Faction a, Faction b)
    {
        if (_runtimeRelationships.TryGetValue((a.factionName, b.factionName), out int val))
        {
            Debug.Log($"Relationship between {a.factionName} and {b.factionName} is {val}");
            return val;
        }
        Debug.Log($"No relationship data between {a.factionName} and {b.factionName}, defaulting to 0");
        return 0;
    }

    public bool IsHostile(Faction a, Faction b)
    {
        return GetRelationship(a, b) < 0;
    }

    public bool IsFriendly(Faction a, Faction b)
    {
        return GetRelationship(a, b) > 0;
    }

    // ----------------------------------------------------
    // STEP 4: Modify relationships dynamically
    // ----------------------------------------------------
    public void ModifyRelationship(Faction a, Faction b, int amount)
    {
        var key = (a.factionName, b.factionName);

        if (!_runtimeRelationships.ContainsKey(key))
            _runtimeRelationships[key] = 0;

        _runtimeRelationships[key] += amount;
    }

    // ----------------------------------------------------
    // STEP 5: Save to disk at exit
    // ----------------------------------------------------
    private void OnApplicationQuit()
    {
        SaveData();
    }

    public void SaveData()
    {
        RelationshipSaveWrapper wrapper = new RelationshipSaveWrapper();
        wrapper.entries = _runtimeRelationships.Select(kvp =>
            new RelationshipSaveEntry
            {
                factionA = kvp.Key.Item1,
                factionB = kvp.Key.Item2,
                value = kvp.Value
            }).ToList();

        var json = JsonUtility.ToJson(wrapper, true);
        File.WriteAllText(SavePath, json);
    }

    // Save structure
    [System.Serializable]
    public class RelationshipSaveWrapper
    {
        public List<RelationshipSaveEntry> entries;
    }

    [System.Serializable]
    public class RelationshipSaveEntry
    {
        public string factionA;
        public string factionB;
        public int value;
    }
}

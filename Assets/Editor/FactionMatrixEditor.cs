using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

public class FactionMatrixEditor : EditorWindow
{
    private Vector2 scroll;
    private List<Faction> factions;

    [MenuItem("Game Tools/Faction Relationship Matrix")]
    public static void ShowWindow()
    {
        FactionMatrixEditor window = GetWindow<FactionMatrixEditor>("Faction Matrix");
        window.Show();
    }

    private void OnEnable()
    {
        LoadFactions();
    }

    private void LoadFactions()
    {
        string[] guids = AssetDatabase.FindAssets("t:Faction");
        factions = new List<Faction>();

        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            Faction f = AssetDatabase.LoadAssetAtPath<Faction>(path);
            factions.Add(f);
        }
    }

    private void OnGUI()
    {
        if (factions == null || factions.Count == 0)
        {
            EditorGUILayout.HelpBox("No Faction assets found.", MessageType.Warning);
            return;
        }

        scroll = EditorGUILayout.BeginScrollView(scroll);

        DrawMatrix();

        EditorGUILayout.EndScrollView();
    }

    private void DrawMatrix()
    {
        // Column headers
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Faction", GUILayout.Width(120));

        foreach (var colFaction in factions)
        {
            EditorGUILayout.LabelField(colFaction.factionName, GUILayout.Width(80));
        }

        EditorGUILayout.EndHorizontal();

        // Body rows
        foreach (var rowFaction in factions)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(rowFaction.factionName, GUILayout.Width(120));

            foreach (var colFaction in factions)
            {
                if (rowFaction == colFaction)
                {
                    EditorGUILayout.LabelField("-", GUILayout.Width(80));
                    continue;
                }

                int currentValue = GetValue(rowFaction, colFaction);
                int newValue = EditorGUILayout.IntField(currentValue, GUILayout.Width(80));

                if (newValue != currentValue)
                {
                    Undo.RecordObject(rowFaction, "Edit Faction Relationship");
                    SetValue(rowFaction, colFaction, newValue);

                    // Optional: make relationships symmetric
                    // Undo.RecordObject(colFaction, "Edit Faction Relationship");
                    // SetValue(colFaction, rowFaction, newValue);

                    EditorUtility.SetDirty(rowFaction);
                    // EditorUtility.SetDirty(colFaction); // For symmetric editing
                }
            }

            EditorGUILayout.EndHorizontal();
        }
    }

    // Helper: Get value from rowFaction → colFaction
    private int GetValue(Faction a, Faction b)
    {
        foreach (var rel in a.defaultRelationships)
        {
            if (rel.otherFaction == b)
                return rel.relationshipValue;
        }
        return 0;
    }

    // Helper: Set value in rowFaction → colFaction
    private void SetValue(Faction a, Faction b, int value)
    {
        foreach (var rel in a.defaultRelationships)
        {
            if (rel.otherFaction == b)
            {
                rel.relationshipValue = value;
                return;
            }
        }

        // If relationship not found, add it
        a.defaultRelationships.Add(new Faction.FactionRelationship
        {
            otherFaction = b,
            relationshipValue = value
        });
    }
}

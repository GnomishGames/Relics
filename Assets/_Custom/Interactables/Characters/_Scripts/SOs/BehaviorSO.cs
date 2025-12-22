using UnityEngine;

[CreateAssetMenu(fileName = "New Behavior", menuName = "Scriptable Object/Character/New Behavior")]
public class BehaviorSO : ScriptableObject
{
    public int roamDistance;
    public float respawnTimer;
    public float despawnTimer;
}

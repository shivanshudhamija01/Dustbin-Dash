using UnityEngine;
[CreateAssetMenu(menuName = "DustbinGame/WasteData", fileName = "WasteData_New")]
public class WasteData : ScriptableObject
{
    [Header("Score")]
    public int baseScore = 10;
    [Header("Movement")]
    public float speedMultiplier = 1f;
    public float driftMultiplier = 1f;
    public float angularVelocityMultiplier = 1f;
    [Header("Bounce")]
    public bool bounceFromWalls = true;
    public float bounceStrength = 1f;

}
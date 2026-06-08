using UnityEngine;

/// <summary>
/// ScriptableObject that defines one type of waste (banana, bottle, can, …).
/// Create via: Assets > Create > DustbinGame > WasteData
/// </summary>
[CreateAssetMenu(menuName = "DustbinGame/WasteData", fileName = "WasteData_New")]
public class WasteData : ScriptableObject
{
    [Header("Visuals")]
    public Sprite sprite;
    public string displayName = "Waste";

    [Header("Score")]
    [Tooltip("Base points awarded when this item is caught.")]
    public int baseScore = 10;

    [Header("Physics Overrides (0 = use spawner defaults)")]
    [Tooltip("Extra fall speed added on top of the spawner's calculated speed.")]
    public float speedBonus = 0f;

    [Tooltip("Extra horizontal drift range added on top of the spawner's drift.")]
    public float driftBonus = 0f;
}
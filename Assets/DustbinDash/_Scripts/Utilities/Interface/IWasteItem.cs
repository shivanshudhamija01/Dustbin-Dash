using UnityEngine;

public interface IWasteItem
{
    WasteData GetConfig();
    int GetScore();
    void Launch(Vector2 startVelocity, float angularDrift);
}

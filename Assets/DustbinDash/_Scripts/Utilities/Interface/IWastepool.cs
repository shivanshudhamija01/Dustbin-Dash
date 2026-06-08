/// <summary>
/// Contract that any waste pool must fulfill.
/// WasteItem depends only on this interface — not on the concrete WastePool class.
/// </summary>
public interface IWastePool
{
    /// <summary>Borrow a ready-to-use WasteItem.</summary>
    WasteItem Get();

    /// <summary>Return a used WasteItem so it can be reused later.</summary>
    void Return(WasteItem item);

    /// <summary>Return every currently active item at once (e.g. on Game Over).</summary>
    void ReturnAll();

    int ActiveCount { get; }
    int TotalCount { get; }
}
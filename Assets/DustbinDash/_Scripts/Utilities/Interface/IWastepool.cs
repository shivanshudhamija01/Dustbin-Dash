/// <summary>
/// Contract that any waste pool must fulfill.
/// WasteItem depends only on this interface — not on the concrete WastePool class.
/// </summary>
public interface IWastePool
{
    WasteItem Get();

    void Return(WasteItem item);

    void ReturnAll();
}

// In waste pool , what i will be doing is that , instead of creating or passing the reference of all these things, it is fine to 
// have the singleton class , and use that , 
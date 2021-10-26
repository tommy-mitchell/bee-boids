using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

#if UNITY_EDITOR
[InitializeOnLoad]
#endif
public class TriboolVector2Processor : InputProcessor<Vector2>
{
    #if UNITY_EDITOR
    static TriboolVector2Processor() => Initialize();
    #endif

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void Initialize() => InputSystem.RegisterProcessor<TriboolVector2Processor>();

    // turns input into a tribool on each axis (-1, 0, 1)
    public override Vector2 Process(Vector2 value, InputControl control)
    {
        float x = value.x > 0 ? 1 : value.x < 0 ? -1 : 0;
        float y = value.y > 0 ? 1 : value.y < 0 ? -1 : 0;

        return new Vector2(x, y);
    }
}
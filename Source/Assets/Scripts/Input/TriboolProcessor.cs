using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

#if UNITY_EDITOR
[InitializeOnLoad]
#endif
public class TriboolProcessor : InputProcessor<float>
{
    #if UNITY_EDITOR
    static TriboolProcessor() => Initialize();
    #endif

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void Initialize() => InputSystem.RegisterProcessor<TriboolProcessor>();

    // turns input into a tribool (-1, 0, 1)
    public override float Process(float value, InputControl control) => value > 0 ? 1 : value < 0 ? -1 : 0;
}
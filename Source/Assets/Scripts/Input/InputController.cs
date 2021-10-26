using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour
{
    public static InputController instance;

    [field: SerializeField]
    public PlayerInput PlayerInput { get; private set; }

    public const string SCHEME_KEYBOARD = "KB";
    public const string SCHEME_GAMEPAD  = "Gamepad";

    private void Awake()
    {
        instance = this;
        PlayerInput ??= GetComponent<PlayerInput>();
    }

    private bool Valid(InputAction.CallbackContext context) => context.phase == InputActionPhase.Started;
    
    public event Action<Vector2> _onMove;
    public event Action          _onSpawnBoid;
    public event Action          _onClearFlock;
    public event Action          _onToggleCursorFollow;
    public event Action<float>   _onZoom;
    public event Action          _onResetZoom;
    public event Action          _onInputChanged;

    public void OnMove(InputAction.CallbackContext context) => _onMove?.Invoke(context.ReadValue<Vector2>());

    public void OnSpawnBoid(InputAction.CallbackContext context) { if(Valid(context)) _onSpawnBoid?.Invoke(); }

    public void OnClearFlock(InputAction.CallbackContext context) { if(Valid(context)) _onClearFlock?.Invoke(); }

    public void OnToggleCursorFollow(InputAction.CallbackContext context) { if(Valid(context)) _onToggleCursorFollow?.Invoke(); }

    public void OnZoom(InputAction.CallbackContext context) => _onZoom?.Invoke(context.ReadValue<float>());

    public void OnResetZoom(InputAction.CallbackContext context) => _onResetZoom?.Invoke();

    public void OnExit(InputAction.CallbackContext context) => Application.Quit();

    public void OnInputChanged() => _onInputChanged?.Invoke();

    public void ToggleCursor() => Cursor.visible = PlayerInput.currentControlScheme == SCHEME_KEYBOARD;
}
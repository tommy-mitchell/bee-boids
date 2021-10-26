using UnityEngine;
using UnityEngine.InputSystem;

public class ChangeSprites : MonoBehaviour
{
    [field: SerializeField]
    private Sprite Keyboard { get; set; }

    [field: SerializeField]
    private Sprite KeyboardPress { get; set; }

    [field: SerializeField]
    private Sprite Gamepad { get; set; }

    [field: SerializeField]
    private Sprite GamepadPress { get; set; }

    private bool InputIsKeyboard => InputController.instance.PlayerInput.currentControlScheme == InputController.SCHEME_KEYBOARD;

    private float TapTime => InputSystem.settings.defaultTapTime;

    private SpriteRenderer _renderer;

    private Sprite regular;
    
    private Sprite press;

    

    private void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>();
        SetSpritesKeyboard();
    }
    
    private void Start()
    {
        // change sprites depending on if using keyboard or controller
        InputController.instance._onInputChanged += () => {
            if(InputIsKeyboard)
                SetSpritesKeyboard();
            else if(!gameObject.name.Contains("Follow"))
                SetSpritesGamepad();

            _renderer.sprite = regular;
        };

        // set appropriate input listeners
        if(gameObject.name.Contains("Follow"))
            InputController.instance._onToggleCursorFollow += () => StartCoroutine(SetPressSprite());
        else if(gameObject.name.Contains("Clear"))
            InputController.instance._onClearFlock         += () => StartCoroutine(SetPressSprite());
        else if(gameObject.name.Contains("Spawn"))
            InputController.instance._onSpawnBoid          += () => StartCoroutine(SetPressSprite());
    }

    private void SetSpritesKeyboard()
    {
        regular = Keyboard;
        press   = KeyboardPress;
    }

    private void SetSpritesGamepad()
    {
        regular = Gamepad;
        press   = GamepadPress;
    }

    private System.Collections.IEnumerator SetPressSprite()
    {
        _renderer.sprite = press;

        yield return new WaitForSeconds(TapTime);

        _renderer.sprite = regular;
    }
}

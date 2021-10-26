using UnityEngine;
using static BoidLibrary.GenericMethods;

public class MoveCamera : MonoBehaviour
{
    private Camera _camera;

    [field: SerializeField]
    private UnityEngine.Experimental.Rendering.Universal.PixelPerfectCamera PixelPerfectCamera { get; set; }

    [field: SerializeField]
    private float MoveSpeed { get; set; } = 5.0f;

    [field: SerializeField, Tooltip("The sound to play when bumping against the bounds of the scene.")]
    private AudioEvent BumpSound { get; set; }

    [field: SerializeField, Tooltip("Time between bump sound plays.")]
    private Cooldown BumpCooldown { get; set; }

    [field: SerializeField]
    private bool PlayBumpSound { get; set; }

    [field: SerializeField, Tooltip("Minimum and maximum PPU values, inclusive.")]
    private Vector2 PPUBounds { get; set; }

    [field: SerializeField, Tooltip("The minimum PPU value above which zoom speed is doubled.")]
    private float PPUFastZoomCutoff { get; set; }

    private Vector2 bounds;

    private Vector2 moveInput;

    private void Start()
    {
        _camera = GetComponent<Camera>();

        InputController.instance._onMove += (_input) => moveInput = _input;
        InputController.instance._onZoom += (_input) => PixelScale += (int) _input;
        InputController.instance._onResetZoom +=  () => PixelScale = 16;

        BumpSound.Init();
        ResetBounds();
    }

    private void Update()
    {
        // get movement on individual axes
        Vector3 moveX = Vector3.right * moveInput.x * MoveSpeed * Time.deltaTime;
        Vector3 moveY = Vector3.up    * moveInput.y * MoveSpeed * Time.deltaTime;

        // see if either axis moves the camera out of bounds
        bool outX = IsOutOfBounds(transform.position + moveX, bounds);
        bool outY = IsOutOfBounds(transform.position + moveY, bounds);

        // if out of bounds, play a sound and zero the applicable input
        if(outX || outY)
        {
            if(PlayBumpSound && BumpCooldown.IsOver)
            {
                BumpSound.Play();
                BumpCooldown.Reset();
            }

            if(outX) moveX = Vector3.zero;
            if(outY) moveY = Vector3.zero;
        }

        // apply remaining input
        transform.position += moveX + moveY;

        BumpCooldown.Timer -= Time.deltaTime;
    }

    // bounds for 16 PPU
    private Vector2 DefaultBounds => new Vector2(20f, 11.25f);

    // sets the PPU in the PixelPerfectCamera to zoom, and adjusts camera bounds accordingly
    private int PixelScale {
        get => PixelPerfectCamera.assetsPPU;
        set {
            // zoom PPU bounds
            if(value < PPUBounds.x || value > PPUBounds.y - 1)
                return;

            // set zoom, incrementing by 2 if above given PPU (for faster zooming)
            int difference = value - PixelPerfectCamera.assetsPPU;
            int newPPU = value > PPUFastZoomCutoff ? value + difference : value;

            /*if(newPPU > PixelPerfectCamera.assetsPPU)
            {
                if(InputController.instance.PlayerInput.currentControlScheme == InputController.SCHEME_KEYBOARD)
                {
                    Vector2 mousePos = UnityEngine.InputSystem.Mouse.current.position.ReadValue();
                    transform.position = _camera.ScreenToWorldPoint(mousePos);
                }
            }*/

            PixelPerfectCamera.assetsPPU = newPPU;

            // set bounds to zero if at full width
            if(newPPU == PPUBounds.x)
                bounds = Vector2.zero;
            else // calculate bounds
            {
                // formula is 'value/#', which is related to 16 -> 15/14, 16/16, 17/18, 18/20, etc
                float fraction = ( (float) newPPU / (float) (16 + (2 * (newPPU - 16))) );
                bounds = DefaultBounds / fraction;
            }

            // move camera if zooming out
            if(IsOutOfBounds(transform.position, bounds))
            {
                // get current quadrant
                Vector2 signs = GetTribool(transform.position);
                transform.position = signs * bounds;
            }
        }
    }

    private void ResetBounds() => bounds = DefaultBounds;
}
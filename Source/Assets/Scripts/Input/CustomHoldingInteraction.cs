#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine.InputSystem;

// https://gist.github.com/Invertex/db99b1b16ca53805ae02697b1a51ea77
namespace Invertex.UnityInputExtensions.Interactions
{
    /// <summary>
    /// Custom Hold interaction for New Input System.
    /// With this, the .performed callback will be called everytime the input system updates. 
    /// Allowing a purely callback based approach to a button hold instead of polling it in an Update() loop and using bools
    /// .started will be called when the pressPoint threshold has been hit.
    /// .performed won't start being called until the 'duration' of holding has been met.
    /// .cancelled will be called when no-longer actuated
    /// </summary>
#if UNITY_EDITOR
    [InitializeOnLoad]
#endif
    public class CustomHoldingInteraction : IInputInteraction
    {
        public bool useDefaultSettingsDuration = false;
        public float duration;
        public bool useDefaultSettingsPressPoint = false;
        public float pressPoint;
        private float durationOrDefault => useDefaultSettingsDuration ? InputSystem.settings.defaultHoldTime : duration;
        private float pressPointOrDefault => useDefaultSettingsPressPoint ? InputSystem.settings.defaultButtonPressPoint : pressPoint;

        private InputInteractionContext ctx;

        private void OnUpdate()
        {
            var isActuated = ctx.ControlIsActuated(pressPointOrDefault);
            var phase = ctx.phase;

            if (phase == InputActionPhase.Canceled || phase == InputActionPhase.Disabled || !isActuated) { Cancel(ref ctx); return; }
            //Continue to trigger the Performed callback for as long as the button is actuated and hasn't been cancelled/disabled
            //Or if we haven't reached performed yet but our min-hold-duration has elapsed, so we can start performing.
            if (phase == InputActionPhase.Performed || (phase != InputActionPhase.Performed && ctx.timerHasExpired)) { ctx.PerformedAndStayPerformed(); }
        }

        public void Process(ref InputInteractionContext context)
        {
            if (context.phase == InputActionPhase.Waiting && context.ControlIsActuated(pressPointOrDefault))
            {
                context.Started();
                context.SetTimeout(durationOrDefault);
                InputSystem.onAfterUpdate += OnUpdate;
            }

            ctx = context; //Ensure our Update always has access to the most recently updated context
        }

        private void Cancel(ref InputInteractionContext context)
        {
            InputSystem.onAfterUpdate -= OnUpdate;
            context.Canceled();
        }

        public void Reset()
        {
            InputSystem.onAfterUpdate -= OnUpdate;
        }

        static CustomHoldingInteraction()
        {
            InputSystem.RegisterInteraction<CustomHoldingInteraction>();
        }
    }
}
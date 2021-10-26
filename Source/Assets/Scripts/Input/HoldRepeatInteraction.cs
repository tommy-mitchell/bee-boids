#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine.InputSystem;

namespace Invertex.UnityInputExtensions.Interactions
{
    /// <summary>
    /// Based on https://forum.unity.com/threads/new-input-system-how-to-use-the-hold-interaction.605587/page-4#post-7314433
    /// </summary>
#if UNITY_EDITOR
    [InitializeOnLoad]
#endif
    public class HoldRepeatInteraction : IInputInteraction
    {
        public bool useDefaultSettingsPressPoint;
        public float pressPoint;
        public bool performInstantly = true;
        public bool useDefaultSettingsFirstDuration;
        public float firstDuration;
        public bool useDefaultSettingsGeneralDuration;
        public float generalDuration;
 
        private InputInteractionContext _context;
        private double _lastPerformed;
        private bool _didWaitFirstDuration;
 
        private float FirstDurationOrDefault => useDefaultSettingsFirstDuration ? InputSystem.settings.defaultHoldTime : firstDuration;
 
        private float GeneralDurationOrDefault => useDefaultSettingsGeneralDuration ? InputSystem.settings.defaultHoldTime : generalDuration;
 
        private float PressPointOrDefault => useDefaultSettingsPressPoint ? InputSystem.settings.defaultButtonPressPoint : pressPoint;
 
        private void OnUpdate()
        {
            var isActuated = _context.ControlIsActuated(PressPointOrDefault);
            var phase = _context.phase;
 
            if (phase == InputActionPhase.Canceled || phase == InputActionPhase.Disabled || !isActuated)
            {
                Cancel(ref _context);
 
                return;
            }
 
            float now = UnityEngine.Time.time;
 
            if (phase != InputActionPhase.Performed)
            {
                if (performInstantly)
                {
                    Perform(now);
                }
                else if (now - _context.startTime >= FirstDurationOrDefault)
                {
                    Perform(now);
                    _didWaitFirstDuration = true;
                }
 
                return;
            }
 
            float duration = !_didWaitFirstDuration ? FirstDurationOrDefault : GeneralDurationOrDefault;
 
            if (now - _lastPerformed >= duration)
            {
                Perform(now);
                _didWaitFirstDuration = true;
            }
        }
 
        private void Perform(float now)
        {
            _lastPerformed = now;
            _context.PerformedAndStayPerformed();
        }
 
        public void Process(ref InputInteractionContext context)
        {
            if (context.phase == InputActionPhase.Waiting && context.ControlIsActuated(PressPointOrDefault))
            {
                context.Started();
 
                InputSystem.onAfterUpdate += OnUpdate;
            }
 
            // Ensure our Update always has access to the most recently updated context
            _context = context;
        }
 
        private void Cancel(ref InputInteractionContext context)
        {
            Reset();
 
            context.Canceled();
        }
 
        public void Reset()
        {
            _lastPerformed = 0;
            _didWaitFirstDuration = false;
 
            InputSystem.onAfterUpdate -= OnUpdate;
        }
 
    #if UNITY_EDITOR
        static HoldRepeatInteraction() => InputSystem.RegisterInteraction<HoldRepeatInteraction>();
    #else
        [UnityEngine.RuntimeInitializeOnLoadMethod(UnityEngine.RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void OnRuntimeMethodLoad() => InputSystem.RegisterInteraction<HoldRepeatInteraction>();
    #endif
    }
}
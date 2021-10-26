using UnityEngine;

public class DrawDistanceGizmo : MonoBehaviour
{
    [SerializeField]
    private BoidSettings _settings;

    private Flock _flock;

    private float SeparationRadius => _settings.SeparationDistance;

    private float LocalFlockRadius => (float) _flock?.LocalFlockDistance;

    private Color SeparationColor { get; } = Color.green;

    private Color LocalFlockColor { get; } = Color.red;

    private void Start() => _flock = GameObject.FindObjectOfType<Flock>();

    // draws two circles around the boid to show the calculated distances
    private void OnDrawGizmosSelected()
    {
        // draw separation circle
        Gizmos.color = SeparationColor;
        Gizmos.DrawWireSphere(transform.position, SeparationRadius);

        #if UNITY_EDITOR
            // use platform dependent compilation so it only exists in editor, otherwise it'll break the build
            if(UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode)
            {
                // draw local flock circle
                Gizmos.color = LocalFlockColor;
                Gizmos.DrawWireSphere(transform.position, LocalFlockRadius);
            }
        #endif
    }
}
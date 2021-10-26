using UnityEngine;
using static BoidLibrary.Constants;

public class DrawBoxGizmo : MonoBehaviour
{
    [field: SerializeField]
    private int PixelRadius { get; set; }

    private Vector2 Radius => new Vector2(PIXELS_PER_UNIT * PixelRadius, PIXELS_PER_UNIT * PixelRadius);

    private void OnDrawGizmosSelected()
    {
        // draw separation circle
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(transform.position, Radius);
    }
}
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "Follow Cursor", menuName = "Boid Demo/Boid Rules/Follow Cursor")]
public class FollowCursor : BoidRule
{
    private Camera _camera = null;
    //private Camera  Camera

    public override void ApplyRule(Boid boid, System.Collections.Generic.List<Boid> localFlock)
    {
        // get camera if null
        if(_camera == null)
            _camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();

        // get position of mouse cursor in world space
        Vector2 cursorPosition = _camera.ScreenToWorldPoint(Pointer.current.position.ReadValue());

        boid.SteerTowards(cursorPosition, Priority);
    }
}
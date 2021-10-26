using UnityEngine;
using UnityEngine.InputSystem;

public class ToggleCursorFollowFlock : Flock
{
    [SerializeField]
    private ScriptableObject _cursorFollowRule;

    private void Awake() => InputController.instance._onToggleCursorFollow += () => ToggleCursorFollow();

    private void ToggleCursorFollow()
    {
        if(_rules.Contains(_cursorFollowRule))
            _rules.Remove(_cursorFollowRule);
        else
            _rules.Add(_cursorFollowRule);
    }
}
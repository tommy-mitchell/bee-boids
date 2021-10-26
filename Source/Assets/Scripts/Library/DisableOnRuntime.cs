using UnityEngine;

[ExecuteInEditMode]
public class DisableOnRuntime : MonoBehaviour
{
    // disable component on runtime
    private void Start() => this.enabled = !Application.isPlaying;
}
using UnityEngine;

[CreateAssetMenu(fileName = "Boid Settings", menuName = "Boid Demo/Boid Settings/Boid")]
public class BoidSettings : ScriptableObject
{
    [field: SerializeField]
    public float MoveSpeed {get; private set; } = 2.0f;

    [field: SerializeField]
    public float SeparationDistance { get; private set; } = 2.5f;

    [field: SerializeField]
    public float RotationSpeed { get; private set; } = 5.0f;
}
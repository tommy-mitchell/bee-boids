using UnityEngine;

[System.Serializable]
public class Cooldown
{
    [field: SerializeField, Tooltip("Cooldown time in seconds.")]
    public float CooldownTime { get; private set; }

    [field: SerializeField, Tooltip("Optional random variation in seconds.")]
    private float RandomRange { get; set; }

    [field: SerializeField]
    public float Timer { get; set; }

    public bool IsOver => Timer <= 0.0f;

    public void Reset() => Timer = CooldownTime + BoidLibrary.GenericMethods.RandomNumber(RandomRange);
}
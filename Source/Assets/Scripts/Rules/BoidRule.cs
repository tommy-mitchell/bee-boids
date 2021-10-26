using System.Collections.Generic;
using UnityEngine;

public abstract class BoidRule : ScriptableObject
{
    [field: SerializeField]
    protected uint Priority { get; set; }

    public abstract void ApplyRule(Boid boid, List<Boid> localFlock);
}
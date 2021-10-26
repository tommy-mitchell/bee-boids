using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using BoidLibrary;

[CreateAssetMenu(fileName = "Separate", menuName = "Boid Demo/Boid Rules/Separate")]
public class Separate : BoidRule
{
    public override void ApplyRule(Boid boid, List<Boid> localFlock)
    {
        // don't apply rule if not in a local flock
        if(localFlock.Count == 0)
            return;

        if(ShouldSeparate(boid, localFlock))
            boid.SteerTowards(GetDisplacement(boid, localFlock), Priority);
    }

    private bool ShouldSeparate(Boid boid, List<Boid> localFlock)
    {
        // get center position of either the local flock, or if there's only one other boid in the flock, it and the current boid
        List<Boid> flock = localFlock.Count > 1 ? localFlock : localFlock.Concat(new Boid[] { boid }).ToList();
        Vector3 flockCenter = BoidMethods.GetAverageProperty(flock, boid => boid.Position);

        // don't separate further if already around max distance apart
        bool shouldSeparate = Vector3.Distance(boid.Position, flockCenter) < boid.Settings.SeparationDistance + GenericMethods.RandomNumber(0.1f);

        return shouldSeparate;
    }

    private Vector3 GetDisplacement(Boid boid, List<Boid> localFlock)
    {
        Vector3 displacement = Vector3.zero;

        // get total displacement
        foreach(Boid other in localFlock)
            displacement += boid.Position - other.Position;

        return displacement;
    }
}
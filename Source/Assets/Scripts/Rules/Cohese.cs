using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using BoidLibrary;

[CreateAssetMenu(fileName = "Cohese", menuName = "Boid Demo/Boid Rules/Cohese")]
public class Cohese : BoidRule
{
    public override void ApplyRule(Boid boid, List<Boid> localFlock)
    {
        // don't apply rule if not in a local flock
        if(localFlock.Count == 0)
            return;

        // if local flock only has one other boid: get position of it and current boid
        if(localFlock.Count == 1)
            boid.SteerTowards(GetAveragePosition(localFlock.Concat( new Boid[] { boid }).ToList()), Priority);
        else
            boid.SteerTowards(GetAveragePosition(localFlock), Priority);
    }

    private Vector3 GetAveragePosition(List<Boid> localFlock) => BoidMethods.GetAverageProperty(localFlock, boid => boid.Position);
}
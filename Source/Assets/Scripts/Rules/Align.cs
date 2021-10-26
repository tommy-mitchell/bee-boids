using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using BoidLibrary;

[CreateAssetMenu(fileName = "Align", menuName = "Boid Demo/Boid Rules/Align")]
public class Align : BoidRule
{
    public override void ApplyRule(Boid boid, List<Boid> localFlock)
    {
        // don't apply rule if not in a local flock
        if(localFlock.Count == 0)
            return;

        // if local flock only has one other boid: get heading of it and current boid
        if(localFlock.Count == 1)
            boid.SteerTowards(GetAverageHeading(localFlock.Concat( new Boid[] { boid }).ToList()), Priority);
        else
            boid.SteerTowards(GetAverageHeading(localFlock), Priority);
    }

    private Vector3 GetAverageHeading(List<Boid> localFlock) => BoidMethods.GetAverageProperty(localFlock, boid => boid.Heading);
}
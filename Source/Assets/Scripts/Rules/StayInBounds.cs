using UnityEngine;

[CreateAssetMenu(fileName = "Stay In Bounds", menuName = "Boid Demo/Boid Rules/Stay In Bounds")]
public class StayInBounds : BoidRule
{
    [SerializeField]
    private Vector2 startPosition = Vector2.zero;

    [SerializeField, Tooltip("Stay between positive and negative X, and positive and negative Y")]
    private Vector2 range = Vector2.zero;

    private Vector2 Boundary => startPosition + range * .97f;

    public override void ApplyRule(Boid boid, System.Collections.Generic.List<Boid> localFlock)
    {
        // if out of bounds, steer towards start position
        if(BoidLibrary.GenericMethods.IsOutOfBounds(boid.Position, Boundary))
            boid.SteerTowards(startPosition, Priority);
    }
}
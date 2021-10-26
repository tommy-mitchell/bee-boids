using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static BoidLibrary.GenericMethods;

[CreateAssetMenu(fileName = "Wander And Pollinate", menuName = "Boid Demo/Boid Rules/Wander And Pollinate")]
public class WanderAndPollinate : BoidRule
{
    [field: SerializeField]
    private float Distance { get; set; }

    private Vector2? _beehivePosition;
    private Vector2  BeehivePosition => (Vector2) ( _beehivePosition ?? ( _beehivePosition = GameObject.FindGameObjectWithTag("Beehive").transform.position ) );

    private List<FlowerController> _flowers;
    private List<FlowerController>  Flowers => _flowers ?? (_flowers = GetFlowers());

    private List<Vector2> _wanderPoints;
    private List<Vector2>  WanderPoints => _wanderPoints ?? (_wanderPoints = GetWanderPoints());

#if UNITY_EDITOR
    // serves as a proto-'Start' method for in-editor play testing
    private void OnEnable()
    {
        // use platform dependent compilation so it only exists in editor, otherwise it'll break the build
        if(UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode)
        {
            _beehivePosition = null;
            _flowers = null;
            _wanderPoints = null;
        }
    }
#endif

    public override void ApplyRule(Boid boid, List<Boid> localFlock)
    {
        // only bees can wander
        if(boid is BeeBoid)
        {
            BeeBoid beeBoid = (BeeBoid) boid;

            // bee has pollinated a flower
            if(beeBoid.HasPollinated)
                beeBoid.SteerTowards(BeehivePosition, 0);
            else if(beeBoid.WanderLocation != null) // bee is already wandering
                beeBoid.SteerTowards((Vector3) beeBoid.WanderLocation, Priority);
            else if(beeBoid.IsActive && Random.value < .9f)
            {
                var points = new List<Vector2>(WanderPoints);

                // add non-depleted flowers
                foreach(var flower in Flowers)
                    if(flower.IsNotDepleted)
                        points.Add(flower.transform.position);

                // remove all points too close to the boid
                points = points.Where(point => Vector2.Distance(beeBoid.Position, point) > 1f).ToList();

                if(points.Count == 0)
                    return;

                var closestPoints = points.Where(point => Vector2.Distance(beeBoid.Position, point) <= Distance);
                Vector2 randomPoint;

                // go to closest points 85% of the time
                if(closestPoints.Count() > 0 && Random.value < .85f)
                    randomPoint = GetRandomElement<Vector2>(closestPoints);
                else
                    randomPoint = GetRandomElement<Vector2>(points);

                // don't add if already selected by another bee
                //if(localFlock.Select(other => ((BeeBoid) other)?.WanderLocation).Where(location => location != null && (Vector2) location == randomPoint).Count() > 0)
                //    return;

                beeBoid.WanderTo(randomPoint);
                beeBoid.SteerTowards(randomPoint, Priority);
            }
        }
    }

    // lazy load flowers and wander points

    private List<FlowerController> GetFlowers()
    {
        var flowers = GameObject.FindGameObjectsWithTag("Flower");

        return flowers.Select(flower => flower.GetComponent<FlowerController>()).ToList();
    }

    private List<Vector2> GetWanderPoints()
    {
        //var flowers = GameObject.FindGameObjectsWithTag("Flower");
        var wanders = GameObject.FindGameObjectsWithTag("Wander Point");

        //return flowers.Concat(wanders).Select(point => (Vector2) point.transform.position).ToList();
        return wanders.Select(point => (Vector2) point.transform.position).ToList();
    }
}
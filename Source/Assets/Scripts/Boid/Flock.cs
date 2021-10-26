using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Flock : MonoBehaviour
{
    protected List<Boid> _boids;

    [SerializeField]
    protected List<ScriptableObject> _rules;

    [field: SerializeField]
    public float LocalFlockDistance { get; private set; } = 5.0f;

    public float BoidCount => _boids.Count;

    protected void Start()
    {
        _boids = new List<Boid>();

        // add all boids already in scene to the list
        foreach(Boid boid in gameObject.GetComponentsInChildren(typeof(Boid)))
            AddBoid(boid);
    }

    public void AddBoid(Boid boid)
    {
        if(_boids.Contains(boid))
            return;

        _boids.Add(boid);
        boid.transform.SetParent(this.transform);
    }

    public void ClearFlock()
    {
        _boids.Clear(); // remove boids from list
        BoidLibrary.GenericMethods.DestroyAllChildren(this.transform); // delete boid objects
    }

    // apply all rules
    protected void Update()
    {
        foreach(var boid in _boids)
        {
            if(boid.IsActive)
            {
                // local flock is all boids (not including self) within the specificed local distance
                List<Boid> localFlock = _boids.Where(other => Vector3.Distance(boid.Position, other.Position) <= LocalFlockDistance).
                                               Where(other => other.gameObject != boid.gameObject && other.IsActive).ToList();

                foreach(BoidRule rule in _rules)
                    rule.ApplyRule(boid, localFlock);
            }
        }
    }
}
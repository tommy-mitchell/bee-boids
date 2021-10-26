using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Boid : MonoBehaviour
{
    protected struct PointPriorityPair {
        public Vector3    Point { get; }
        public    uint Priority { get; }

        public PointPriorityPair(Vector3 point, uint priority)
        {
            Point    = point;
            Priority = priority;
        }
    }

    public Vector3 Position => transform.position;
    public Vector3  Heading => transform.up; // direction that the x-axis is facing

    [field: SerializeField]
    public BoidSettings Settings { get; private set; }

    public bool IsActive { get; protected set; } = true;

    // points to be steered towards on update
    protected List<PointPriorityPair> steerPoints;

    protected void Start() => steerPoints = new List<PointPriorityPair>();

    protected void Update()
    {
        if(steerPoints.Count > 0)
        {
            // get all points with the highest priority
            uint highestPriority = steerPoints.Min(point => point.Priority);

            // get cumulative vector of points to steer towards
            //Vector3 steerPosition = steerPoints.Aggregate((vec1, vec2) => vec1 + vec2);

            Vector3 steerPosition = Vector3.zero;

            for(int i = steerPoints.Count - 1; i >= 0; i--)
            {
                if(steerPoints[i].Priority == highestPriority)
                {
                    steerPosition += steerPoints[i].Point;
                    steerPoints.RemoveAt(i);
                }
            }

            RotateTo(steerPosition);
        }

        // move boid in current facing direction by move speed
        transform.position += Heading * Settings.MoveSpeed * Time.deltaTime;
    }

    protected void RotateTo(Vector3 point)
    {
        // ensure that no accidental z-axis rotation occurs
        point.z = Position.z;

        Vector3 vectorToTarget = point - Position;

        // rotate z-axis so that x-axis is facing forwards
        Quaternion rotation = Quaternion.LookRotation(Vector3.forward, vectorToTarget);

        // slerp by RotationSpeed if not already at the correct rotation
        if(rotation != transform.rotation)
            transform.rotation = Quaternion.Slerp(rotation, transform.rotation, Mathf.Exp(-Settings.RotationSpeed * Time.deltaTime));
    }

    public void SteerTowards(Vector3 point, uint priority) => steerPoints?.Add(new PointPriorityPair(point, priority));
}
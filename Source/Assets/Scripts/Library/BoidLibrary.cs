using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BoidLibrary
{
    public static class Constants
    {
        // 1/16
        public static float PIXELS_PER_UNIT = .0625f;
    }

    public static class BoidMethods
    {
        public static Vector3 GetAverageProperty<T>(List<T> list, System.Func<T, Vector3> selector) => GetTotalProperty(list, selector) / list.Count;

        public static Vector3 GetTotalProperty<T>(List<T> list, System.Func<T, Vector3> selector)
        {
            var positions = list.Select(selector).ToList();

            return positions.Aggregate(Vector3.zero, (acc, next) => acc + next);
        }
    }

    public static class GenericMethods
    {
        public static void DestroyAllChildren(Transform transform)
        {
            for(int i = transform.childCount - 1; i >= 0; i--)
                GameObject.DestroyImmediate(transform.GetChild(i).gameObject);
        }

        // returns a random float between [-Range, Range]
        public static float RandomNumber(float range) => Random.Range(-range, range);

        public static bool IsOutOfBounds(Vector2 position, Vector2 bounds)
        {
            // get non-negative version of the given position
            Vector2 absPosition = new Vector2(Mathf.Abs(position.x), Mathf.Abs(position.y));

            // out of bounds if absolute value of X or Y is greater than the boundary's position
            bool isOutOfBounds = absPosition.x > bounds.x || absPosition.y > bounds.y;

            return isOutOfBounds;
        }

        public static float RoundTo(float value, float multipleOf) => Mathf.Round(value / multipleOf) * multipleOf;

        public static Vector3 RoundTo(Vector3 value, float multipleOf)
        {
            float roundedX = RoundTo(value.x, multipleOf);
            float roundedY = RoundTo(value.y, multipleOf);
            float roundedZ = RoundTo(value.z, multipleOf);

            return new Vector3(roundedX, roundedY, roundedZ);
        }

        public static float GetTribool(float value) => value > 0 ? 1 : value < 0 ? -1 : 0;

        public static Vector2 GetTribool(Vector2 value)
        {
            float x = GetTribool(value.x);
            float y = GetTribool(value.y);

            return new Vector2(x, y);
        }

        public static T GetRandomElement<T>(IEnumerable<T> collection) => collection.ElementAt(Random.Range(0, collection.Count()));
    }

    /*public static class GenericTypes
    {
        [System.Serializable]
        public struct KeyValuePair<K, V> {
            [SerializeField]
            public K Key;
            [SerializeField]
            public V Value;
        }
    }*/
}
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Flower Types", menuName = "Boid Demo/Flower Types")]
public class FlowerTypes : ScriptableObject
{
    [System.Serializable]
    public struct FlowerSprites {
        [field: SerializeField]
        public Sprite FlowerBase { get; private set; }

        [field: SerializeField]
        public Sprite FlowerPollinated { get; private set; }

        [field: SerializeField]
        public Sprite FlowerDepleted { get; private set; }
    }

    [System.Serializable]
    private struct InspectorPair {
        public string Name;
        public FlowerSprites Sprites;
    }

    [SerializeField]
    private List<InspectorPair> _flowerTypes;

    public Dictionary<string, FlowerSprites> AllFlowerTypes => _flowerTypes.ToDictionary(pair => pair.Name, pair => pair.Sprites);
}
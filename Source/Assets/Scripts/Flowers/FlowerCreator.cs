using System.Linq;
using UnityEngine;

[ExecuteInEditMode]
public class FlowerCreator : MonoBehaviour
{
    [SerializeField]
    private FlowerTypes _flowers;

    // get a random key from the FlowerType dictionary
    private string RandomFlowerType => _flowers.AllFlowerTypes.Keys.ElementAt(Random.Range(0, _flowers.AllFlowerTypes.Keys.Count));

    private void Start()
    {
        // randomly set flower sprite on prefab instantiation
        GetComponent<FlowerController>().SetFlowerType(RandomFlowerType);
        // remove creator component
        DestroyImmediate(this);
    }
}
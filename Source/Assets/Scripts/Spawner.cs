using System.Collections;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField]
    private Flock _flock;

    [SerializeField]
    private GameObject _boidPrefab;

    [SerializeField, Tooltip("How far away from the spawner a Boid can spawn.")]
    private float distance = 1.0f;

    [SerializeField, Tooltip("The number of Boids that will be spawned on start.")]
    private int numberOfStartingBoids = 25;

    [SerializeField, Tooltip("The time between each spawn at start.")]
    private float delay = 0.1f;

    // get a random distance value
    private float RandomNumber => BoidLibrary.GenericMethods.RandomNumber(distance);

    // set up input listeners
    private void Awake()
    {
        InputController.instance._onSpawnBoid  += () => SpawnBoid();
        InputController.instance._onClearFlock += () => _flock.ClearFlock();
    }

    private void Start() => StartCoroutine(SpawnStartingBoids());

    private IEnumerator SpawnStartingBoids()
    {
        // spawn the given number of boids, waiting the given delay in between
        for(int i = 0; i < numberOfStartingBoids; i++)
        {
            SpawnBoid();
            yield return new WaitForSeconds(delay);
        }
    }

    private void SpawnBoid()
    {
        // get a random position and rotation for the new boid
        Vector3 randomPosition = transform.position + new Vector3(RandomNumber, RandomNumber, 0);
        Vector3 randomRotation = new Vector3(0, 0, Random.Range(0f, 360f));

        // create and number the boid
        var newBoid = Instantiate(_boidPrefab, randomPosition, Quaternion.Euler(randomRotation)).GetComponent<Boid>();
        newBoid.name = $"{_boidPrefab.name} {_flock.BoidCount + 1}";

        _flock.AddBoid(newBoid);
    }
}
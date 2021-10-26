using System.Linq;
using UnityEngine;

public class FlowerController : MonoBehaviour
{
    [field: SerializeField]
    private SpriteRenderer Renderer { get; set; }

    [field: SerializeField]
    private FlowerTypes FlowerTypes { get; set; }

    [field: SerializeField, Tooltip("The time it takes for a depleted flower to return to normal.")]
    private Cooldown DepletionCooldown { get; set; }
    
    [field: SerializeField, Tooltip("The time it takes for a regular flower to be ready for pollination.")]
    private Cooldown PollinationCooldown { get; set; }
    
    private string flowerType;
    private FlowerTypes.FlowerSprites Flowers => FlowerTypes.AllFlowerTypes[flowerType];

    public void SetFlowerType(string flowerType, bool onCreation = true)
    {
        this.flowerType = flowerType;
        Renderer.sprite = Flowers.FlowerBase;

        gameObject.name = $"{flowerType} Flower";
    }

    private void Start()
    {
        // set type to the type already set in the name
        SetFlowerType(gameObject.name.Split(' ')[0], onCreation: false);

          DepletionCooldown.Reset();
        PollinationCooldown.Reset();
    }

    private void Update()
    {
        CheckForStateSwitch();

        if(StateIsDepleted)
              DepletionCooldown.Timer -= Time.deltaTime;
        if(StateIsBase)
            PollinationCooldown.Timer -= Time.deltaTime;
    }

    private void CheckForStateSwitch()
    {
        if(StateIsDepleted && DepletionCooldown.IsOver)
        {
            // switch to regular state
            Renderer.sprite = Flowers.FlowerBase;
            DepletionCooldown.Reset();
        }
        else if(StateIsBase && PollinationCooldown.IsOver)
        {
            // switch to pollinateable state
            Renderer.sprite = Flowers.FlowerPollinated;
            PollinationCooldown.Reset();
        }
    }

    private bool StateIsBase       => Renderer.sprite == Flowers.FlowerBase;
    private bool StateIsPollinated => Renderer.sprite == Flowers.FlowerPollinated;
    private bool StateIsDepleted   => Renderer.sprite == Flowers.FlowerDepleted;

    public bool CanBePollinated => StateIsPollinated;
    public bool IsNotDepleted   => !StateIsDepleted;

    public void Pollinate() => Renderer.sprite = Flowers.FlowerDepleted;
}
using UnityEngine;

public class BeeBoid : Boid
{
    private SpriteRenderer _beeRenderer;
    private Animator       _anim;

    [field: SerializeField, Tooltip("The time (in seconds) that the bee remains in the hive.")]
    private Cooldown PollenDepositTime { get; set; }

    [field: SerializeField, Tooltip("The time (in seconds) that it takes to hover once over a point.")]
    private Cooldown HoverTime { get; set; }

    [field: SerializeField, Tooltip("The range (inclusive) for the number of hovers the bee performs at a point.")]
    private Vector2Int NumberOfHovers { get; set; } = new Vector2Int(0, 3);

    [field: SerializeField, Tooltip("The range (inclusive) for the number of pixels the bee hovers over.")]
    private Vector2Int HoverDistance { get; set; } = new Vector2Int(4, 6);

    [field: SerializeField, Tooltip("The sound to play when entering the beehive.")]
    private AudioEvent EnterSound { get; set; }

    [field: SerializeField, Tooltip("The sound to play when exiting the beehive.")]
    private AudioEvent ExitSound { get; set; }

    [field: SerializeField, Tooltip("The sound to play when pollinating a flower.")]
    private AudioEvent PollinateSound { get; set; }

    public bool      HasPollinated { get; private set; } = false;
    public Vector2? WanderLocation { get; private set; } = null;

    private const string BEE_REGULAR    = "Bee_Fly";
    private const string BEE_POLLINATED = "Bee_Fly_Pollinated";

    private string currentState;

    private new void Start()
    {
        base.Start();

        _beeRenderer = transform.GetComponentInChildren<SpriteRenderer>();
        _anim        = transform.GetComponentInChildren<Animator>();

        PollenDepositTime.Reset();
        HoverTime.Reset();

        EnterSound.Init();
         ExitSound.Init();
        PollinateSound.Init();

        // popping out of beehive
        ExitSound.Play();

        SetState(BEE_REGULAR);
    }

    private new void Update()
    {
        if(IsActive)
        {
            base.Update();

            // rotate bee to be upright while moving left
            _beeRenderer.flipY = transform.eulerAngles.z < 180;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(HasPollinated && other.gameObject.CompareTag("Beehive")) // enter beehive
            StartCoroutine(EnterHive(other.transform.position));
        else if(WanderLocation != null && other.transform.position == WanderLocation) // hover over set point
            StartCoroutine(Hover(other.gameObject));
    }
    
    private void SetState(string newState)
    {
        if(currentState == newState)
            return;

        currentState = newState;
        _anim.Play(newState);
    }

    private System.Collections.IEnumerator EnterHive(Vector2 hivePosition)
    {
        // "enter" beehive (i.e. disappear)
        EnterSound.Play();
        _beeRenderer.enabled = false;
        IsActive = false;
        transform.position = hivePosition;

        yield return new WaitForSeconds(PollenDepositTime.CooldownTime);

        // reappear after waiting
        ExitSound.Play();
        _beeRenderer.enabled = true;
        IsActive = true;
        HasPollinated = false;
        PollenDepositTime.Reset();
        SetState(BEE_REGULAR);
    }

    private float HorizontalAngle => 90f + Random.Range(-2f, 8f);
    private int Sign => _beeRenderer.flipY ? 1 : -1;
    private int RandomDistance => Random.Range(HoverDistance.x, HoverDistance.y);

    private System.Collections.IEnumerator Hover(GameObject wanderObject)
    {
        // boid is inactive while hovering
        IsActive = false;

        // move half the distance to the point
        transform.position = ( Position + wanderObject.transform.position ) / 2.0f;
        
        // stay in current position
        yield return new WaitForSecondsRealtime(HoverTime.CooldownTime / 2.0f);
        HoverTime.Reset();

        // rotate towards horizontal
        /*float lerp = Mathf.Lerp(transform.localEulerAngles.z, HorizontalAngle, .75f);
        transform.localEulerAngles = Vector3.forward * lerp * Sign;
        _beeRenderer.flipY = transform.eulerAngles.z < 180;*/

        // hover back and forth
        for(int times = 0; times < Random.Range(NumberOfHovers.x, NumberOfHovers.y); times++)
        {
            // rotate to be about horizontal
            transform.eulerAngles = Vector3.forward * HorizontalAngle * Sign;
    
            // move X pixels forward while hovering
            int pixels = RandomDistance;
            float step = 1.0f / (pixels * 2);
            for(int i = 0; i < pixels * 2; i++)
            {
                transform.position += Heading * BoidLibrary.Constants.PIXELS_PER_UNIT * (step * pixels);
                yield return new WaitForSeconds(step);
            }

            // hover
            yield return new WaitForSecondsRealtime(HoverTime.CooldownTime);
            HoverTime.Reset();

            // flip direction
            _beeRenderer.flipY = !_beeRenderer.flipY;
        }

        // pollinate if the point is a flower
        if(wanderObject.CompareTag("Flower"))
        {
            FlowerController flower = wanderObject.GetComponent<FlowerController>();

            if(flower.CanBePollinated)
            {
                flower.Pollinate();
                PollinateSound.Play();
                SetState(BEE_POLLINATED);
                HasPollinated = true;
            }
        }

        // reset state and set boid active again
        WanderLocation = null;
        HoverTime.Reset();
        IsActive = true;
    }

    public void WanderTo(Vector3 position) => WanderLocation = position;
}
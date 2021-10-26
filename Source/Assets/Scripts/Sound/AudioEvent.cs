using UnityEngine;

[CreateAssetMenu(fileName = "(AudioEvent)", menuName = "Boid Demo/Audio Event")]
public class AudioEvent : ScriptableObject
{
    [field: SerializeField]
    private AudioClip Audio { get; set; }

    [field: SerializeField, Range(0.0f, 1.0f)]
    private float Volume { get; set; } = 1.0f;

    [field: SerializeField, Range(0.0f, 2.0f)]
    private float Pitch { get; set; } = 1.0f;

    [field: SerializeField, Range(0.0f, 1.0f)]
    private float PitchRange { get; set; } = 0.0f;

    // create source on runtime to make it easier to play audio
    private AudioSource _source = null;

    public void Init()
    {
        _source = new GameObject($"{Audio.name} Source", typeof(AudioSource)).GetComponent<AudioSource>();
        _source.transform.SetParent(GameObject.Find("Audio").transform);
    }

    public void Play(AudioSource source = null)
    {
        // allow for previewing (violates open-closed principle, but good for a demo)
        if(source == null)
            source = _source;

        // play audio with volume and pitch (and optional specificed random range)
        source.clip = Audio;
        source.volume = Volume;
        source.pitch = Pitch + BoidLibrary.GenericMethods.RandomNumber(PitchRange);

        source.Play();
    }
}
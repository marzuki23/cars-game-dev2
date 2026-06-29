using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class EngineSound : MonoBehaviour
{
    public Rigidbody carRigidbody;

    private AudioSource audioSource;

    [Header("Pitch Mesin")]
    public float minPitch = 0.8f;
    public float maxPitch = 2.0f;

    [Header("Kecepatan Maksimum")]
    public float maxSpeed = 30f;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        // Ambil volume dari pengaturan menu (0-100)
        float volume = PlayerPrefs.GetFloat("MusicVolume", 50);
        audioSource.volume = volume / 100f;
    }

    private void Update()
    {
        float speed = carRigidbody.linearVelocity.magnitude;

        float t = Mathf.Clamp01(speed / maxSpeed);

        audioSource.pitch = Mathf.Lerp(minPitch, maxPitch, t);
    }
}
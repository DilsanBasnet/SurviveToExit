using System.Threading;
using UnityEngine;

public class LaserController : MonoBehaviour
{
   public Transform laserLight;
   public AudioClip laserSfx;

   public float minInterval = 1.5f;
   public float maxInterval = 3.5f;

   public float laserSpeed = 12f;
   public float maxTravelDistance = 20f;

   public float minAngle = 0f;
   public float maxAngle = 360f;
   
   private AudioSource audioSource;
   private float timer;
   private float nextInterval;
   private bool isFired = false;
   public float laserVolume = 0.5f;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if(audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        if(laserLight == null && transform.childCount > 0)
        {
            laserLight = transform.GetChild(0);
        }

        if(laserLight != null)
        {
            laserLight.gameObject.SetActive(false);
        }
        ResetTimer();
    }

    void Update()
    {
        timer += Time.deltaTime;

        if(timer >= nextInterval)
        {
        FireLaser();
            ResetTimer();
        }

        if(isFired && laserLight != null)
        {
            laserLight.Translate(Vector3.right * laserSpeed * Time.deltaTime, Space.Self);

            if(Vector3.Distance(transform.position, laserLight.position) >= maxTravelDistance)
            {
                isFired = false;
                laserLight.gameObject.SetActive(false);
            }
        }
    }

    void FireLaser()
    {
        if(laserLight != null)
        {
            laserLight.position = transform.position;
            laserLight.gameObject.SetActive(true);


            float chosenAngle = Random.Range(minAngle, maxAngle);
            laserLight.rotation = Quaternion.Euler(0f, 0f, chosenAngle);
            isFired = true;

    }

    if(laserSfx != null && audioSource != null) {
        audioSource.PlayOneShot(laserSfx, laserVolume);
    }  }

    void ResetTimer()
{
    timer = 0f;
    nextInterval = Random.Range(minInterval, maxInterval);
}
}
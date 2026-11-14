using System.Collections;
using UnityEngine;

public class PortalTeleporter : MonoBehaviour
{
    public Transform targetPortal;
    private static bool canTeleport = true;

    public AudioClip portalEnteringSfx;
    public float portalVolume = 0.5f;
    private static AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        if(audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

      if(!canTeleport) return;

      if(collision.CompareTag("Player") || collision.GetComponent<Player>() != null)
        {
            if(targetPortal != null)
            {

                if(portalEnteringSfx != null && audioSource != null)
                {
                    audioSource.PlayOneShot(portalEnteringSfx, portalVolume);

                }


                StartCoroutine(TeleportCooldown());

                collision.transform.position = targetPortal.position;

            }
        }  
    }

    private IEnumerator TeleportCooldown()
    {
        canTeleport = false;

        yield return new WaitForSeconds(0.3f);
        canTeleport = true;
    }
}

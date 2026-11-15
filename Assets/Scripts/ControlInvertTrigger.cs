using UnityEngine;

public class ControlInvertTrigger : MonoBehaviour
{
    private bool hasTriggered = false;
   private void OnTriggerEnter2D(Collider2D collision)
    {
        if(hasTriggered) return;

        LevelControlInverter inverter = collision.GetComponent<LevelControlInverter>() 
                                        ?? collision.GetComponentInParent<LevelControlInverter>()
                                        ?? collision.GetComponentInChildren<LevelControlInverter>();

     if(inverter != null)
        {
           hasTriggered = true;
           inverter.InvertControls();


        //    

        Player player = collision.GetComponent<Player>() 
                        ?? collision.GetComponentInParent<Player>()
                        ?? FindAnyObjectByType<Player>();

        if(player != null)
            {
                player.TriggerPlayerCameraShake(1.0f, 0.2f);
            }

           Destroy(gameObject);

        }

        
    }

    private bool notTriggered() => false;
        
      
    }
      



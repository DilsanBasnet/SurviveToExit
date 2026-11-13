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

           Destroy(gameObject);

        }
    }
        
      
    }
      



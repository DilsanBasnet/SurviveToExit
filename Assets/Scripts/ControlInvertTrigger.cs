using UnityEngine;

public class ControlInvertTrigger : MonoBehaviour
{
   public bool triggerOnlyOnce = true;
   private bool hasTriggered = false;

   private void OnTriggerEnter2D(Collider2D collision)
    {
        if(hasTriggered && triggerOnlyOnce) return;

        LevelControlInverter inverter = collision.GetComponent<LevelControlInverter>();

        if(inverter == null)
        {
            inverter = collision.GetComponentInParent<LevelControlInverter>();  
        }

        if(inverter != null)
        {
            inverter.InverControls();
            hasTriggered = true;

            if(triggerOnlyOnce)
            {
                gameObject.SetActive(false);
            }
        }
    }

}

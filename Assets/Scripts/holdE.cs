using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.InputSystem;

public class holdE : MonoBehaviour
{
    public Transform player;
    public Image fillImage;
    public GameObject visualRoot;
    public GameObject gameObjectToActivate;

    public float interactDistance = 2.5f;
    public float requiredHoldTime = 2.0f;
    private float currentHoldTimer = 0f;
    private bool isActivated = false;
    public Animator animator;

    void Start()
    {
       if(gameObjectToActivate != null)
        {
            gameObjectToActivate.SetActive(false);
        }
        currentHoldTimer = 0f;

        if(fillImage != null)
        {
            fillImage.fillAmount = 0f;
        }

        if(visualRoot != null)
        {
            visualRoot.SetActive(true);
        }
    }

    void Update()
    {
        if(isActivated || player == null)
        {
            return;
        }
        
        float distance = Vector2.Distance(transform.position, player.position);
        bool isHoldingE = Keyboard.current != null && Keyboard.current.eKey.isPressed;
        
        if(distance <= interactDistance)
        {
             if(isHoldingE)
            {
                currentHoldTimer += Time.deltaTime;

                if(fillImage != null)
                {
                    fillImage.fillAmount = Mathf.Clamp01(currentHoldTimer / requiredHoldTime);
                }

                if(currentHoldTimer >= requiredHoldTime)
                {
                    ActivateObject();
                }
            }
            else
            {
                currentHoldTimer = 0f;
                if(fillImage != null)
                {
                    fillImage.fillAmount = 0f;
                }
            }
        }
        else
        {
            currentHoldTimer = 0f;
            if(fillImage != null)
            {
                fillImage.fillAmount = 0f;
            }
        }

        
    }

    private void ActivateObject()
    {
        isActivated = true;

        if(gameObjectToActivate != null)
        {
            gameObjectToActivate.SetActive(true);
        }

        if(visualRoot != null)
        {
            visualRoot.SetActive(false);
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactDistance);
    }
}

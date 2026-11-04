using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int rocketCoins  = 0;
    public int PlayerCoins = 0;


    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } 
        else
        {
            Destroy(gameObject);
        }
    }

public void AddRocketCoins(int amount)
    {
        rocketCoins += amount;

    }

    public void AddPlayerCoins(int amount)
    {
        PlayerCoins += amount;
        
    }

    }


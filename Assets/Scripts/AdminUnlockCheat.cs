using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class AdminUnlockCheat : MonoBehaviour
{
   // Admin cheat script bro, For only reviewers & developer me only 😆

   public string cheatCode = "dilsan";
   private string currentInput = "";

   public int totalLevels = 25;
   public LevelSelectManager levelAdminUnlock;

    void Update()
    {
        if(Keyboard.current == null) return;

        foreach (var key in Keyboard.current.allKeys)
        {
            if(key.wasPressedThisFrame)
            {
                char letter = GetCharFromKey(key);

                if (letter != '\0')
                {
                    currentInput += letter;

                    if(currentInput.Length > cheatCode.Length)
                    {
                        currentInput = currentInput.Substring(currentInput.Length - cheatCode.Length);
                    }

                    if(currentInput.ToLower() == cheatCode.ToLower())
                    {
                        UnlockAllLevels();
                        currentInput = "" ;
                    }
                }
            }
        }
    }
    private char GetCharFromKey(KeyControl key)
    {
        string keyName = key.name;

        if(keyName.Length == 1 && char.IsLetter(keyName[0]))
        {
            return keyName[0];
        }

        return '\0';
        
    }

    private void UnlockAllLevels()
    {
        Debug.Log("ADMIN CHEAT ACTIVATED: only for Developer & Reviewers... 😆");
        

        PlayerPrefs.SetInt("LevelUnlocked", totalLevels);
        PlayerPrefs.Save();

        levelAdminUnlock.UpdateLevelStates();
    }
}

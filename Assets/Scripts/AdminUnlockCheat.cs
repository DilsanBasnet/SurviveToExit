using UnityEngine;
using UnityEngine.InputSystem;

public class AdminUnlockCheat : MonoBehaviour
{
   // Admin cheat bro, For only reviewers & developer me 😆

   public string cheatCode = "dilsan";
   private string currentInput = "";

   public int totalLevels = 16;

    void Update()
    {
        if(Keyboard.current == null) return;
    }


}

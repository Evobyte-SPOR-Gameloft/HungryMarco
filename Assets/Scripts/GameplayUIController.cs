using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameplayUIController : MonoBehaviour
{
    public void RestartGame()
    {
        SceneManager.LoadScene("Gameplay");
    }

    public void GoBack()
    {
        SceneManager.LoadScene("PlayerSelect");
    }
}//claSS

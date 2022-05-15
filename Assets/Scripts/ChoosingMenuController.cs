using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChoosingMenuController : MonoBehaviour
{
    public void PlayTheGame()
    {
        int chosenPlayer = int.Parse(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name);

        GameManager.instance.CharIndex = chosenPlayer;

        SceneManager.LoadScene("Gameplay");
    }

    public void GoBack()
    {
        SceneManager.LoadScene("MainMenu");
    }

}//class

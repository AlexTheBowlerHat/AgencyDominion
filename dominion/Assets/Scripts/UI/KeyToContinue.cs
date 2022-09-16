using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class KeyToContinue : MonoBehaviour
{
    int currentSceneIndex;
    Scene nextScene;
    private void Start()
    {
       currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
       nextScene = SceneManager.GetSceneByBuildIndex(1);
    }

    //Checks if any button has been pressed each frame, not costly as only used for a disclaimer scene seen briefly
    //Then loads the start menu when input detected
    void Update()
    {
        if (Keyboard.current.anyKey.isPressed || Mouse.current.leftButton.isPressed || Mouse.current.middleButton.isPressed || Mouse.current.rightButton.isPressed && currentSceneIndex == 0)
        {
            SceneManager.LoadSceneAsync("StartMenu", LoadSceneMode.Single);
            SetNextScene();
        }
    }
    //Unloads disclaimer
    private static IEnumerator SetNextScene()
    {
        while (!SceneManager.GetSceneByName("StartMenu").isLoaded)
        {
            yield return new WaitForSeconds(0.1f);
        }
        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(1));
        SceneManager.UnloadSceneAsync("Disclaimer");
    }

}

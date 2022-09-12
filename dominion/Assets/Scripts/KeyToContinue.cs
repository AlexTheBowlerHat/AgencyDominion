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
    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.anyKey.isPressed || Mouse.current.leftButton.isPressed || Mouse.current.middleButton.isPressed || Mouse.current.rightButton.isPressed && currentSceneIndex == 0)
        {
            SceneManager.LoadSceneAsync("StartMenu", LoadSceneMode.Single);
            SetNextScene();
        }
    }
    
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

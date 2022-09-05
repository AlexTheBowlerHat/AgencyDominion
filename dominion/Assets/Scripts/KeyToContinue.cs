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
       nextScene = SceneManager.GetSceneByName("TestLevel");
    }
    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.anyKey.isPressed || Mouse.current.leftButton.isPressed || Mouse.current.middleButton.isPressed || Mouse.current.rightButton.isPressed && currentSceneIndex == 0)
        {
            SceneManager.LoadSceneAsync(nextScene, LoadSceneMode.Single);
            
        }
    }
    private static IEnumerator LoadNextScene()
    {
        while (!nextScene.isLoaded)
        {
            yield return new WaitForSeconds(0.1f);
        }
        SceneManager.SetActiveScene(nextScene);
        SceneManager.UnloadSceneAsync("Disclaimer");
    }

}

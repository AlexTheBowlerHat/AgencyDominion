using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class BackToMain : MonoBehaviour
{
    Button exitButton;
    // Start is called before the first frame update
    void Start()
    {
        var rootElement = GetComponent<UIDocument>().rootVisualElement;
        //Makes the exit button under the UI document switch to the start menu
        exitButton = rootElement.Q<Button>("Exit");
        exitButton.RegisterCallback<ClickEvent>(ev => SwitchScene("StartMenu"));
    }
    //For switching to the desired scene
    void SwitchScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}

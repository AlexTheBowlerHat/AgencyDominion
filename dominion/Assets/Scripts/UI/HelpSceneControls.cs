using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class HelpSceneControls : MonoBehaviour
{
    Button exitButton;
    // Start is called before the first frame update
    void Start()
    {
        var rootElement = GetComponent<UIDocument>().rootVisualElement;
        exitButton = rootElement.Q<Button>("Exit");
        exitButton.RegisterCallback<ClickEvent>(ev => SwitchScene("StartMenu"));
    }
    void SwitchScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}

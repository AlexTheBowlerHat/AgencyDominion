using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class StartMenuControls : MonoBehaviour
{
    Button playButton;
    Button helpButton;
    Button optionsButton;
    Button creditsButton;
    Button quitButton;
    // Start is called before the first frame update
    void Start()
    {
        var rootElement = GetComponent<UIDocument>().rootVisualElement;

        playButton = rootElement.Q<Button>("PlayButton");
        helpButton = rootElement.Q<Button>("HelpButton");
        optionsButton = rootElement.Q<Button>("OptionsButton");
        creditsButton = rootElement.Q<Button>("CreditsButton");
        quitButton = rootElement.Q<Button>("QuitButton");

        playButton.RegisterCallback<ClickEvent>(ev => SwitchScene("TestLevel"));
        helpButton.RegisterCallback<ClickEvent>(ev => SwitchScene("HelpScene"));
        optionsButton.RegisterCallback<ClickEvent>(ev => SwitchScene("OptionsScene"));
        creditsButton.RegisterCallback<ClickEvent>(ev => SwitchScene("CreditsScene"));
        quitButton.RegisterCallback<ClickEvent> (ev => QuitButtonPressed());
    }

    public void SwitchScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    void QuitButtonPressed()
    {
        Application.Quit();
        Debug.Log("Quit");
    }
}

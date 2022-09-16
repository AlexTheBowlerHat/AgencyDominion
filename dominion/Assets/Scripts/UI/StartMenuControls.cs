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
    bool allowClick = false;
    // Start is called before the first frame update
    void Start()
    {
        //References to all the buttons
        var rootElement = GetComponent<UIDocument>().rootVisualElement;

        playButton = rootElement.Q<Button>("PlayButton");
        helpButton = rootElement.Q<Button>("HelpButton");
        optionsButton = rootElement.Q<Button>("OptionsButton");
        creditsButton = rootElement.Q<Button>("CreditsButton");
        quitButton = rootElement.Q<Button>("QuitButton");

        //Binds buttons to methods relevent to their function
        playButton.RegisterCallback<ClickEvent>(ev => SwitchScene("TestLevel"));
        helpButton.RegisterCallback<ClickEvent>(ev => SwitchScene("HelpScene"));
        optionsButton.RegisterCallback<ClickEvent>(ev => SwitchScene("OptionsScene"));
        creditsButton.RegisterCallback<ClickEvent>(ev => SwitchScene("CreditsScene"));
        quitButton.RegisterCallback<ClickEvent> (ev => QuitButtonPressed());
        
        //Here to stop users accidentally clicking buttons after switching scenes
        StartCoroutine(Waiter());
    }

    IEnumerator Waiter()
    {
        yield return new WaitForSeconds(0.2f);
        allowClick = true;
    }

    public void SwitchScene(string sceneName)
    {
        if(allowClick)
        {
            SceneManager.LoadScene(sceneName);
        }
    }

    void QuitButtonPressed()
    {
        if(allowClick)
        {
        Application.Quit();
        //Debug.Log("Quit");
        }
        
    }
}

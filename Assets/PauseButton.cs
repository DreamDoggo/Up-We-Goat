using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseButton : MonoBehaviour
{
    [SerializeField] GameObject ExitButton;
    [SerializeField] GameObject PauseBackground;
    ButtonManager buttonManager;

    public static bool GameIsPaused = false;

    private void Awake()
    {
        buttonManager = FindObjectOfType<ButtonManager>();
    }

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnClicked);
        GetComponent<Button>().onClick.AddListener(PauseGame); 
        ExitButton.SetActive(false);
        PauseBackground.SetActive(false);
    }

    public void OnClicked() 
    {
        if (buttonManager != null) 
        {
            buttonManager.PlayButtonSoundEffect(buttonManager.ButtonClickSound, buttonManager.ButtonClickSoundDelay);
        }
    }

    public void PauseGame() 
    {
        if (GameIsPaused) 
        {
            UnpauseGame();
        }
        else 
        {
            Time.timeScale = 0.0f;
            GameIsPaused = true;
            ExitButton.SetActive(true);
            PauseBackground?.SetActive(true);
        }
    }

    public void UnpauseGame() 
    {
        Time.timeScale = 1f;
        GameIsPaused = false;
        ExitButton.SetActive(false);
        PauseBackground?.SetActive(false);
    }
}

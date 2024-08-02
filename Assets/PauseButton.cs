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
        GetComponent<Button>().onClick.AddListener(OnClicked);
        GetComponent<Button>().onClick.AddListener(PauseGame); 
        MusicManager.ComingFromPause = false;
    }

    private void Start()
    {
        ExitButton.SetActive(false);
        PauseBackground.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) 
        {
            OnClicked();
            if (GameIsPaused) 
            {
                UnpauseGame();
            }
            else 
            {
                PauseGame();
            }
        }
    }

    public void OnClicked() 
    {
        if (buttonManager != null) 
        {
            buttonManager.PlayButtonSoundEffect(buttonManager.ButtonClickSound, buttonManager.ButtonClickSoundDelay);
        }
        MusicManager.ComingFromPause = !MusicManager.ComingFromPause;
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

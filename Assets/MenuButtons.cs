using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuButtons : MonoBehaviour
{
    [SerializeField] Text scoreText;
    [SerializeField] Text textForDeathScreen;
    [SerializeField] Text congratsText;
    public void changeScene(string scene){
        PauseButton.GameIsPaused = false;
        SceneManager.LoadScene(scene);
    }
    public void exit(){
        Application.Quit(); 
    }
    public void debugResetHighscore(){
        PlayerPrefs.SetInt("highscore", 0);
    }
    void Start()
    {
        if (SceneManager.GetActiveScene().name == "GameOver" || SceneManager.GetActiveScene().name == "MainMenu") 
        {
            textForDeathScreen.text = PlayerPrefs.GetInt("score").ToString();
            if (PlayerPrefs.GetInt("score") >= PlayerPrefs.GetInt("highscore"))
            {
                congratsText.text = "Highscore achieved!";
            }

            if (PlayerPrefs.GetInt("highscore") > 0)
            {
                string bleh = PlayerPrefs.GetInt("highscore").ToString();
                Debug.Log(bleh);
                scoreText.text = bleh;
            }

        }
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour
{
    public void changeScene(string scene){
        PauseButton.GameIsPaused = false;
        SceneManager.LoadScene(scene);
    }
    public void exit(){
        Application.Quit(); 
    }
}

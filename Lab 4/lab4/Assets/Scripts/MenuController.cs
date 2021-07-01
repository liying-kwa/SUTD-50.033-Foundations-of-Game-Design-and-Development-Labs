using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour {

    void Awake() {
        Time.timeScale = 0.0f;
    }

    public void StartButtonClicked() {
        foreach (Transform eachChild in transform) {
            if (eachChild.name != "Score" && eachChild.name != "EventSystem" && eachChild.name != "Powerups") {
                //Debug.Log("Child found. Name: " + eachChild.name);
                // disable them
                eachChild.gameObject.SetActive(false);
                Time.timeScale = 1.0f;
            }
        }
    }

    public void RestartButtonClicked() {
        Debug.Log("Restart button clicked!");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}

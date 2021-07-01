using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public GameObject spawnManagerObject;
	private SpawnManager spawnManager;
    public  Text score;
	private  int playerScore =  0;

    // Game Over
    GameObject[] gameoverObjects;

    private  int _healthPoints; 

    //healthPoints is a basic property  
    public  int healthPoints { 
        get { 
            //Some other code  
            // ...
            return _healthPoints; 
        } 
        set { 
            // Some other code, check etc
            // ...
            _healthPoints = value; // value is the amount passed by the setter
        } 
    }

    override  public  void  Awake(){
        base.Awake();
    }

    void Start()
    {
        spawnManager = spawnManagerObject.GetComponent<SpawnManager>();

        gameoverObjects = GameObject.FindGameObjectsWithTag("ShowOnGameover");
        foreach(GameObject g in gameoverObjects){
			g.SetActive(false);
		}

        // subscribe to player event
        GameManager.OnPlayerDeath  +=  GameoverSequence;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        gameoverObjects = GameObject.FindGameObjectsWithTag("ShowOnGameover");
        foreach(GameObject g in gameoverObjects){
			g.SetActive(false);
		}
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void increaseScore(){
		playerScore  +=  1;
		score.text  =  "SCORE: "  +  playerScore.ToString();
        spawnManager.spawnFromPooler(ObjectType.gombaEnemy);
	}

    public void damagePlayer(){
        OnPlayerDeath();
    }

    public delegate void gameEvent();

    public  static  event  gameEvent OnPlayerDeath;

    void  GameoverSequence(){
        // Mario dies
        Debug.Log("Game Over");
        // do whatever you want here, animate etc
        // ...
        showGameover();
    }
    void showGameover() {
        Time.timeScale = 0.0f;
        foreach(GameObject g in gameoverObjects) {
			g.SetActive(true);
		}
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CentralManager : MonoBehaviour
{
	public static CentralManager centralManagerInstance;
    public GameObject gameManagerObject;
	private GameManager gameManager;
    public  GameObject powerupManagerObject;
    private  PowerUpManager powerUpManager;
	
	void  Awake() {
		centralManagerInstance = this;
	}

    // Start is called before the first frame update
    void Start()
    {
        gameManager = gameManagerObject.GetComponent<GameManager>();
        powerUpManager  =  powerupManagerObject.GetComponent<PowerUpManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void increaseScore(){
		gameManager.increaseScore();
	}

    public void damagePlayer(){
        gameManager.damagePlayer();
    }

    public  void  consumePowerup(KeyCode k, GameObject g){
        powerUpManager.consumePowerup(k,g);
    }

    public  void  addPowerup(Texture t, int i, ConsumableInterface c){
        powerUpManager.addPowerup(t, i, c);
    }

}

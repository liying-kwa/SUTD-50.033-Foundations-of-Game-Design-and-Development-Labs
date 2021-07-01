using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyControllerEV : MonoBehaviour
{
    public GameConstantsB gameConstants;
	private  Rigidbody2D enemyBody;
    private  Vector2 velocity;
	private SpriteRenderer enemySprite;
    private  int moveRight;
	private  float originalX;

    // events to subscribe
    public UnityEvent onPlayerDeath;
    public UnityEvent onEnemyDeath;

    // callbacks must be PUBLIC
    public void PlayerDeathResponse()
    {
        GetComponent<Animator>().SetBool("playerIsDead", true);
        velocity = Vector3.zero;
    }

    // Start is called before the first frame update
    void Start()
    {
        enemyBody  =  GetComponent<Rigidbody2D>();
		enemySprite = GetComponent<SpriteRenderer>();
        moveRight  =  Random.Range(0, 2) ==  0  ?  -1  :  1;
        ComputeVelocity();
    }

	void OnEnable()
	{
		originalX  =  transform.position.x;
	}

    // Update is called once per frame
    void Update()
    {
        if (Mathf.Abs(enemyBody.position.x  -  originalX) <  gameConstants.maxOffset) {
            // move gomba
			MoveEnemy();
		}
		else {
			// change direction
			moveRight  *=  -1;
			ComputeVelocity();
			MoveEnemy();
		}
		if (moveRight > 0) {
			transform.localScale = new Vector3(1, 1, 1);
		} else {
			transform.localScale = new Vector3(-1, 1, 1);
		}
		
    }

    void  OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.tag == "Player") {
			// check if collides on top
			float yoffset = (other.transform.position.y  -  this.transform.position.y);
			if (yoffset  >  0.75f){
				KillSelf();
                onEnemyDeath.Invoke();
			}
			else {
				// hurt player, implement later
                onPlayerDeath.Invoke();
			}
		}
	}

    void ComputeVelocity(){
        velocity = new Vector2((moveRight) * gameConstants.maxOffset  /  gameConstants.enemyPatroltime, 0);
    }
    void  MoveEnemy() {
        enemyBody.MovePosition(enemyBody.position + velocity * Time.fixedDeltaTime);
    }

     void  KillSelf() {
		// enemy dies
		StartCoroutine(flatten());
		//Debug.Log("Kill sequence ends");
	}
    
    IEnumerator flatten() {
		//Debug.Log("Flatten starts");
		int steps =  5;
		float stepper =  1.0f/(float) steps;

		for (int i =  0; i  <  steps; i  ++){
			this.transform.localScale  =  new  Vector3(this.transform.localScale.x, this.transform.localScale.y  -  stepper, this.transform.localScale.z);

			// make sure enemy is still above ground
			this.transform.position  =  new  Vector3(this.transform.position.x, gameConstants.groundSurface  +  GetComponent<SpriteRenderer>().bounds.extents.y, this.transform.position.z);
			yield  return  null;
		}
		//Debug.Log("Flatten ends");
		this.gameObject.SetActive(false);
        //this.transform.localScale = new Vector3(1, 1, 1);
		//Debug.Log("Enemy returned to pool");
		yield break;
	}

    
}

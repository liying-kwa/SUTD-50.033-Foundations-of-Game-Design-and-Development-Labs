using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedMushroom : MonoBehaviour, ConsumableInterface
{
    public  GameConstants gameConstants;
    private float speed = 4;
    private int currentDirection;
    private Rigidbody2D mushroomBody;

    public Texture t;
	public void consumedBy(GameObject player) {
		// give player jump boost
		player.GetComponent<PlayerController>().upSpeed += 10;
		StartCoroutine(removeEffect(player));
	}

	IEnumerator removeEffect(GameObject player) {
		yield  return  new  WaitForSeconds(5.0f);
		player.GetComponent<PlayerController>().upSpeed -= 10;
        Destroy(gameObject);
	}

    // Start is called before the first frame update
    void Start()
    {
        mushroomBody = GetComponent<Rigidbody2D>();
        // random left or right
        int randbinary = UnityEngine.Random.Range(0, 2);
        if (randbinary == 0) {
            currentDirection = 1;
        } else {
            currentDirection = -1;
        }
        mushroomBody.AddForce(new Vector2(0, 0.15f), ForceMode2D.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        mushroomBody.velocity = new Vector2(currentDirection * speed, mushroomBody.velocity.y);
    }

    void OnCollisionEnter2D(Collision2D col) {
        if (col.gameObject.CompareTag("Obstacle")) {
            currentDirection *= -1;
        };
        if (col.gameObject.CompareTag("Player")){
            // update UI
            CentralManager.centralManagerInstance.addPowerup(t, 1, this);
            GetComponent<Collider2D>().enabled  =  false;
            StartCoroutine(enlargeAndShrink(gameObject));
        }
    }

    // void OnBecameInvisible(){
    //     Destroy(gameObject);
    // }

    IEnumerator enlargeAndShrink(GameObject consumable) {
        int addSteps = 1;
        float addStepper = 0.1f;
        int shrinkSteps = 2;
		float shrinkStepper =  1.0f/(float) shrinkSteps;

		for (int i =  0; i  <  addSteps; i  ++){
			consumable.transform.localScale  =  new  Vector3(this.transform.localScale.x + addStepper, this.transform.localScale.y  +  addStepper, this.transform.localScale.z);

			// make sure enemy is still above ground
			consumable.transform.position  =  new  Vector3(this.transform.position.x, gameConstants.groundSurface  +  GetComponent<SpriteRenderer>().bounds.extents.y, this.transform.position.z);
			yield  return  null;
		}

        for (int i =  0; i  <  shrinkSteps; i  ++){
			consumable.transform.localScale  =  new  Vector3(this.transform.localScale.x  -  shrinkStepper, this.transform.localScale.y  -  shrinkStepper, this.transform.localScale.z);

			// make sure enemy is still above ground
			consumable.transform.position  =  new  Vector3(this.transform.position.x, gameConstants.groundSurface  +  GetComponent<SpriteRenderer>().bounds.extents.y, this.transform.position.z);
			yield  return  null;
		}
    }
}

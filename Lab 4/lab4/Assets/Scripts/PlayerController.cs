using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {
    
    private bool moving;
    public float speed = 130;
    public float maxSpeed = 10;
    public float upSpeed = 50;
    private Rigidbody2D marioBody;
    private SpriteRenderer marioSprite;
    private bool faceRightState = true;
    // Scoring System
    public Transform enemyLocation;
    // public Text scoreText;
    // private int score = 0;
    private bool onGroundState = true;
    // private bool countScoreState = false;
    // Animation
    private  Animator marioAnimator;
    private AudioSource marioAudio;
    // DustCloud Animation
    public ParticleSystem dustCloud;


    // Start is called before the first frame update
    void Start() {
        // Set to be 30 FPS
        Application.targetFrameRate =  30;
        marioBody = GetComponent<Rigidbody2D>();
        marioSprite = GetComponent<SpriteRenderer>();
        marioAnimator  =  GetComponent<Animator>();
        marioAudio = GetComponent<AudioSource>();

        // subscribe to player event
        GameManager.OnPlayerDeath  +=  PlayerDiesSequence;
    }

    void FixedUpdate() {
        // dynamic rigidbody
        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        if (Mathf.Abs(moveHorizontal) > 0) {
            Vector2 movement = new Vector2(moveHorizontal, 0);
            if (marioBody.velocity.magnitude < maxSpeed)
                marioBody.AddForce(movement * speed);
        } else {
            // stop horizontal movement
            marioBody.velocity = new Vector2(0, marioBody.velocity.y);
        }

        // toggle state (flip)
        if (Input.GetKeyDown("a") && faceRightState){
            faceRightState = false;
            marioSprite.flipX = true;
            // Animation
            if (Mathf.Abs(marioBody.velocity.x) >  1.0) 
	            marioAnimator.SetTrigger("onSkid");
        }
        if (Input.GetKeyDown("d") && !faceRightState) {
            faceRightState = true;
            marioSprite.flipX = false;
            // Animation
            if (Mathf.Abs(marioBody.velocity.x) >  1.0) 
	            marioAnimator.SetTrigger("onSkid");
        }

        // Jump
        if (Input.GetKeyDown("space") && onGroundState) {
            marioBody.AddForce(Vector2.up * upSpeed, ForceMode2D.Impulse);
            onGroundState = false;
            // countScoreState = true; //check if Gomba is underneath
        }

        // // Count score
        // if (!onGroundState && countScoreState) {
        //     if (Mathf.Abs(transform.position.x - enemyLocation.position.x) < 0.5f) {
        //         countScoreState = false;
        //         score++;
        //         Debug.Log(score);
        //     }
        // }

        // Animation
        marioAnimator.SetFloat("xSpeed", Mathf.Abs(marioBody.velocity.x));
        marioAnimator.SetBool("onGround", onGroundState);

        // Consumable mushrooms
        if (Input.GetKeyDown("z")){
            CentralManager.centralManagerInstance.consumePowerup(KeyCode.Z,this.gameObject);
        }
        if (Input.GetKeyDown("x")){
            CentralManager.centralManagerInstance.consumePowerup(KeyCode.X,this.gameObject);
        }

    }

    // called when the cube hits the floor
    void OnCollisionEnter2D(Collision2D col) {
        if (col.gameObject.CompareTag("Ground") || col.gameObject.CompareTag("Obstacle")) {
            dustCloud.Play();
            onGroundState = true; // back on ground
            // countScoreState = false; // reset score state
            // scoreText.text = "Score: " + score.ToString();
        };
    }

    void OnTriggerEnter2D(Collider2D other) {
        // if (other.gameObject.CompareTag("Enemy")) {
        //     Debug.Log("Collided with Gomba!");
        //     showGameover();
        // }
    }

    

    void PlayJumpSound() {
	    marioAudio.PlayOneShot(marioAudio.clip);
    }

    void  PlayerDiesSequence(){
        // Mario dies
        Debug.Log("Mario dies");
        // do whatever you want here, animate etc
        // ...
    }

    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerEV : MonoBehaviour
{
    private float force;
    public IntVariable marioUpSpeed;
    public IntVariable marioMaxSpeed;
    public GameConstantsB gameConstants;
	  
	// other components and interal state
    private Rigidbody2D marioBody;
    private SpriteRenderer marioSprite;
    private  Animator marioAnimator;
    private AudioSource marioAudio;
    public ParticleSystem dustCloud;
    private bool isDead = false;
    private bool isADKeyUp = true;
    private bool faceRightState = true;
    private bool isSpacebarUp = true;
    private bool onGroundState = true;
    //private bool countScoreState = false;
    // cast powerups
    public CustomCastEvent onPlayerCastPowerup;


    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate =  30;
        marioUpSpeed.SetValue(gameConstants.playerMaxJumpSpeed);
        marioMaxSpeed.SetValue(gameConstants.playerMaxSpeed);
        force = gameConstants.playerDefaultForce;

        marioBody = GetComponent<Rigidbody2D>();
        marioSprite = GetComponent<SpriteRenderer>();
        marioAnimator  =  GetComponent<Animator>();
        marioAudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) > 0) {
            isADKeyUp = false;
        } else {
            isADKeyUp = true;
        }

        if (Input.GetKeyDown("a")) {
            faceRightState = false;
            if (Mathf.Abs(marioBody.velocity.x) >  1.0) 
	            marioAnimator.SetTrigger("onSkid");
        } else if (Input.GetKeyDown("d")) {
            faceRightState = true;
            if (Mathf.Abs(marioBody.velocity.x) >  1.0) 
	            marioAnimator.SetTrigger("onSkid");
        }

        if (Input.GetKeyDown("space")) {
            isSpacebarUp = false;
        } else {
            isSpacebarUp = true;
        }

        if (!isDead)
        {
            //check if a or d is pressed currently
            if (!isADKeyUp)
            {
                marioSprite.flipX = faceRightState ? false : true;
                float direction = faceRightState ? 1.0f : -1.0f;
                Vector2 movement = new Vector2(force * direction, 0);
                if (marioBody.velocity.magnitude < marioMaxSpeed.Value)
                    marioBody.AddForce(movement);
            }

            if (!isSpacebarUp && onGroundState)
            {
                marioBody.AddForce(Vector2.up * marioUpSpeed.Value, ForceMode2D.Impulse);
                onGroundState = false;
                // part 2
                marioAnimator.SetBool("onGround", onGroundState);
                //countScoreState = true; //check if Gomba is underneath
            }

            // Animation
            marioAnimator.SetFloat("xSpeed", Mathf.Abs(marioBody.velocity.x));
            marioAnimator.SetBool("onGround", onGroundState);
        }
        // cast powerups
        if (Input.GetKeyDown("z")){
            onPlayerCastPowerup.Invoke(KeyCode.Z);
        } else if (Input.GetKeyDown("x")) {
            onPlayerCastPowerup.Invoke(KeyCode.X);
        }
    }

    void OnCollisionEnter2D(Collision2D col) {
        if (col.gameObject.CompareTag("Ground") || col.gameObject.CompareTag("Obstacle")) {
            dustCloud.Play();
            onGroundState = true; // back on ground
            //countScoreState = false;
        };
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Enemy")) {
            Debug.Log("Collided with Gomba!");
            //isDead = true;;
        }
    }

    void PlayJumpSound() {
	    marioAudio.PlayOneShot(marioAudio.clip);
    }

    public void PlayerDiesSequence()
    {
        isDead = true;
        marioAnimator.SetBool("isDead", true);
        GetComponent<Collider2D>().enabled = false;
        marioBody.AddForce(Vector3.up * 30, ForceMode2D.Impulse);
        marioBody.gravityScale = 5;
        StartCoroutine(dead());
    }
    
    IEnumerator dead()
    {
        yield return new WaitForSeconds(3.0f);
        marioBody.bodyType = RigidbodyType2D.Static;
    }
}

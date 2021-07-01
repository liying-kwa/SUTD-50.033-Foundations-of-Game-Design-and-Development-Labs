using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionBoxController : MonoBehaviour
{

    public  Rigidbody2D rigidBody;
    public  SpringJoint2D springJoint;
    public  GameObject greenmushPrefab; // the spawned mushroom prefab
    public GameObject redmushPrefab;
    public  SpriteRenderer spriteRenderer;
    public  Sprite usedQuestionBox; // the sprite that indicates empty box instead of a question mark
    private bool hit =  false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void  OnCollisionEnter2D(Collision2D col)
    {
        GameObject consumablePrefab = Random.Range(0, 2) ==  0  ?  greenmushPrefab  :  redmushPrefab;
        if (col.gameObject.CompareTag("Player") &&  !hit) {
            hit  =  true;
            rigidBody.AddForce(new  Vector2(0, rigidBody.mass*20), ForceMode2D.Impulse);
            // spawn the mushroom prefab slightly above the box
            Instantiate(consumablePrefab, new  Vector3(this.transform.position.x, this.transform.position.y  +  1.0f, this.transform.position.z), Quaternion.identity);
            // start coroutine
            StartCoroutine(DisableHittable());
        }
    }

    // Coroutine
    IEnumerator DisableHittable() {
	    if (!ObjectMovedAndStopped()){
            yield return new WaitUntil(() => ObjectMovedAndStopped());
        }

        //continues here when the ObjectMovedAndStopped() returns true
        spriteRenderer.sprite = usedQuestionBox; // change sprite to be "used-box" sprite
        rigidBody.bodyType = RigidbodyType2D.Static; // make the box unaffected by Physics

        //reset box position
        this.transform.localPosition = Vector3.zero;
        springJoint.enabled = false; // disable spring
    }

    bool  ObjectMovedAndStopped() {
        return Mathf.Abs(rigidBody.velocity.magnitude) < 0.01;
    }

}

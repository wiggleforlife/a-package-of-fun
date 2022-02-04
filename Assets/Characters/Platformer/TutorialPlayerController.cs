using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TutorialPlayerController : MonoBehaviour {

    [SerializeField] RuntimeAnimatorController animControllerLeft;
    [SerializeField] RuntimeAnimatorController animControllerRight;
    [SerializeField] Sprite spriteLeft;
    [SerializeField] Sprite spriteRight;
    [SerializeField] LayerMask tileLayer;
    [SerializeField] GameObject littleBox;
    [SerializeField] Text deathText;
    [SerializeField] Text winText;
    [SerializeField] Text tutorialText;

    
    Rigidbody2D rb;
    Collider2D collider;
    float distToGround;
    bool canJump = false;
    bool canDrop = false;
    bool conveyerBelt = false;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
        distToGround = collider.bounds.extents.y;
    }

    void Update() {
        float xinput = Input.GetAxis("Horizontal");
        transform.position = transform.position + new Vector3(xinput * (8f) * Time.deltaTime, 0, 0);
        if (xinput > 0) {
            switchToRight();
        } else if (xinput < 0) {
            switchToLeft();
        }

        if (Input.GetKeyDown(KeyCode.Escape)) {
            SceneManager.LoadScene(0);
        }

        if (littleBox.GetComponent<PickUppable>().canBePickedUp == true) {
            if (Input.GetKeyDown(KeyCode.E)) {
                StartCoroutine(pickUp());
            }
        }

        if (littleBox.GetComponent<PickUppable>().isPickedUp == true && canDrop == true) {
            if (Input.GetKeyDown(KeyCode.E)) {
                littleBox.GetComponent<PickUppable>().isPickedUp = false;
                canDrop = false;
            }
        }

        if (transform.position.x > -6 && transform.position.x < -4) {
            tutorialText.GetComponent<Text>().text = "Press W to jump";
        } else if (transform.position.x > -2 && transform.position.x < 1 && littleBox.GetComponent<PickUppable>().isPickedUp == false) {
            tutorialText.GetComponent<Text>().text = "Press E to pick up the little box";
        } else if (transform.position.x > -2 && transform.position.x < 1 && littleBox.GetComponent<PickUppable>().isPickedUp == true) {
            tutorialText.GetComponent<Text>().text = "Jump over the saws, or they will kill you";
        } else if (transform.position.x > -5.5f && transform.position.x < 10) {
            tutorialText.GetComponent<Text>().text = "Stand on top of the conveyer belt and it will move you";
        } else if (transform.position.x > 12) {
            tutorialText.GetComponent<Text>().text = "Put the little box in the box taker to complete the level";
        }
    }

    void FixedUpdate() {
        transform.rotation = new Quaternion(0, 0, 0, 0);
        if (Input.GetAxis("Jump (Platformer)") == 1 && canJump == true) {
            rb.velocity = rb.velocity + Vector2.up * 5;
        }
        if (conveyerBelt) {
            transform.position += new Vector3(0.1f, 0, 0);
        }
    }
    

    public void switchToLeft() {
        Animator animator = GetComponent<Animator>();
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        animator.runtimeAnimatorController = animControllerLeft;
        spriteRenderer.sprite = spriteLeft;
    }

    public void switchToRight() {
        Animator animator = GetComponent<Animator>();
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        animator.runtimeAnimatorController = animControllerRight;
        spriteRenderer.sprite = spriteRight;
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        canJump = true;
        if (collision.gameObject.tag == "Damager") {
            StartCoroutine(dead());
        }else if (collision.gameObject.tag == "Goal" && littleBox.GetComponent<PickUppable>().isPickedUp == true) {
            StartCoroutine(win());
        } else if (collision.gameObject.tag == "Movement") {
            conveyerBelt = true;

        }
    }

    private void OnCollisionExit2D(Collision2D collision) {
        canJump = false;
        if (collision.gameObject.tag == "Movement") {
            conveyerBelt = false;
        }
    }

    public IEnumerator pickUp() {
        int num = 1;
        littleBox.GetComponent<PickUppable>().isPickedUp = true;
        while (num == 1) {yield return new WaitForSeconds(1); num = 0;}
        canDrop = true;
    }

    public IEnumerator dead() {
        int num = 1;
        tutorialText.GetComponent<Text>().enabled = false;
        deathText.GetComponent<Text>().enabled = true;
        while (num == 1) {yield return new WaitForSeconds(1); num = 0;}
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public IEnumerator win() {
        tutorialText.GetComponent<Text>().enabled = false;
        winText.GetComponent<Text>().enabled = true;
        littleBox.GetComponent<PickUppable>().isPickedUp = false;
        littleBox.GetComponent<SpriteRenderer>().enabled = false;
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(0);
    }

}

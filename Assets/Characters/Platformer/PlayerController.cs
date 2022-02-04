using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{

    [SerializeField] RuntimeAnimatorController animControllerLeft;
    [SerializeField] RuntimeAnimatorController animControllerRight;
    [SerializeField] Sprite spriteLeft;
    [SerializeField] Sprite spriteRight;
    [SerializeField] GameObject littleBox;
    [SerializeField] Text deathText;
    [SerializeField] Text winText;
    [SerializeField] Text timerText;
    [SerializeField] int time;
    [SerializeField] bool timerEnabled = true;
    [SerializeField] float conveyerSpeed;


    Rigidbody2D rb;
    Collider2D collider;
    float distToGround;
    bool canJump = false;
    bool canDrop = false;
    bool conveyerBelt = false;
    bool reverseConveyerBelt = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
        distToGround = collider.bounds.extents.y;
        if (timerEnabled)
        {
            timerText.GetComponent<Text>().text = time.ToString();
            StartCoroutine(timer());
        }
        else
        {
            timerText.GetComponent<Text>().enabled = false;
        }

    }

    void Update()
    {
        float xinput = Input.GetAxis("Horizontal");
        Debug.Log(xinput);
        if (xinput > 0)
        {
            switchToRight();
        }
        else if (xinput < 0)
        {
            switchToLeft();
        }

        if (littleBox.GetComponent<PickUppable>().canBePickedUp == true)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                StartCoroutine(pickUp());
            }
        }

        if (littleBox.GetComponent<PickUppable>().isPickedUp == true && canDrop == true)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                littleBox.GetComponent<PickUppable>().isPickedUp = false;
                canDrop = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(0);
        }
    }

    void FixedUpdate()
    {
        transform.rotation = new Quaternion(0, 0, 0, 0);
        if (Input.GetAxis("Jump (Platformer)") == 1 && canJump == true)
        {
            rb.velocity = rb.velocity + Vector2.up * 5;
        }
        if (conveyerBelt)
        {
            transform.position += new Vector3(conveyerSpeed, 0, 0);
        }
        else if (reverseConveyerBelt)
        {
            transform.position += new Vector3(-conveyerSpeed, 0, 0);
        }
        if (transform.position.y < -10)
        {
            StartCoroutine(dead());
        }

        float xinput = Input.GetAxis("Horizontal");
        rb.velocity = rb.velocity + new Vector2(xinput * 5f, 0);
        Debug.Log(rb.velocity);
        transform.rotation = new Quaternion(0, 0, 0, 0);
    }

    //Sprite flipping that depends on movement direction
    public void switchToLeft()
    {
        Animator animator = GetComponent<Animator>();
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        animator.runtimeAnimatorController = animControllerLeft;
        spriteRenderer.sprite = spriteLeft;
    }

    public void switchToRight()
    {
        Animator animator = GetComponent<Animator>();
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        animator.runtimeAnimatorController = animControllerRight;
        spriteRenderer.sprite = spriteRight;
    }

    //Collision detection for damage, jumping capabilities, 
    private void OnCollisionEnter2D(Collision2D collision)
    {
        canJump = true;
        if (collision.gameObject.tag == "Damager")
        {
            StartCoroutine(dead());
        }
        else if (collision.gameObject.tag == "Goal" && littleBox.GetComponent<PickUppable>().isPickedUp == true)
        {
            StartCoroutine(win());
        }
        else if (collision.gameObject.tag == "Movement")
        {
            conveyerBelt = true;
        }
        else if (collision.gameObject.tag == "ReverseMovement")
        {
            reverseConveyerBelt = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        canJump = false;
        if (collision.gameObject.tag == "Movement")
        {
            conveyerBelt = false;
        }
        else if (collision.gameObject.tag == "ReverseMovement")
        {
            reverseConveyerBelt = false;
        }
    }

    //Pick up the smol box
    public IEnumerator pickUp()
    {
        int num = 1;
        littleBox.GetComponent<PickUppable>().isPickedUp = true;
        while (num == 1) { yield return new WaitForSeconds(1); num = 0; }
        canDrop = true;
    }

    //Play the death effect
    public IEnumerator dead()
    {
        int num = 1;
        deathText.GetComponent<Text>().enabled = true;
        while (num == 1) { yield return new WaitForSeconds(1); num = 0; }
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    //Play the time up effect
    public IEnumerator timeOut()
    {
        int num = 1;
        deathText.GetComponent<Text>().text = "Time's Up'";
        deathText.GetComponent<Text>().enabled = true;
        while (num == 1) { yield return new WaitForSeconds(1); num = 0; }
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    //Timer and end recognition
    public IEnumerator timer()
    {
        int timer = time;
        while (timer > 0)
        {
            yield return new WaitForSeconds(1);
            timer -= 1;
            timerText.GetComponent<Text>().text = timer.ToString();
        }
        if (timer < 1)
        {
            StartCoroutine(timeOut());
        }
    }

    //Win effect
    public IEnumerator win()
    {
        winText.GetComponent<Text>().enabled = true;
        littleBox.GetComponent<PickUppable>().isPickedUp = false;
        littleBox.GetComponent<SpriteRenderer>().enabled = false;
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(0);
    }

}

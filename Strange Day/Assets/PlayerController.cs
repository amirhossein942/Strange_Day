using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour, iDamageable
{
    public float movementSpeed = 1f;

    public float collisionOffset = 0.05f;

    public bool isHit = false;

    public bool isBouncing = false;

    public float TimetoColor;

    public ContactFilter2D movementFilter;

    Vector2 movementInput;

    SpriteRenderer spriteRenderer;

    Rigidbody2D rb;

    Animator animator;

    public Animator animator2;

    Collider2D coll;

    Color defaultColor;

    Vector2 lastVelocity;

    public PlayerInput pi;

    List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();

    bool canMove = true;

    public swordAttack swordAttack;

    public float health;

    public GameObject DMGText;

    public float Health   
    {
        set 
        { 
            health = value;
            if (health <= 0) 
            {
                RectTransform rectTransform = Instantiate(DMGText).GetComponent<RectTransform>();
                rectTransform.transform.position = Camera.main.WorldToScreenPoint(gameObject.transform.position);

                Canvas canvas = GameObject.FindObjectOfType<Canvas>();
                rectTransform.SetParent(canvas.transform);

                pi.enabled = false;
                DeathAn();
               
            }
        
        
        }

        get { return health; } 
    
    
    }

    public bool _targetable = true;

    public bool Targetable { get {return _targetable;} 
    
        set { _targetable = value;
            
            animator.gameObject.GetComponent<Animator>().enabled = false;            
            coll.enabled = false;
            
            rb.simulated = false;
            pi.enabled = false;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>(); 
        spriteRenderer = GetComponent<SpriteRenderer>();
        defaultColor = spriteRenderer.color;
        coll = GetComponent<Collider2D>();
        pi = GetComponent<PlayerInput>();
       
    }


    private void FixedUpdate()
    {
        lastVelocity = rb.velocity;

        //Debug.Log(lastVelocity.magnitude);
        //Only when B O O L is true we get to these parts
        if (canMove)
        {
            //If movement input ain't 0, try to move 
            if (movementInput != Vector2.zero)
            {
                bool success = TryMove(movementInput);

                if (!success)
                {

                    success = TryMove(new Vector2(movementInput.x, 0));
                  
                }

                if (!success)
                {
                    success = TryMove(new Vector2(0, movementInput.y));
                }


                animator.SetBool("isMoving", success);

               
            }
            else
            {
                animator.SetBool("isMoving", false);
                
            }

            //set direction of sprite to movement direction 
            if (movementInput.x < 0)
            {
                spriteRenderer.flipX = true;
            }
            else if (movementInput.x > 0)
            {
                spriteRenderer.flipX = false;
            }

        }
    }

    private bool TryMove(Vector2 direction) 
    {
        if (direction != Vector2.zero)
        {
            int Count = rb.Cast(
                   direction, // X and Y between 1 and -1 representing the direction from the body to look for collisions
                   movementFilter, // Settings determining where a collision can occur on, such as layers to collide with
                   castCollisions, // List to store found collisions into after Cast is finished
                   movementSpeed * Time.fixedDeltaTime + collisionOffset// Amount of Cast equal to the movement plus an offset     
                   );
            
            if (Count == 0)
            {
                if(!isBouncing)rb.MovePosition(rb.position + movementSpeed * Time.fixedDeltaTime * direction );
              //rb.AddForce(direction * Time.deltaTime * movementSpeed);               
                return true;
            }
            else                         
                return false;
            
        }

        else
            return false;
    }

    public void OnMove(InputValue movementValue)
    {
        movementInput = movementValue.Get<Vector2>();
    }

    void OnFire() {

        animator.SetTrigger("swordAttack");
    }

    public void SwordAttack() 
    {
        LockMovement();

        if (spriteRenderer.flipX == true)
        {
            swordAttack.LeftAttack();
        }

        if (spriteRenderer.flipX == false)
        {
            swordAttack.RightAttack();
        } 
    }

    public void LockMovement() {
        canMove = false;
    
    }

    public void UnlockMovement() {
        canMove = true;
        swordAttack.Stopattack();
    }

    public void OnHit(float damage, Vector2 knockback)
    {
        Health -= damage;
        rb.AddForce(knockback, ForceMode2D.Impulse);
        //rb.MovePosition(knockback * Time.fixedDeltaTime);
        Debug.Log(knockback);
    }

    public void OnHit(float damage)
    {
        Health -= damage;
    }

    public void DeathAn() 
    {
        animator.SetTrigger("Death");
    }

    public void PlayerFucked()
    {
        
        Targetable = false;
        
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.tag == "GameController")
        {

            return;
        }

        else if (collision.CompareTag("Chest"))
        {
            Debug.Log("Ass");
            animator2 = GameObject.Find("Chest").GetComponent<Animator>();
            animator2.SetBool("New Bool", true);
            if (Health != 0 && Health < 9) {
                float diff = 9 - Health;
                Health += diff;            
            }
             
        }

        else if (collision.CompareTag("Enemy"))
        {


            if (!isHit) {

                isHit = true;
                RectTransform rectTransform = Instantiate(DMGText).GetComponent<RectTransform>();
                rectTransform.transform.position = Camera.main.WorldToScreenPoint(gameObject.transform.position);

                Canvas canvas = GameObject.FindObjectOfType<Canvas>();
                rectTransform.SetParent(canvas.transform);


                StartCoroutine("SwitchColor");

            }


        }
    }

    

    void StopBounce()
    {
        isBouncing = false;
    }

    public IEnumerator SwitchColor() 
    {
        spriteRenderer.color = new Color(1f, 0.30196078f, 0.30196078f);
        yield return new WaitForSeconds(TimetoColor);
        spriteRenderer.color = defaultColor;
        isHit = false;
    }
}

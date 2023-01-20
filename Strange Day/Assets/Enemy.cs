using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, iDamageable
{
    public Detection detector;

    public Collider2D coll;

    Animator animator;

    public float health = 6f;

    public Rigidbody2D rb;

    public Rigidbody2D player_rb;

    public float damage = 3f;

    public float KnockBackForce = 800f;

    public float deathKnock = 300f;

    public float speed = 500f;

    public GameObject Text_TMP;

    public float Health
    {

        set
        {
            print(value);
            health = value;
            if (health <= 0)
            {
                RectTransform rectTransform = Instantiate(Text_TMP).GetComponent<RectTransform>();   
                rectTransform.transform.position = Camera.main.WorldToScreenPoint(gameObject.transform.position);

                Canvas canvas = GameObject.FindObjectOfType<Canvas>();
                rectTransform.SetParent(canvas.transform);


                Defeated();

            }

            if (health > 0) 
            {
                animator.SetTrigger("Hit");
                
                RectTransform rectTransform = Instantiate(Text_TMP).GetComponent<RectTransform>();
                rectTransform.transform.position = Camera.main.WorldToScreenPoint(gameObject.transform.position);   

                Canvas canvas = GameObject.FindObjectOfType<Canvas>();
                rectTransform.SetParent(canvas.transform);
                
            }

        }

        get
        {
            return health;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
       

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (detector.detectedList.Count > 0) {
         
            Vector2 direction = (detector.detectedList[0].transform.position - transform.position).normalized;
            rb.AddForce(direction * speed * Time.deltaTime);
        }
         
    }

    public void Defeated()
    {
        coll.enabled = false;
        rb.simulated = false;
        animator.SetTrigger("Defeated");

    }



    public void RemoveEnemy()
    {
        coll.enabled = false;
        rb.simulated = false;
        Destroy(gameObject);
    }

    public void Idle() {
        animator.SetTrigger("Idle");
 
    }

    public void OnHit(float damage, Vector2 knockback)
    {
        Health -= damage;
        rb.AddForce(knockback);
        //rb.MovePosition(knockback);
        Debug.Log(knockback);
    }

    public void OnHit(float damage)
    {
        Health -= damage;  
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
       
        if (collision.tag == "GameController") {

            return;
        }
        else if (collision.tag == ("Player")) 
        {
            //Debug.Log("Player touched");
            player_rb = collision.GetComponent<Rigidbody2D>();
            PlayerController player = collision.GetComponent<PlayerController>();
            
            //Vector3 PlayerPos = collision.transform.position;
            Vector2 Direction = (collision.transform.position - transform.position).normalized;
            Vector2 KnockBack = Direction * KnockBackForce;
            //Vector2 deathKnockVec = Direction * deathKnock;

            if (player != null)
            {
                
                    player.OnHit(damage, KnockBack);

               
                 
            }
        }
    }
}

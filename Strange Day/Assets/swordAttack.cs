using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class swordAttack : MonoBehaviour
{
    public float damage = 3f;

    public Vector3 right = new Vector3(0.15f, -0.14f, 0);
    public Vector3 left = new Vector3(-0.15f, -0.1f, 0);
    public float knockbackForce = 50f;

    public Collider2D swordCollider;

    // Start is called before the first frame update
    private void Start()
    {
        //Notice no box collider, cause colision can be on objs other than boxes
        //swordCollider = GetComponent<Collider2D>();
         
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void RightAttack() 
    {
        swordCollider.enabled = true;
        transform.localPosition = right;
    }

    public void LeftAttack() 
    {        
        swordCollider.enabled = true;
        transform.localPosition = new Vector3(-0.15f, -0.1f, 0);
    }

    public void Stopattack() 
    {
        swordCollider.enabled = false;   
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "GameController") {

            return;
        }

        if (other.CompareTag("Enemy")) 
        {

            Enemy enemy = other.GetComponent<Enemy>();
            Vector3 ParentPos = gameObject.GetComponentInParent<Transform>().position;
            Vector2 direction = (Vector2) (other.gameObject.transform.position - ParentPos).normalized;
            Vector2 knockback = direction * knockbackForce;

            
            if (enemy != null) 
            {
                enemy.OnHit(damage, knockback);
            
            }
            
        }
    }


}

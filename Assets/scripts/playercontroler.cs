using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class playercontroler : MonoBehaviour
{
    public float movespeed = 5f;
    private float InputX;

    private bool isFlip = false;

    private Rigidbody2D rig;
    private Animator ani;

    public Transform floor;
    public Transform attackpoint;
    public float attackrange = .3f;
    private int jumpcount = 0;

    public LayerMask groundMask, enemyMask;

    private bool isground;

    public void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        ani = GetComponent<Animator>();
    }

    public void Update()
    {
        rig.velocity = new Vector2(movespeed * InputX, rig.velocity.y);

        //判斷移動
        ani.SetBool("isrun", Mathf.Abs(rig.velocity.x) > 0);

        //判斷跳躍
        ani.SetBool("isgrounded", isground);
        ani.SetFloat("yVelocity", rig.velocity.y); 

        //判斷翻轉
        if(!isFlip)
        {
            if(rig.velocity.x < 0)
            {
                isFlip = true;
                transform.Rotate(0.0f, 180.0f, 0.0f);
            }
        }
        else 
        {
            if(rig.velocity.x > 0)
            {
                isFlip = false;
                transform.Rotate(0.0f, 180.0f, 0.0f);
            }
        }

        //判斷觸地
        isground = Physics2D.OverlapCircle(floor.position, .2f, groundMask);
        if(isground)
        {
            jumpcount = 0;
        }

           
    }
    //移動
    public void Move(InputAction.CallbackContext context)
    {
        InputX = context.ReadValue<Vector2>().x;
    }
    //跳躍
    public void Jump(InputAction.CallbackContext context)
    {
        if(isground)
        {
            rig.velocity = new Vector2(rig.velocity.x,7);
            ani.SetBool("attack", false);
            jumpcount++; 
        }
        else if(jumpcount<=2)
        {
            rig.velocity = new Vector2(rig.velocity.x,7);
            ani.SetBool("attack", false);
            jumpcount++;
        }

    }
    //制空圈
    private void OnDrawGizmos() 
    {
        Gizmos.DrawWireSphere(floor.position, .2f);
        Gizmos.DrawWireSphere(attackpoint.position, attackrange);
    }

    private void CheckAttackHit()
    {
        //宣告打到的目標物件(可能為空)
        Collider2D[] detectedObjects = Physics2D.OverlapCircleAll(attackpoint.position, attackrange, enemyMask);

        foreach(Collider2D collider in detectedObjects)
        {
            Debug.Log(collider.gameObject.name);
            collider.gameObject.SendMessage("onDamage", 10.0f);
        }
    }


    //攻擊
    public void Attack(InputAction.CallbackContext context)
    {
        if(isground)
        {
            ani.SetBool("attack", true);
        }
        
    }
    //結束攻擊
    public void EndAttack()
    {
        ani.SetBool("attack", false);
    }

    
}

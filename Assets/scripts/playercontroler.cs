using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class playercontroler : MonoBehaviour
{
    public float movespeed = 5f;    private float InputX;

    private bool isFlip = false;

    private Rigidbody2D rig;
    private Animator ani;
    public Animator boxAni;
    public Transform floor, attackpoint;
    public float attackrange = .3f, hp;
    private int jumpcount = 0, coin = 0;
    public Text coincount, hptext;
    public LayerMask groundMask, enemyMask;
    private bool isground;
    public Collider2D usaul, slide;

    public void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        ani = GetComponent<Animator>();
        hp = 30.0f;
        slide.enabled = false;
        usaul.enabled = true;
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
    //下墜
    public void Slide(InputAction.CallbackContext context)
    {
        if(!isground)
        {
            rig.velocity = new Vector2(rig.velocity.x,-17);
        }
        else
        {
            rig.velocity = new Vector2(rig.velocity.x,0);
        }
        slide.enabled = true;
        usaul.enabled = false;
        ani.SetBool("attack", false);
        ani.SetBool("isSlide", true);
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

    public void endSlide()
    {
        slide.enabled = false;
        usaul.enabled = true;
        ani.SetBool("isSlide", false);
    }
    private void OnTriggerEnter2D(Collider2D col) 
    {   
        Flag flag = FindObjectOfType<Flag>();
        if (col.gameObject.CompareTag("Box"))
        {
            Debug.Log("gocha");
            boxAni = col.gameObject.GetComponent<Animator>();
            if (boxAni != null)
            {
                boxAni.SetTrigger("open");
                coin += 1;
                col.tag = "openedBox";
                coincount.text = coin.ToString();
            }
        }
        
        if (col.gameObject.CompareTag("Flag")) // 假设你为 flag 小物件设置了 "Flag" 标签
        {
            flag.OnPlayerTouchFlag(col.gameObject);
        }

        if (col.gameObject.CompareTag("TransportGate")) // 假设你为 flag 小物件设置了 "Flag" 标签
        {
            flag.OnPlayerTouchTransportGate(col.gameObject);
        }

        if (col.gameObject.CompareTag("Fire")) // 假设你为 flag 小物件设置了 "Flag" 标签
        {
            this.onDamage(5);
        }
    }
    private void OnCollisionEnter2D(Collision2D other) 
    {
        if(other.gameObject.CompareTag("Enemy"))
        {
            //Debug.Log("AAAAAAAAAAA");
            if(transform.position.x < other.transform.position.x)//角色在敵人左邊
            {
                rig.velocity = new Vector2(-5,rig.velocity.y);
            }
            else
            {
                rig.velocity = new Vector2(5,rig.velocity.y);
            }
        }
    }
    public void onDamage(float damage)
    {
        hp = hp - damage;
        ani.SetBool("isHurt", true);
        hptext.text = hp.ToString();
        if(hp <= 0)
        {
            //Destory()
            //ani.SetBool("death", true);
            ;
        }
    }
    private void hurtover()
    {
        ani.SetBool("isHurt", false);
    }
}

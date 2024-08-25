using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class playercontroler : MonoBehaviour
{
    public float movespeed = 5f,jumpHigh = 7;    
    private float InputX;
    private bool isFlip = false;

    private Rigidbody2D rig;
    private Animator ani;
    private Animator boxAni;
    public Transform floor, attackpoint;
    public float attackrange = .3f, hp, hpmax = 30.0f;
    private int jumpcount = 0, coin = 0;
    public int jumptotal = 2;
    public Text coincount, hptext;
    public LayerMask groundMask, enemyMask;
    private bool isground;
    public Collider2D usaul, slide;
    //public GameObject healthstone, bluelight;

    public void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        ani = GetComponent<Animator>();
        hp = hpmax;
        slide.enabled = false;
        usaul.enabled = true;
        //bluelight.SetActive(true);
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
    }

    public void Update()
    {
        if (Time.deltaTime == 0)
        {
            Debug.LogWarning("Time.deltaTime 为 0，这可能导致帧时间问题。");
        }
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
            rig.velocity = new Vector2(rig.velocity.x,jumpHigh);
            ani.SetBool("attack", false);
            jumpcount++; 
        }
        else if(jumpcount<=jumptotal)
        {
            rig.velocity = new Vector2(rig.velocity.x,jumpHigh);
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
    //判斷攻擊
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
    // 結束滑翔
    public void endSlide()
    {
        slide.enabled = false;
        usaul.enabled = true;
        ani.SetBool("isSlide", false);
    }
    private void OnTriggerEnter2D(Collider2D col) 
    {   
        Flag flag = FindObjectOfType<Flag>();
        HealthStone stonelight = FindObjectOfType<HealthStone>();
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
        
        if (col.gameObject.CompareTag("Flag")) // 碰到旗幟觸發
        {
            flag.OnPlayerTouchFlag(col.gameObject);
        }

        if (col.gameObject.CompareTag("TransportGate")) // 碰到傳送門觸發
        {
            flag.OnPlayerTouchTransportGate(col.gameObject);
        }

        if (col.gameObject.CompareTag("Fire")) // 碰到火球觸發
        {
            this.onDamage(5);
        }
        if (col.gameObject.CompareTag("HealthStone")) // 碰到旗幟觸發
        {
            stonelight.OnPlayerTouchFlag(col.gameObject);
        }
    }
    public void hpHealth()
    {
        hp = hpmax;
        hptext.text = hp.ToString();
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
    //被攻擊(生物、火球)
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
    //受傷動畫
    private void hurtover()
    {
        ani.SetBool("isHurt", false);
    }
}

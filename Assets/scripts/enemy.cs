using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy : MonoBehaviour
{
    private Animator ani;
    private Rigidbody2D rig;
    public float moveSpeed = 2f, hp;
    public Transform attackpoint_enemy, innerattack, player, rightpoint, leftpoint;
    public LayerMask playerMask;
    public float attackrange = 5f, Innerrange = 0.5f, left_x, right_x;
    private bool In_range, Inner, isFlip = false;
    private void Start()
    {
        ani = GetComponent<Animator>();
        hp = 20.0f;
        rig = GetComponent<Rigidbody2D>();
        left_x = leftpoint.position.x;
        right_x = rightpoint.position.x;
        Destroy(leftpoint.gameObject);
        Destroy(rightpoint.gameObject);
        Debug.Log(left_x);
        Debug.Log(right_x); 
    }

    // Update is called once per frame
    private void Update()
    {
        ani.SetBool("isrun", Mathf.Abs(rig.velocity.x) > 0);//判斷移動

        if(!isFlip)//判斷翻轉
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

        //float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        if (hp <= 0)
        {
            Destroy(gameObject, 2f);  // 销毁当前对象
        }
        Check_player();
    }

    public void onDamage(float damage)
    {
        hp = hp - damage;
        ani.SetTrigger("ingured");
        if(hp <= 0)
        {
            //Destory()
            ani.SetBool("death", true);
        }
    }

    private void OnDrawGizmos() 
    {
        Gizmos.DrawWireSphere(attackpoint_enemy.position, attackrange);
        Gizmos.DrawWireSphere(player.position, .2f);
        Gizmos.DrawWireSphere(innerattack.position, Innerrange);
    }

    private void Check_player()
    {
        In_range = Physics2D.OverlapCircle(attackpoint_enemy.position, attackrange, playerMask);//判斷玩家有沒有進入行動區域
        Inner = Physics2D.OverlapCircle(innerattack.position, Innerrange, playerMask);//判斷玩家有沒有進入攻擊區域
        if(transform.position.x >= left_x && transform.position.x <= right_x)
        {
            if(Inner)//進入攻擊區域
            {
                ani.SetBool("inrange", true);
            }
            else if(In_range && !Inner)//進入行動區域，尚未進入攻擊區域
            {
                Vector2 direction = (player.position - transform.position).normalized;
                rig.velocity = new Vector2(direction.x * moveSpeed, rig.velocity.y);
            }
        }
        else if(transform.position.x >= left_x)//卡在右點
        {
            if(Inner)//進入攻擊區域
            {
                ani.SetBool("inrange", true);
            }
            else if(In_range && !Inner)//進入行動區域，尚未進入攻擊區域
            {
                Vector2 direction = (player.position - transform.position).normalized;
                if(player.position.x <= right_x)
                {
                    rig.velocity = new Vector2(direction.x * moveSpeed, rig.velocity.y);
                }
            }
        }
        else if(transform.position.x <= right_x)//卡在左點
        {
            if(Inner)//進入攻擊區域
            {
                ani.SetBool("inrange", true);
            }
            else if(In_range && !Inner)//進入行動區域，尚未進入攻擊區域
            {
                Vector2 direction = (player.position - transform.position).normalized;
                if(player.position.x >= left_x)
                {
                    rig.velocity = new Vector2(direction.x * moveSpeed, rig.velocity.y);
                }
                
            }
        }
    }

    private void CheckAttackHit()
    {
        //宣告打到的目標物件(可能為空)
        ani.SetBool("inrange", false);
        Collider2D[] colliders = Physics2D.OverlapCircleAll(innerattack.position, Innerrange, playerMask);
        foreach (Collider2D collider in colliders)
        {
            playercontroler playerHealth = collider.GetComponent<playercontroler>();
            if (playerHealth != null)
            {
                playerHealth.onDamage(5);
                Debug.Log("Player hit! Health reduced by 5.");
                break; // 如果找到玩家，跳出循环
            }
        }
    }

}
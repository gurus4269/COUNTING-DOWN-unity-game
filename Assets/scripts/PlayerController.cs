using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("detail")]
    public float HP;
    public float HPmax;
    public float movePower = 10f;
    public float dashPower = 20f;
    public float dashCD = 10f;
    public float EP = 500f;
    public float skillCD = 10f;
    private float jumpPower = 25f; //Set Gravity Scale in Rigidbody2D Component to 5
    public Text HPcount, EPcount;
    //控制角色
    private Rigidbody2D rig;
    private Animator ani;
    private float InputX;
    private bool isground, isFlip;
    public Transform floor;
    public LayerMask groundMask, enemyMask;

    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        ani = GetComponent<Animator>();
        HP = HPmax;
        HPcount.text = HP.ToString();
        EPcount.text = EP.ToString();
        // slide.enabled = false;
        // usaul.enabled = true;
        //bluelight.SetActive(true);
        // QualitySettings.vSyncCount = 0;
        // Application.targetFrameRate = 60;
    }

    // Update is called once per frame
    void Update()
    {
        rig.velocity = new Vector2(movePower * InputX, rig.velocity.y);

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
        
    }
    public void Move(InputAction.CallbackContext context)
    {
        InputX = context.ReadValue<Vector2>().x;
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if(isground)
        {
            rig.velocity = new Vector2(rig.velocity.x,jumpPower);
            ani.SetBool("attack", false);
        }
    }
    public void Slide(InputAction.CallbackContext context)
    {
        if(!isground)
        {
            rig.velocity = new Vector2(rig.velocity.x,-17);
        }
        // slide.enabled = true;
        // usaul.enabled = false;
        // ani.SetBool("attack", false);
        // ani.SetBool("isSlide", true);
    }
}

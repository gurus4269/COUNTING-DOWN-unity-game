using UnityEngine;
using UnityEngine.UI;

namespace ClearSky
{
    public class SimplePlayerController : MonoBehaviour
    {
        [Header("detail")]
        public float HP;
        public float HPmax;
        public float movePower = 10f;
        public float dashPower = 20f;
        public float dashCD = 0.01f;
        public float EP = 50f;
        public float EPmax = 50f;
        public float skillCD = 10f;
        private float jumpPower = 25f; //Set Gravity Scale in Rigidbody2D Component to 5
        public Text HPcount, EPcount;

        private Rigidbody2D rb;
        private Animator anim;
        Vector3 movement;
        private int direction = 1;
        bool isJumping = false;
        private bool alive = true, isFlip = false;
        //private bool isground;


        // Start is called before the first frame update
        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            anim = GetComponent<Animator>();
            HPcount.text = HP.ToString();
            EPcount.text = EP.ToString();
        }

        private void Update()
        {
            Restart();
            if (alive)
            {
                Hurt();
                Die();
                Attack();
                Jump();
                Run();
                Slide();
                Dash();
                
            }
            
        }

        public Vector2 lockedScale = new Vector2(0.2f, 0.2f);

        void LateUpdate()
        {
            // 在每帧的最后强制设置对象的缩放值为 lockedScale。
            transform.localScale = lockedScale;
        }
        private void OnTriggerEnter2D(Collider2D other)
        {
            anim.SetBool("isJump", false);
        }


        void Run()
        {
            Vector3 moveVelocity = Vector3.zero;
            //anim.SetBool("isRun", false);


            // if (Input.GetAxisRaw("Horizontal") < 0)
            // {
            //     direction = -1;
            //     //Debug.Log(direction);
            //     moveVelocity = Vector3.left;
            //     if(!isFlip)
            //     {
            //         isFlip = true;
            //         transform.rotation = Quaternion.Euler(0, 180, 0);
            //     }
            //     //transform.localScale = new Vector3(-1, 1, 1);
                
            //     if (!anim.GetBool("isJump"))
            //         anim.SetBool("isRun", true);

            // }
            // if (Input.GetAxisRaw("Horizontal") > 0)
            // {
            //     direction = 1;
            //     //Debug.Log(direction);
            //     moveVelocity = Vector3.right;
            //     if(isFlip)
            //     {
            //         isFlip = false;
            //         transform.rotation = Quaternion.Euler(0, 0, 0);
            //     }

            //     //transform.localScale = new Vector3(2, -1, 1);
            //     if (!anim.GetBool("isJump"))
            //         anim.SetBool("isRun", true);

            // }
            if (!anim.GetBool("isJump"))
            {
                anim.SetBool("isRun", true);
            }     
            moveVelocity = Vector3.right;
            transform.position += moveVelocity * movePower * Time.deltaTime;
        }
        void Jump()
        {
            if ((Input.GetButtonDown("Jump") || Input.GetAxisRaw("Vertical") > 0) && !anim.GetBool("isJump"))
            {
                isJumping = true;
                anim.SetBool("isJump", true);
            }
            if (!isJumping)
            {
                return;
            }

            rb.velocity = Vector2.zero;

            Vector2 jumpVelocity = new Vector2(0, jumpPower);
            rb.AddForce(jumpVelocity, ForceMode2D.Impulse);

            isJumping = false;
        }
        void Attack()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                anim.SetTrigger("attack");
            }
        }
        void Hurt()
        {
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                anim.SetTrigger("hurt");
                if (direction == 1)
                    rb.AddForce(new Vector2(-5f, 1f), ForceMode2D.Impulse);
                else
                    rb.AddForce(new Vector2(5f, 1f), ForceMode2D.Impulse);
            }
        }
        void Die()
        {
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                anim.SetTrigger("die");
                alive = false;
            }
        }
        void Slide()
        {
            if (Input.GetAxisRaw("Vertical") < 0 )
            {
                Vector3 moveVelocity = Vector3.zero;
                moveVelocity = Vector3.up;
                transform.position += moveVelocity * -jumpPower * Time.deltaTime;
            }
        }
        void Dash()
        {
            Vector3 moveVelocity = Vector3.zero;
            if (Input.GetAxisRaw("Horizontal") > 0 && dashCD > 0)
            {
                if (!anim.GetBool("isJump"))
                {
                    anim.SetBool("isRun", true);
                }    
                moveVelocity = Vector3.right;
                if(EP > 0)
                {
                    EP -= 0.1f;
                    EPcount.text = ((int)EP).ToString();
                    transform.position += moveVelocity * dashPower * Time.deltaTime;
                }
                
            }
            else
            {
                if(EP < EPmax)
                {
                    EP += dashCD;
                    EPcount.text =  ((int)EP).ToString();
                }
            }
        }
        void Restart()//改成回到存檔點
        {
            if (Input.GetKeyDown(KeyCode.Alpha0))
            {
                anim.SetTrigger("idle");
                alive = true;
            }
        }
    }
}
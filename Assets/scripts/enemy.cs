using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy : MonoBehaviour
{
    private float hp;
    private Animator ani;
    // Start is called before the first frame update
    private void Start()
    {
        ani = GetComponent<Animator>();
        hp = 20.0f;
        Debug.Log("hi i am enemy my hp is:" + hp);
    }

    // Update is called once per frame
    private void Update()
    {
        
    }

    public void onDamage(float damage)
    {
        hp = hp - damage;
        ani.SetTrigger("ingured");
        Debug.Log("hp:" + hp);
        if(hp <= 0)
        {
            ani.SetBool("death", true);
        }
    }
}

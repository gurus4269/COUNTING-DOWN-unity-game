using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    //public Collider2D player;
    public GameObject board;//浮動的提示板
    public GameObject window;//會跳出來的畫面
    private bool touch;
    // Start is called before the first frame update
    void Start()
    {
        board.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E) && touch == true)
        {
            window.SetActive(true);
            Time.timeScale = 0f;
        }
    }
    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.gameObject.CompareTag("Player"))
        {
            //Debug.Log("Player entered the trigger.");
            board.SetActive(true);
            touch = true;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        // 只有当 Player 离开触发器时，才禁用 board
        if (other.gameObject.CompareTag("Player"))
        {
            board.SetActive(false);
            touch = false;
        }
    }
    
}

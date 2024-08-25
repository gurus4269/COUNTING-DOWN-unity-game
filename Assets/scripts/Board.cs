using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    //public Collider2D player;
    public GameObject board;
    // Start is called before the first frame update
    void Start()
    {
        board.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player entered the trigger.");
            board.SetActive(true);
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        // 只有当 Player 离开触发器时，才禁用 board
        if (other.gameObject.CompareTag("Player"))
        {
            board.SetActive(false);
        }
    }
    
}

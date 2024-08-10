using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        // 檢查碰撞物體的層級是否為 "ground"
        if (collision.gameObject.layer == LayerMask.NameToLayer("ground"))
        {
            // 銷毀火球
            Destroy(gameObject);
        }
    }
}

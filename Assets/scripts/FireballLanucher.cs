using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.Animations;

public class FireballLanucher : MonoBehaviour
{
    public GameObject fireballPrefab;  // 參考火球的 Prefab
    public float fireballSpeed = 10f;  // 火球的速度
    public float fireRate = 1f;        // 火球的發射頻率
    private float nextFireTime = 0f;

    void Update()
    {
        // 持續發射火球
        if (Time.time > nextFireTime)
        {
            nextFireTime = Time.time + 1f / fireRate;
            FireFireball();
        }
    }

    void FireFireball()
    {
        // 生成火球
        GameObject fireball = Instantiate(fireballPrefab, transform.position, transform.rotation);
        // 賦予火球向前的速度
        Rigidbody2D rb = fireball.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = Vector2.right * -fireballSpeed;
        }
    }
}

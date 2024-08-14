using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthStone : MonoBehaviour
{
    public GameObject[] BlueLights;
    //private bool lightUpOrDown = true;

    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject BlueLight in BlueLights)
        {
            BlueLight.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // 这里不再需要检查 lightUpOrDown，因为协程会单独处理重启逻辑
    }

    // 检测碰撞，只有和玩家接触的石碑重启
    /*private void OnTriggerEnter2D(Collider2D col) 
    {   
        playercontroler player = FindObjectOfType<playercontroler>();

        foreach(GameObject BlueLight in BlueLights)
        {
            if (col.gameObject.CompareTag("player") && BlueLight.activeSelf) // 石碑有碰到玩家且可用
            {
                BlueLight.SetActive(false); // 关闭光源
                StartCoroutine(ActivateAfterDelayCoroutine(BlueLight, 30.0f)); // 启动协程重启该光源
                player.hpHealth(); // 回复玩家健康
                break; // 只处理第一个碰到的光源
            }
        }
    }*/

    public void OnPlayerTouchFlag(GameObject light)
    {
        playercontroler player = FindObjectOfType<playercontroler>();
        foreach (GameObject BlueLight in BlueLights)
        {
            if(light == BlueLight)
            {
                BlueLight.SetActive(false);
                StartCoroutine(ActivateAfterDelayCoroutine(BlueLight, 30.0f)); // 启动协程重启该光源
                player.hpHealth(); // 回复玩家健康
                break; // 只处理第一个碰到的光源
                //lightUpOrDown = false;
            }
        }
    }

    // 协程，用来处理延迟启用
    private IEnumerator ActivateAfterDelayCoroutine(GameObject BlueLight, float delay)
    {
        yield return new WaitForSeconds(delay);

        // 重新启用目标 GameObject
        BlueLight.SetActive(true);
    }
}
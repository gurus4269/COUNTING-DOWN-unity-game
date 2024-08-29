using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Rolling : MonoBehaviour
{
    public ScrollRect scrollRect;
    public float scrollSpeed = 0.1f; // 滾動速度

    private void Update()
    {
        if (scrollRect != null)
        {
            // 自動向下滾動
            scrollRect.verticalNormalizedPosition -= scrollSpeed * Time.deltaTime;

            //如果滾動到底部，則回到頂部
            if (scrollRect.verticalNormalizedPosition <= 0)
            {
                scrollSpeed = 0f;
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
        }
    }
}

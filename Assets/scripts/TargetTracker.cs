using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetTracker : MonoBehaviour 
{
    public Transform player;
    public Flag flagManager;
    public Image arrow;
    public Camera CineCamera;
    public float edgeOffset = 20f;
    private Transform currentTarget; 
    
    // Update is called once per frame
    void Update()
    {
        if (flagManager.AreAllFlagsCollected())
        {
            // 指向 TransportGate
            currentTarget = flagManager.TransportGate.transform;
        }
        else
        {
            // 指向当前的 flag
            GameObject currentFlag = flagManager.GetSelectedFlag();
            if (currentFlag != null)
            {
                currentTarget = currentFlag.transform;
            }
        }

        if (currentTarget != null)
        {
            UpdateArrow(currentTarget);
        }
    }
    void UpdateArrow(Transform target)
    {
        Vector2 screenPos = CineCamera.WorldToScreenPoint(target.position);

        // 判断目标是否在屏幕外
        bool isOffScreen = screenPos.x < 0 || screenPos.x > Screen.width || screenPos.y < 0 || screenPos.y > Screen.height;

        arrow.gameObject.SetActive(isOffScreen); // 如果目标在屏幕外，则显示箭头

        if (isOffScreen)
        {
            // 计算目标与屏幕中心的方向
            Vector2 direction = screenPos - new Vector2(Screen.width / 2, Screen.height);
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;

            // 计算箭头位置
            Vector2 arrowPos = screenPos;
            arrowPos.x = Mathf.Clamp(arrowPos.x, edgeOffset, Screen.width - edgeOffset);
            arrowPos.y = Mathf.Clamp(arrowPos.y, edgeOffset, Screen.height - edgeOffset);

            // 更新箭头的旋转和位置
            arrow.rectTransform.rotation = Quaternion.Euler(0, 0, angle);
            arrow.rectTransform.position = arrowPos;
        }
    }
}

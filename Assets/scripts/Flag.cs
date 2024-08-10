using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Flag : MonoBehaviour
{
    public GameObject[] flags;
    public GameObject TransportGate;
    private List<GameObject> availableFlags; // 用于存储未被选中的 flag 子对象
    private GameObject selectedFlag;
    private bool gateActivated;
    private int flagcount;
    private int count = 0;
    public Text countFlag;

    

    // Start is called before the first frame update
    protected void Start()
    {
        //countFlag = count.te
        flagcount = flags.Length;
        availableFlags = new List<GameObject>(flags);
        TransportGate.SetActive(false);
        SelectRandomFlag();
    }

    // Update is called once per frame
    protected void Update()
    {
        if(!IsAnyFlagSelected())
        {
            SelectRandomFlag();
        }
        
        if(flagcount == count)
        {
            SelectTransportGate();
        }
    }

    void SelectRandomFlag()
    {
        if (availableFlags.Count > 0)
        {
            int randomIndex = Random.Range(0, availableFlags.Count);
            selectedFlag = availableFlags[randomIndex];
            selectedFlag.SetActive(true); // 勾选

            // 从可用的 flag 列表中移除已选中的 flag
            availableFlags.RemoveAt(randomIndex);
        }
    }

    bool IsAnyFlagSelected()
    {
        foreach (GameObject flag in flags)
        {
            if (flag.activeSelf)
            {
                return true;
            }
        }
        return false;
    }

    public void OnPlayerTouchFlag(GameObject flag)
    {
        if (flag == selectedFlag)
        {
            selectedFlag.SetActive(false); // 取消勾选
            SelectRandomFlag(); // 随机选择另一个小物件
            count ++;
            countFlag.text = count.ToString();
        }
    }

    void SelectTransportGate()
    {
        TransportGate.SetActive(true); // 勾选并显示胜利物件
    }

    public void OnPlayerTouchTransportGate(GameObject transportGate)
    {
        if (transportGate == TransportGate)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

            // 这里你可以添加显示胜利 UI 或进行其他处理的代码
        }
    }
    public GameObject GetSelectedFlag()
    {
        return selectedFlag;
    }
    public bool AreAllFlagsCollected()
    {
        return count >= flagcount;
    }
}

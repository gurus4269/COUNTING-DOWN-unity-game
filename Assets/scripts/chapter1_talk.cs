using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class chapter1_talk : MonoBehaviour
{
    public GameObject talkUI;
    public TextAsset chapterdialog;//對話的資料
    public SpriteRenderer right,left,middle;//人物圖片
    public TMP_Text nameText;//顯示的名稱
    public TMP_Text dialog;//顯示的對話
    public Button nextButton;//next按鈕
    public GameObject optionButton;//選項的prefabs
    public Transform buttonGroup;//選項父節點(自動排序)
    public List<Sprite> sprites = new List<Sprite>();//人物編號
    Dictionary<string, Sprite> characterImg = new Dictionary<string, Sprite>();//角色人物-->圖片
    private int dialogcount = 0;//保存當前的對話Index;
    private string[]  dialogRows;
    

    private void Awake() 
    {
        characterImg["Chole"] = sprites[0];
        characterImg["Ella"] = sprites[1];
    }

    void Start()
    {
        StartCoroutine(WaitOneSecond());
        readText(chapterdialog);
        readline();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator WaitOneSecond()
    {
        // 等待一秒
        yield return new WaitForSeconds(1f);
        // 一秒後執行的程式碼
        talkUI.SetActive(true);
        //Debug.Log("一秒已過去");
    }
    public void UpdateText(string _name, string _text)
    {
        nameText.text = _name;
        dialog.text = _text;
    }
    public void UpdateImage(string _name, int _pos)
    {
        if(_pos == 1)//左
        {
            left.sprite = characterImg[_name];
        }
        else if(_pos == 2)//中
        {
            middle.sprite = characterImg[_name];
        }
        else if(_pos == 3)//右
        {
            right.sprite = characterImg[_name];
        }
    }
    public void readText(TextAsset _textAsset)
    {
        dialogRows = _textAsset.text.Split('\n');
        
    }
    public void readline()
    {
        foreach(var row in dialogRows)
        {
            string[] cells = row.Split(',');
            if(cells[0] == "#" && int.Parse(cells[1]) == dialogcount)
            {
                UpdateText(cells[2], cells[3]);
                UpdateImage(cells[2], cells[4] == "left" ? 1 : (cells[4] == "middle" ? 2 : 3));

                dialogcount = int.Parse(cells[5]);
                break;
            }
            else if(cells[0] == "&" && int.Parse(cells[1]) == dialogcount)
            {
                nextButton.gameObject.SetActive(false);
                GenrateOption(int.Parse(cells[1]));
                buttonGroup.gameObject.SetActive(true);
            }
            else if(cells[0] == "END")
            {
                Debug.Log("OVER");
            }
        }
    }
    public void onClickNext()
    {
        readline();
    }
    public void GenrateOption(int _index)
    {
        string[] cells = dialogRows[_index + 1].Split(',');
        if(cells[0] == "&")
        {
            GameObject button = Instantiate(optionButton, buttonGroup);//生成
            button.GetComponentInChildren<TMP_Text>().text = cells[3];
            button.GetComponent<Button>().onClick.AddListener
            (   
                delegate 
                {
                    OnOptionClick(int.Parse(cells[5]));
                }
            );
            GenrateOption(_index + 1);
        }   
    }
    public void OnOptionClick(int _id)
    {
        dialogcount = _id;
        int count = buttonGroup.childCount;
        for(int i = 0;i < count;i++)
        {
            Destroy(buttonGroup.GetChild(i).gameObject);
        }
        buttonGroup.gameObject.SetActive(false);
        nextButton.gameObject.SetActive(true);
        readline();
    }
}

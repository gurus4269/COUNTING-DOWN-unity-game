using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class choose : MonoBehaviour
{
    private string levelname;
    public void ChooseGame()
    {
        levelname = "level1";
        SceneManager.LoadScene(levelname);
    }
}

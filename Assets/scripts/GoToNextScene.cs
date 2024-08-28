using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToNextScene : MonoBehaviour
{
    // Start is called before the first frame update
    public void GoToNext()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        Debug.Log("Loaded scene: " + SceneManager.GetActiveScene().name);
        if(Time.timeScale == 0f)
        {
            Time.timeScale = 1f;
        }
    }
}

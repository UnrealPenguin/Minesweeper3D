using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuitGame : MonoBehaviour
{
    private GameObject BTN_exit;
    // Start is called before the first frame update
    void Start()
    {
        BTN_exit = GameObject.Find("BTN_exit");
        BTN_exit.GetComponent<Button>().onClick.AddListener(qGame);
    }
    
    void qGame(){
        Application.Quit();
    }
}


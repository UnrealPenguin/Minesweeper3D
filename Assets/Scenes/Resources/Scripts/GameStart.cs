using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameStart : MonoBehaviour
{
    [SerializeField] private Button startBtn;
    GameObject canvas_ingame;
    // Start is called before the first frame update
    void Start()
    {
        startBtn.GetComponent<Button>().onClick.AddListener(startGame);
        canvas_ingame = GameObject.Find("Canvas_inGame");
        canvas_ingame.SetActive(false);
    }

    private void startGame(){
        Destroy(GameObject.Find("Canvas_start"));
        GameObject.Find("Generator").GetComponent<Minesweeper>().enabled=true;
        canvas_ingame.SetActive(true);
    }
}

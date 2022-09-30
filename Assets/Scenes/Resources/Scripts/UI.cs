using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI : MonoBehaviour
{
    private GameObject TMP_minesLeft;
    private int minesLeft;
    
    void Start(){
        TMP_minesLeft = GameObject.Find("TX_minesLeft");
    }

    void Update(){
        TMP_minesLeft.GetComponent<TextMeshProUGUI>().text ="Bombs left: " + 
                                                            GameObject.Find("Generator").GetComponent<Minesweeper>().getTotalBombs();
    }
}

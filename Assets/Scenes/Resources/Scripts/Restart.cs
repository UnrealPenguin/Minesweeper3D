using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Restart : MonoBehaviour
{
    [SerializeField] private Button restartBtn;
    void Start()
    {
        restartBtn.GetComponent<Button>().onClick.AddListener(resetGame);
    }
    public void resetGame(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}

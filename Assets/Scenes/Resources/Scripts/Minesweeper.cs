using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using TMPro;

public class Minesweeper : MonoBehaviour
{
    private int[,,] matrix;
    private int rows, columns, layers;
    private int bombCount; // Number of bomb to create and display
    private int winCon; // 
    private int[] bombArray;
    private int RNG;
    private int maxSearch;
    private int increment = 0;
    private int goodGuess = 0;
    private List<int> allBlocks; 
    private float[] center = new float[3]; // Used to center the matrix    
    GameObject[] numberMesh;
    private Mesh defaultMesh; 
    private Material defaultMat;
    private GameObject TMP_message;
    private GameObject restartBtn;

    // Start is called before the first frame update
    void Start()
    {
        // # of rows column and layers from user input
        rows = (int)GameObject.Find ("Slider_Row").GetComponent <Slider> ().value;
        columns = (int)GameObject.Find ("Slider_Column").GetComponent <Slider> ().value;;
        layers = (int)GameObject.Find ("Slider_Layer").GetComponent <Slider> ().value;;

        // columns = rows = layers = 3;
        matrix = new int[rows,columns,layers];

        center[0] = rows/2;
        center[1] = columns/2;
        center[2] = layers/2;
        
        numberMesh = createMeshNumber();

        allBlocks = new List<int>(rows*columns*layers); // Initialize list with total amount of blocks

        var difficulty = GameObject.Find ("Diff_slider").GetComponent <Slider> ().value;
        bombCount = Mathf.RoundToInt((rows*columns*layers)*difficulty); //5% bombcount is mimum, 20% bombcount is max

        winCon = bombCount; // need to have this many good guesses to win

        bombArray = new int[bombCount];
        // Create an array of size [0, bombCount] to compare against the allBlocks
        for(int i = 0; i<bombCount; i++){
            bombArray[i] = i;
        }

        defaultMesh = Resources.Load<MeshFilter>("Meshes/Block").sharedMesh;
        defaultMat = Resources.Load<Material>("Materials/M_Block");
        
        TMP_message = GameObject.Find("TX_message"); // To announce win or lost
        restartBtn = GameObject.Find("BTN_restart"); // Button to restart game after win or lost
        restartBtn.SetActive(false);

        for(int x=0; x<rows; x++){
            for(int y=0; y<columns;y++){
                for(int z=0; z<layers;z++){

                    //Non repeating random numbers to determine which blocks will have mines
                    RNG = Random.Range(0, rows*columns*layers);

                    while(allBlocks.Contains(RNG)){
                        RNG = Random.Range(0, rows*columns*layers);
                    }
                    
                    allBlocks.Add(RNG);

                    //compares the number in allBlocks to a number from [0,bombCount], if equal creates a bomb at this block
                    Blocks block = new Blocks(x, y, z, center, defaultMesh, defaultMat, matrix, isEqual(allBlocks[increment], bombArray));
                    increment++;
                }
            }
        }
    }

    void Update(){
        if(this.goodGuess == this.winCon && this.bombCount == 0){
            gameOver("You Win");
        }
    }

    // Int, Int[] -> Bool
    // Checks if _rdmNum is in array, if it is return true and remvoe it from the array
    private bool isEqual(int _rdmNum, int[] _bombArray){
        if(_bombArray.Contains(_rdmNum)){
            this.bombArray = _bombArray.Where(val => val != _rdmNum).ToArray();
            return true;
        }
        return false;
    }

    // creates an array with number meshes from 1 to 26
    private GameObject[] createMeshNumber(){
        //26 (9x3-1) is the max number of bomb that can occur around a single block
        GameObject[] tempArray = new GameObject[26]; 
        for(int i=0;i<tempArray.Length;i++){
            tempArray[i] = Resources.Load<GameObject>("Meshes/"+i.ToString());
        }
        return tempArray;
    }
    
    //reveals the whole matrix when game is lost
    public void gameOver(string _msg){
        restartBtn.SetActive(true);
         for(int x=0; x<rows; x++){
            for(int y=0; y<columns;y++){
                for(int z=0; z<layers;z++){
                    // if it hasnt been revealed yet reveal everything
                    if( GameObject.Find("x:"+x+"y:"+y+"z:"+z) != null){
                        GameObject.Find("x:"+x+"y:"+y+"z:"+z).GetComponent<BlockManager>().revealAll();     
                        Destroy(GameObject.Find("x:"+x+"y:"+y+"z:"+z).GetComponent<BoxCollider>());
                    }              
                }
            }
        } 
        TMP_message.GetComponent<TextMeshProUGUI>().text = _msg;
        TMP_message.GetComponent<TextMeshProUGUI>().enabled=true;
        
        this.enabled=false; //stops this script from running
    }

    // GETTER SETTER
    public int getTotalBombs()
    {
        return this.bombCount;
    }
    public GameObject[] getNumberMesh(){
        return this.numberMesh;
    }

    public void setBombCount(int _newCount){
        this.bombCount += _newCount;
    }

    public void setGoodGuess(int _newScore){
        this.goodGuess += _newScore;
    }

}

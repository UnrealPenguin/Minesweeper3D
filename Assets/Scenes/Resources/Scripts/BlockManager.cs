using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockManager : MonoBehaviour
{
    private int x, y, z;
    private int neighbourCount;
    private int totalBomb;
    private int[,,] matrix;
    private bool isBomb;
    private string state;
    private string display;
    List<GameObject> neighbourBlocks = new List<GameObject>();
    private Color defaultColor; 
    private Color hoverColor = new Color(0.8f, 0.8f, 0.7f); //color uses value from [0, 1]
    private Color someColor = new Color(0.8f, 0.2f, 0.2f);
    private Color flagColor = new Color(0.2f, 0.2f, 0.8f);
    private Color grey = new Color(0.1f,0.1f,0.1f, 0.01f);

    GameObject parentObj;
    
    // Start is called before the first frame update
    public void initialize(int _x, int _y, int _z, int[,,] _matrix, bool _isBomb){
        this.x = _x;
        this.y = _y;
        this.z = _z;
        this.matrix = _matrix;
        this.isBomb = _isBomb;
        this.state = "default";
        this.display = "normal";
        defaultColor = this.GetComponent<Renderer>().material.color;
    }

    void Start(){
        neighbours(); 
        parentObj = GameObject.Find("Generator");
        this.transform.parent = parentObj.transform;

        totalBomb = parentObj.GetComponent<Minesweeper>().getTotalBombs();

    }

    // Checks neighbouring blocks to count # of bombs in proximity to the current block
    public void neighbours(){
        
        for(int x = this.x-1; x<this.x+2; x++){
            for(int y = this.y-1; y<this.y+2; y++){
                for(int z = this.z-1; z<this.z+2; z++){
                    if(GameObject.Find("x:"+x+"y:"+y+"z:"+z) != null){

                        //dont add self to the list of neighbour blocks
                        if(this.gameObject.name != GameObject.Find("x:"+x+"y:"+y+"z:"+z).name){ 
                            this.neighbourBlocks.Add(GameObject.Find("x:"+x+"y:"+y+"z:"+z));
                        }

                        if(GameObject.Find("x:"+x+"y:"+y+"z:"+z).GetComponent<BlockManager>().getBlockType()){
                            this.state = "info";
                            this.neighbourCount+=1;
                        }
                            
                    }
                }
            }
        }
    }

    // Changes current block to given color
    public void setColor(Color _newColor) {
        gameObject.GetComponent<Renderer>().material.color = _newColor;
    }

    void OnMouseEnter(){
        if(this.display == "normal")
            this.setColor(hoverColor);
    }

    void OnMouseExit(){
        if(this.display == "normal")
            this.setColor(defaultColor);
    }

    void OnMouseOver() {

        if(Input.GetMouseButtonDown(0)){
            if(this.display != "revealed" && this.display != "flagged"){
                reveal(this.gameObject);
            }

        }

        if(Input.GetMouseButtonDown(1)){
            switch(this.display){
                case "flagged":
                    flagBomb(1, "normal", defaultColor);
                    break;
                default:
                    flagBomb(-1, "flagged", flagColor);
                    break;
            }
           
        }
    }

    // Reveals the blocks content
    private void reveal(GameObject _block){

        if(_block.GetComponent<BlockManager>().getIsBomb()){
            
            _block.GetComponent<MeshFilter>().sharedMesh = Resources.Load<MeshFilter>("Meshes/Bomba").sharedMesh;

            //game over when hitting a mine
            GameObject.Find("Generator").GetComponent<Minesweeper>().gameOver("Game over! You lost");

        }else if(_block.GetComponent<BlockManager>().getNeighbourCount()>0){

            _block.GetComponent<BlockManager>().setDisplay("revealed");
            _block.GetComponent<BlockManager>().setColor(grey);

            var _numberMesh = GameObject.Find("Generator").GetComponent<Minesweeper>().getNumberMesh();
            // creates a mesh (as a child of generator) that displays the # of neighbouring bombs 
            var numMesh = Instantiate(_numberMesh[_block.GetComponent<BlockManager>().getNeighbourCount()], parentObj.transform); 
            numMesh.transform.localPosition = _block.transform.localPosition;
            numMesh.AddComponent<RotateNumber>();
            numMesh.GetComponent<Renderer>().material.color = Color.red;
            Destroy(_block.GetComponent<BoxCollider>());
        }else{
            _block.GetComponent<BlockManager>().setDisplay("revealed");
            //recursively call reveal inside revealEmpty to reveal all empty adjacent blocks
            _block.GetComponent<BlockManager>().revealEmpty(_block.GetComponent<BlockManager>().getNeighbourBlocks());
            _block.SetActive(false); //removes useless blocks            
        }
    }
    
    // Reveals all empty neighbours/info blocks when clicking on an empty square 
    private void revealEmpty(List<GameObject> _neighbourBlocks){
        for(int i =0; i<_neighbourBlocks.Count;i++){
            if( _neighbourBlocks[i].GetComponent<BlockManager>().getDisplay() == "normal" &&
                (_neighbourBlocks[i].GetComponent<BlockManager>().getState() == "info" || 
                _neighbourBlocks[i].GetComponent<BlockManager>().getState() == "default")) 
                {
                    reveal(_neighbourBlocks[i]);
            }
        }
    }
    
    // Reveals all for game over
    public void revealAll(){
        if(this.display != "revealed"){
            if(this.GetComponent<BlockManager>().getIsBomb()){
                this.GetComponent<MeshFilter>().sharedMesh = Resources.Load<MeshFilter>("Meshes/Bomba").sharedMesh;
            }else if(this.GetComponent<BlockManager>().getNeighbourCount()>0){
                this.GetComponent<BlockManager>().setColor(grey);
                var _numberMesh = GameObject.Find("Generator").GetComponent<Minesweeper>().getNumberMesh();
                // creates a mesh (as a child of generator) that displays the # of neighbouring bombs 
                var numMesh = Instantiate(_numberMesh[this.getNeighbourCount()], parentObj.transform); 
                numMesh.transform.localPosition = this.transform.localPosition;
                numMesh.AddComponent<RotateNumber>();
                numMesh.GetComponent<Renderer>().material.color = Color.red;

                Destroy(this.GetComponent<BoxCollider>());
            }else{
                Destroy(gameObject); 
            }
        }
        
    }   

    // Int String Color
    // Flags the block as a bomb or unflags it and updates its state
    private void flagBomb(int _bombCount, string _newDisplay, Color _setColor){
        this.setDisplay(_newDisplay);
        this.setColor(_setColor);
        //reduces or adds bomb count by 1
        GameObject.Find("Generator").GetComponent<Minesweeper>().setBombCount(_bombCount);

        // using the negative of bombcount as the parameters as it provides the appropriate value 
        if(this.isBomb){
            GameObject.Find("Generator").GetComponent<Minesweeper>().setGoodGuess(-_bombCount);
        }
    }

    // GETTER SETTER
    public string getState() {
        return this.state;
    }

    public string getDisplay(){
        return this.display;
    }

    public bool getIsBomb(){
        return this.isBomb;
    }

    public int getNeighbourCount(){
        return this.neighbourCount;
    }

    public List<GameObject> getNeighbourBlocks(){
        return this.neighbourBlocks;
    }
    public bool getBlockType(){
        return this.isBomb;
    }

    public void setDisplay(string _newDisplay){
        this.display = _newDisplay;
    }

    


}

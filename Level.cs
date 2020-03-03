using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


public class Level : MonoBehaviour
{

    public enum SPRITE
    {
        ONEPLAY = 1,
        TWOPLAY,
        THREEPLAY,
        FOURPLAY,
        FIVEPLAY,
        SIXPLAY,
        SEVENPLAY,
        EIGHTPLAY,
        ONEWON,
        TWOWON,
        THREEWON,
        FOURWON,
        FIVEWON,
        SIXWON,
        SEVENWON,
        EIGHTWON,
        ONELOST,
        TWOLOST,
        THREELOST,
        FOURLOST,
        FIVELOST,
        SIXLOST,
        SEVENLOST,
        EIGHTLOST,
        REVEALED,
        REVEALEDLOST,
        REVEALEDWON,
        HIDDEN,
        QUESTION,
        FLAG,
        MINELOST,
        MINEWON,
    };

    //block prefab
    public GameObject blockPreFab;
    //block scale
    private float scale = .3f;
    //gap between blocks
    private float gap = .15f;
    //offset from center
    private float leftOffset, topOffset;
    //animations delay
    private float animDelay = 0.1f;

    //sprites
    [HideInInspector]
    public Dictionary<SPRITE, Sprite> sprites;


    //level number
    private int levelNum;
    //rows an cols
    private int rows, cols;

    //number of mines in the level
    private int numMines;
    //number of blocks revealed
    private int numBlocksRevealed;
    //total number of blocks
    private int numBlocksMax;



    //block matrix
    public Block[,] allBlocks;

    public static Level instance = null;

    void MakeSingleton()
    {
        //Debug.Log("level instance:");
        if (instance != null)
        {
            DestroyImmediate(gameObject);
        }
        else
        {
            instance = this;
            //DontDestroyOnLoad (gameObject);
        }
    }

    void Awake()
    {
        Debug.Log("level awake:");
        MakeSingleton();
    }

    // Use this for initialization
    void Start()
    {
        Debug.Log("level start");

        levelNum = Controller.instance.LevelNum;
        numMines = Controller.instance.NumMines;
        Debug.Log("level: " + levelNum + " num mines: " + numMines);
        sprites = Controller.instance.sprites;
        setLevel();
        generateBlocks();
        //Controller.instance.Level = this;
        Controller.instance.GameIsPlaying = true;

    }

    //game play++++++++++++++++++++++++++++++++++++++

  
    //left mouse click on a block 
    public void processLeftClick(Block selectBlock)
    {

        if (numBlocksRevealed == 0)
        {
            //first click: set mines and reveal block
            generateMines(selectBlock);
            initMineCount();
        }

        //check if block is playable
        if (selectBlock.IsPlayable)
        {
            if (selectBlock.HasMine)
            {
                Debug.Log("block has mine");
                //reveal block
                //selectBlock.revealMineCount();
                //game lost procedure
                StartCoroutine(endGame());

            }
            else if (selectBlock.MineCount > 0)
            {
                //has mines around it
                selectBlock.revealMineCount();
                numBlocksRevealed++;
                //set block as not playable
                selectBlock.IsPlayable = false;

            }
            else
            {
                // Debug.Log("block has no mines around it");
                //reveal block
               StartCoroutine(revealContigousBlocks(selectBlock));
            }//check mine

        }
    }

    private IEnumerator revealContigousBlocks(Block selectedBlock)
    {

        Stack<Block> stack = new Stack<Block>();
        //push the selected block onto the stack to start the process
        stack.Push(selectedBlock);
        //iterate until stack is empty
        int failSafe = 0;
        while (stack.Count > 0 && failSafe < 500)
        {
            //Debug.Log("stack: " + stack.Count);
            //pop the block off the top of the stack
            Block block = stack.Pop();
            // Debug.Log("pop block: " + block);
            //reveal block mine count
            block.revealMineCount();
            //set block as not playable
            block.IsPlayable = false;
            //update number of blocks reavealed
            numBlocksRevealed++;
            if (block.MineCount == 0)
            {
                //get all the blocks around the given block that are in play and not mines
                List<Block> contigiouusBlocks = getContigiousBlocks(block.Row, block.Col);
                // Debug.Log("contigious blocks: " + contigiouusBlocks.Count);
                //place all surrounding blocks on to the stack, only if not already on the stack, and not mines
                addBlockstoStack(stack, contigiouusBlocks);
                //showStack(stack);
            }
            //Debug.Log("num max/revealed: " + numBlocksMax + " " + numBlocksRevealed);
            failSafe++;
            yield return animDelay;
        }//while


    }//revealContigousBlocks


    //add all contigious blocks with no contigious mines to the stack, if not already on the stack
    //stack and contigious blocks are not null
    private void addBlockstoStack(Stack<Block> stack, List<Block> contigiousBlocks)
    {
        for (int i = 0; i < contigiousBlocks.Count; i++)
        {
            //Debug.Log("check add to stack: " + contigiousBlocks[i]);
            if (!contigiousBlocks[i].HasMine && !stack.Contains(contigiousBlocks[i]))
            {
                //Debug.Log("add block to stack: " + contigiousBlocks[i]);
                stack.Push(contigiousBlocks[i]);
            }
        }
    }

    private IEnumerator endGame()
    {
        //halt all game play operations
        Controller.instance.GameIsPlaying = false;
        
        //reveal all blocks
        for (int y = 0; y < rows; y++)
        {
            //yield return new WaitForSeconds(.5f);
            for (int x = 0; x < cols; x++)
            {
                Debug.Log("reveal block: " + x + " " + y);
                Block block = allBlocks[y, x];
                block.revealMineCount();
                yield return new WaitForSeconds(animDelay);
            }
        }

    }

    //level set up+++++++++++++++++++++++++++++++++++++++++++++++++++++++++


    private void generateBlocks()
    {
        Debug.Log("generate blocks");
        GameObject blockHolder = new GameObject("blocks");
        GameObject go;
        allBlocks = new Block[rows, cols];

        float xpos, ypos, zpos = 89;

        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < cols; c++)
            {
                xpos = -leftOffset + c * (scale + gap);
                ypos = -topOffset + r * (scale + gap);
                go = Instantiate(blockPreFab, new Vector3(xpos, ypos, zpos), Quaternion.identity) as GameObject;
                go.transform.parent = blockHolder.transform;
                go.transform.localScale = new Vector3(scale, scale, scale);
                //update block script data
                Block block = go.GetComponent<Block>();

                // block.SpriteRenderer.sprite = sprites[SPRITE.HIDDEN];
                block.Row = r;
                block.Col = c;

                //add cube cut matrix
                allBlocks[r, c] = block;
            }
        }
    }



    //set mine count for all blocks
    private void initMineCount()
    {
        //iterate through all blocks
        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < cols; c++)
            {   //get all contigious blocks
                List<Block> blocks = getContigiousBlocks(r, c);
                allBlocks[r, c].MineCount = calcMines(blocks);
                //allBlocks[r, c].revealMineCount();
            }
        }
    }//mine count


    //get all contigious blocks around given row and col
    //-playable

    //returned list can be empty, but not null
    private List<Block> getContigiousBlocks(int row, int col)
    {
        List<Block> blocks = new List<Block>();

        for (int r = row - 1; r <= row + 1; r++)
        {
            for (int c = col - 1; c <= col + 1; c++)
            {
                //avoid adding given position
                if (r != row || c != col)
                {
                    //check if in bounds, playable
                    if (inBounds(r, c) && allBlocks[r, c].IsPlayable)
                    {
                        //Debug.Log("add block: " + r + " " + c);
                        blocks.Add(allBlocks[r, c]);
                    }
                }
            }
        }
        return blocks;
    }//get contigious 

    //count all mines on contigious blocks
    private int calcMines(List<Block> contigiousBlocks)
    {
        int counter = 0;
        for (int i = 0; i < contigiousBlocks.Count; i++)
        {
            //check if in bounds
            if (contigiousBlocks[i].HasMine)
            {
                counter++;
            }
        }
        return counter;
    }//calc mines



    //return true if r, c are in bounds
    private bool inBounds(int r, int c)
    {
        return r >= 0 && r < rows && c >= 0 && c < cols;
    }


    //generate mines, no mine on selected block
    public void generateMines(Block selectBlock)
    {

        int index, row, col;
        int counter = 0;
        Block block;
        int failsafe = 0;
        while (counter < numMines && failsafe < 100)
        {
            //generate random number from 0 to num bocks
            index = UnityEngine.Random.Range(0, rows * cols);
            //calcuate the row and col
            row = index / cols;
            col = index % cols;
            //get block in matrix
            block = allBlocks[row, col];
            if (!block.HasMine && (selectBlock.Row != row || selectBlock.Col != col))
            {
                //set mine
                block.HasMine = true;
                block.SpriteRenderer.sprite = sprites[SPRITE.MINEWON];
                counter++;
                Debug.Log(counter + " mine set:" + row + " " + col);
            }
            failsafe++;
        }//while

    }
    private void setLevel()
    {

        switch (levelNum)
        {

            case 1:
                {
                    rows = 10;
                    cols = 10;

                }
                break;

            case 2:
                {
                    rows = 15;
                    cols = 15;

                }
                break;

            case 3:
                {
                    rows = 20;
                    cols = 20;
                }
                break;
        }

        //offsets from center
        leftOffset = cols * (scale + gap) / 2f - scale;
        topOffset = rows * (scale + gap) / 2f;
        numBlocksMax = rows * cols - numMines;
        numBlocksRevealed = 0;

    }


}//class


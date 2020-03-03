using UnityEngine;
using System.Collections;

public class Block : MonoBehaviour
{

    //position in matrix
    private int row, col;
    //mine on this block?
    private bool hasMine;
    //number of mines around this block
    private int mineCount;
    //is this block still playable?
    private bool isPlayable;


    //sprite state: hidden, question, mark
   // private Level.SPRITE blockState

    private SpriteRenderer spriteRenderer;


  
    // Use this for initialization
    void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        isPlayable = true;
        mineCount = 0;
        hasMine = false;
       // blockState = Level.SPRITE.HIDDEN;

        //spriteRenderer.sprite = Level.instance.sprites[blockState];
    }


    
    //reveal mines count
    public void revealMineCount()
    {
        if (mineCount == 0)
        {
            spriteRenderer.sprite = Level.instance.sprites[Level.SPRITE.REVEALED];
        }
        else if (!hasMine)
        {
            Level.SPRITE spriteType = (Level.SPRITE)mineCount;
            spriteRenderer.sprite = Level.instance.sprites[spriteType];
        }
    }

    //getters and setter

    //game object in play
    public bool IsPlayable
    {
        get { return isPlayable; }
        set
        {
            isPlayable = value;
        }
    }

    public bool HasMine
    {
        get { return hasMine; }
        set { hasMine = value; }
    }

    public int Row
    {
        get { return row; }
        set { row = value; }
    }

    public int Col
    {
        get { return col; }
        set { col = value; }
    }



    public int MineCount
    {
        get { return mineCount; }
        set
        {
            mineCount = value;
        }
    }


    public SpriteRenderer SpriteRenderer
    {
        get { return spriteRenderer; }
        set { spriteRenderer = value; }
    }

    /*
    public Level.SPRITE BlockState
    {
        get { return blockState; }
        set
        {
            blockState = value;
            //spriteRenderer.sprite = Level.instance.sprites[blockState];
        }
    }
    */

    public override string ToString()
    {
        return row + " " + col + " " + hasMine + " " + MineCount;
    }
}

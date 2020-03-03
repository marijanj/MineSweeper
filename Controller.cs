using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class Controller : MonoBehaviour
{

    public enum GAMESTATE
    {
        WON, LOST, PLAY
    }

    private bool gameIsPlaying;
    private int levelNum;
    private int numMines;

    //sprites
    [HideInInspector]
    public Dictionary<Level.SPRITE, Sprite> sprites;


    public static Controller instance = null;

    void MakeSingleton()
    {
        Debug.Log("controller instance:");
        if (instance != null)
        {
            DestroyImmediate(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    void Awake()
    {
        Debug.Log("controller awake:");
        MakeSingleton();
    }


    // Use this for initialization
    void Start()
    {
        Debug.Log("controller start:");
        setSprites();
        InitializeStart();

    }

    public void InitializeStart()
    {
        gameIsPlaying = false;
        levelNum = 1;
        numMines = 10;
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            Controller.instance.InitializeStart();
            if (SceneManager.GetActiveScene().name == "Game Scene")
            {
                SceneManager.LoadScene("Menu Scene", LoadSceneMode.Single);
            }
        }

        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (gameIsPlaying)
        {
            if (Input.GetMouseButtonUp(0))
            {
                Debug.Log("left click ");
                RaycastHit2D hit1 = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
                if (hit1.collider != null)
                {
                    Block block1 = hit1.collider.gameObject.GetComponent<Block>();
                    if (block1 != null)
                    {
                        Level.instance.processLeftClick(block1);
                    }
                }
            }
            else if (Input.GetMouseButtonUp(1))
            {
                Debug.Log("right click: ");
            }//button clicked
        }//game is playing
    }//update




    private void setSprites()
    {
        sprites = new Dictionary<Level.SPRITE, Sprite>();

        Sprite s;

        //numbered play sprites

        s = Resources.Load<Sprite>("oneplay");
        sprites.Add(Level.SPRITE.ONEPLAY, s);

        s = Resources.Load<Sprite>("twoplay");
        sprites.Add(Level.SPRITE.TWOPLAY, s);

        s = Resources.Load<Sprite>("threeplay");
        sprites.Add(Level.SPRITE.THREEPLAY, s);

        s = Resources.Load<Sprite>("fourplay");
        sprites.Add(Level.SPRITE.FOURPLAY, s);

        s = Resources.Load<Sprite>("fiveplay");
        sprites.Add(Level.SPRITE.FIVEPLAY, s);

        s = Resources.Load<Sprite>("sixplay");
        sprites.Add(Level.SPRITE.SIXPLAY, s);

        s = Resources.Load<Sprite>("sevenplay");
        sprites.Add(Level.SPRITE.SEVENPLAY, s);

        s = Resources.Load<Sprite>("eightplay");
        sprites.Add(Level.SPRITE.EIGHTPLAY, s);

        //numbered win sprites

        s = Resources.Load<Sprite>("onewon");
        sprites.Add(Level.SPRITE.ONEWON, s);

        s = Resources.Load<Sprite>("twowon");
        sprites.Add(Level.SPRITE.TWOWON, s);

        s = Resources.Load<Sprite>("threewon");
        sprites.Add(Level.SPRITE.THREEWON, s);

        s = Resources.Load<Sprite>("fourwon");
        sprites.Add(Level.SPRITE.FOURWON, s);

        s = Resources.Load<Sprite>("fivewon");
        sprites.Add(Level.SPRITE.FIVEWON, s);

        s = Resources.Load<Sprite>("sixwon");
        sprites.Add(Level.SPRITE.SIXWON, s);

        s = Resources.Load<Sprite>("sevenwon");
        sprites.Add(Level.SPRITE.SEVENWON, s);

        s = Resources.Load<Sprite>("eightwon");
        sprites.Add(Level.SPRITE.EIGHTWON, s);

        //numbered lost sprites

        s = Resources.Load<Sprite>("onelost");
        sprites.Add(Level.SPRITE.ONELOST, s);

        s = Resources.Load<Sprite>("twolost");
        sprites.Add(Level.SPRITE.TWOLOST, s);

        s = Resources.Load<Sprite>("threelost");
        sprites.Add(Level.SPRITE.THREELOST, s);

        s = Resources.Load<Sprite>("fourlost");
        sprites.Add(Level.SPRITE.FOURLOST, s);

        s = Resources.Load<Sprite>("fivelost");
        sprites.Add(Level.SPRITE.FIVELOST, s);

        s = Resources.Load<Sprite>("sixlost");
        sprites.Add(Level.SPRITE.SIXLOST, s);

        s = Resources.Load<Sprite>("sevenlost");
        sprites.Add(Level.SPRITE.SEVENLOST, s);

        s = Resources.Load<Sprite>("eightlost");
        sprites.Add(Level.SPRITE.EIGHTLOST, s);

        //rest of images

        s = Resources.Load<Sprite>("hidden");
        sprites.Add(Level.SPRITE.HIDDEN, s);

        s = Resources.Load<Sprite>("minelost");
        sprites.Add(Level.SPRITE.MINELOST, s);

        s = Resources.Load<Sprite>("minewon");
        sprites.Add(Level.SPRITE.MINEWON, s);

        s = Resources.Load<Sprite>("question");
        sprites.Add(Level.SPRITE.QUESTION, s);

        s = Resources.Load<Sprite>("flag");
        sprites.Add(Level.SPRITE.FLAG, s);

        s = Resources.Load<Sprite>("revealedlost");
        sprites.Add(Level.SPRITE.REVEALEDLOST, s);

        s = Resources.Load<Sprite>("revealedwon");
        sprites.Add(Level.SPRITE.REVEALEDWON, s);

        s = Resources.Load<Sprite>("revealed");
        sprites.Add(Level.SPRITE.REVEALED, s);
    }


    //getters and setters

    public int LevelNum
    {
        get { return levelNum; }
        set { levelNum = value; }
    }

    public int NumMines
    {
        get { return numMines; }
        set { numMines = value; }
    }


    public bool GameIsPlaying
    {
        get { return gameIsPlaying; }
        set { gameIsPlaying = value; }
    }


}//class

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelHandler : MonoBehaviour
{

    public Text numMinesText;
    public Slider minesSlider;

    public void SelectLevel(int level)
    {
        Debug.Log("level: " + level);
        Controller.instance.LevelNum = level;
        SceneManager.LoadScene("Game Scene", LoadSceneMode.Single);
    }


    public void MinesSlider()
    {
        Debug.Log("mine slider");
        //get slider value
        int numMines = (int)minesSlider.value;
        //update the text
        numMinesText.text = "mines: " + numMines.ToString();
        //update controller
        Controller.instance.NumMines = numMines;
    }


}

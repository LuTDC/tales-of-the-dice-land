using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Slider[] sliders = new Slider[12];

    [SerializeField]
    private Character[] characters = new Character[12];

    [SerializeField]
    private GameObject victoryScreen;
    [SerializeField]
    private TextMeshProUGUI victoryText;

    [SerializeField]
    private GameObject chooseScreen;

    [SerializeField]
    private Animator diceAnimator;
    [SerializeField]
    private GameObject dice;
    [SerializeField]
    private GameObject diceBackground;

    // Start is called before the first frame update
    void Start()
    {
        dice.transform.localScale = new Vector3(1, 1, 1);
        dice.transform.position = new Vector3(0, 4.3f, dice.transform.position.z);
    }

    public void PassTurnButton(){
        FindObjectOfType<Manager>().PassTurn();
    }

    public void SetSliderValue(int i, int hp){
        sliders[i].value = hp;
    }

    public Character[] GetCharacters(){
        return characters;
    }

    public void EndGame(int player){
        victoryScreen.SetActive(true);
        victoryText.text = "PLAYER " + player + " WINS!";

        FindObjectOfType<AudioManager>().Play("Victory");
        FindObjectOfType<Manager>().isPaused = true;
    }

    public void ReturnToMenu(){
        Destroy(FindObjectOfType<BGMusic>());
        SceneManager.LoadScene("Menu");
    }

    public void RollDice(){
        diceAnimator.SetInteger("diceValue", 0);
        diceAnimator.SetTrigger("roll");

        dice.transform.localScale = new Vector3(5, 5, 1);
        dice.transform.position = new Vector3(0, 0, dice.transform.position.z);

        diceBackground.SetActive(true);

        FindObjectOfType<Manager>().isPaused = true;
    }

    public void StopDice(int value){
        diceAnimator.SetInteger("diceValue", value);

        dice.transform.localScale = new Vector3(1, 1, 1);
        dice.transform.position = new Vector3(0, 4.3f, dice.transform.position.z);

        diceBackground.SetActive(false);

        FindObjectOfType<Manager>().isPaused = false;
    }

    public void ShowChooseScreen(){
        chooseScreen.SetActive(true);
        FindObjectOfType<Manager>().isPaused = true;
        StartCoroutine(HideChooseScreen());
    }

    private IEnumerator HideChooseScreen(){
        yield return new WaitForSeconds(3);

        chooseScreen.SetActive(false);
        FindObjectOfType<Manager>().isPaused = false;
    }
}

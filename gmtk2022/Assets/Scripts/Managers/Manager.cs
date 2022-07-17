using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    [SerializeField]
    private Character[] characters = new Character[12];

    private int currentPlayer = 0;
    private Character currentCharacter;

    private int P1ActiveCharacters = 6, P2ActiveCharacters = 6;

    private bool isFinished = false;
    public bool isPaused = false;

    private int p1last = -1, p1lalast = -1, p2last = -1, p2lalast = -1;

    public void SetOrders(){
        int[] charactersIndex = BubbleSort();

        int j = 2;

        for(int i = 11; i >= 0; i--){
            characters[charactersIndex[i]].ChangeOrder(j);
            j++;
        }
    }

    private int[] BubbleSort(){
        int[] charactersIndex = {0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12};

        for(int i = 0; i < 11; i++) 
            for(int j = 0; j < 11 - i; j++) 
                if(characters[j].transform.position.y > characters[j+1].transform.position.y){ 
                    int temp = charactersIndex[j]; 
                    charactersIndex[j] = charactersIndex[j + 1]; 
                    charactersIndex[j + 1] = temp; 
                }

        return charactersIndex;
    }

    public void ChangePlayer(){
        if(!isFinished){
            if(currentPlayer == 0) currentPlayer = 1;
            else currentPlayer = 0;

            SelectCharacter();
        }
    }

    public void SelectCharacter(){
        if(currentCharacter != null) currentCharacter.gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
        int r = Random.Range(6*currentPlayer, 6+6*currentPlayer);

        while(!characters[r].GetActive()){
            r = Random.Range(6*currentPlayer, 6+6*currentPlayer);
        }

        currentCharacter = characters[r];

        FindObjectOfType<UIManager>().RollDice();
        FindObjectOfType<AudioManager>().Play("Dice");
        StartCoroutine(DiceRoutine(r+1));
    }

    private IEnumerator DiceRoutine(int value){
        bool canChoose = false;

        if(currentPlayer == 0){
            if(value == p1last && value == p1lalast) canChoose = true;

            p1lalast = p1last;
            p1last = value;
        }
        else{
            value /= 2;
            if(value == p2last && value == p2lalast) canChoose = true;

            p2lalast = p2last;
            p2last = value;
        }
        yield return new WaitForSeconds(2);

        FindObjectOfType<UIManager>().StopDice(value);
        FindObjectOfType<AudioManager>().Stop("Dice");

        if(canChoose){
            FindObjectOfType<UIManager>().ShowChooseScreen();
            LightUpCharacters();
        }
        else{
            currentCharacter.gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.5f);

            currentCharacter.LightPossibleTiles();
            currentCharacter.LightPossibleCharacters();

            currentCharacter.BeChosen();
        }
    }

    private void LightUpCharacters(){
        for(int i = 6*currentPlayer; i < 6+6*currentPlayer; i++) characters[i].LightUp(Color.blue, 1);
    }

    public void ChooseCharacter(GameObject character){
        for(int i = 6*currentPlayer; i < 6+6*currentPlayer; i++) characters[i].PutOut();

        currentCharacter = character.GetComponent<Character>();
        currentCharacter.gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.5f);

        currentCharacter.LightPossibleTiles();
        currentCharacter.LightPossibleCharacters();

        currentCharacter.BeChosen();
    }

    public Character GetCurrentCharacter(){
        return currentCharacter;
    }

    public void ChangeCurrentCharacterPosition(GameObject tile){
        currentCharacter.ChangePosition(tile.GetComponent<Tile>());
    }

    public int GetCurrentPlayer(){
        return currentPlayer;
    }

    public Character[] GetCharacters(){
        return characters;
    }

    public bool IsOccupied(GameObject tile){
        for(int i = 0; i < 12; i++){
            if(characters[i].GetCurrentTile().gameObject == tile && characters[i].GetActive()) return true;
        }

        return false;
    }

    public void PassTurn(){
        currentCharacter.GetUnchosen();
    }

    public void LoseCharacter(){
        if(currentPlayer == 0) P2ActiveCharacters--;
        else P1ActiveCharacters--;

        if(P2ActiveCharacters == 0){
            FindObjectOfType<UIManager>().EndGame(1);
            isFinished = true;
        }
        else if(P1ActiveCharacters == 0){
            FindObjectOfType<UIManager>().EndGame(2);
            isFinished = true;
        }
    }

    public bool GetGameStatus(){
        return isFinished;
    }
}

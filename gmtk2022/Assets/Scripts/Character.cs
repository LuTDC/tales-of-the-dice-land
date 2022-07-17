using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Color currentColor;

    private Tile currentTile;

    private int hp = 10;
    private bool isActive = true;

    private int index;

    private bool isPossible = false;
    private bool canBeChosen = false;
    private bool isCurrent = false;
    private int nActions = 0;

    void Awake(){
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentColor = spriteRenderer.color;
    }

    public void SetIndex(int i){
        index = i;
    }

    public void ChangePositionInitial(Tile tile){
        transform.position = tile.GetPlayerPosition().position;
        currentTile = tile;

        FindObjectOfType<Manager>().SetOrders();
    }

    public void ChangePosition(Tile tile){
        transform.position = tile.GetPlayerPosition().position;
        currentTile = tile;

        FindObjectOfType<AudioManager>().Play("Grass");

        LightPossibleCharacters();
        UseAction();

        FindObjectOfType<Manager>().SetOrders();
    }

    public void ChangeOrder(int order){
        spriteRenderer.sortingOrder = order;
    }

    public void LightPossibleTiles(){
        currentTile.LightUpAdjacents(gameObject);
    }

    private List<Character> GetCharactersInRange(Character[] characters, int player){
        List<Character> charactersInRange = new List<Character>();
        int tileIndex = currentTile.transform.GetSiblingIndex();
        int parentTileIndex = currentTile.transform.parent.GetSiblingIndex();

        if(gameObject.tag == "Knight"){
            List<GameObject> possibleTiles = new List<GameObject>();

            if(tileIndex - 1 >= 0) possibleTiles.Add(currentTile.transform.parent.GetChild(tileIndex-1).gameObject);
            if(tileIndex + 1 <= 15) possibleTiles.Add(currentTile.transform.parent.GetChild(tileIndex+1).gameObject);
            if(parentTileIndex - 1 >= 0) possibleTiles.Add(currentTile.transform.parent.parent.GetChild(currentTile.transform.parent.GetSiblingIndex() - 1).GetChild(tileIndex).gameObject);
            if(parentTileIndex + 1 <= 7) possibleTiles.Add(currentTile.transform.parent.parent.GetChild(currentTile.transform.parent.GetSiblingIndex() + 1).GetChild(tileIndex).gameObject);

            for(int i = 6-6*player; i < 12-6*player; i++){
                for(int j = 0; j < possibleTiles.Count; j++){
                    if(characters[i].GetCurrentTile().gameObject == possibleTiles[j] && characters[i].GetActive()){
                        charactersInRange.Add(characters[i]);
                        break;
                    }
                }
            }
        }
        else if(gameObject.tag == "Mage"){
            List<GameObject> possibleTiles = new List<GameObject>();

            for(int i = 1; i <= 10; i++){
                if(tileIndex - 1*i >= 0) possibleTiles.Add(currentTile.transform.parent.GetChild(tileIndex-1*i).gameObject);
                if(tileIndex + 1*i <= 15) possibleTiles.Add(currentTile.transform.parent.GetChild(tileIndex+1*i).gameObject);
                if(parentTileIndex - 1*i >= 0) possibleTiles.Add(currentTile.transform.parent.parent.GetChild(parentTileIndex - 1*i).GetChild(tileIndex).gameObject);
                if(parentTileIndex + 1*i <= 7) possibleTiles.Add(currentTile.transform.parent.parent.GetChild(parentTileIndex + 1*i).GetChild(tileIndex).gameObject);
            }

            for(int i = 6-6*player; i < 12-6*player; i++){
                for(int j = 0; j < possibleTiles.Count; j++){
                    if(characters[i].GetCurrentTile().gameObject == possibleTiles[j] && characters[i].GetActive()){
                        charactersInRange.Add(characters[i]);
                        break;
                    }
                }
            }
        }

        return charactersInRange;
    }

    public void LightPossibleCharacters(){
        int player = FindObjectOfType<Manager>().GetCurrentPlayer();
        Character[] characters = FindObjectOfType<Manager>().GetCharacters();

        if(gameObject.tag == "Knight" || gameObject.tag == "Mage"){
            List<Character> charactersInRange = GetCharactersInRange(characters, player);

            for(int i = 0; i < charactersInRange.Count; i++) charactersInRange[i].LightUp(Color.red, 0);
        }
        else for(int i = 6*player; i < 6+6*player; i++){
            if(characters[i]. GetActive()) characters[i].LightUp(Color.green, 0);
        }
    }

    public void PutOutPossibleCharacters(){
        int player = FindObjectOfType<Manager>().GetCurrentPlayer();
        Character[] characters = FindObjectOfType<Manager>().GetCharacters();

        if(gameObject.tag == "Knight" || gameObject.tag == "Mage"){
            List<Character> charactersInRange = GetCharactersInRange(characters, player);

            for(int i = 0; i < charactersInRange.Count; i++) charactersInRange[i].PutOut();
        }
        else for(int i = 6*player; i < 6+6*player; i++){
            if(characters[i]. GetActive()) characters[i].PutOut();
        }
    }

    public void LightUp(Color newColor, int type){
        spriteRenderer.color = newColor;
        currentColor = spriteRenderer.color;

        if(type == 0) isPossible = true;
        else canBeChosen = true;
    }

    public void PutOut(){
        spriteRenderer.color = new Color(1, 1, 1, 1);
        currentColor = spriteRenderer.color;
        isPossible = false;
        canBeChosen = false;
    }

    private void OnMouseEnter(){
        if((isPossible || canBeChosen) && !FindObjectOfType<Manager>().isPaused) spriteRenderer.color = new Color(1, 1, 1, 0.5f);
    }

    private void OnMouseExit(){
        if((isPossible || canBeChosen) && !FindObjectOfType<Manager>().isPaused) spriteRenderer.color = currentColor;
    }

    private void OnMouseDown(){
        if(isPossible && !FindObjectOfType<Manager>().isPaused) ReceiveAction();
        else if(canBeChosen && !FindObjectOfType<Manager>().isPaused) GetChosen();
    }

    public Tile GetCurrentTile(){
        return currentTile;
    }

    private void ReceiveAction(){
        Character currentCharacter = FindObjectOfType<Manager>().GetCurrentCharacter();

        if(currentCharacter.gameObject.tag == "Knight"){
            hp -= 3;
            FindObjectOfType<AudioManager>().Play("Sword");
        }
        else if(currentCharacter.gameObject.tag == "Mage"){
            hp -= 4;
            FindObjectOfType<AudioManager>().Play("Magic");
        }
        else{
            hp += 1;
            FindObjectOfType<AudioManager>().Play("Heal");
        }

        if(hp < 0) hp = 0;
        if(hp > 10) hp = 10;

        if(hp == 0) Die();

        FindObjectOfType<UIManager>().SetSliderValue(index, hp);

        currentCharacter.PutOutPossibleCharacters();
        currentCharacter.UseAction();
    }

    public void GetChosen(){
        FindObjectOfType<Manager>().ChooseCharacter(gameObject);
    }

    public void BeChosen(){
        isCurrent = true;

        if(gameObject.tag == "Knight" || gameObject.tag == "Mage") nActions = 2;
        else nActions = 1;
    }

    public void UseAction(){
        nActions -= 1;

        if(nActions == 0) GetUnchosen();
    }

    public void GetUnchosen(){
        isCurrent = false;
        nActions = 0;

        PutOutPossibleCharacters();
        currentTile.PutOutAdjacents(gameObject);

        FindObjectOfType<Manager>().ChangePlayer();
    }

    private void Die(){
        if(isActive){
            isActive = false;

            Character currentCharacter = FindObjectOfType<Manager>().GetCurrentCharacter();

            if(currentCharacter.gameObject.tag == "Knight" || currentCharacter.gameObject.tag == "Mage") currentCharacter.LightPossibleTiles();

            spriteRenderer.enabled = false;
            GetComponent<Collider>().enabled = false;

            FindObjectOfType<Manager>().LoseCharacter();
        }
    }

    public bool GetActive(){
        return isActive;
    }

    public int GetHP(){
        return hp;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Transform playerPosition;

    private Color currentColor;

    private bool isPossible = false;

    void Awake(){
        playerPosition = transform.GetChild(0).transform;

        spriteRenderer = GetComponent<SpriteRenderer>();
        currentColor = spriteRenderer.color;
    }

    public Transform GetPlayerPosition(){
        return playerPosition;
    }

    private void OnMouseEnter(){
        if(isPossible && !FindObjectOfType<Manager>().isPaused) spriteRenderer.color = new Color(1, 1, 1, 0.5f);
    }

    private void OnMouseExit(){
        if(isPossible && !FindObjectOfType<Manager>().isPaused) spriteRenderer.color = currentColor;
    }

    private void OnMouseDown(){
        if(isPossible && !FindObjectOfType<Manager>().isPaused){
            Manager manager = FindObjectOfType<Manager>();
            manager.GetCurrentCharacter().GetCurrentTile().PutOutAdjacents(manager.GetCurrentCharacter().gameObject);
            manager.ChangeCurrentCharacterPosition(gameObject);
        }
    }

    public void LightUpAdjacents(GameObject character){
        int index = transform.GetSiblingIndex();
        int parentIndex = transform.parent.GetSiblingIndex();

        if(character.tag == "Mage"){
            if(parentIndex - 1 >= 0) transform.parent.parent.GetChild(parentIndex-1).GetChild(index).gameObject.GetComponent<Tile>().LightUp();
            if(index - 1 >= 0) transform.parent.GetChild(index-1).gameObject.GetComponent<Tile>().LightUp();
            if(index + 1 <= 15) transform.parent.GetChild(index+1).gameObject.GetComponent<Tile>().LightUp();
            if(parentIndex + 1 <= 7) transform.parent.parent.GetChild(parentIndex+1).GetChild(index).gameObject.GetComponent<Tile>().LightUp();
        }
        if(character.tag == "Knight"){
            for(int i = 1; i <= 5; i++){
                if(parentIndex - 1*i >= 0) transform.parent.parent.GetChild(parentIndex-1*i).GetChild(index).gameObject.GetComponent<Tile>().LightUp();
                if(index - 1*i >= 0) transform.parent.GetChild(index-1*i).gameObject.GetComponent<Tile>().LightUp();
                if(index + 1*i <= 15) transform.parent.GetChild(index+1*i).gameObject.GetComponent<Tile>().LightUp();
                if(parentIndex + 1*i <= 7) transform.parent.parent.GetChild(parentIndex+1*i).GetChild(index).gameObject.GetComponent<Tile>().LightUp();
            }
        }
    }

    public void PutOutAdjacents(GameObject character){
        int index = transform.GetSiblingIndex();
        int parentIndex = transform.parent.GetSiblingIndex();

        if(character.tag == "Mage"){
            if(parentIndex - 1 >= 0) transform.parent.parent.GetChild(parentIndex-1).GetChild(index).gameObject.GetComponent<Tile>().PutOut();
            if(index - 1 >= 0) transform.parent.GetChild(index-1).gameObject.GetComponent<Tile>().PutOut();
            if(index + 1 <= 15) transform.parent.GetChild(index+1).gameObject.GetComponent<Tile>().PutOut();
            if(parentIndex + 1 <= 7) transform.parent.parent.GetChild(parentIndex+1).GetChild(index).gameObject.GetComponent<Tile>().PutOut();
        }
        if(character.tag == "Knight"){
            for(int i = 1; i <= 5; i++){
                if(parentIndex - 1*i >= 0) transform.parent.parent.GetChild(parentIndex-1*i).GetChild(index).gameObject.GetComponent<Tile>().PutOut();
                if(index - 1*i >= 0) transform.parent.GetChild(index-1*i).gameObject.GetComponent<Tile>().PutOut();
                if(index + 1*i <= 15) transform.parent.GetChild(index+1*i).gameObject.GetComponent<Tile>().PutOut();
                if(parentIndex + 1*i <= 7) transform.parent.parent.GetChild(parentIndex+1*i).GetChild(index).gameObject.GetComponent<Tile>().PutOut();
            }
        }
    }

    public void LightUp(){
        if(!FindObjectOfType<Manager>().IsOccupied(gameObject)){

            spriteRenderer.color = new Color(0, 1, 0, 1);
            currentColor = spriteRenderer.color;

            isPossible = true;
        }
    }

    public void PutOut(){
        spriteRenderer.color = new Color(1, 1, 1, 1);
        currentColor = spriteRenderer.color;

        isPossible = false;
    }
}

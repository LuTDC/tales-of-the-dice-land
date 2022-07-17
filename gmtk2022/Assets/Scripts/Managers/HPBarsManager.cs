using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPBarsManager : MonoBehaviour
{
    [SerializeField]
    private GameObject P1Bars, P2Bars;

    void Update()
    {
        if(!FindObjectOfType<Manager>().GetGameStatus()){
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if(mousePosition.x <= -2.95f && mousePosition.y >= 1.5f) HideP1Bars();
            else ShowP1Bars();

            if(mousePosition.x >= 2.95f && mousePosition.y >= 1.5f) HideP2Bars();
            else ShowP2Bars();
        }
    }

    private void HideP1Bars(){
        P1Bars.SetActive(false);
    }

    private void HideP2Bars(){
        P2Bars.SetActive(false);
    }

    private void ShowP1Bars(){
        P1Bars.SetActive(true);

        UIManager manager = FindObjectOfType<UIManager>();

        for(int i = 0; i < 6; i++){
            manager.SetSliderValue(i, manager.GetCharacters()[i].GetHP());
        }
    }

    private void ShowP2Bars(){
        P2Bars.SetActive(true);

        UIManager manager = FindObjectOfType<UIManager>();

        for(int i = 6; i < 12; i++){
            manager.SetSliderValue(i, manager.GetCharacters()[i].GetHP());
        }
    }
}

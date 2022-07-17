using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitialSetup : MonoBehaviour
{
    [SerializeField]
    private Character[] characters = new Character[12];

    [SerializeField]
    private Tile[] initialTiles = new Tile[12];

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < 12; i++) characters[i].SetIndex(i);
        SetPositions();
    }

    private void SetPositions(){
        for(int i = 0; i < 12; i++) characters[i].ChangePositionInitial(initialTiles[i]);

        FindObjectOfType<Manager>().SelectCharacter();
    }
}

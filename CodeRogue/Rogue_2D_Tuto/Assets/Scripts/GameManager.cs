using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager instance = null; //For Singleton Class
    public BoardManager boardScript;
    public int playerFoodPoints = 100;
    [HideInInspector] public bool playersTurn = true;



    private int level = 3;

	// Use this for initialization
	void Awake () {

        if (instance == null)
            instance = this;
        else if (instance != null)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject); //Does not destroy between scenes

        boardScript = GetComponent<BoardManager>();
        InitGame();
		
	}

    void InitGame() {

        boardScript.SetupScene(level);
    
    }

    void GameOver() {

        enabled = false;

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}

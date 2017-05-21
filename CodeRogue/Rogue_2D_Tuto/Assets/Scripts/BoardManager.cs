using UnityEngine;
using System; //seriliasable attribute -> modify variables appear isnpector editor and show and hide
using System.Collections; 
using System.Collections.Generic; // to use lists
using Random = UnityEngine.Random;


public class BoardManager : MonoBehaviour {

    [Serializable]
    public class Count {

        public int minimum;
        public int maximum;

        public Count(int min, int max) {

            minimum = min;
            maximum = max;
        
        }
    
    }

    // Dimensions of the game board
    public int columns = 8;
    public int rows = 8;
    public Count wallCount = new Count(5, 9); //minimum of 5 walls per level and maximum of 9
    public Count foodCount = new Count(1, 5); //min food and max food per level

    // Varaible to hold the prefabs

    public GameObject exit; //only one exit object

    public GameObject[] floorTiles;
    public GameObject[] wallTiles;
    public GameObject[] foodTiles;
    public GameObject[] enemyTiles;
    public GameObject[] outerWallTiles;

    private Transform boardHolder; //hierarchy purposes (child objects to that)
    private List<Vector3> gridPositions = new List<Vector3>(); //Track possible positions on gameBoard (keep track of objects)

    void InitialiseList() {

        gridPositions.Clear(); //Clear list

        //Creates the possible space to put objects in the list (leaves a corridor so there is always a path to exit
        for (int x = 1; x < columns - 1; x++) {
            for (int y = 1; y < rows - 1; y++) {
                gridPositions.Add(new Vector3(x, y, 0f)); //0f because we are in 2D         
            }        
        }

    }

    void BoardSetup() { 
    
        boardHolder = new GameObject ("Board").transform;

        //Creates the outer wall
        for (int x = -1; x < columns + 1; x++)
        {
            for (int y = -1; y < rows + 1; y++)
            {
                GameObject toInstantiate = floorTiles[Random.Range(0, floorTiles.Length)]; //random tiles for the floor
                if ( x == -1 || x == columns || y == -1 || y == rows )
                    toInstantiate = outerWallTiles[Random.Range(0, outerWallTiles.Length)]; //If on the borders, choose form outerwalls

                GameObject instance = Instantiate(toInstantiate, new Vector3(x, y, 0f), Quaternion.identity) as GameObject; //Quaternion.identity == no rotation ?

                instance.transform.SetParent(boardHolder); //Parent will be boardHolder           
            }
        }

    }

    Vector3 RandomPosition() {

        int randomIndex = Random.Range(0, gridPositions.Count); //Random number within range (0 and numbers of positions)
        Vector3 randomPosition = gridPositions[randomIndex]; //Vector to gridPosition at randomely select index
        gridPositions.RemoveAt(randomIndex); //Remove grid from gridList so there is no 2 objects in the same tile
        return randomPosition;
    
    }

    void LayoutObjectAtRandom(GameObject[] tileArray, int minimum, int maximum) {

        int objectCount = Random.Range(minimum, maximum + 1); //How many of objects per level
        for (int i = 0; i < objectCount ; i++) {
            Vector3 randomPosition = RandomPosition(); //Choose randomPosition
            GameObject tileChoice = tileArray[Random.Range(0, tileArray.Length)]; //between 0 and number of tiles
            Instantiate(tileChoice, randomPosition, Quaternion.identity);
        }
    
    }

    public void SetupScene(int level) {

        BoardSetup();
        InitialiseList();
        LayoutObjectAtRandom(wallTiles, wallCount.minimum, wallCount.maximum);
        LayoutObjectAtRandom(foodTiles, foodCount.minimum, foodCount.maximum);
        int enemyCount = (int)Mathf.Log(level, 2f);
        LayoutObjectAtRandom(enemyTiles, enemyCount, enemyCount);
        Instantiate(exit, new Vector3(columns - 1, rows - 1, 0f), Quaternion.identity);
    
    }
}

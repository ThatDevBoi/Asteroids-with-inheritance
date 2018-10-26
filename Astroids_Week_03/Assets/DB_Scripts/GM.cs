using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GM : MonoBehaviour
{
    #region Variables 
    [SerializeField]
    private GameObject Bullet_Prefab;       // Links in the IDE 
    [SerializeField]
    private GameObject PC_ship_Prefab;      // Links in the IDE 
    [SerializeField]
    private GameObject UFO;                 // Links to the IDE
    [SerializeField]
    public GameObject[] RockPrefabs;       // Links in the IDE 
    public bool PC_dead = false;            // Boolean flag to declare if or if not the plyer is dead
    [SerializeField]
    private int PC_lives = 3;               // How many lives the player has 
    [SerializeField]
    private float respawn_PC_timer = 5;     // Timer to respawn the player
    public static int score;                // Destroying a rock UI Value (Score)
    [SerializeField]
    private Text player_score;
    #endregion


    #region SingleTone
    public static GM s_GM;      // Allows acess to singletone having this static allows to acess it without knowing instance

    void Awake()            // Runs before start function is called
    {
        if(s_GM == null)        // Has the GM been set up
        {
            s_GM = this;        // No, its the first time setting the GM up so set its instance
            DontDestroyOnLoad(gameObject);      // Dont Destroy the GM ever it survives through scenes
        }
        else if(s_GM != this)       // if this gets called again then keep old version and delete new one
        {
            Destroy(gameObject);        // Delete the new GM
        }
    }
    #endregion


    #region Start Function
    // Use this for initialization
    void Start ()
    {
        InitialiseGame();                               // Calls To spawn PC and Rocks Prefabs

        player_score = GameObject.FindGameObjectWithTag("scoreText").GetComponent<Text>();      // Find Text Component  
    }
    #endregion


    #region Update Function
    void Update()
    {
        if(PC_dead)                                     // If the Player is dead flagged by PC_dead boolean
        {
            if (PC_lives > 0)                           // and if the PC_lives is greater than 0
            {
                respawn_PC_timer -= Time.deltaTime;     // start the respawn timer 
                if(respawn_PC_timer <= 0)               // when the respawn timer is more than or equal to 0
                {
                    respawn_PC_timer = 5;               // reset the timer value so its not always going down
                    CreatePlayerShip();                 // call the player creation function
                    PC_lives--;
                    PC_dead = false;                    // Make sure PC_dead changes to false when player returns to the scene
                }
            }
            else                                        // However if everything declaring with if isnt true
                PC_dead = false;                        // The player never died 
            
        }

        //if(GameObject.FindGameObjectWithTag("Rock") == null)
        //{
        //    Reload_Scene();
        //}

        player_score.text = "Score:" + score.ToString();
    }
    #endregion


    #region CreateGame
    void InitialiseGame()                               // Sets up the game
    {
        CreatePlayerShip();                             // Create PC GameObject
        // Calculating how many rocks need to be spawned into the scene
        for(int Rock_Amount = 0; Rock_Amount < 3; Rock_Amount++)
        {
            CreateRock(0);                              // Call Creation of rocks function
        }
    }
    #endregion


    #region CreatePlayer
    public static void CreatePlayerShip()
    {
        Debug.Assert(s_GM.PC_ship_Prefab != null, "PC Ship prefab not linked in the IDE");      // Calls an error if Player Ship Prefa is not linked onto the GM 
        Instantiate(s_GM.PC_ship_Prefab, Vector3.zero, Quaternion.identity);        // Clones the player prefab in the scene    
    }
    #endregion


    #region Change Scene
    public void Reload_Scene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    #endregion


    #region Create BigRock
    public static void CreateRock(int vIndex)
    {
        Debug.Assert(s_GM.RockPrefabs != null, "RockPreaabs not linked in the IDE");        // Error appears if the Rock Prefab isnt linked
        if(vIndex < s_GM.RockPrefabs.Length)
        {
            Instantiate(s_GM.RockPrefabs[vIndex], RandomScreenPosition, Quaternion.identity);   // Spawns rock GameObject in a random position in the scene
        }
        else
        {
            Debug.LogFormat("RockPrefab Index {0} out of range", vIndex);
        } 
    }
    #endregion


    #region Create Meduim Rocks
    public static void Meduim_Rock_Spawn()
    {
        // Calculating how many rocks need to be spawned into the 
        for (int Rock_Amount = 0; Rock_Amount < 2; Rock_Amount++)
        {
            Instantiate(s_GM.RockPrefabs[1], RandomScreenPosition, Quaternion.identity);        // Spawns The Next Rock In The Array of prefab GameObjects
        }
    }
    #endregion


    #region Ceate UFO
    public static void CreaeUFO(int V_ufo_Index)
    {
        
    }

    #endregion


    #region Shooting Bullet
    public static void CreateBullet(Vector3 vPosition, Vector3 vDirection)
    {
        Debug.Assert(s_GM.Bullet_Prefab != null, "BulletPrefab isn't linked to the IDE");       // Shows error in the console that the Bullet prefab isnt attached to the GM
        Bullet tBullet = Instantiate(s_GM.Bullet_Prefab, vPosition, Quaternion.identity).GetComponent<Bullet>();        // Finding the bulet script
        Debug.Assert(tBullet != null, "Bullet Script Missing");         // Checks if the Bullet Script is missing from the Bullet GaneObject
        tBullet.FireBullet(vPosition, vDirection);      // Where the Bullet will fire the next position of the bullet
    }
    #endregion


    #region Scoring Points
    void ScorePoints(int AddPoints)
    {
        score += AddPoints;                 // Take Whatever Score is and add to it
    }


    #endregion


    #region Utilities
    public static Vector3 RandomScreenPosition
    {
        get
        {
            float tHeight = Camera.main.orthographicSize;           // Letting the computer figure out what the camera can see
            float tWidth = Camera.main.aspect * tHeight;            // Use aspect ratio to work out the width of  the screen
            return new Vector3(Random.Range(-tWidth, tWidth), Random.Range(-tHeight, tHeight), 0.0f);
        }
    }
    #endregion


    
}
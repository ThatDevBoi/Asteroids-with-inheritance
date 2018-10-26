using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Rock_Controller : Fake_Physics
{
    private float Rotation = 0.0f;
    public GameObject Explosion_Prefab;             // Reference to the explosion prefab
    public GameObject[] RockPrefabs;
    public string RockTag = "Rock";
    public int points;              // Int value of the Prefabs points
    public GameObject displayText_TextMesh_prefab;          // Reference to the text mesh prefab that'll display on screen when Object dies


    #region Start Function
    // Use this for initialization
    protected override void Start()
    {
        base.Start();      // Call the FakePhysics Start Function
        PC_Collider.isTrigger = false;      // Making sure the rocks arent triggers 
        mvelocity = new Vector3(Random.Range(-speed, speed), Random.Range(-speed, speed), 0);   // Lets computer decide which way this object will move
        Rotation = Random.Range(-Rotation_speed, Rotation_speed);   // Rotates the gameObject with a value rate 
	}
    #endregion


    #region Rock Inheritance Movement
    protected override void DoMove()
    {
        transform.Rotate(0, 0, Rotation * Time.deltaTime);      // Rotates the rock per frame
        transform.position += mvelocity * Time.deltaTime;       // Figures out the new position of the rock
    }
    #endregion


    #region Object Collision
    protected override void ObjectHit(Fake_Physics Other_Objects)
    {
        base.ObjectHit(Other_Objects);                  // Referencing the base Function from Fake Physics 
        PC_Collider.enabled = false;                    // Turns off collision

        // Destroys the Big Rock and Plays sound effect and particle element
        if (PC_Collider.enabled == false)                // If Collision is off
        {
            Destroy(gameObject);                        // Destroy this object   
            GameObject Particle_Element = Instantiate(Explosion_Prefab, transform.position, Quaternion.identity);        // Spawn Particle elements on Asteroid Death
            Destroy(Particle_Element, 3f);              // Destroys Explosion Prefab after 3 

            // Tell the GM to score some points
            GM.s_GM.SendMessage("ScorePoints", points);

            Flickering_TextMesh();

            foreach (GameObject GOrock in RockPrefabs)
            {
                Instantiate(GOrock, transform.position, Quaternion.identity);
            }
        }
    }
    #endregion


    void Flickering_TextMesh()
    {
        var Go = Instantiate(displayText_TextMesh_prefab, transform.position, Quaternion.identity);           // Spawn a clone of the Text Mesh prefab 
        Go.GetComponent<TextMesh>().text = points.ToString();               // Displays the Text of the prefabed TextMesh Clone to whatever the point value is of each asteroid
        Destroy(Go, 1.25f);                                                    // Destroys clone in 3 seconds
    }
}

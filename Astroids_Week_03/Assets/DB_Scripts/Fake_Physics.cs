using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Fake_Physics : MonoBehaviour
{
    #region IDE Components
    // Components
    protected SpriteRenderer PC_Sprite;       // Used to add a sprite renderer, without component added error will occur
    protected Rigidbody2D PC_RB;              // Rigidbody reference getting physics, but we only want sfe classes messing/using this
    protected Collider2D PC_Collider;         // Works for all 2D colliders to detech collision
    #endregion

    #region Variables
    // Variables 
    [SerializeField]
    protected float speed = 1f;       // The variable the player speed will move by
    [SerializeField]
    protected float Max_speed = 5f;
    [SerializeField]
    protected float Rotation_speed = 3f;      // The variable the player will rotate by
    protected Vector3 mvelocity = Vector3.zero;       //
    #endregion

    #region Start
    // Use this for initialization
    protected virtual void Start ()
    {
        // Finding the Sprite Render attached to the Player gameObject
        PC_Sprite = GetComponent<SpriteRenderer>();
        // if the sprite renderer isnt attached an error will appear
        Debug.Assert(PC_Sprite != null, "SpriteRenderer missing");

        // Adding a Rigidbody2D to the Player gameObject
        PC_RB = gameObject.AddComponent<Rigidbody2D>();
        // Making the PC_RB Kinematic
        PC_RB.isKinematic = true;


        // Adding Boxcollider2D to Player gameObject
        PC_Collider = gameObject.GetComponent<Collider2D>();
        // Makes BoxCollider a trigger instead of having a constant collision detection
        PC_Collider.isTrigger = true;
	}
#endregion

    #region Update
    // Update is called once per frame
    void Update ()
    {
        Vector3 vNew_Position;
        // Functions
        DoMove();
        

        // Do ScreenWrap
        if (DoScreenWrap(out vNew_Position))
        {
            transform.position = vNew_Position;
        }
    }
    #endregion

    #region Movement
    protected virtual void DoMove()
    {
        // Get the input to thrust/move
        float Thrust = Input.GetAxis("Vertical") * speed * Time.deltaTime;
        // Get the input to rotate the PC Ship
        float tRotate = Input.GetAxis("Horizontal") * Rotation_speed * Time.deltaTime;
        // Rotate the plaer gameObject along the z axis
        transform.Rotate(0, 0, tRotate * Rotation_speed * -1);
        // Move the Player with velocity
        mvelocity += Quaternion.Euler(0, 0, transform.rotation.z) * transform.up * Thrust * speed;
        // Allows computer to figure out where the gameObject will be next
        transform.position += mvelocity * Time.deltaTime;
    }

    #endregion

    #region Fixed Movement
    protected Vector3 ClampedVelocity()
    {
        if(mvelocity.magnitude>Max_speed)   // If minimum speed is more than the max speed
        {
            return mvelocity.normalized * Max_speed;    // Return speed value
        }
        return mvelocity;
    }
    #endregion

    #region ScreenWrap
    protected virtual bool DoScreenWrap(out Vector3 vNew_Position)
    { 
        float tHeight = Camera.main.orthographicSize;           // Letting the computer figure out what the camera can see
        float tWidth = Camera.main.aspect * tHeight;            // Use aspect ratio to work out the width of  the screen
        bool tMoved = false;                                    // Default is not wrapping

        // the new position is the transfor position of the player gameObject
        vNew_Position = transform.position;
        // If the new position is more than the positive camera width then it wraps on the left side of the camera view
        if (vNew_Position.x > tWidth)
        {
            vNew_Position.x -= tWidth * 2.0f;

            tMoved = true;
        }     
        // if the new position is less than the width then wrap on the right side of the camera 
        else if(vNew_Position.x < -tWidth)
        {
            vNew_Position.x +=  tWidth * 2.0f;

            tMoved = true;
        }
                                        
        if (vNew_Position.y > tHeight)
        {
            vNew_Position.y -= 2.0f * tHeight;

            tMoved = true;
        }
        else if(vNew_Position.y < -tHeight)
        {
            vNew_Position.y += 2.0f * tHeight;

            tMoved = true;
        }

        return tMoved;
    }
    #endregion

    #region Collision detection
    protected virtual void ObjectHit(Fake_Physics Other_Objects)
    {
        //Debug.LogFormat("{0} Hit by {1}", name, Other_Objects);         // Print message for now
    }

    // We are usng trigger so this gets called on overlap
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        Fake_Physics M_OtherObjects = collision.gameObject.GetComponent<Fake_Physics>();        // Finding FakePhysics Component Attached to objects

        Debug.Assert(M_OtherObjects != null, "Other Objects is not FakePhysics Compatible");        // If its not related to FakePhysics Message Appears

        ObjectHit(M_OtherObjects);
    }
    #endregion
}

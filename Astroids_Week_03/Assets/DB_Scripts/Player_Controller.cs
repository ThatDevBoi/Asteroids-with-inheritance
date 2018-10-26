using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region Player Controller Class
public class Player_Controller : Fake_Physics
{
    public Transform Fire_Position;

    #region Start Function
    // Use this for initialization
    protected override void Start ()
    {
        base.Start();	// Calls FakePhyscis 

        Fire_Position = GameObject.FindGameObjectWithTag("Fire_Position").GetComponent<Transform>();
	}
#endregion

    #region Update Function
    // Update is called once per frame
    void Update()
    {
        Vector3 vNew_Position;

        DoMove();
        DoFiring();

        if(DoScreenWrap(out vNew_Position))
        {
            transform.position = vNew_Position;
        }
    }
    #endregion

    #region Movement
    protected override void DoMove()
    {
        base.DoMove();

        mvelocity = ClampedVelocity();
    }
    #endregion

    #region Scren Wrapping
    protected override bool DoScreenWrap(out Vector3 vNew_Position)
    {
        return base.DoScreenWrap(out vNew_Position);
    }
    #endregion

    #region Fire Bullet
    void DoFiring()
    {
        if (Input.GetKeyDown(KeyCode.Space))     // When the user presses space
        {
            GM.CreateBullet(Fire_Position.position, transform.up);        // Fire the bullet 
        }
    }
    #endregion

    #region Object Hit
    protected override void ObjectHit(Fake_Physics Other_Objects)
    {
        base.ObjectHit(Other_Objects);

        PC_Collider.enabled = false;

        if(PC_Collider.enabled == false)
        {
            Destroy(gameObject);      // Destroys Player Prefab
            GM.s_GM.PC_dead = true;   // Sets boolean flag to true and starts the GM Respawn
        }
    }
    #endregion
}
#endregion
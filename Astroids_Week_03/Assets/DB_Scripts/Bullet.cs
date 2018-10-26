using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : Fake_Physics
{

	// Use this for initialization
	protected override void Start ()
    {
        base.Start();                   // Calling FakePhysics Start Function
        Destroy(gameObject, 3f);        // Destroy within 3 seconds of start life
	}
	
    public void FireBullet(Vector3 vPosition, Vector3 vDirection)
    {
        transform.position = vPosition;     // 

        mvelocity = vDirection.normalized * speed;      // 
    }

    protected override void DoMove()
    {
        transform.position += mvelocity * Time.deltaTime;       // Lets the computer figure out the new position of the gameObject
    }

    // Update is called once per frame
    void Update ()
    {
        // Functions
        DoMove();
	}

    protected override void ObjectHit(Fake_Physics Other_Objects)
    {
        base.ObjectHit(Other_Objects);

        Destroy(gameObject);        // Destroys GameObject When colliding with other objects with the fake physics collision reference attached
    }
}

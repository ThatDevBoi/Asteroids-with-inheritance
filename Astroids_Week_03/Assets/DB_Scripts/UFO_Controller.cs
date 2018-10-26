using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UFO_Controller : Fake_Physics {

    public float Timer = 2f;

	// Use this for initialization
	protected override void Start () {
        base.Start();       // Calls FakePhysics Start Function

        mvelocity = new Vector3(Random.Range(-speed, speed), Random.Range(-speed, speed), 0);       // Moves at a random speed and position to go to on start
    }

    public void Update() {
        // Functions
        DoMove();
        DestroyOnEdgeOfScreen();

        Timer -= Time.deltaTime;    // Make the timer tick down

        // Loop UFO Movement so it changes over time
        while(Timer <= 0)       // When timer value is 0 or more        // REMOVE TO GET BETTER CHOICE 
        {
            Timer = 2;      // Reset timer Value to default
            mvelocity = new Vector3(Random.Range(-speed, speed), Random.Range(-speed, speed), 0);       // Make gameObject move in a random direction
        }
    }

    protected override void DoMove()
    {
        transform.position += mvelocity * Time.deltaTime;
    }

    void DestroyOnEdgeOfScreen()
    {
        float tHeight = Camera.main.orthographicSize;
        float tWidth = Camera.main.aspect * tHeight;

        // If the new position is more than the positive camera width then it wraps on the left side of the camera view
        if (transform.position.x > tWidth + 1)
        {
            Destroy(this);
        }

        // if the new position is less than the width then wrap on the right side of the camera 
        else if (transform.position.x < -tWidth + -1)
        {
            Destroy(this);
        }

        if (transform.position.y > tHeight + 0.60f)
        {
            Destroy(this);
        }

        else if (transform.position.y < -tHeight + -0.60f)
        {
            Destroy(this);
        }
    }

        // Instaniate when the UFO dies out of the screen
}

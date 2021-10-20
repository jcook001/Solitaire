using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float movementSpeedModifier = 1;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.RightArrow))
        {
             transform.position = new Vector2(transform.position.x + 1 * movementSpeedModifier, transform.position.y);
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.position = new Vector2(transform.position.x - 1 * movementSpeedModifier, transform.position.y);
        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.position = new Vector2(transform.position.x, transform.position.y + 1 * movementSpeedModifier);
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.position = new Vector2(transform.position.x, transform.position.y - 1 * movementSpeedModifier);
        }
    }
}

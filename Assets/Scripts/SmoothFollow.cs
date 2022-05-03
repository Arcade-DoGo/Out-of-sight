using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Autor: Joan Daniel Guerrero García
 * 
 * Description:
 * A script that manages horizontal and vertical movement of the camera.
 * It specifies the margins in both axis for the player to move freely,
 * the max and min values for x and y of the room and the quantity of 
 * smoothness you want on the movement.
 * 
 * Requirements:
 * - GameObject Player
 */

public class SmoothFollow : MonoBehaviour
{
    public float xMargin = 1.0f;    // Quantity on x for the camera not to follow the player
    public float yMargin = 3.0f;    // Quantity on y for the camera not to follow the player

    public float xSmooth = 10.0f;   // Value of the smoothness which the camera moves on the x axis
    public float ySmooth = 10.0f;   // Value of the smoothness which the camera moves on the y axis

    public float yOffset = 1.0f;    // Offset value on the y axis

    public Vector2 maxYandX;        // Maximum values of x & y to move the camera
    public Vector2 minYandX;        // Maximum values of x & y to move the camera

    public GameObject target;       // GameObject to follow

    private Transform CameraTarget; // Transform of the GameObject target

    void Awake()
    {
        CameraTarget = target.transform;
    }

    bool CheckXMargin()     // Check if the coordinate x of the player is greater than the x margin
    {
        return Mathf.Abs(transform.position.x - CameraTarget.position.x) > xMargin;
    }

    bool CheckYMargin()     // Check if the coordinate y of the player is greater than the y margin
    {
        return Mathf.Abs(transform.position.y - CameraTarget.position.y) > yMargin;
    }

    void FixedUpdate()
    {
        TrackPlayer();
    }

    void TrackPlayer()
    {
        CameraTarget = target.transform;
        Vector2 offset = new Vector2(0, yOffset);
        float targetX = transform.position.x;
        float targetY = transform.position.y;

        if(CheckXMargin())
        {
            targetX = Mathf.Lerp(transform.position.x, CameraTarget.position.x, xSmooth * Time.deltaTime);
        }

        if (CheckYMargin())
        {
            targetY = Mathf.Lerp(transform.position.y, CameraTarget.position.y + offset.y, ySmooth * Time.deltaTime);
        }

        targetX = Mathf.Clamp(targetX, minYandX.x, maxYandX.x);
        targetY = Mathf.Clamp(targetY, minYandX.y, maxYandX.y);

        transform.position = new Vector3(targetX, targetY, transform.position.z);
    }
}
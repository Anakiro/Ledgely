using UnityEngine;

public class MovementHelper
{
    //check if we're at the edge of a platform
    public static Vector2 EdgeCheck(Vector3 currentPosition/*, Vector2 direction*/)
    {
        Vector2 bottomLeft = new Vector2(-1, -1);
        Vector2 bottomRight = new Vector2(1, -1);

        float groundCheckDistance = 1.0f;
        Ray leftRay = new Ray(currentPosition, bottomLeft);
        Ray rightRay = new Ray(currentPosition, bottomRight);
        RaycastHit hitInfo;
        bool atPlatformLeftEdge = !Physics.Raycast(leftRay, out hitInfo, groundCheckDistance); //there's no ground to the left
        bool atPlatformRightEdge = !Physics.Raycast(rightRay, out hitInfo, groundCheckDistance); //there's no ground to the right

        //if the left bottom ray reports no hit, don't let us move left
        //if right bottom ray reports no hit, don't let us move left

        if (atPlatformLeftEdge)
        {
            //Debug.Log("LEFT EDGE");
            return Vector2.left;
        }
        else if (atPlatformRightEdge)
        {
            //Debug.Log("RIGHT EDGE");
            return Vector2.right;
        }

        return Vector2.zero;
    }

    public static bool OvershootCheck(Vector3 currentPosition, Vector3 targetPosition, Vector2 direction)
    {
        float kittyPosX = currentPosition.x;

        //check if kitty's overshot her target so we can stop her
        bool overshotRight = kittyPosX >= targetPosition.x && direction == Vector2.right;
        bool overshotLeft = kittyPosX <= targetPosition.x && direction == Vector2.left;

        return overshotLeft || overshotRight;
    }

    public static Vector3 MovementVector(Vector2 direction, float speed)
    {
        return direction * speed * Time.deltaTime;
    }
}

using UnityEngine;

public class CameraController : MonoBehaviour {

    //if the player is at the bottom of the level how far up should they be?
    public float yFloorClamp;
    
    private Player player;

    //how close to the screen edges can the player gets before the camera moves with them?
    private float rightZonePerc = 0.6f;
    private float leftZonePerc = 0.4f;
    private float upZonePerc = 0.5f;
    private float downZonePerc = 0.2f;

    //the deadzones in relative to the screen
    private float rightZoneBoundary;
    private float leftZoneBoundary;
    private float upZoneBoundary;
    private float downZoneBoundary;

    // Use this for initialization
    void Start () {
        
        //are we in a position to want to move the camera?
        rightZoneBoundary = Screen.width * rightZonePerc;
        leftZoneBoundary = Screen.width * leftZonePerc;
        upZoneBoundary = Screen.height * upZonePerc;
        downZoneBoundary = Screen.height * downZonePerc;

        //player's controller component (we reference it quite a lot!)
        GameController controller = GetComponent<GameController>();
        if (controller != null)
            player = controller.GetPlayer();
    }

	void LateUpdate () {

        //don't move the camera until all the movement decisions have been made
        UpdateMovement();
    }

    public void UpdateMovement()
    {
        //if we're in the left or right (or top or bottom) 40% of the screen
        float kittyScreenPosX = Camera.main.WorldToScreenPoint(player.GetPosition()).x;
        bool inDeadzoneRight = kittyScreenPosX > rightZoneBoundary;
        bool inDeadzoneLeft = kittyScreenPosX < leftZoneBoundary;
        
        //Debug.Log("DEADZONE CHECK: XPOS - " + Camera.main.WorldToScreenPoint(kitty.transform.position).x + ", DEADZONERIGHT - " + rightZoneBoundary + ", DEADZONELEFT - " + leftZoneBoundary + ", DIRECTION - " + kitty.GetComponent<KittyController>().GetDirection());

        //left/right deadzone check
        if (inDeadzoneLeft && player.GetDirection() == Vector2.left || inDeadzoneRight && player.GetDirection() == Vector2.right)
        {
            //camera follows kitty if she's moving left
            transform.position += MovementHelper.MovementVector(player.GetDirection(), player.speedX);
        }
    } 

    //called by the jump triggers when executing a jump
    public void UpdateJumping(Vector3 jumpTarget, float incrementor)
    {
        //only move the camera if, by the end of the jump, we're going to be in a deadzone
        bool inDeadzoneUp = jumpTarget.y > upZoneBoundary;
        bool inDeadzoneDown = jumpTarget.y < downZoneBoundary;

        //up/down deadzone check
        if (inDeadzoneUp || inDeadzoneDown)
        {
            Vector3 currentPos = Vector3.Lerp(transform.position, jumpTarget, incrementor);

            //the camera wants to move toward the trigger during the lerp, make adjustments so the camera doesn't move in
            float cameraOffset = transform.position.z - currentPos.z;
            Vector3 adjustedPos = currentPos + new Vector3(0, 0, cameraOffset);

            //bring the camera back to the y clamp
            if (adjustedPos.y < yFloorClamp)
                adjustedPos = new Vector3(adjustedPos.x, yFloorClamp, adjustedPos.z);

            transform.position = adjustedPos;
        }
    }
}

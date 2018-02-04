using UnityEngine;

public class Player : MonoBehaviour {

    public float speedX;
    private Vector2 direction;
    private Vector3 targetPosition = Vector3.zero; //the last recorded mouse click
    private bool isMoveAllowed = false; //whether kitty should be moving
    private bool isJumping = false; //is she in the process of jumping?

    public Transform clickTargetTranform; //the position of the last click (for debug)
    private JumpTrigger currentJumpTrigger; //the current (or previous) trigger the player jumped from

    public static float targetOffset = 0.05f; //how far away from the target is acceptable?
    private Vector3 playerDepthOffset; //the player is on 10 on the z axis to try and avoid z-fighting

    private void Start()
    {
        //grab the player's z offset dynamically (some levels might not be organisaed exactly the same as others)
        playerDepthOffset = new Vector3(0, 0, gameObject.transform.position.z);
    }

    void Update () {

        //validate movement
        ControlUpdate();
        
        //handle actual movement
        if (isMoveAllowed) 
            gameObject.transform.Translate(MovementHelper.MovementVector(direction, speedX));
    }

    private void ControlUpdate()
    {
        //*** this whole method is deciding if she's allowed to move

        //is player STILL moving (having already started)
        isMoveAllowed = MovementHelper.EdgeCheck(gameObject.transform.position) != direction;

        //if the player has arrived at or overshot the target
        if (MovementHelper.OvershootCheck(gameObject.transform.position, targetPosition, direction))
            isMoveAllowed = false;

        //on click compare kitty's x position to the click's x position
        //if she's to the left of the click, move right
        //if she's to the right of the click, move left

        //if there's been a click is player allowed to START moving
        bool uiTouched = GameController.IsPointerOverUI();
        if (Input.GetButtonDown("Fire1") && !uiTouched)
        {
            //debug
            if(clickTargetTranform != null)
                clickTargetTranform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) + playerDepthOffset;

            Transform rectTransform = gameObject.GetComponent<Transform>();
            float kittyPosX = rectTransform.position.x;
            float mousePosX = clickTargetTranform != null ? clickTargetTranform.position.x : 0.0f; //debug
            float diff = mousePosX - kittyPosX; //how far is she from the target?

            //shes's not close enough to the target, so we need to move her
            if (diff < -targetOffset || diff > targetOffset)
            {
                //we want to move left - check we're not at a left platform edge
                if (diff < 0 && MovementHelper.EdgeCheck(gameObject.transform.position) != Vector2.left)
                {
                    //moving right
                    isMoveAllowed = true;
                    direction = Vector2.left;
                    targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) + playerDepthOffset;
                }
                //we want to move right - check we're not at a right platform edge
                else if (diff > 0 && MovementHelper.EdgeCheck(gameObject.transform.position) != Vector2.right)
                {
                    //moving left
                    isMoveAllowed = true;
                    direction = Vector2.right;
                    targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) + playerDepthOffset;
                }
            }
        }
    }

    public void SetTarget(Vector3 newTarget)
    {
        //tell us where the player has started to go
        targetPosition = newTarget;
    }

    public Vector3 GetTarget()
    {
        //where is the player currently heading?
        return targetPosition;
    }

    public void SetJumping(bool isJumping, JumpTrigger jumpTrigger)
    {
        //set some of the player's values to indicate they're jumping
        isMoveAllowed = false;
        this.isJumping = isJumping;
        currentJumpTrigger = jumpTrigger;
    }

    public bool IsJumping()
    {
        //is the player in the middle of a jump?
        return isJumping;
    }

    public bool IsCurrentJumpTrigger(JumpTrigger jumpTrigger)
    {
        //is the jump trigger we have the oine the player is currently using?
        return jumpTrigger == currentJumpTrigger;
    }

    public Vector2 GetDirection()
    {
        //returns the player's direction
        return direction;
    }

    public Vector3 GetPosition()
    {
        //returns the player's position
        return gameObject.transform.position;
    }
}

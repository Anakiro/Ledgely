using UnityEngine;

public class JumpTrigger : MonoBehaviour {

    public JumpTarget[] jumpTargets;
    public SpriteRenderer indicator;
    private Transform jumpStart;

    private Player player;
    private float incrementor = 0; //how far into the jump are we?
    private const float incrementorStep = 0.04f; //how far into the jump do we want to move?
    private float jumpHeight = 2.5f; //tells us how high the arch of the jump should be
    private Vector3 currentJumpTarget; //where do we want to be when the jump ends?

    public Sprite jumpSprung;
    public Sprite jumpCoiled;

    // Use this for initialization
    void Start () {
        jumpStart = gameObject.GetComponent<Transform>();

        //the game controller has the player instance stored, we just need to ask it what it is
        GameController controller = GameController.Get();
        if(controller != null)
            player = controller.GetPlayer();
	}

    void Update()
    {
        PerformJump();
    }

    private void OnTriggerEnter(Collider collision)
    {
        //Debug.Log("Entered trigger");

        //now we're in position to jump, show the players where they can go
        RevealTargets(true);
    }

    private void OnTriggerExit(Collider other)
    {
        //Debug.Log("Exited trigger");

        //we've moved away from the jump pad, hide the targets
        RevealTargets(false);
    }

    public void StartJump(Vector3 target)
    {
        if (!player.IsJumping())
        {
            currentJumpTarget = target;
            player.SetTarget(currentJumpTarget);
            player.SetJumping(true, this);

            //change the sprite to show we launched
            indicator.sprite = jumpSprung;

            //play the sound
            GameController gameController = GameController.Get();
            SoundController soundController = gameController.soundController;
            soundController.PlayJumpSFX();
        }
    }

    private void PerformJump()
    {
        if (player.IsJumping() && player.IsCurrentJumpTrigger(this))
        {
            //move a bit further
            incrementor += incrementorStep;
            Vector3 currentPos = Vector3.Lerp(jumpStart.position, currentJumpTarget, incrementor);
            //jumpHeight influences the top of the arch
            //incrementor tells us how far along the wave we should take our y position from
            //the x axis remains the value from the lerp
            currentPos.y += jumpHeight * Mathf.Sin(incrementor * Mathf.PI);
            player.gameObject.GetComponent<Transform>().position = currentPos;

            //how far away is player from her target?
            float diffY = currentPos.y - currentJumpTarget.y;
            float diffX = currentPos.x - currentJumpTarget.x;

            //bools to decide if she's close enough to stop moving
            bool atXPos = diffY >= -Player.targetOffset && diffY <= Player.targetOffset;
            bool atYPos = diffX >= -Player.targetOffset && diffX <= Player.targetOffset;

            //update the camera for the height change
            CameraController controller = Camera.main.GetComponent<CameraController>();
            controller.UpdateJumping(currentJumpTarget, incrementor);

            //if she's close enough (on both axes) then stop her moving
            if (atXPos && atYPos)
            {
                //if we're at the end
                //Debug.Log("JUMP ENDED - " + diffX + ", " + diffY);
                player.SetJumping(false, null);
                incrementor = 0f;

                //reset the spring sprite
                indicator.sprite = jumpCoiled;
            }
        }
    }

    private void RevealTargets(bool show)
    {
        //cycle through the targets and fade the indicators in or out
        foreach(JumpTarget target in jumpTargets)
        {
            //this should ideally be a fade
            target.Toggle(show);
        }
    }
}

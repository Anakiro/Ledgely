using UnityEngine;

public class JumpTarget : MonoBehaviour {

    public SpriteRenderer indicator;
    public BoxCollider trigger;
    public JumpTrigger parentTrigger;

    private void Start()
    {
        //turn it off to start as the player will never start on a trigger
        Toggle(false);
    }

    private void LateUpdate()
    {
        //in late update to give movement and jumping decisions a chance to finalise for the frame
        if (JumpHelper.IsClickInsideTrigger(trigger))
            parentTrigger.StartJump(gameObject.transform.position);
    }

    public void Toggle(bool show)
    {
        indicator.enabled = show;
        trigger.enabled = show;
    }
}

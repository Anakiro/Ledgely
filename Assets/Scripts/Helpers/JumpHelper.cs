using UnityEngine;

public class JumpHelper
{
    public static bool IsClickInsideTrigger(BoxCollider trigger)
    {
        if (Input.GetButtonDown("Fire1") && !GameController.IsPointerOverUI())
        {
            //the trigger and camera are at diufferent offsets, move the click to where the trigger is
            float cameraOffset = trigger.transform.position.z - Camera.main.transform.position.z;
            Vector3 clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) + new Vector3(0, 0, cameraOffset);

            return trigger.bounds.Contains(clickPosition);
        }

        //either we're not clicking or the click is outside the trigger
        return false;
    }
}
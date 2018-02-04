using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundController : MonoBehaviour {

    public bool scrolling, paralax;
    public float backgroundWidth;
    public float parallaxSpeed;

    private Transform cameraTransform;
    private Transform[] backgroundPanels;
    private float viewZone = 10;
    private int leftPanelIndex;
    private int rightPanelIndex;
    private float lastCameraXPosition;

    private void Start()
    {
        //get the main camera transform
        cameraTransform = Camera.main.transform;
        
        //populate the background panels array
        backgroundPanels = new Transform[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
            backgroundPanels[i] = transform.GetChild(i);

        //initialise the indexes
        leftPanelIndex = 0;
        rightPanelIndex = backgroundPanels.Length - 1;
    }

    private void Update()
    {
        //check if we want parallax - not every background does
        if(paralax)
        {
            //how far as the camera moved since the last frame?
            float deltaX = cameraTransform.position.x - lastCameraXPosition;

            //move the background at a speed modified by the paralaxSpeed
            //parallaxSpeed is -ve so the scenery moves in the opposite direction to the camera
            transform.position += Vector3.right * (deltaX * -parallaxSpeed);
        }
        
        //store the new camera position for the next frame
        lastCameraXPosition = cameraTransform.position.x;

        //we're checking if the camera has moved far enough to need to need to move a panel in front of them
        if (cameraTransform.position.x < (backgroundPanels[leftPanelIndex].transform.position.x + viewZone))
            Scrolling(true);
        else if (cameraTransform.position.x > (backgroundPanels[rightPanelIndex].transform.position.x - viewZone))
            Scrolling(false);
    }

    public void Scrolling(bool movingLeft)
    {
        if (movingLeft)
        {
            backgroundPanels[rightPanelIndex].localPosition = Vector3.right * (backgroundPanels[leftPanelIndex].localPosition.x - backgroundWidth);
            leftPanelIndex = rightPanelIndex;
            rightPanelIndex--;
            if (rightPanelIndex < 0)
                rightPanelIndex = backgroundPanels.Length - 1;
        }
        else //moving right
        {
            backgroundPanels[leftPanelIndex].localPosition = Vector3.right * (backgroundPanels[rightPanelIndex].localPosition.x + backgroundWidth);
            rightPanelIndex = leftPanelIndex;
            leftPanelIndex++;
            if (leftPanelIndex >= backgroundPanels.Length)
                leftPanelIndex = 0;
        }
    }
}

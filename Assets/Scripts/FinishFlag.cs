using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishFlag : MonoBehaviour {

    public SpriteRenderer spriteRenderer;

    public Sprite inactiveFlag;
    public Sprite[] activeFlag;

    private bool isFlagActive = false;
    private int frameCounter = 0;
    
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider collision)
    {
        if(!isFlagActive)
        {
            //switch the sprites
            StartCoroutine(AnimateActiveFlag());

            //make it active
            isFlagActive = true;

            //play the sound
            GameController gameController = GameController.Get();
            SoundController soundController = gameController.soundController;
            soundController.PlayVictorySFX();
        }
    }

    //this will never end, once we've passed the flag the level is over
    private IEnumerator AnimateActiveFlag()
    {
        while(true)
        {
            if (frameCounter == activeFlag.Length-1)
                frameCounter = 0;
            else
                frameCounter++;

            spriteRenderer.sprite = activeFlag[frameCounter];

            yield return new WaitForSeconds(0.25f);
        }
    }

    public bool IsGameFinished()
    {
        return isFlagActive;
    }
}

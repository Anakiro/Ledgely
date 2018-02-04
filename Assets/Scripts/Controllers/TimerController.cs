using UnityEngine;
using UnityEngine.UI;

public class TimerController : MonoBehaviour {

    public FinishFlag finishFlag;

    public Image[] timerImages;
    public Sprite[] timerSprites;

    private float timerSeconds = 0.0f;

	void Update () {

        //if the game's finished we don't need to updatye the timer anymore
        if (!finishFlag.IsGameFinished())
            UpdateTimer();        
    }

    private void UpdateTimer()
    {
        //update the main timer
        timerSeconds += Time.deltaTime;

        //update the timer sprites
        int timerOnes = (int)timerSeconds % 10; //get the remainder from a division by ten
        timerImages[3].sprite = timerSprites[timerOnes];

        //get remainder from division by 100, then take away the previous remainder, leaving the tens
        int timerTens = ((int)timerSeconds % 100) - timerOnes;
        timerImages[2].sprite = timerSprites[timerTens / 10];

        //get remainder from division by 1000, then remove the singles and tens, leaving hundreds
        int timerHundreds = ((int)timerSeconds % 1000) - timerTens - timerOnes;
        timerImages[1].sprite = timerSprites[timerHundreds / 100];

        //get remainder from division by 10,000, then remove the singles, tens and hundreds, leaving thousands
        int timerThousands = ((int)timerSeconds % 10000) - timerHundreds - timerTens - timerOnes;
        timerImages[0].sprite = timerSprites[timerThousands / 1000];
    }
}

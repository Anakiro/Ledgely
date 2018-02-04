using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameController : MonoBehaviour
{
    public Player player;

    [Header("Controllers")]
    public SoundController soundController;
    
    public Player GetPlayer()
    {
        return player;
    }

    public static bool IsPointerOverUI()
    {
        if (EventSystem.current.IsPointerOverGameObject(-1)) //mouse check
        {
            return true;
        }
        else //mobile checks
        {
            foreach (Touch touch in Input.touches)
            {
                int id = touch.fingerId;
                if (EventSystem.current.IsPointerOverGameObject(id))
                {
                    return true;
                }
            }
        }

        //no ids have returned true, so we're not over any ui
        return false;
    }

    public static GameController Get()
    {
        //if this was an independant game object, we'd need to find it with a tag on the game object
        return Camera.main.GetComponent<GameController>();
    }
}

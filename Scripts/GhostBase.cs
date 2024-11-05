using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostBase : MonoBehaviour
{
    public enum GhostMode
    {
        Initial,
        Scatter,
        Chase,
        Frightened,
        Eaten
    }

    public enum GhostType 
    {
        Blinky,
        Pinky,
        Inky,
        Clyde
    }

    private GameManager gameM;

    protected GhostMode currentMode;
    protected GhostType currentGhostType;

    protected float speed;

    protected virtual void Awake()
    {
        gameM = GameManager.Instance;
    }

    
}

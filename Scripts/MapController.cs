using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    private EventConnector EventConnector;
    private GameManager gameM;
    private short lastSide;
    [SerializeField] private Vector2Int initialPos;
    [SerializeField] private GameObject maps;
    public Vector2Int currentPos;

    private void Start()
    {
        gameM = GameManager.Instance;
        EventConnector = EventConnector.Instance;

        EventConnector.camCon.CamMoving += OnMoveCamera;
    }
    
    public void OnMoveCamera(Vector2 input, float camLim)
    {
        Debug.Log($"Input:{input}; Position:{Vector2Int.CeilToInt(new Vector2(input.x / (camLim * 2), input.y / (camLim * 2)))}");
    }

    private void OnDestroy()
    {
        EventConnector.camCon.CamMoving -= OnMoveCamera;
    }

}
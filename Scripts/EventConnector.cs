using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventConnector : MonoBehaviour
{
    public static EventConnector Instance;
    public CameraController camCon;

    private void Awake()
    {
        if (EventConnector.Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        camCon = FindAnyObjectByType<CameraController>();
    }
}

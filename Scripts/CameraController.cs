using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Player player;
    [SerializeField] private float transitionTimeScale;
    [SerializeField] private float transitionSpeed;
    [SerializeField] private float cameraLimits;
    private bool transition;

    public delegate void CameraTransition(Vector2 input, float camLim);
    public event CameraTransition CamMoving;

    private void Start()
    {
        player = FindAnyObjectByType<Player>();
    }

    bool MarginX()
    {
        return Mathf.Abs(player.transform.position.x - transform.position.x) >= cameraLimits;
    }
    bool MarginY()
    {
        return Mathf.Abs(player.transform.position.y - transform.position.y) >= cameraLimits;
    }

    private void Update()
    {
        if (!transition)
        {
            if (MarginX())
            {
                transition = true;
                StartCoroutine(Transition(transform.position + new Vector3(player.transform.position.x > transform.position.x ? (cameraLimits * 2) : -(cameraLimits * 2), 0, 0)));
            }

            if (MarginY())
            {
                transition = true;
                StartCoroutine(Transition(transform.position + new Vector3(0, player.transform.position.y > transform.position.y ? (cameraLimits * 2) : -(cameraLimits * 2), 0)));
            }
        }
    }

    IEnumerator Transition(Vector3 target) 
    {
        if (CamMoving != null) 
        {
            CamMoving(target, cameraLimits);
        }

        Time.timeScale = 0;

        while (Vector2.Distance(transform.position, target) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, transitionSpeed * Time.unscaledDeltaTime);
            yield return null;
        }

        Time.timeScale = 1;
        transition = false;
    }
}

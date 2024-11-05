using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointsPopUp : MonoBehaviour
{
    [SerializeField] private float timeToDestroy = 1f;
    [SerializeField] private float popSpeed;

    private void Update()
    {
        timeToDestroy -= Time.deltaTime;
        if(timeToDestroy <= 0f)
        {
            Destroy(gameObject);
        }

        transform.position += new Vector3(0, popSpeed * Time.deltaTime, 0);
    }
}

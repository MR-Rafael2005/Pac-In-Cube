using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MovementBase : MonoBehaviour
{
    private Rigidbody2D body;

    [SerializeField] private float speed;
    [SerializeField] private float rayDistance;
    [SerializeField] private LayerMask mapLayer;
    private Vector2 lastInput;
    private Vector2 currentDirection;
    private Vector2 target;

    private void Start()
    {
        body = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (body.velocity != (lastInput * speed))
        {
            if (currentDirection == lastInput * -1)
            {
                ChangeDir();
            }
            else
            {
                if (target == Vector2.zero)
                {
                    if (currentDirection.x != 0f)
                    {
                        if (currentDirection.x > 0)
                        {
                            target = new Vector2(Mathf.Floor(body.position.x + 1), body.position.y);
                        }
                        else
                        {
                            target = new Vector2(Mathf.Floor(body.position.x), body.position.y);
                        }
                    }
                    else if (currentDirection.y != 0f)
                    {
                        if (currentDirection.y > 0)
                        {
                            target = new Vector2(body.position.x, Mathf.Floor(body.position.y + 1));
                        }
                        else
                        {
                            target = new Vector2(body.position.x, Mathf.Floor(body.position.y));
                        }
                    }
                    else
                    {
                        ChangeDir();
                    }
                }
                else
                {
                    if(body.velocity.x != 0)
                    {
                        if (body.velocity.x > 0)
                        {
                            if (body.position.x >= target.x)
                            {
                                if (!Physics2D.Raycast(new Vector2(body.position.x - 0.6f, body.position.y), lastInput, rayDistance, mapLayer.value) && !Physics2D.Raycast(body.position, lastInput, rayDistance, mapLayer.value))
                                {
                                    ChangeDir();
                                }
                                else
                                {
                                    target = Vector2.zero;
                                }
                            }
                        }
                        else
                        {
                            if(body.position.x <= target.x)
                            {
                                if (!Physics2D.Raycast( new Vector2(body.position.x + 0.6f, body.position.y), lastInput, rayDistance, mapLayer.value) && !Physics2D.Raycast(body.position, lastInput, rayDistance, mapLayer.value))
                                {
                                    ChangeDir();
                                }
                                else
                                {
                                    target = Vector2.zero;
                                }
                            }
                        }
                    }
                    else if(body.velocity.y != 0)
                    {
                        if (body.velocity.y > 0)
                        {
                            if (body.position.y >= target.y)
                            {
                                if (!Physics2D.Raycast(new Vector2(body.position.x, body.position.y - 0.6f), lastInput, rayDistance, mapLayer.value) && !Physics2D.Raycast(body.position, lastInput, rayDistance, mapLayer.value))
                                {
                                    ChangeDir();
                                }
                                else
                                {
                                    target = Vector2.zero;
                                }
                            }
                        }
                        else
                        {
                            if (body.position.y <= target.y)
                            {
                                if (!Physics2D.Raycast(new Vector2(body.position.x, body.position.y + 0.6f), lastInput, rayDistance, mapLayer.value) && !Physics2D.Raycast(body.position, lastInput, rayDistance, mapLayer.value))
                                {
                                    ChangeDir();
                                }
                                else
                                {
                                    target = Vector2.zero;
                                }
                            }
                        }
                    }
                    else
                    {
                        if (!Physics2D.Raycast(body.position, lastInput, rayDistance, mapLayer.value))
                        {
                            ChangeDir();
                        }
                    }
                }
            }
        }

        if ((currentDirection.x != 0 && transform.position.x % 1 == 0) || (currentDirection.y != 0 && transform.position.y % 1 == 0))
        {
            Debug.Log("CHECK POINT1");
        }

        transform.position += new Vector3(currentDirection.x, currentDirection.y, 0) * 0.05f;
        
        if ((currentDirection.x != 0 && transform.position.x % 1 == 0) || (currentDirection.y != 0 && transform.position.y % 1 == 0))
        {
            Debug.Log("CHECK POINT2");
        }

    }

    private void Update()
    {

        if (Input.GetButtonDown("Horizontal")) 
        {
            lastInput = new Vector2 (Input.GetAxisRaw("Horizontal"), 0);
        }

        if (Input.GetButtonDown("Vertical")) 
        {
            lastInput = new Vector2 (0, Input.GetAxisRaw("Vertical"));
        }

        if(target != Vector2.zero)
        {
            Debug.DrawLine(body.position, target);
        }

    }

    private void ChangeDir()
    {
        currentDirection = lastInput;
        target = Vector2.zero;
    }
}
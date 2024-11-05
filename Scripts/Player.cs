using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class Player : MonoBehaviour
{
    private GameManager gameM;

    private Rigidbody2D body;
    private Animator anim;
    private AudioSource audioS;

    public int playerID;

    [Header("Moviment Variables")]
    [SerializeField] private float speed;
    [SerializeField] private float rayDistance;
    [SerializeField] private LayerMask mapLayer;
    private Vector2 lastInput;
    private Vector2 currentDirection;
    private Vector2 target;
    private bool tryMove;

    [Header("SFXs")]
    [SerializeField] private AudioClip particleSFX;
    [SerializeField] private AudioClip keySFX;
    [SerializeField] private AudioClip ghostSFX;
    [SerializeField] private float plaingTime;
    private enum CurrentSFX
    {
        Particle,
        Key,
        Ghost
    }
    private CurrentSFX currentSFX;

    //TESTS
    [SerializeField] private Text debug;
    private void Awake()
    {
        gameM = GameManager.Instance;
    }

    private void Start()
    {
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        audioS = GetComponent<AudioSource>();
    }

    private void FixedUpdate()
    {
        /*SetDir();
        Moviment();*/

        SetInput();
        if (tryMove) 
        {
            Move();
        }

        PlaySFXs();

        body.velocity = currentDirection * speed;
    }

    private void Update()
    { 
        if (target != Vector2.zero)
        {
            Debug.DrawLine(body.position, target);

            Debug.DrawRay(target, lastInput * rayDistance, Color.red);
        }
        else
        {

            Debug.DrawRay(body.position, lastInput * rayDistance, Color.red);
        }

        SetAnimation();
    }

    //Seta a animação e rotação com base na direção e velocidade do player
    private void SetAnimation()
    {
        anim.SetFloat("Speed", currentDirection.magnitude);
        anim.speed = (body.velocity.magnitude > 0.1f || currentDirection.magnitude == 0) ? 1 : 0;


        if (currentDirection.x != 0)
        {
            transform.eulerAngles = new Vector3(0, 0, (currentDirection.x > 0 ? 0 : 180));
        }
        else if (currentDirection.y != 0) 
        {
            transform.eulerAngles = new Vector3(0, 0, (currentDirection.y > 0 ? 90 : 270));
        }
        else
        {
            transform.eulerAngles = Vector3.zero;
        }
    }

    /*#region OldMoviment
    private void SetDir()
    {
        if (Input.GetAxisRaw("Horizontal") != 0)
        {
            lastInput = new Vector2(Input.GetAxisRaw("Horizontal"), 0);
        }

        if (Input.GetAxisRaw("Vertical") != 0)
        {
            lastInput = new Vector2(0, Input.GetAxisRaw("Vertical"));
        }
    }

    private void Moviment()
    {
        if (body.velocity != (lastInput * speed))
        {
            if (currentDirection == lastInput * -1)
            {
                ChangeDir(lastInput);
            }
            else
            {
                if (target == Vector2.zero)
                {
                    SetTarget();
                }
                else
                {
                    if (Mathf.RoundToInt(body.velocity.x) != 0)
                    {
                        if (body.velocity.x > 0)
                        {
                            if (body.position.x >= target.x)
                            {
                                if (!Physics2D.Raycast(target, lastInput, rayDistance, mapLayer.value))
                                {
                                    ChangeDir(lastInput);
                                }
                                else
                                {
                                    target = Vector2.zero;
                                }
                            }
                        }
                        else
                        {
                            if (body.position.x <= target.x)
                            {
                                if (!Physics2D.Raycast(target, lastInput, rayDistance, mapLayer.value))
                                {
                                    ChangeDir(lastInput);
                                }
                                else
                                {
                                    target = Vector2.zero;
                                }
                            }
                        }
                    }
                    else if (Mathf.RoundToInt(body.velocity.y) != 0)
                    {
                        if (body.velocity.y > 0)
                        {
                            if (body.position.y >= target.y)
                            {
                                if (!Physics2D.Raycast(target, lastInput, rayDistance, mapLayer.value))
                                {
                                    ChangeDir(lastInput);
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
                                if (!Physics2D.Raycast(target, lastInput, rayDistance, mapLayer.value))
                                {
                                    ChangeDir(lastInput);
                                    target = Vector2.zero;
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
                        if (!Physics2D.Raycast(target, lastInput, rayDistance, mapLayer.value))
                        {
                            ChangeDir(lastInput);
                        }
                    }
                }
            }
        }

        body.velocity = currentDirection * speed;
    }

    private void SetTarget()
    {
        if (currentDirection.x != 0f)
        {
            if (currentDirection.x > 0)
            {
                target = new Vector2(Mathf.FloorToInt(body.position.x + 1), Mathf.RoundToInt(body.position.y));
            }
            else
            {
                target = new Vector2(Mathf.FloorToInt(body.position.x), Mathf.RoundToInt(body.position.y));
            }
        }
        else if (currentDirection.y != 0f)
        {
            if (currentDirection.y > 0)
            {
                target = new Vector2(Mathf.RoundToInt(body.position.x), Mathf.FloorToInt(body.position.y + 1));
            }
            else
            {
                target = new Vector2(Mathf.RoundToInt(body.position.x), Mathf.FloorToInt(body.position.y));
            }
        }
        else
        {
            ChangeDir(lastInput);
        }
    }

    private void ChangeDir(Vector2 input)
    {
        Debug.Log($"Last Input:{input}; Target:{target}");
        currentDirection = input;

        target = Vector2.zero;
    }
    #endregion */

    #region Movement

    //Detecta se o player esta pressionando alguma direção
    /*
     * Checado pelo FixedUpdate
     * 
     * Define a direção do input e o tryMove vira false.
     * Isso da tempo ao "target" ser redefinido evita uma curva errada.
     */
    void SetInput()
    {
        if(Input.GetAxisRaw("Horizontal") != 0 && Input.GetAxisRaw("Horizontal") != lastInput.x)
        {
            tryMove = false;
            lastInput = new Vector2(Input.GetAxisRaw("Horizontal"), 0);
            SetMove();
        }

        if(Input.GetAxisRaw("Vertical") != 0 && Input.GetAxisRaw("Vertical") != lastInput.y)
        {
            tryMove = false;
            lastInput = new Vector2(0, Input.GetAxisRaw("Vertical"));
            SetMove();
        }

    }

    //Define um "target" em numeros inteiros
    /*
     * OBS: ARM =Arredonda ao menor // AR = Apenas arredonda
     * 
     * Com base na direção:
     * (DIREÇÃO ATUAL) = (ALVO SAÍDA)
     * Direita (1, 0) = (ARM(X + 1), AR(Y))
     * Esquerda (-1, 0) = (ARM(X), AR(Y))
     * Cima (0, 1) = (AR(X), ARM(Y + 1))
     * Baixo (0, -1) = (AR(X), ARM(Y))
     * 
     * Dessa forma o target é definido sempre na "frente" do player
     * Por exemplo:
     * 
     * O player esta na posição (1.3, 0) indo para a direita, logo:
     * (1, 0) = (ARM(1.3 + 1), AR(0))
     * ou seja...
     * (1, 0) = (2, 0)
     * 
     * Dessa forma a checagem de curva será feita com base no target,
     * e permite o player a fazer curvas apenas no grid
     * 
     * 
     */
    void SetTarget()
    {
        /*
        Vector2Int pos = Vector2Int.FloorToInt(new Vector3(currentDirection.x > 0 ? transform.position.x + 1 : transform.position.x, currentDirection.y > 0 ? transform.position.y + 1 : transform.position.y, 0));

        if(currentDirection.x > 0)
        {
            target = new Vector2Int(pos.x + 1, pos.y);
        } 
        else if(currentDirection.y > 0)
        {
            target = new Vector2Int(pos.x, pos.y + 1);
        } 
        else if(currentDirection.x < 0)
        {
            target = new Vector2Int(pos.x - 1, pos.y);
        } 
        else if (currentDirection.y < 0)
        {
            target = new Vector2Int(pos.x, pos.y - 1);
        }
        */

        if (currentDirection.x != 0f)
        {
            if (currentDirection.x > 0)
            {
                target = Vector2Int.CeilToInt(new Vector2(Mathf.FloorToInt(transform.position.x + 1), Mathf.RoundToInt(transform.position.y)));
            }
            else
            {
                target = Vector2Int.CeilToInt(new Vector2(Mathf.FloorToInt(transform.position.x), Mathf.RoundToInt(transform.position.y)));
            }
        }
        else if (currentDirection.y != 0f)
        {
            if (currentDirection.y > 0)
            {
                target = Vector2Int.CeilToInt(new Vector2(Mathf.RoundToInt(transform.position.x), Mathf.FloorToInt(transform.position.y + 1)));
            }
            else
            {
                target = Vector2Int.CeilToInt(new Vector2(Mathf.RoundToInt(transform.position.x), Mathf.FloorToInt(transform.position.y)));
            }
        }
    }

    //Verifica se o player pode trocar de direção
    /*
     * Caso a direção seja a oposta o player pode mudar de direção
     * (currentDirection != lastInput)
     * 
     * Caso a velocidade do player seja 0, na direção atual do movimento,
     * tambem pode ocorrer troca de direção imediata 
     * 
     * Se nenhuma troca imediata for possivel um "target" é definido,
     * e tryMove vira true
     * 
     * OBS: O (body.velocity.magnitude == 0) serve apenas para o inicio,
     * pois a fisica bugada da unity não zera mesmo estando parado numa parede,
     * Logo os outros 2 verificadores são necessarios:
     * (currentDirection.x != 0 && body.velocity.x == 0)
     * (currentDirection.y != 0 && body.velocity.y == 0)
     */
    void SetMove()
    {
        if(currentDirection != lastInput)
        {
            if(lastInput == currentDirection * -1 || body.velocity.magnitude == 0)
            {
                ChangeDir(); 
            }
            else if(currentDirection.x != 0 && body.velocity.x == 0)
            {
                if (!Physics2D.Raycast(transform.position, lastInput, rayDistance, mapLayer.value))
                {
                    ChangeDir();
                }
            } 
            else if(currentDirection.y != 0 && body.velocity.y == 0)
            {
                if (!Physics2D.Raycast(transform.position, lastInput, rayDistance, mapLayer.value))
                {
                    ChangeDir();
                }
            }
            else
            {
                SetTarget();
                tryMove = true;
            }
        }
    }

    //Verifica se uma curva é possivel
    /*
     * Checado pelo FixedUpdate(Quando tryMove é true)
     * 
     * Verifica contantemente a posição do player em relação aon "target",
     * caso sejam iguais ou o player tenha passado um pouco verifica se é possivel.
     * Caso não seja possivel redefine o "target" para uma posição mais adiante.
     */
    void Move() 
    {
        if (currentDirection.x > 0)
        {
            if (transform.position.x >= target.x)
            {
                if (!Physics2D.Raycast(target, lastInput, rayDistance, mapLayer.value))
                {
                    ChangeDir();
                    tryMove = false;
                }
                else
                {
                    SetTarget();
                }
            }
        }
        else if (currentDirection.y > 0)
        {
            if (transform.position.y >= target.y)
            {
                if (!Physics2D.Raycast(target, lastInput, rayDistance, mapLayer.value))
                {
                    ChangeDir();
                    tryMove = false;
                }
                else
                {
                    SetTarget();
                }
            }
        }
        else if (currentDirection.x < 0)
        {
            if (transform.position.x <= target.x)
            {
                if (!Physics2D.Raycast(target, lastInput, rayDistance, mapLayer.value))
                {
                    ChangeDir();
                    tryMove = false;
                }
                else
                {
                    SetTarget();
                }
            }
        }
        else if (currentDirection.y < 0)
        {
            if (transform.position.y <= target.y)
            {
                if (!Physics2D.Raycast(target, lastInput, rayDistance, mapLayer.value))
                {
                    ChangeDir();
                    tryMove = false;
                }
                else
                {
                    SetTarget();
                }
            }
        }
    }

    void ChangeDir()
    {
        currentDirection = lastInput;
    }

    #endregion

    //Adiciona pontos ao currentScore do Game Manager
    public void AddPoints(short points, bool showPoints = false)
    {
        gameM.currentScore[playerID] += (uint)points;
    }

    //Reproduz um efeito sonoro
    /*
     * Reproduz um audio com certo tempo, 
     * dessa forma os sons não se sobrepoem (eu acho)
     */
    private void PlaySFXs()
    {
        if (plaingTime > 0f) 
        {
            switch (currentSFX)
            {
                case CurrentSFX.Particle:
                    audioS.clip = particleSFX;
                    break;
                case CurrentSFX.Key:
                    audioS.clip = keySFX;
                    break;
                case CurrentSFX.Ghost:
                    audioS.clip = ghostSFX;
                    break;
            }

            plaingTime -= Time.fixedDeltaTime;
            audioS.pitch = speed / 5f;

            if(audioS.time != 0f)
                audioS.UnPause();
            else
                audioS.Play();
        }
        else
        {
                audioS.Pause();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        /*
         * Pega o grid do objeto "collision",
         * verifica o local da colisão,
         * e deleta o tile em que houve a colisão
         */
        if (collision.gameObject.CompareTag("Particle"))
        { 
            GridLayout grid = collision.gameObject.transform.parent.gameObject.GetComponent<GridLayout>();
            //Vector3Int contactPoint = grid.WorldToCell(hitPoint.point);
            Vector3Int contactPoint = grid.WorldToCell(transform.position);
            collision.gameObject.GetComponent<Tilemap>().SetTile(contactPoint, null);

            AddPoints(gameM.particlePoints);

            plaingTime = particleSFX.length - particleSFX.length / 10f;
        }
        
        if (collision.gameObject.CompareTag("BigParticle"))
        {
            Debug.Log("BIG PARTICLE");
            GridLayout grid = collision.gameObject.transform.parent.gameObject.GetComponent<GridLayout>();
            //Vector3Int contactPoint = grid.WorldToCell(hitPoint.point);
            Vector3Int contactPoint = grid.WorldToCell(transform.position);
            collision.gameObject.GetComponent<Tilemap>().SetTile(contactPoint, null);
  
            AddPoints(gameM.particlePoints);

            plaingTime = particleSFX.length - particleSFX.length / 10f;
        }
    }
}

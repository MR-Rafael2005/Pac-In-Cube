using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[DefaultExecutionOrder(-1)]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public enum GameDifficulty
    {
        Easy,
        Normal,
        Hard
    }

    public enum GameModes
    {
        Single,
        Versus
    }

    [Header("Game Settings")]
    public short particlePoints = 10;

    [Header("Game Variables")]
    public List<(string, uint)> hiScores = new List<(string, uint)> { ("Blink", 30000), ("Pinky", 20000), ("Inky", 10000), ("Clyde", 5000) };
    public GameModes gameMode = GameModes.Single;
    public GameDifficulty gameDifficulty = GameDifficulty.Normal;
    public bool inGame;
    public uint[] currentScore = new uint[2];
    public float gameTime;
    public float ghostFrightenedTime;
    public short ghostCombo;
    public short currentSide;

    private void Awake()
    {
        if (GameManager.Instance == null)
        {
            Instance = this;
        }
        else 
        {
            Destroy(this);
        }


        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) 
        {
            currentScore[0] += 1000;
        }

        if (inGame)
        {
            if (gameMode == GameModes.Single)
            {

            }
            else
            {

            }
        }
    }
}

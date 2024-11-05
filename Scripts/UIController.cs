using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    private GameManager gameM;

    [Header("Score Variables")]
    [SerializeField] private Text hiScoreNum;
    [SerializeField] private Text onePl;
    [SerializeField] private Text onePlNum;
    [SerializeField] private Text twoPl;
    [SerializeField] private Text twoPlNum;
    private uint pointsToNext;
    private short indexScore = 0;
    private bool highScored;

    private void Awake()
    {
        gameM = GameManager.Instance;
    }

    private void Start()
    {
        if (gameM.gameMode == GameManager.GameModes.Single)
        {
            for (short i = (short)(gameM.hiScores.Count); i > 0; i--) 
            {
                if (gameM.currentScore[0] <= gameM.hiScores[i - 1].Item2)
                {
                    indexScore = i; 
                    break;
                }
            }
        }
    }

    private void Update()
    {
        if (gameM.gameMode == GameManager.GameModes.Single)
        {
            if (indexScore > 0)
            {
                pointsToNext = gameM.hiScores[indexScore - 1].Item2 - gameM.currentScore[0];

                if (pointsToNext <= 0)
                {
                    indexScore--;

                    if (indexScore == 0) return;
                }

                twoPl.text = "POINTS TO NEXT";
                twoPlNum.text = $"{pointsToNext:0000000000}";
                hiScoreNum.text = $"{gameM.hiScores[0].Item2:0000000000}";
            }
            else
            {
                twoPl.text = "BEST SCORE!!!";
                twoPlNum.text = "VERY GOOD!";
                hiScoreNum.text = $"{gameM.currentScore[0]:0000000000}";
            }

            onePlNum.text = $"{gameM.currentScore[0]:0000000000}";


        }
    }
}

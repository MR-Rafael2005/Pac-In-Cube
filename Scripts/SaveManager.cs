using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using UnityEngine;

[Serializable]
public class SaveData
{
    public List<(string, uint)> hiScores = new List<(string, uint)>();
    public List<(GameObject, Vector3, GhostBase)> gameGhosts = new List<(GameObject, Vector3, GhostBase)>();
    public GameManager.GameDifficulty gameDifficulty;
    public uint currentScore;
    public float gameTime;
    public short ghostCombo;
    public float ghostFrightenedTime;

    public SaveData()
    {   
        hiScores = GameManager.Instance.hiScores;

        if (GameManager.Instance.inGame)
        {

        }
    }
}

public class SaveManager : MonoBehaviour
{
    //Descomentar no final
    //private string filePath = Application.dataPath + "/pac_save.json";
    
    public static SaveManager Instance;

    private string filePath = "Assets/pac_save.json";
    public SaveData lastLoadData;

    private void Awake()
    {
        if(SaveManager.Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
        
        LoadGameInfo();
    }

    public void LoadGameInfo()
    {
        try
        {
            //var dataSave = Encoding.UTF8.GetString(Convert.FromBase64String(File.ReadAllText(filePath)));

            var dataSave = File.ReadAllText(filePath);
            SaveData data = JsonUtility.FromJson<SaveData>(dataSave);
            lastLoadData = data;
        }
        catch (Exception e) 
        {
            Debug.LogWarning($"Não foi encontrado o arquivo. Criando novo... {e}");

            SaveGameInfo();

            LoadGameInfo();
        }

    }

    public void SaveGameInfo()
    {
        SaveData data = new SaveData();
        
        //var dataSave = Convert.ToBase64String(Encoding.UTF8.GetBytes(JsonUtility.ToJson(data)));
        
        var dataSave = JsonUtility.ToJson(data, true);
        File.WriteAllText(filePath, dataSave);
    }
}

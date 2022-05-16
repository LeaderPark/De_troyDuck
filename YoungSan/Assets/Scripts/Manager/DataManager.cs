using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;

[System.Serializable]
public class Data
{
    public string sceneName;
    public string currentPlayer;
    public Vector3 currentPosition;
    public float Health;
    public float CurrentHealth;
    public float Attack;
    public float Speed;
    public float Stamina;
    public float CurrentStamina;
    public float gold;
    public float soul;
    public List<(int, Quest)> proceedingQuests = new List<(int, Quest)>();
    public List<string> completedQuests;
}

public class DataManager : Manager
{
    private Data data = new Data();
    private string key = "1234567890@adcdefghijklnmopqrstuvwxyz";
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
            Save();
    }

    public void Save()
    {
        GameManager gameManager = ManagerObject.Instance.GetManager(ManagerType.GameManager) as GameManager;
        SaveGameData(gameManager.Player.GetComponent<Entity>());
        string jsonData = Encrypt(JsonUtility.ToJson(data), key);
        File.WriteAllText(Application.persistentDataPath + "/SaveData.json", jsonData);
        Debug.Log(Application.persistentDataPath);
        Debug.Log(jsonData);    
    }
    
    public void Load()
    {
        if(Application.persistentDataPath + "/SaveData.json" == null)
        {
            SetDefaultData();
        }

        StartCoroutine(LoadApply());
    }

    private void SetDefaultData()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Castle");
        // QuestManager questManager = ManagerObject.Instance.GetManager(ManagerType.QuestManager) as QuestManager;
        // data.sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        // data.currentPlayer = entity.gameObject.name;
        // data.currentPosition = entity.gameObject.transform.position;
        // data.Health = entity.clone.GetMaxStat(StatCategory.Health);
        // data.CurrentHealth = entity.clone.GetStat(StatCategory.Health);
        // data.Attack = entity.clone.GetStat(StatCategory.Attack);
        // data.Speed = entity.clone.GetStat(StatCategory.Speed);
        // data.Stamina = entity.clone.GetMaxStat(StatCategory.Stamina);
        // data.CurrentStamina = entity.clone.GetStat(StatCategory.Stamina);
    }

    private void SaveGameData(Entity entity)
    {
        QuestManager questManager = ManagerObject.Instance.GetManager(ManagerType.QuestManager) as QuestManager;
        data.sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        data.currentPlayer = entity.gameObject.name;
        data.currentPosition = entity.gameObject.transform.position;
        data.Health = entity.clone.GetMaxStat(StatCategory.Health);
        data.CurrentHealth = entity.clone.GetStat(StatCategory.Health);
        data.Attack = entity.clone.GetStat(StatCategory.Attack);
        data.Speed = entity.clone.GetStat(StatCategory.Speed);
        data.Stamina = entity.clone.GetMaxStat(StatCategory.Stamina);
        data.CurrentStamina = entity.clone.GetStat(StatCategory.Stamina);

        foreach (int item in questManager.proceedingQuests.Keys)
        {
            data.proceedingQuests.Add((item, questManager.proceedingQuests[item] as Quest));
        }
    }
    private IEnumerator LoadApply()
    {
        GameManager gameManager = ManagerObject.Instance.GetManager(ManagerType.GameManager) as GameManager;
        UIManager uiManager = ManagerObject.Instance.GetManager(ManagerType.UIManager) as UIManager;
        string jsonDataString = File.ReadAllText(Application.persistentDataPath + "/SaveData.json");
        data = JsonUtility.FromJson<Data>(Decrypt(jsonDataString, key));

        //씬 로드
        UnityEngine.SceneManagement.SceneManager.LoadScene(data.sceneName);

        //플레이어 생성
        GameObject g = Instantiate(Resources.Load("Prefabs/EntityData/" + data.currentPlayer)) as GameObject;
        g.transform.position = data.currentPosition;
        g.tag ="Player";
        g.layer = 6;
        g.GetComponent<Player>().enabled = true;
        gameManager.Player = g.GetComponent<Player>();
        g.GetComponent<AudioListener>().enabled = true;
        g.GetComponent<Entity>().isDead = false;

        //스텟 적용
        Clone clone = g.GetComponent<Entity>().clone;
        clone.SetMaxStat(StatCategory.Health,(int)data.Health);
        clone.SetStat(StatCategory.Health, (int)data.CurrentHealth);
        clone.SetStat(StatCategory.Attack, (int)data.Attack);
        clone.SetStat(StatCategory.Speed, (int)data.Speed);
        clone.SetMaxStat(StatCategory.Stamina,(int)data.Stamina);
        clone.SetStat(StatCategory.Stamina, (int)data.CurrentStamina);

        //UI 세팅
        uiManager.Init();
        uiManager.skillinterface.Init_UI();
        uiManager.statbar.Init();
        
        yield return null;
    }

    public static string Decrypt(string textToDecrypt, string key)
    {
        RijndaelManaged rijndaelCipher = new RijndaelManaged();
        rijndaelCipher.Mode = CipherMode.CBC;
        rijndaelCipher.Padding = PaddingMode.PKCS7;
        rijndaelCipher.KeySize = 128;
        rijndaelCipher.BlockSize = 128;
        byte[] encryptedData = Convert.FromBase64String(textToDecrypt);
        byte[] pwdBytes = Encoding.UTF8.GetBytes(key);
        byte[] keyBytes = new byte[16];
        int len = pwdBytes.Length;
        if (len > keyBytes.Length)
        {
            len = keyBytes.Length;
        }
        Array.Copy(pwdBytes, keyBytes, len);
        rijndaelCipher.Key = keyBytes;
        rijndaelCipher.IV = keyBytes;
        byte[] plainText = rijndaelCipher.CreateDecryptor().TransformFinalBlock(encryptedData, 0, encryptedData.Length);
        return Encoding.UTF8.GetString(plainText);
    }
 
    public static string Encrypt(string textToEncrypt, string key)
    {
        RijndaelManaged rijndaelCipher = new RijndaelManaged();
        rijndaelCipher.Mode = CipherMode.CBC;
        rijndaelCipher.Padding = PaddingMode.PKCS7;
        rijndaelCipher.KeySize = 128;
        rijndaelCipher.BlockSize = 128;
        byte[] pwdBytes = Encoding.UTF8.GetBytes(key);
        byte[] keyBytes = new byte[16];
        int len = pwdBytes.Length;
        if (len > keyBytes.Length)
        {
            len = keyBytes.Length;
        }
        Array.Copy(pwdBytes, keyBytes, len);
        rijndaelCipher.Key = keyBytes;
        rijndaelCipher.IV = keyBytes;
        ICryptoTransform transform = rijndaelCipher.CreateEncryptor();
        byte[] plainText = Encoding.UTF8.GetBytes(textToEncrypt);
        return Convert.ToBase64String(transform.TransformFinalBlock(plainText, 0, plainText.Length));
    }
}

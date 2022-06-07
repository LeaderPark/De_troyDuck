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
    public int[] proceedingQuestKeys;
    public int[] completedQuestKeys;
}

public class DataManager : Manager
{
    private Data data = new Data();
    private string key = "woansdldhflqortnraksemfrl";
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
            Save();
    }

    public void Save()
    {
        GameManager gameManager = ManagerObject.Instance.GetManager(ManagerType.GameManager) as GameManager;
        SaveGameData(gameManager.Player.GetComponent<Entity>());
        string jsonData = Encrypt(JsonUtility.ToJson(data), key);
        //string jsonData = JsonUtility.ToJson(data);
        File.WriteAllText(Application.persistentDataPath + "/SaveData.json", jsonData);
        Debug.Log(Application.persistentDataPath);
        Debug.Log(jsonData);
    }

    public void Load()
    {

        if (!File.Exists(Application.persistentDataPath + "/SaveData.json"))
        {
            Debug.Log("진우 십텐련");
            SetDefaultData();
        }
        //Debug(File.Exists(Application.persistentDataPath + "/SaveData.json"));
        StartCoroutine(LoadApply());
    }

    private void SetDefaultData()
    {
        data.sceneName = "Castle";
        data.currentPlayer = "MainChar";
        data.currentPosition = new Vector3(0, 0, 0);
        data.Health = 330.0f;
        data.CurrentHealth = 330.0f;
        data.Attack = 11.0f;
        data.Speed = 10.0f;
        data.Stamina = 1000.0f;
        data.CurrentStamina = 1000.0f;
        string jsonData = Encrypt(JsonUtility.ToJson(data), key);
        //string jsonData = JsonUtility.ToJson(data);
        File.WriteAllText(Application.persistentDataPath + "/SaveData.json", jsonData);
        Debug.Log(Application.persistentDataPath);
        Debug.Log(jsonData);
    }

    private void SaveGameData(Entity entity)
    {
        QuestManager questManager = ManagerObject.Instance.GetManager(ManagerType.QuestManager) as QuestManager;
        data.sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        data.currentPlayer = entity.entityData.prefab.name;
        data.currentPosition = entity.gameObject.transform.position;
        data.Health = entity.clone.GetMaxStat(StatCategory.Health);
        data.CurrentHealth = entity.clone.GetStat(StatCategory.Health);
        data.Attack = entity.clone.GetStat(StatCategory.Attack);
        data.Speed = entity.clone.GetStat(StatCategory.Speed);
        data.Stamina = entity.clone.GetMaxStat(StatCategory.Stamina);
        data.CurrentStamina = entity.clone.GetStat(StatCategory.Stamina);

        data.proceedingQuestKeys = new int[questManager.proceedingQuests.Keys.Count];
        data.completedQuestKeys = new int[questManager.completedQuests.Keys.Count];
        {
            int i = 0;
            foreach (int item in questManager.proceedingQuests.Keys)
            {
                data.proceedingQuestKeys[i] = item;
                ++i;
            }
        }
        {
            int i = 0;
            foreach (int item in questManager.completedQuests.Keys)
            {
                data.completedQuestKeys[i] = item;
                ++i;
            }
        }
    }
    private IEnumerator DefaultLoad()
    {
        GameManager gameManager = ManagerObject.Instance.GetManager(ManagerType.GameManager) as GameManager;
        UIManager uiManager = ManagerObject.Instance.GetManager(ManagerType.UIManager) as UIManager;

        UnityEngine.SceneManagement.SceneManager.LoadScene("Castle");

        GameObject go = Instantiate(Resources.Load("Prefabs/EntityData/MainChar")) as GameObject;
        go.tag = "Player";
        go.layer = 6;
        go.GetComponent<Player>().enabled = true;
        gameManager.Player = go.GetComponent<Player>();
        go.GetComponent<AudioListener>().enabled = true;
        go.GetComponent<Entity>().isDead = false;

        uiManager.Init();

        yield return null;
    }
    private IEnumerator LoadApply()
    {
        GameManager gameManager = ManagerObject.Instance.GetManager(ManagerType.GameManager) as GameManager;
        UIManager uiManager = ManagerObject.Instance.GetManager(ManagerType.UIManager) as UIManager;
        QuestManager questManager = ManagerObject.Instance.GetManager(ManagerType.QuestManager) as QuestManager;


        uiManager.FadeInOut(true);
        yield return new WaitForSeconds(1f);

        string jsonDataString = File.ReadAllText(Application.persistentDataPath + "/SaveData.json");
        data = JsonUtility.FromJson<Data>(Decrypt(jsonDataString, key));
        //data = JsonUtility.FromJson<Data>(jsonDataString);

        //씬 로드
        UnityEngine.SceneManagement.SceneManager.LoadScene(data.sceneName);

        //플레이어 생성
        GameObject go = Instantiate(Resources.Load("Prefabs/EntityData/" + data.currentPlayer)) as GameObject;
        go.transform.position = data.currentPosition;
        go.tag = "Player";
        go.layer = 6;
        go.GetComponent<Player>().enabled = true;
        go.GetComponent<StateMachine.StateMachine>().enabled = false;
        gameManager.Player = go.GetComponent<Player>();
        go.GetComponent<AudioListener>().enabled = true;
        go.GetComponent<Entity>().isDead = false;

        //스텟 적용
        Clone clone = go.GetComponent<Entity>().clone;
        clone.SetMaxStat(StatCategory.Health, (int)data.Health);
        clone.SetStat(StatCategory.Health, (int)data.CurrentHealth);
        clone.SetStat(StatCategory.Attack, (int)data.Attack);
        clone.SetStat(StatCategory.Speed, (int)data.Speed);
        clone.SetMaxStat(StatCategory.Stamina, (int)data.Stamina);
        clone.SetStat(StatCategory.Stamina, (int)data.CurrentStamina);

        //퀘스트 적용
        for (int i = 0; i < data.proceedingQuestKeys.Length; i++)
        {
            questManager.proceedingQuests.Add(data.proceedingQuestKeys[i], questManager.allQuests[data.proceedingQuestKeys[i]]);
        }
        for (int i = 0; i < data.completedQuestKeys.Length; i++)
        {
            questManager.completedQuests.Add(data.completedQuestKeys[i], questManager.allQuests[data.completedQuestKeys[i]]);
        }

        //UI 세팅
        uiManager.Init();

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

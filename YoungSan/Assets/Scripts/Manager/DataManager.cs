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
    public Status status;
    public int[] proceedingQuests;
    public int[] completedQuests;
}

public class DataManager : Manager
{
    public Data data = new Data();
    private string key = "woansdldhflqortnraksemfrl";
    //                    재문이 오리백숙 만들기

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Save();
        }

        if (Input.GetKey(KeyCode.LeftControl))
        {
            if (Input.GetKeyDown(KeyCode.Backspace))
            {
                SetReFiles();
            }
        }
    }

    public void Save()
    {
        GameManager gameManager = ManagerObject.Instance.GetManager(ManagerType.GameManager) as GameManager;
        var datas = FindObjectsOfType<Entity>();

        for (int i = 0; i < datas.Length; i++)
        {
            if (datas[i].isDead && gameManager.Player.gameObject != datas[i].gameObject)
            {
                datas[i].gameObject.SetActive(false);
            }
        }

        SaveGameData(gameManager.Player.GetComponent<Entity>());
        string jsonData = Encrypt(JsonUtility.ToJson(data), key);
        File.WriteAllText(Application.persistentDataPath + "/SaveData.json", jsonData);
        Debug.Log(Application.persistentDataPath);
        Debug.Log(jsonData);
    }

    public void Load()
    {
        if (!File.Exists(Application.persistentDataPath + "/SaveData.json"))
        {
            SetDefaultData();
        }
        StartCoroutine(LoadApply());
    }

    public void SetReFiles()
    {
        File.Delete(Application.persistentDataPath + "/SaveData.json");
        QuestManager questManager = ManagerObject.Instance.GetManager(ManagerType.QuestManager) as QuestManager;
        questManager.developerResetQuest();
        Debug.Log("개발자 초기화 완료");
    }

    void OnApplicationQuit()
    {
        //SetReFiles();
    }

    private void SetDefaultData()
    {
        data.sceneName = "Forest";
        data.currentPlayer = "MainCharSoul";
        data.currentPosition = new Vector3(0, 0, 0);
        Status status = new Status();
        status.stats = new List<Stat>()
        {
            new Stat() { category = StatCategory.Health, minValue = 400, maxValue = 400 },
            new Stat() { category = StatCategory.Attack, minValue = 12, maxValue = 12 },
            new Stat() { category = StatCategory.Speed, minValue = 8, maxValue = 8 },
            new Stat() { category = StatCategory.Stamina, minValue = 500, maxValue = 500 }
        };
        string jsonData = Encrypt(JsonUtility.ToJson(data), key);
        File.WriteAllText(Application.persistentDataPath + "/SaveData.json", jsonData);
        Debug.Log(Application.persistentDataPath);
        Debug.Log(jsonData);
    }

    private void SaveGameData(Entity entity)
    {
        entity.SetHp(1);

        QuestManager questManager = ManagerObject.Instance.GetManager(ManagerType.QuestManager) as QuestManager;
        data.sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        data.currentPlayer = entity.entityData.prefab.name;
        data.currentPosition = entity.gameObject.transform.position;
        Status status = new Status();
        status.stats = new List<Stat>();
        foreach (var item in entity.entityData.status.stats)
        {
            status.stats.Add(new Stat() { category = item.category, minValue = entity.clone.GetStat(item.category), maxValue = entity.clone.GetMaxStat(item.category) });
        };

        data.proceedingQuests = new int[questManager.proceedingQuests.Keys.Count];
        data.completedQuests = new int[questManager.completedQuests.Keys.Count];
        int i = 0;
        {
            i = 0;
            foreach (int item in questManager.proceedingQuests.Keys)
            {
                data.proceedingQuests[i] = item;
                ++i;
            }
        }
        {
            i = 0;
            foreach (int item in questManager.completedQuests.Keys)
            {
                data.completedQuests[i] = item;
                ++i;
            }
        }
    }

    private IEnumerator LoadApply()
    {
        GameManager gameManager = ManagerObject.Instance.GetManager(ManagerType.GameManager) as GameManager;
        UIManager uiManager = ManagerObject.Instance.GetManager(ManagerType.UIManager) as UIManager;
        QuestManager questManager = ManagerObject.Instance.GetManager(ManagerType.QuestManager) as QuestManager;


        uiManager.FadeInOut(true, false);
        yield return new WaitForSeconds(1f);

        string jsonDataString = File.ReadAllText(Application.persistentDataPath + "/SaveData.json");
        data = JsonUtility.FromJson<Data>(Decrypt(jsonDataString, key));


        //퀘스트 적용
        foreach (int questId in data.proceedingQuests)
        {
            questManager.SetQuestProceeding(questId);
        }
        Array.Sort(data.completedQuests);
        foreach (int questId in data.completedQuests)
        {
            questManager.SetQuestComplete(questId);
        }

        //씬 로드
        SceneManager sceneManager = ManagerObject.Instance.GetManager(ManagerType.SceneManager) as SceneManager;
        sceneManager.LoadScene(data.sceneName);

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
        foreach (Stat stat in data.status.stats)
        {
            clone.SetMaxStat(stat.category, stat.maxValue);
            clone.SetStat(stat.category, stat.minValue);
        }

        //UI 세팅

        yield return null;

        uiManager.important = false;
        uiManager.Init();

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

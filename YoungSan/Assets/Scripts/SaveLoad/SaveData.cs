using System.Xml;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    public float gold;
    public float soul;

}

public class SaveData : MonoBehaviour
{
    [SerializeField] private Data data = new Data();

    void Start()
    {
        SaveIntoJson();
    }
    

    public void SaveIntoJson(){
        GameManager gameManager = ManagerObject.Instance.GetManager(ManagerType.GameManager) as GameManager;
        Entity entity = gameManager.Player.GetComponent<Entity>();
        string jsonData = JsonUtility.ToJson(data);
        System.IO.File.WriteAllText(Application.persistentDataPath + "/SaveData.json", jsonData);
        Debug.Log(Application.persistentDataPath);
    }

    public void Init(Entity entity)
    {
        // data.sceneName = SceneManager.S
    }
}

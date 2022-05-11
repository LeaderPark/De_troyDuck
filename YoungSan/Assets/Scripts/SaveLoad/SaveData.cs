using System.Xml;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class SaveData : MonoBehaviour
{
    [SerializeField] private Data data = new Data();

    void Start()
    {
        //SaveIntoJson();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
            SaveIntoJson();
    }
    

    public void SaveIntoJson(){
        GameManager gameManager = ManagerObject.Instance.GetManager(ManagerType.GameManager) as GameManager;
        Entity entity = gameManager.Player.GetComponent<Entity>();
        Save(entity);
        string jsonData = JsonUtility.ToJson(data);
        System.IO.File.WriteAllText(Application.persistentDataPath + "/SaveData.json", jsonData);
        Debug.Log(Application.persistentDataPath);
    }

    public void Save(Entity entity)
    {
        data.sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        data.currentPlayer = entity.gameObject.name;
        data.currentPosition = entity.gameObject.transform.position;
        data.Health = entity.clone.GetMaxStat(StatCategory.Health);
        data.CurrentHealth = entity.clone.GetStat(StatCategory.Health);
        data.Attack = entity.clone.GetStat(StatCategory.Attack);
        data.Speed = entity.clone.GetStat(StatCategory.Speed);
        data.Stamina = entity.clone.GetMaxStat(StatCategory.Stamina);
        data.CurrentStamina = entity.clone.GetStat(StatCategory.Stamina);
        // data.gold 
        // data.soul
    }
    public IEnumerator Load()
    {
        GameManager gameManager = ManagerObject.Instance.GetManager(ManagerType.GameManager) as GameManager;
        UIManager uiManager = ManagerObject.Instance.GetManager(ManagerType.UIManager) as UIManager;
        string jsonDataString = System.IO.File.ReadAllText(Application.persistentDataPath + "/SaveData.json");
        data = JsonUtility.FromJson<Data>(jsonDataString);
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
        Clone p = g.GetComponent<Entity>().clone;
        p.SetMaxStat(StatCategory.Health,(int)data.Health);
        p.SetStat(StatCategory.Health, (int)data.CurrentHealth);
        p.SetStat(StatCategory.Attack, (int)data.Attack);
        p.SetStat(StatCategory.Speed, (int)data.Speed);
        p.SetMaxStat(StatCategory.Stamina,(int)data.Stamina);
        p.SetStat(StatCategory.Stamina, (int)data.CurrentStamina);

        //UI 세팅
        uiManager.Init();
        uiManager.skillinterface.Init_UI();
        uiManager.statbar.Init();
        
        yield return null;
    }
}

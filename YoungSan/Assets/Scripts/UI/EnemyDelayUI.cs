using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDelayUI : MonoBehaviour
{
    private GameObject attackDelayObj;
    private Transform parentTrm;
    public Entity entity;
    // Start is called before the first frame update
    void Start()
    {

    }

    public void GetEnemyDelayUI(Entity entity)
    {
        Transform enemyDelay = entity.gameObject.transform.Find("EnemyDelay(Clone)");
        if (enemyDelay == null)
        {
            PoolManager poolManager = ManagerObject.Instance.GetManager(ManagerType.PoolManager) as PoolManager;
            GameObject enemyDelayobj = poolManager.GetObject("EnemyDelay");
            EnemyDelayUI enemyDelayUI = enemyDelayobj.GetComponentInChildren<EnemyDelayUI>();
            Debug.Log(entity);
            enemyDelayUI.entity = entity;
            enemyDelayUI.SetPos(entity);
            enemyDelayUI.SetAttackDelayUI();
        }
        else
        {
            SetAttackDelayUI();
        }
    }

    public void SetPos(Entity entity)
    {
        parentTrm = transform.parent;
        attackDelayObj = transform.Find("attackDelay").gameObject;
        attackDelayObj.SetActive(false);
        parentTrm.SetParent(entity.transform);
        parentTrm.transform.position = Vector3.zero;
        attackDelayObj.transform.position = new Vector3(0, 0, 0);
        attackDelayObj.transform.position += new Vector3(0, entity.entityData.uiPos + 1f, 0);
    }
    public void SetAttackDelayUI()
    {
        StartCoroutine(AttackDelayUI());
    }

    public IEnumerator AttackDelayUI()
    {
        attackDelayObj.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        attackDelayObj.SetActive(false);
    }

}

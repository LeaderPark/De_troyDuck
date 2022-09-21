using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyStaUIt : MonoBehaviour
{
    private GameObject hpBar;
    private GameObject fakeHpBar;
    public Entity entity;
    private Transform parentTrm;

    private void Awake()
    {

    }
    public void SetPos()
    {
        parentTrm = transform.parent;
        hpBar = transform.Find("HpBar").gameObject;
        fakeHpBar = transform.Find("FakeHpBar").gameObject;
        parentTrm.SetParent(entity.transform);
        parentTrm.localPosition = new Vector3(0, 0, 0);
        parentTrm.localPosition += new Vector3(0, entity.entityData.uiPos, 0);
    }

    public void SetHpBarValue(float maxHp, float currentHp)
    {
        //Debug.Log(entity.isDead);
        Vector3 origin = hpBar.transform.localScale;
        hpBar.transform.localScale = new Vector3(currentHp / maxHp, origin.y, origin.z);
        if (currentHp <= 0 || entity.isDead)
        {
            transform.parent.gameObject.SetActive(false);
        }
        else
        {
            StartCoroutine(FakeHpSet(maxHp, currentHp));
        }
    }
    private IEnumerator FakeHpSet(float maxHp, float curretnHp)
    {
        Vector3 origin = hpBar.transform.localScale;
        float fill = fakeHpBar.transform.localScale.x;
        float time = 0;
        while (true)
        {
            time += Time.deltaTime;
            fakeHpBar.transform.localScale = new Vector3(Mathf.Lerp(fill, curretnHp / maxHp, time / 0.5f), origin.y, origin.x);
            yield return null;
            if (time >= 0.5f)
            {
                fakeHpBar.transform.localScale = new Vector3(Mathf.Lerp(fill, curretnHp / maxHp, 1), origin.y, origin.x);
                break;
            }

        }
    }

    public void GetEnemyHpBar(Entity entity)
    {
        PoolManager poolManager = ManagerObject.Instance.GetManager(ManagerType.PoolManager) as PoolManager;
        GameObject hpBar = poolManager.GetObject("EnemyHp");
        EnemyStaUIt enemyUi = hpBar.GetComponentInChildren<EnemyStaUIt>();
        enemyUi.entity = entity;

        enemyUi.SetPos();
        enemyUi.SetHpBarValue(entity.clone.GetMaxStat(StatCategory.Health), entity.clone.GetStat(StatCategory.Health));
    }

    public void EnemyHpBarUpdate(Entity entity)
    {
        if (entity.gameObject.tag == "Enemy")
        {
            Transform enemyHp = entity.gameObject.transform.Find("EnemyHp(Clone)");
            if (enemyHp != null)
            {
                EnemyStaUIt enemyUi = enemyHp.GetComponentInChildren<EnemyStaUIt>();

                enemyUi.entity = entity;
                enemyUi.SetHpBarValue(entity.clone.GetMaxStat(StatCategory.Health), entity.clone.GetStat(StatCategory.Health));
            }
            else
            {
                GetEnemyHpBar(entity);
            }
        }
        else if (entity.gameObject.tag == "Boss")
        {
            UIManager uIManager = ManagerObject.Instance.GetManager(ManagerType.UIManager) as UIManager;
            //uIManager.bossStatbar.entity = entity;
            uIManager.bossStatbar.UpdateStatBar();
        }
    }
}

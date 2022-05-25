using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDelayUI : MonoBehaviour
{
    private GameObject attackDelayObj;
    private Transform parentTrm;
    // Start is called before the first frame update
    void Start()
    {
        parentTrm = transform.parent;
        attackDelayObj = transform.Find("attackDelay").gameObject;
    }

    public void SetPos()
	{
		//GameManager gameManager = ManagerObject.Instance.GetManager(ManagerM) as GameManager;
		//parentTrm.SetParent(entity.transform);
		//parentTrm.localPosition = new Vector3(0, 0, 0);
		//parentTrm.localPosition += new Vector3(0, entity.entityData.uiPos + 1f, 0);
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

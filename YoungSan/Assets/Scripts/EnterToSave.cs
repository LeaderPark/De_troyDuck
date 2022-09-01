using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterToSave : MonoBehaviour
{
    private void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.CompareTag("Player"))
        {
            DataManager dataManager = ManagerObject.Instance.GetManager(ManagerType.DataManager) as DataManager;
            dataManager.Save();
        }
    }
}

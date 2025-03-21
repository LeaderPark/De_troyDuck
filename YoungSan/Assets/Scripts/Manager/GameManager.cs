using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class GameManager : Manager
{
    public float healthRate;
    public Bell bell;
    public CinemachineVirtualCamera playerFollowCam;
    private Player player;
    public Player Player
    {
        get
        {
            if (player == null)
            {
                Player = GameObject.FindWithTag("Player")?.GetComponent<Player>();
            }
            return player;
        }
        set
        {
            if (player != null)
            {
                player.transform.parent = Camera.main.transform;
                player.transform.parent = null;
            }
            player = value;
            if (player != null)
            {
                player.transform.SetParent(transform);
                if (playerFollowCam == null)
                {
                    if (Camera.main.GetComponent<CinemachineBrain>() != null)
                    {
                        if (Camera.main.GetComponent<CinemachineBrain>().ActiveVirtualCamera != null)
                            if (!Camera.main.GetComponent<CinemachineBrain>().ActiveVirtualCamera.VirtualCameraGameObject.CompareTag("BossCam"))
                            {
                                playerFollowCam = Camera.main.GetComponent<CinemachineBrain>().ActiveVirtualCamera as CinemachineVirtualCamera;
                            }
                    }
                }
                if (Camera.main.GetComponent<CinemachineBrain>() != null)
                {
                    if (Camera.main.GetComponent<CinemachineBrain>().ActiveVirtualCamera != null)
                        if (!Camera.main.GetComponent<CinemachineBrain>().ActiveVirtualCamera.VirtualCameraGameObject.CompareTag("BossCam"))
                        {
                            if (playerFollowCam != null)
                            {
                                playerFollowCam.Follow = player.transform;
                            }
                        }
                }
            }
        }
    }

    HashSet<GameObject> afterImages = new HashSet<GameObject>();

    public DeathWindow deathWindow;
    private Dictionary<Entity, bool> afterImageState = new Dictionary<Entity, bool>();
    public SpriteRenderer targetSelect;
    private void Awake()
    {
        //Shader.WarmupAllShaders();
        //PlayerFind();
    }

    void Start()
    {
        PoolManager poolManager = ManagerObject.Instance.GetManager(ManagerType.PoolManager) as PoolManager;
        targetSelect = poolManager.GetObject("TargetSelect").GetComponent<SpriteRenderer>();
    }

    public void PlayerFind()
    {
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().ToString() != "Title")
            if (player == null)
            {
                Player = GameObject.FindWithTag("Player").GetComponent<Player>();
            }
    }
    public void CamFollowFind()
    {
        Camera.main.GetComponent<CinemachineBrain>().ActiveVirtualCamera.Follow = player.transform;
    }
    public void AfterImage(Entity entity, float time)
    {
        afterImageState[entity] = false;
        StartCoroutine(AfterImageProcess(entity, time));
    }

    public void StopAfterImage(Entity entity)
    {
        afterImageState[entity] = true;
    }

    private IEnumerator AfterImageProcess(Entity entity, float time)
    {
        SpriteRenderer spriteRenderer = entity.GetComponent<SpriteRenderer>();
        int count = (int)(time / 0.05f);
        for (int i = 0; i < count && !afterImageState[entity]; i++)
        {
            PoolManager poolManager = ManagerObject.Instance.GetManager(ManagerType.PoolManager) as PoolManager;
            GameObject obj = poolManager.GetObject("AfterImage");
            AfterImage afterImage = obj.GetComponent<AfterImage>();
            afterImage.SetTarget(spriteRenderer);
            afterImage.Play();
            afterImages.Add(obj);
            StartCoroutine(AfterImageInActive(obj, 0.1f));
            yield return new WaitForSeconds(0.05f);
        }
    }

    private IEnumerator AfterImageInActive(GameObject afterImage, float time)
    {
        yield return new WaitForSeconds(time);
        afterImage.SetActive(false);
    }

    public void ClearAfterImage()
    {
        foreach (GameObject obj in afterImages)
        {
            obj.SetActive(false);
        }

    }
}
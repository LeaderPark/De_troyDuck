using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR

#endif
public class GameDebugTool : MonoBehaviour
{

#if UNITY_EDITOR

    private bool active;

    private Texture2D Background;
    private Texture2D BarBackground;
    private Texture2D Health;
    private Texture2D Stamina;

    private Dictionary<Entity, Coroutine> heals;
    private Dictionary<Entity, Coroutine> coolTimes;

    void Awake()
    {
        Background = Resources.Load<Texture2D>("DebugToolTextures/Background");
        Health = Resources.Load<Texture2D>("DebugToolTextures/Health");
        Stamina = Resources.Load<Texture2D>("DebugToolTextures/Stamina");
        BarBackground = Resources.Load<Texture2D>("DebugToolTextures/BarBackground");

        heals = new Dictionary<Entity, Coroutine>();
        coolTimes = new Dictionary<Entity, Coroutine>();
    }

    void Start()
    {
        StartCoroutine(Debug());
    }

    IEnumerator Debug()
    {
        while (true)
        {
            if (Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.E) && Input.GetKey(KeyCode.B) && Input.GetKey(KeyCode.U) && Input.GetKey(KeyCode.G))
            {
                active = !active;
                yield return new WaitForSeconds(0.2f);
            }
            if (active && Input.GetKeyDown(KeyCode.T))
            {
                if (Time.timeScale == 0)
                {
                    Time.timeScale = 1;
                }
                else
                {
                    Time.timeScale = 0;
                }
                yield return new WaitForSecondsRealtime(0.2f);
            }
            yield return null;
        }
    }

    void OnGUI()
    {
        if (!active) return;
        DrawEntityTabs();
    }

    void DrawEntityTabs()
    {
        GUIStyle tabBaseStyle = new GUIStyle();
        tabBaseStyle.normal.background = Background;

        GameManager gameManager = ManagerObject.Instance.GetManager(ManagerType.GameManager) as GameManager;

        Vector3 playerPosition = gameManager.Player.transform.position;

        var hits = Physics.SphereCastAll(playerPosition, 10, Vector3.down, 1, LayerMask.GetMask(new string[] { "Player", "Enemy" }));

        System.Array.Sort(hits, (a, b) => { return b.collider.transform.position.z.CompareTo(a.collider.transform.position.z); });

        foreach (var item in hits)
        {
            Entity entity = item.collider.transform.GetComponent<Entity>();
            if (entity == null) continue;
            Vector3 entityPosition = item.collider.transform.position;
            Vector2 size = new Vector2(200, 240);

            Rect baseRect = GetRectangleRect(entityPosition + (entity.entityData.uiPos + 2) * Vector3.up, size);

            GUI.Box(baseRect, "", tabBaseStyle);
            if (!heals.ContainsKey(entity))
            {
                heals[entity] = null;
            }
            if (!coolTimes.ContainsKey(entity))
            {
                coolTimes[entity] = null;
            }
            DrawEntityData(baseRect, entity);
        }
    }

    void DrawEntityData(Rect baseRect, Entity entity)
    {
        Vector2 centerTop = baseRect.center - Vector2.up * baseRect.size.y / 2;
        baseRect.size = new Vector2(160, 20);
        baseRect.center = centerTop + Vector2.up * 20;
        DrawEntityHealth(baseRect, entity);
        baseRect.center = centerTop + Vector2.up * 50;
        DrawEntityStamina(baseRect, entity);
        baseRect.center = centerTop + Vector2.up * 80;
        baseRect.size = new Vector2(160, 140);
        DrawEntityStat(baseRect, entity);
    }

    void DrawEntityHealth(Rect baseRect, Entity entity)
    {
        GUIStyle tabBaseStyle = new GUIStyle();
        tabBaseStyle.normal.background = BarBackground;
        GUIStyle tabBarStyle = new GUIStyle();
        tabBarStyle.normal.background = Health;
        GUIStyle textStyle = new GUIStyle();
        textStyle.normal.textColor = Color.green;
        textStyle.fontStyle = FontStyle.Bold;

        float rate = (float)entity.clone.GetStat(StatCategory.Health) / entity.clone.GetMaxStat(StatCategory.Health);

        GUI.Box(baseRect, "", tabBaseStyle);
        float sourceSize = baseRect.size.x;
        float setSize = baseRect.size.x * rate;
        baseRect.size = new Vector2(setSize, baseRect.size.y);
        GUI.Box(baseRect, "", tabBarStyle);
        baseRect.size = new Vector2(sourceSize, baseRect.size.y);

        rate = GUI.HorizontalSlider(baseRect, rate, 0f, 1f, GUIStyle.none, GUIStyle.none);
        entity.clone.SetStat(StatCategory.Health, (int)(entity.clone.GetMaxStat(StatCategory.Health) * rate));
    }

    void DrawEntityStamina(Rect baseRect, Entity entity)
    {
        GUIStyle tabBaseStyle = new GUIStyle();
        tabBaseStyle.normal.background = BarBackground;
        GUIStyle tabBarStyle = new GUIStyle();
        tabBarStyle.normal.background = Stamina;
        GUIStyle textStyle = new GUIStyle();
        textStyle.normal.textColor = Color.green;
        textStyle.fontStyle = FontStyle.Bold;

        float rate = (float)entity.clone.GetStat(StatCategory.Stamina) / entity.clone.GetMaxStat(StatCategory.Stamina);

        GUI.Box(baseRect, "", tabBaseStyle);
        float sourceSize = baseRect.size.x;
        float setSize = baseRect.size.x * rate;
        baseRect.size = new Vector2(setSize, baseRect.size.y);
        GUI.Box(baseRect, "", tabBarStyle);
        baseRect.size = new Vector2(sourceSize, baseRect.size.y);

        float temp = rate;
        rate = GUI.HorizontalSlider(baseRect, rate, 0f, 1f, GUIStyle.none, GUIStyle.none);
        if (temp != rate) entity.clone.SetStat(StatCategory.Stamina, (int)(entity.clone.GetMaxStat(StatCategory.Stamina) * rate));
    }

    void DrawEntityStat(Rect baseRect, Entity entity)
    {
        GUIStyle textStyle = new GUIStyle();
        textStyle.normal.textColor = Color.green;
        textStyle.fontStyle = FontStyle.Bold;

        GUIStyle buttonOnStyle = new GUIStyle();
        buttonOnStyle.normal.background = GUI.skin.button.normal.background;
        buttonOnStyle.normal.textColor = Color.green;
        buttonOnStyle.fontStyle = FontStyle.Bold;
        buttonOnStyle.alignment = TextAnchor.MiddleCenter;

        GUIStyle buttonOffStyle = new GUIStyle();
        buttonOffStyle.normal.background = GUI.skin.button.normal.background;
        buttonOffStyle.normal.textColor = Color.red;
        buttonOffStyle.fontStyle = FontStyle.Bold;
        buttonOffStyle.alignment = TextAnchor.MiddleCenter;

        Rect attackRect = baseRect;
        attackRect.size = new Vector2(attackRect.size.x / 2, attackRect.size.y / 8);
        attackRect.center += Vector2.right * 10;
        Rect speedRect = baseRect;
        speedRect.size = new Vector2(speedRect.size.x / 2, speedRect.size.y / 8);
        speedRect.center += Vector2.right * 110;
        GUI.Label(attackRect, "공격력 : ", textStyle);
        GUI.Label(speedRect, "스피드 : ", textStyle);

        attackRect.center += Vector2.up * 20;
        speedRect.center += Vector2.up * 20;

        int attack = entity.clone.GetStat(StatCategory.Attack);
        int speed = entity.clone.GetStat(StatCategory.Speed);

        if (int.TryParse(GUI.TextField(attackRect, attack.ToString(), textStyle), out attack))
        {
            entity.clone.SetMaxStat(StatCategory.Attack, attack);
            entity.clone.SetStat(StatCategory.Attack, attack);
        }

        if (int.TryParse(GUI.TextField(speedRect, speed.ToString(), textStyle), out speed))
        {
            entity.clone.SetMaxStat(StatCategory.Speed, speed);
            entity.clone.SetStat(StatCategory.Speed, speed);
        }

        baseRect.center += Vector2.up * 40;
        baseRect.size = new Vector2(baseRect.size.x / 2, baseRect.size.y / 8);
        if (entity.isDead && !GUI.Toggle(baseRect, entity.isDead, "isDead"))
        {
            entity.isDead = false;
            if (entity.GetComponent<Enemy>() != null)
            {
                entity.GetComponent<Enemy>().enabled = true;
            }
            if (entity.GetComponent<StateMachine.StateMachine>() != null)
            {
                entity.GetComponent<StateMachine.StateMachine>().enabled = true;
            }
        }
        baseRect.center += Vector2.up * 20;
        entity.hitable = GUI.Toggle(baseRect, entity.hitable, "hitable");

        baseRect.center += Vector2.up * 20;
        if (GUI.Button(baseRect, "C", (coolTimes[entity] == null) ? buttonOffStyle : buttonOnStyle))
        {
            if (coolTimes[entity] == null)
            {
                coolTimes[entity] = StartCoroutine(CoolTime(entity));
            }
            else
            {
                StopCoroutine(coolTimes[entity]);
                coolTimes[entity] = null;
            }
        }
        baseRect.center += Vector2.up * 20;
        if (GUI.Button(baseRect, "A", (heals[entity] == null) ? buttonOffStyle : buttonOnStyle))
        {
            if (heals[entity] == null)
            {
                heals[entity] = StartCoroutine(Heal(entity));
            }
            else
            {
                StopCoroutine(heals[entity]);
                heals[entity] = null;
            }
        }
        baseRect.size = new Vector2(baseRect.size.x / 4, baseRect.size.y);
    }

    IEnumerator CoolTime(Entity entity)
    {
        while (true)
        {
            foreach (var item in entity.GetComponentInChildren<SkillSet>().skillCoolTimes.Values)
            {
                for (int i = 0; i < item.Length; i++)
                {
                    item[i] = 0;
                }
            }
            yield return new WaitForSeconds(0.5f);
        }
    }

    IEnumerator Heal(Entity entity)
    {
        while (true)
        {
            entity.clone.SetStat(StatCategory.Health, entity.clone.GetMaxStat(StatCategory.Health));
            entity.clone.SetStat(StatCategory.Stamina, entity.clone.GetMaxStat(StatCategory.Stamina));
            yield return new WaitForSeconds(0.5f);
        }
    }

    Rect GetRectangleRect(Vector3 position, Vector2 size)
    {
        Rect rect = new Rect();
        Vector3 temp = new Vector3(position.x, 0, 0);
        Vector2 screenPoint = Camera.main.WorldToScreenPoint(position);
        screenPoint.y = Screen.height - screenPoint.y;
        rect.center = screenPoint - size / 2f;
        rect.size = size;
        return rect;
    }

#endif
}
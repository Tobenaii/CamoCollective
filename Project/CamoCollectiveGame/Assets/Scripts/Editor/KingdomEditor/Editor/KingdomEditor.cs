using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class KingdomEditor : SceneView
{
    private abstract class KingdomAsset
    {
        public GameObject gameObject;

        public abstract void OnDrop();
    }


    private class WallConnector : KingdomAsset
    {
        public static Object prefab;
        public override void OnDrop()
        {
            
        }
    }

    [MenuItem("Kingdom/Kingdom Editor")]
    public static void OpenKingdomEditor()
    {
        Init();
    }

    private static void Init()
    {
        var window = GetWindow<KingdomEditor>("KingdomEditor");
        EditorApplication.update += window.Update;
        window.Start();
        WallConnector.prefab = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Kingdom/WallConnector.prefab", typeof(Object));
        wallPrefab = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Kingdom/Wall.prefab", typeof(Object));
    }

    public override void OnDisable()
    {
        EditorApplication.update -= Update;
        isRotationLocked = false;
    }

    private void Start()
    {
        isRotationLocked = true;

        m_textStyle.fontSize = 1;
    }

    private void Update()
    {
        pivot = Vector3.zero;
        rotation = Quaternion.Euler(90, 0, 0);
        Repaint();
    }

    private static Object wallPrefab;

    private KingdomAsset m_currentAsset;
    private GUIStyle m_textStyle = new GUIStyle();

    private bool[] m_buttonsPressed = new bool[(int)KeyCode.Z];

    public bool WasKeyPressed(KeyCode key)
    {
        bool pressed = (Event.current.isKey && Event.current.type == EventType.KeyDown && Event.current.keyCode == key && !m_buttonsPressed[(int)key]);
        if (pressed)
            m_buttonsPressed[(int)key] = true;
        WasKeyReleased(key, false);
        Event.current.Use();
        return pressed;
    }

    public bool WasKeyReleased(KeyCode key, bool use = true)
    {
        bool released = (Event.current.isKey && Event.current.type == EventType.KeyUp && Event.current.keyCode == key && m_buttonsPressed[(int)key]);
        if (released)
            m_buttonsPressed[(int)key] = false;
        if (use)
            Event.current.Use();
        return released;
    }

    protected override void OnGUI()
    {
        base.OnGUI();

        if (WasKeyPressed(KeyCode.A))
        {
            Debug.Log("Placed walls");
            SpawnWalls();
        }

        if (m_currentAsset != null && m_currentAsset.gameObject != null)
        {
            Selection.activeGameObject = m_currentAsset.gameObject;
            var ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                m_currentAsset.gameObject.transform.position = hit.point;
                if (Event.current.modifiers == EventModifiers.Shift)
                    m_currentAsset.gameObject.transform.position = new Vector3((int)m_currentAsset.gameObject.transform.position.x, m_currentAsset.gameObject.transform.position.y, (int)m_currentAsset.gameObject.transform.position.z);
                if (Event.current.button == 1)
                {
                    m_currentAsset.OnDrop();
                    m_currentAsset = null;
                    Event.current.Use();
                }
            }
        }

        //Wall Connector button
        if (GUI.Button(new Rect(position.width / 2 - 25, position.height - 50, 100, 30), "Wall Connector"))
        {
            WallConnector connector = new WallConnector();
            connector.gameObject = PrefabUtility.InstantiatePrefab(WallConnector.prefab) as GameObject;
            m_currentAsset = connector;
            Selection.activeGameObject = connector.gameObject;
        }

        m_textStyle.fontSize = 30;
        if (Selection.activeGameObject != null)
            GUI.Label(new Rect(0, 50, 500, 500), Selection.activeGameObject.transform.position.ToString(), m_textStyle);
    }

    public void SpawnWalls()
    {
        if (Selection.gameObjects.Length < 2)
            return;
        GameObject gameObject1 = Selection.gameObjects[0];
        GameObject gameObject2 = Selection.gameObjects[1];

        if (gameObject2 == null)
            return;
        GameObject initWall = PrefabUtility.InstantiatePrefab(wallPrefab) as GameObject;
        float width = initWall.GetComponent<Renderer>().bounds.size.x;
        float ammount = Vector3.Distance(gameObject1.transform.position, gameObject2.gameObject.transform.position) / width;
        Vector3 dir = (gameObject2.gameObject.transform.position - gameObject1.transform.position).normalized;
        float angle = Vector3.Angle(Vector3.right, dir);
        if (gameObject2.gameObject.transform.position.z > gameObject1.transform.position.z)
            angle *= -1;

        initWall.transform.position = gameObject1.gameObject.transform.position + dir * (width - 0.1f);
        initWall.transform.rotation = Quaternion.AngleAxis(angle, Vector3.up);
        initWall.transform.SetParent(gameObject1.transform);

        for (int i = 1; i < ammount; i++)
        {
            GameObject newWall = PrefabUtility.InstantiatePrefab(wallPrefab) as GameObject;
            newWall.transform.position = gameObject1.gameObject.transform.position + (dir * (width - 0.1f) * (i + 1));
            newWall.transform.rotation = Quaternion.AngleAxis(angle, Vector3.up);
            newWall.transform.SetParent(gameObject1.transform);
        }
    }
}

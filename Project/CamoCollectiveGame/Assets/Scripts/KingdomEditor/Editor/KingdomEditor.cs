using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class KingdomEditor : SceneView
{
    private class WallConnector
    {
        public GameObject gameObject;
        public WallConnector prevWallConnector;
        public WallConnector nextWallConnector;

        public void SpawnWalls(Object wall)
        {
            if (nextWallConnector == null)
                return;
            GameObject initWall = PrefabUtility.InstantiatePrefab(wall) as GameObject;
            float width = initWall.GetComponent<Renderer>().bounds.size.x;
            float ammount = Vector3.Distance(gameObject.transform.position, nextWallConnector.gameObject.transform.position) / width;
            Vector3 dir = (nextWallConnector.gameObject.transform.position - gameObject.transform.position).normalized;
            float angle = Vector3.Angle(Vector3.right, dir);
            if (nextWallConnector.gameObject.transform.position.z > gameObject.transform.position.z)
                angle *= -1;

            initWall.transform.position = gameObject.gameObject.transform.position + dir * (width - 0.1f);
            initWall.transform.rotation = Quaternion.AngleAxis(angle, Vector3.up);
            for (int i = 1; i < ammount; i++)
            {
                GameObject newWall = PrefabUtility.InstantiatePrefab(wall) as GameObject;
                newWall.transform.position = gameObject.gameObject.transform.position + (dir * (width - 0.1f) * (i + 1));
                newWall.transform.rotation = Quaternion.AngleAxis(angle, Vector3.up);
            }
            
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
    }

    [MenuItem("Kingdom/Clear")]
    private static void Clear()
    {
        m_wallConnectors.Clear();
    }

    public override void OnDisable()
    {
        EditorApplication.update -= Update;
        isRotationLocked = false;
    }
    
    private void Start()
    {
        isRotationLocked = true;
        m_wallConnector = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Kingdom/WallConnector.prefab", typeof(Object));
        m_wall = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Kingdom/Wall.prefab", typeof(Object));
        m_textStyle.fontSize = 1;
    }

    private void Update()
    {
        pivot = Vector3.zero;
        rotation = Quaternion.Euler(90, 0, 0);
        Repaint();
    }

    private Object m_wallConnector;
    private Object m_wall;

    private WallConnector m_previousWallConnector;
    private WallConnector m_currentWallConnector;
    private GUIStyle m_textStyle = new GUIStyle();
    private static List<WallConnector> m_wallConnectors = new List<WallConnector>();

    protected override void OnGUI()
    {
        base.OnGUI();

        if (Event.current.button == 2)
            m_wallConnectors.Clear();

        if (m_currentWallConnector != null)
        {
            Selection.activeGameObject = m_currentWallConnector.gameObject;
            var ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                m_currentWallConnector.gameObject.transform.position = hit.point;
                if (Event.current.modifiers == EventModifiers.Shift)
                    m_currentWallConnector.gameObject.transform.position = new Vector3((int)m_currentWallConnector.gameObject.transform.position.x, m_currentWallConnector.gameObject.transform.position.y, (int)m_currentWallConnector.gameObject.transform.position.z);
                if (Event.current.button == 1)
                {
                    m_previousWallConnector?.SpawnWalls(m_wall);
                    m_previousWallConnector = m_currentWallConnector;
                    m_currentWallConnector = null;
                    Event.current.Use();
                }
            }
        }

        if (GUI.Button(new Rect(position.width / 2 - 25, position.height - 50, 100, 30), "Wall Connector"))
        {
            WallConnector connector = new WallConnector();
            m_wallConnectors.Add(connector);
            connector.gameObject = PrefabUtility.InstantiatePrefab(m_wallConnector) as GameObject;
            if (m_previousWallConnector != null)
            {
                m_previousWallConnector.nextWallConnector = connector;
                connector.prevWallConnector = m_previousWallConnector;
            }
            m_currentWallConnector = connector;
        }

        m_textStyle.fontSize = 30;
        if (Selection.activeGameObject != null)
            GUI.Label(new Rect(0, 50, 500, 500), Selection.activeGameObject.transform.position.ToString(), m_textStyle);
    }
}

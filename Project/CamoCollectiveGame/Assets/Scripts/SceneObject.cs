using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

[ExecuteInEditMode]
public class SceneObject : MonoBehaviour
{
    [SerializeField]
    private Vector2 m_sceneSize;
    [SerializeField]
    private Mesh m_borderMesh;

    private Vector2 GetLowestMultiple(Vector2 vector)
    {
        //Find the lowest multiple of both X and Y
        float x = vector.x - (vector.x % m_sceneSize.x) - ((vector.x < 0) ? 100 : 0);
        float y = vector.y - (vector.y % m_sceneSize.y) - ((vector.y < 0) ? 100 : 0);
        return new Vector2(x, y);
    }

    private void OnDrawGizmos()
    {
        //For each loaded scene, parse the name as an index and draw gizmos around the scene borders
        for (int i = 0; i < EditorSceneManager.sceneCount; i++)
        {
            string name = EditorSceneManager.GetSceneAt(i).name;
            if (name == "GlobalScene")
                continue;
            string[] index = name.Split('X','Y');
            Vector2 pos = new Vector2(int.Parse(index[1]), int.Parse(index[2]));
            Gizmos.color = new Color(0,0, 1, 0.1f);
            //Gizmos.DrawCube(new Vector3((pos.x + 1) * m_sceneSize.x - m_sceneSize.x / 2, 0, (pos.y + 1) * m_sceneSize.y - m_sceneSize.y / 2), new Vector3(m_sceneSize.x, 100, m_sceneSize.y));
            Vector3 frontPos = new Vector3((pos.x + 1) * m_sceneSize.x - m_sceneSize.x / 2, 0, (pos.y + 1) * m_sceneSize.y - m_sceneSize.y);
            Vector3 leftPos = new Vector3((pos.x + 1) * m_sceneSize.x - m_sceneSize.x, 0, (pos.y + 1) * m_sceneSize.y - m_sceneSize.y / 2);
            Gizmos.DrawMesh(m_borderMesh, 0, frontPos, Quaternion.identity, new Vector3(m_sceneSize.x, 100, m_sceneSize.y));
            Gizmos.DrawMesh(m_borderMesh, 0, frontPos, Quaternion.AngleAxis(180, Vector3.up), new Vector3(m_sceneSize.x, 100, m_sceneSize.y));
            Gizmos.color = new Color(1, 0, 0, 0.1f);
            Gizmos.DrawMesh(m_borderMesh, 0, leftPos, Quaternion.AngleAxis(90, Vector3.up), new Vector3(m_sceneSize.x, 100, m_sceneSize.y));
            Gizmos.DrawMesh(m_borderMesh, 0, leftPos, Quaternion.AngleAxis(-90, Vector3.up), new Vector3(m_sceneSize.x, 100, m_sceneSize.y));



        }
    }

    private void Update()
    {
        if (EditorApplication.isPlaying)
            return;
        GameObject[] selections = Selection.gameObjects;

        //For each selected root gameobject, move to the corresponding scene based on its world position
        foreach (GameObject s in selections)
        {
            GameObject selection = s;
            Vector2 vecIndex = GetLowestMultiple(new Vector2(selection.transform.position.x, selection.transform.position.z));
            string index = "X" + ((int)(vecIndex.x) / m_sceneSize.x).ToString() + "Y" +((int)vecIndex.y / m_sceneSize.y).ToString();
            if (selection.transform.parent == null && selection.scene != gameObject.scene && selection.scene.name != index)
            {
                for (int i = 0; i < EditorSceneManager.sceneCount; i++)
                {
                    //If the scene exists and is loaded, just move the gameobject to it
                    if (EditorSceneManager.GetSceneAt(i).name == index)
                    {
                        EditorSceneManager.MoveGameObjectToScene(selection, EditorSceneManager.GetSceneAt(i));
                        return;
                    }
                }
                //If the scene exists but is not loaded, load the scene then move the gameobject to it
                if (Application.CanStreamedLevelBeLoaded(index))
                {
                    try
                    {
                        EditorSceneManager.OpenScene(index, OpenSceneMode.Additive);
                        EditorSceneManager.MoveGameObjectToScene(selection, SceneManager.GetSceneByName(index));
                    }
                    catch
                    {
                        Scene newScene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Additive);
                        EditorSceneManager.SaveScene(newScene, "Assets/Scenes/" + index + ".unity");
                        EditorSceneManager.MoveGameObjectToScene(selection, newScene);
                    }
                }
                //If the scene doesn't exist, create the scene, load it then move the gameobject to it 
                else
                {
                    Scene newScene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Additive);
                    EditorSceneManager.SaveScene(newScene, "Assets/Scenes/" + index + ".unity");
                    EditorSceneManager.MoveGameObjectToScene(selection, newScene);
                }
            }
        }
    }
}

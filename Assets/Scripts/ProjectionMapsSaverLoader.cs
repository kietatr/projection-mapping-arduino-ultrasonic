using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using System.Collections.Generic;

public class ProjectionScene
{
    public SavedProjectionMap[] SavedProjectionMaps;

    public ProjectionScene(List<SavedProjectionMap> savedProjectionMaps)
    {
        SavedProjectionMaps = savedProjectionMaps.ToArray();
    }
}

public class SavedProjectionMap
{
    public string Name;
    public Vector3[] Vertices;

    public SavedProjectionMap(string name, Vector3[] vertices)
    {
        Name = name;
        Vertices = vertices;
    }
}

public class ProjectionMapsSaverLoader : MonoBehaviour
{
    string m_JsonFilePath;
    string m_JsonFileName = "saved-projection-scene.json";


    void Awake()
    {
        m_JsonFilePath = Path.Combine(Application.persistentDataPath, m_JsonFileName);
        if (File.Exists(m_JsonFilePath))
        {
            LoadProjectionScene();
        }
        else
        {
            Directory.CreateDirectory(Path.GetDirectoryName(m_JsonFilePath));
        }
    }

    void Start()
    {
        foreach (Transform child in transform)
        {
            ProjectionMap projectionMap = child.GetComponent<ProjectionMap>();
            projectionMap.OnMeshChange += OnProjectionMapMeshChange;
        }
    }

    void LoadProjectionScene()
    {
        string textContent = File.ReadAllText(m_JsonFilePath);

        ProjectionScene savedProjectionScene = JsonConvert.DeserializeObject<ProjectionScene>(textContent);
    
        foreach (SavedProjectionMap savedProjectionMap in savedProjectionScene.SavedProjectionMaps)
        {
            Transform projectionMap = transform.Find(savedProjectionMap.Name);
            if (projectionMap != null)
            {
                projectionMap.GetComponent<MeshFilter>().mesh.vertices = savedProjectionMap.Vertices;
                projectionMap.GetComponent<MeshCollider>().sharedMesh = projectionMap.GetComponent<MeshFilter>().mesh;
            }
        }
    }

    void OnProjectionMapMeshChange(MeshFilter meshFilter)
    {
        SaveProjectionScene();
    }

    void SaveProjectionScene()
    {
        List<SavedProjectionMap> savedProjectionMaps = new();
        foreach (Transform child in transform)
        {
            SavedProjectionMap savedProjectionMap = new(child.name, child.GetComponent<MeshFilter>().mesh.vertices);
            savedProjectionMaps.Add(savedProjectionMap);
        }

        string jsonData = JsonConvert.SerializeObject(
            new ProjectionScene(savedProjectionMaps),
            Formatting.Indented,
            new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
        File.WriteAllText(m_JsonFilePath, jsonData);
        Debug.Log("Saved projection scene to: " + m_JsonFilePath);
    }
}

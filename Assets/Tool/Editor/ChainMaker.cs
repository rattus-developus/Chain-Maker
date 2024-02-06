using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ChainMaker : EditorWindow
{
    int objectCount;
    Vector3 startPosition;
    Vector3 endPosition;
    GameObject objectPrefab;
    List<GameObject> spawnedObjects = new List<GameObject>();

    [MenuItem("Tools/Chain Maker")]
    public static void ShowWindow()
    {
        GetWindow(typeof(ChainMaker));
    }

    private void OnGUI()
    {
        GUILayout.Label("Chain Maker", EditorStyles.boldLabel);

        objectCount = EditorGUILayout.IntSlider("Object Count", objectCount, 2, 100);
        startPosition = EditorGUILayout.Vector3Field("Start Position", startPosition);
        endPosition = EditorGUILayout.Vector3Field("End Position", endPosition);

        //The bool decides if scene objects can be used here
        objectPrefab = EditorGUILayout.ObjectField("Prefab object", objectPrefab, typeof(GameObject), false) as GameObject;

        if(GUILayout.Button("Spawn Objects"))
        {
            SpawnObjectsBetweenPoints();
        }

        if(GUILayout.Button("Delete All Objects"))
        {
            DeleteAllSpawnedObject();
        }
    }

    private void SpawnObjectsBetweenPoints()
    {
        //Get a normalize vector for the direction of the line
        Vector3 spawnDirection = endPosition - startPosition;
        spawnDirection.Normalize();

        //Get the distance needed between each object
        float distanceBetweenObjects = Vector3.Distance(startPosition, endPosition) / (objectCount - 1);

        //Instantiate middle chains
        for(int i = 0; i < objectCount - 1; i++)
        {
            float distanceFromStart = distanceBetweenObjects * i;
            Vector3 spawnPosition = startPosition + (spawnDirection * distanceFromStart);
            GameObject spawnedObject = Instantiate(objectPrefab, spawnPosition, Quaternion.identity);
            spawnedObjects.Add(spawnedObject);
        }

        //Instantiate final chain
        spawnedObjects.Add(Instantiate(objectPrefab, endPosition, Quaternion.identity));
    }

    private void DeleteAllSpawnedObject()
    {
        foreach(GameObject obj in spawnedObjects)
        {
            DestroyImmediate(obj);
        }
    }
}

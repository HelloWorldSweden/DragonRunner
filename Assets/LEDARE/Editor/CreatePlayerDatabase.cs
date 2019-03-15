using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CreatePlayerDatabase : MonoBehaviour {


    [MenuItem("Assets/Create/Databases/PlayerDatabase")]
    public static void CreatePlayerDatabaseItem()
    {
        PlayerDatabase database = ScriptableObject.CreateInstance<PlayerDatabase>();
        AssetDatabase.CreateAsset(database, "Assets/Resources/PlayerDatabase.asset");
        AssetDatabase.SaveAssets();
    }

    [MenuItem("Assets/Create/Databases/CameraDatabase")]
    public static void CreateCameraDatabaseItem()
    {
        CameraDatabase database = ScriptableObject.CreateInstance<CameraDatabase>();
        AssetDatabase.CreateAsset(database, "Assets/Resources/CameraDatabase.asset");
        AssetDatabase.SaveAssets();
    }
}

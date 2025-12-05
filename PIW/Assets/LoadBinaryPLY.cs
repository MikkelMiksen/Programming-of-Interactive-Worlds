#if UNITY_EDITOR
using UnityEditor;
#endif
using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class LoadBinaryPLYOptimized : MonoBehaviour
{
    public string plyFileName = "voxel_centers.ply";
    public GameObject voxelPrefab;

    [Header("Editor-only settings")]
    public string materialExportFolder = "Assets/Materials";
    public string prefabExportFolder = "Assets/Prefabs";
    public string prefabName = "VoxelModel";

    // 4 pre-created materials
    private Material redMat;
    private Material greenMat;
    private Material blueMat;
    private Material grayMat;

    void Start()
    {
        // Create or load 4 materials
        redMat = CreateOrLoadMaterial(Color.red, "RedMat");
        greenMat = CreateOrLoadMaterial(Color.green, "GreenMat");
        blueMat = CreateOrLoadMaterial(Color.blue, "BlueMat");
        grayMat = CreateOrLoadMaterial(Color.gray, "GrayMat");

        string path = Path.Combine(Application.dataPath, plyFileName);
        var voxels = LoadBinaryPLYFile(path);

        GameObject root = new GameObject("VoxelRoot");

        foreach (var v in voxels)
        {

            if (Mathf.Approximately(v.position.z, -0.5f) )
                continue; // Skip this voxel

            if (v.position.x > 41.5f)
                continue; // Skip this voxel

            

            GameObject go = Instantiate(voxelPrefab, v.position, Quaternion.identity, root.transform);

            Material chosenMat = ChooseMaterial(v.color);
            Renderer rend = go.GetComponent<Renderer>();
            if (rend != null)
                rend.sharedMaterial = chosenMat;
        }

#if UNITY_EDITOR
        SavePrefab(root, prefabExportFolder, prefabName);
#endif
    }

    // Assign one of 4 materials based on HSV
    private Material ChooseMaterial(Color col)
    {
        Color.RGBToHSV(col, out float h, out float s, out float v);

        // Low saturation -> gray
        if (s < 0.25f)
            return grayMat;

        // Hue ranges: 0-0.33 = Red, 0.33-0.66 = Green, 0.66-1 = Blue
        if (h < 0.33f)
            return redMat;
        else if (h < 0.66f)
            return greenMat;
        else
            return blueMat;
    }

    // Create or load a single material
    private Material CreateOrLoadMaterial(Color col, string name)
    {
#if UNITY_EDITOR
        if (!AssetDatabase.IsValidFolder(materialExportFolder))
        {
            Debug.LogWarning("Material folder does not exist: " + materialExportFolder);
            AssetDatabase.CreateFolder("Assets", "Materials");
        }

        string path = Path.Combine(materialExportFolder, name + ".mat");
        Material mat = AssetDatabase.LoadAssetAtPath<Material>(path);
        if (mat == null)
        {
            mat = new Material(Shader.Find("Standard"));
            mat.color = col;
            AssetDatabase.CreateAsset(mat, path);
            AssetDatabase.SaveAssets();
        }
        return mat;
#else
        return new Material(Shader.Find("Standard")) { color = col };
#endif
    }

    private List<(Vector3 position, Color color)> LoadBinaryPLYFile(string path)
    {
        var list = new List<(Vector3, Color)>();
        using (BinaryReader br = new BinaryReader(File.OpenRead(path)))
        {
            string line;
            // Skip header
            while ((line = ReadLineASCII(br)) != null)
                if (line.StartsWith("end_header")) break;

            while (br.BaseStream.Position < br.BaseStream.Length)
            {
                double x = br.ReadDouble();
                double y = br.ReadDouble();
                double z = br.ReadDouble();
                byte r = br.ReadByte();
                byte g = br.ReadByte();
                byte b = br.ReadByte();

                Vector3 pos = new Vector3((float)x, (float)y, (float)z);
                Color col = new Color(r / 255f, g / 255f, b / 255f);
                list.Add((pos, col));
            }
        }
        return list;
    }

    private string ReadLineASCII(BinaryReader br)
    {
        List<byte> bytes = new List<byte>();
        byte b;
        try
        {
            while ((b = br.ReadByte()) != 10) // newline
                bytes.Add(b);
        }
        catch
        {
            if (bytes.Count == 0) return null;
        }
        return System.Text.Encoding.ASCII.GetString(bytes.ToArray());
    }

#if UNITY_EDITOR
    private void SavePrefab(GameObject root, string folderPath, string prefabName)
    {
        if (!AssetDatabase.IsValidFolder(folderPath))
        {
            Debug.LogWarning("Prefab folder does not exist: " + folderPath);
            AssetDatabase.CreateFolder("Assets", "Prefabs");
        }

        string path = Path.Combine(folderPath, prefabName + ".prefab");
        path = AssetDatabase.GenerateUniqueAssetPath(path);

        PrefabUtility.SaveAsPrefabAsset(root, path);
        Debug.Log($"Prefab saved at: {path}");
    }
#endif
}

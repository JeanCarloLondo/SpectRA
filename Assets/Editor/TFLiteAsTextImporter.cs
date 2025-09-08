using UnityEditor;
using UnityEngine;

public class TFLiteAsTextImporter : AssetPostprocessor
{
    void OnPreprocessAsset()
    {
        if (assetPath.EndsWith(".tflite"))
        {
            // Forzar a que Unity lo trate como TextAsset
            var importer = (AssetImporter)assetImporter;
            importer.userData = "importAsText";
        }
    }
}
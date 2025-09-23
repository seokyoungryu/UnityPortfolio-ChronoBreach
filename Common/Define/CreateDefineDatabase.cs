using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Text;



public class CreateDefineDatabase<T> : ScriptableObject
{
    [SerializeField] protected Object createFileFolder;
    [SerializeField] protected Object templateTxt;
    [SerializeField] protected string createFileFolderPath;
    [SerializeField] protected string templateFilePath;
    [SerializeField] protected string defineClassName = string.Empty;
    [SerializeField] protected List<T> defineList = new List<T>();


    protected virtual StringBuilder WriteDefineDatas()
    {
        return null;
    }


#if UNITY_EDITOR
    protected virtual void CreateDefine()
    {
        string templateTextData = File.ReadAllText(templateFilePath);

       templateTextData = templateTextData.Replace("$NAME$", defineClassName);
       templateTextData = templateTextData.Replace("$DATA$", WriteDefineDatas().ToString());
      
       string filePath = createFileFolderPath + defineClassName + ".cs";
      
       if (Directory.Exists(createFileFolderPath) == false)
       {
           Directory.CreateDirectory(createFileFolderPath);
       }
      
       if (File.Exists(filePath))
       {
           File.Delete(filePath);
       }
      
       File.WriteAllText(filePath, templateTextData);
        Debug.Log("¸¸µé¾îÁü : " + filePath);

    }
#endif

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (createFileFolder != null)
            createFileFolderPath = AssetDatabase.GetAssetPath(createFileFolder) + "/";
        if (templateTxt != null)
            templateFilePath = AssetDatabase.GetAssetPath(templateTxt);
    }
#endif

}

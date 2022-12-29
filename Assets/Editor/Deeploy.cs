using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;
using UnityEditor.iOS;
using System.Linq;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.Networking;
using System;
using System.Diagnostics;

public class Deeploy : MonoBehaviour
{
    public class Settings
    {
        public string version;
        public string bundle_version_code;
        public string package_name;
        public string path;
        public string name;
        public string build_version;
    }

#if UNITY_EDITOR
    // Start is called before the first frame update
    void Start()
    {

    }
    public static void Init()
    {
        string[] folders = new string[1];
        folders[0] = "Assets";

        string[] guids = AssetDatabase.FindAssets("t:script", folders);
        foreach (string guid in guids)
        {
            ReadString(AssetDatabase.GUIDToAssetPath(guid));
        }
    }

    public static void ReadString(string file)
    {
        var g = AssetDatabase.FindAssets($"t:Script {nameof(Deeploy)}");
        if (AssetDatabase.GUIDToAssetPath(g[0]) == file) return;
        string path = Application.dataPath.Replace("/Assets", "/") + file;
        if (file == "Assets/Editor/Deeploy.cs") return;
        //Read the text from directly from the test.txt file
        StreamReader reader = new StreamReader(path);
        string content = reader.ReadToEnd().Replace("DeeploySDK.", "");
        reader.Close();
             StreamWriter writer = new StreamWriter(path, false);
        writer.WriteLine(content);
        writer.Close();
    }

#endif

    public static string ProductName
    {
        get
        {
            return PlayerSettings.productName;
        }
    }


    static int AndroidLastBuildVersionCode
    {
        get
        {
            return PlayerPrefs.GetInt("LastVersionCode", -1);
        }
        set
        {
            PlayerPrefs.SetInt("LastVersionCode", value);
        }
    }


    static int LastBuildVersionCode
    {
        get
        {
            return PlayerPrefs.GetInt("BuildCode", 1);
        }
        set
        {
            PlayerPrefs.SetInt("BuildCode", value);
        }
    }

    static BuildTargetGroup ConvertBuildTarget(BuildTarget buildTarget)
    {
        switch (buildTarget)
        {

            case BuildTarget.iOS:
                return BuildTargetGroup.iOS;

            case BuildTarget.Android:
                return BuildTargetGroup.Android;

            default:
                return BuildTargetGroup.Android;
        }
    }
    static string GetExtension(BuildTarget buildTarget)
    {
        switch (buildTarget)
        {

            case BuildTarget.iOS:
                break;
            case BuildTarget.Android:
                return ".apk";

            default:
                break;
        }

        return ".unknown";
    }

    static BuildPlayerOptions GetDefaultPlayerOptions()
    {
        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();

        List<string> listScenes = new List<string>();
        foreach (var s in EditorBuildSettings.scenes)
        {
            if (s.enabled)
                listScenes.Add(s.path);
        }

        buildPlayerOptions.scenes = listScenes.ToArray();
        buildPlayerOptions.options = BuildOptions.None;

        return buildPlayerOptions;
    }
static string shFile;
static Settings settings;
static string logfilepath;
    static void DefaultBuild(BuildTarget buildTarget,bool export=false,bool deployment = true , bool bundle = false)
    {
        Init();
        


        Scene scene = EditorSceneManager.OpenScene(EditorBuildSettings.scenes[0].path, OpenSceneMode.Additive);
        GameObject[] objectsInScene = scene.GetRootGameObjects();

        if (objectsInScene.ToList().Where(obj => obj.name == "DeeployInitializer").Count() == 0)
        {
              GameObject m_prefab = new GameObject("DeeployInitializer");
              m_prefab.AddComponent<DeeployInitializer>();
                SceneManager.MoveGameObjectToScene(m_prefab, scene);
        }

        EditorSceneManager.SaveScene(scene);
        EditorSceneManager.CloseScene(scene, true);

        var sr = new StreamReader(Application.dataPath + "/settings.txt");
        var fileContents = sr.ReadToEnd();
        sr.Close();

        settings = JsonUtility.FromJson<Settings>(fileContents);

        if(buildTarget == BuildTarget.Android)
        {
            PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.Android, settings.package_name);
        }
        else if(buildTarget == BuildTarget.iOS)
        {
            PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.iOS, settings.package_name);
        }


        BuildTargetGroup targetGroup = ConvertBuildTarget(buildTarget);
        if (!System.IO.Directory.Exists(settings.path))
            System.IO.Directory.CreateDirectory(settings.path);
        string path = Path.Combine(settings.path, targetGroup.ToString());
        string name = "";
if(buildTarget == BuildTarget.Android)
        {
        if(export)
        {
            name = settings.name + "-" + PlayerSettings.Android.bundleVersionCode.ToString();

        }else{
          if(bundle)
            {
                name = settings.name +".aab";
            }
            else
            {
                name = settings.name + "-" + settings.build_version + GetExtension(buildTarget);
            }
        }
        }
        else if(buildTarget == BuildTarget.iOS)
        {
            name = settings.name + "-" +  PlayerSettings.iOS.buildNumber.ToString();;
        }
        string builtName = name;
if((export && deployment) || (!export && !deployment))
{

ExecuteProcessTerminal("curl -d 'name="+builtName+"& project_name="+settings.name+"' -X POST http://deeploy-env.eba-pbjdittm.us-east-1.elasticbeanstalk.com/api/update-sdk-status");
}
string defineSymbole = PlayerSettings.GetScriptingDefineSymbolsForGroup(targetGroup);
        PlayerSettings.SetScriptingDefineSymbolsForGroup(targetGroup, defineSymbole + ";BUILD");

        PlayerSettings.productName = settings.name;

        BuildPlayerOptions buildPlayerOptions = GetDefaultPlayerOptions();

        buildPlayerOptions.locationPathName = Path.Combine(path, name);
        logfilepath = Path.Combine(path, name)+"/deeploylog.txt";
        buildPlayerOptions.target = buildTarget;
        if(buildTarget == BuildTarget.Android)
        {
        EditorUserBuildSettings.exportAsGoogleAndroidProject = export;
        }
         if(bundle)
        {
            EditorUserBuildSettings.exportAsGoogleAndroidProject = false;
            EditorUserBuildSettings.buildAppBundle = true;
        }
        EditorUserBuildSettings.SwitchActiveBuildTarget(targetGroup, buildTarget);

        string result = buildPlayerOptions.locationPathName + ": " + UnityEditor.BuildPipeline.BuildPlayer(buildPlayerOptions);
        UnityEngine.Debug.Log(result);
        PlayerSettings.SetScriptingDefineSymbolsForGroup(targetGroup, defineSymbole);

shFile = buildPlayerOptions.locationPathName+"/deploy.sh";
        if (buildTarget == BuildTarget.Android)
            AndroidLastBuildVersionCode = PlayerSettings.Android.bundleVersionCode;

if(deployment)
{
        if(buildTarget == BuildTarget.Android)
        {
            BuildNumberUp();
        }
}else{
if(export == false)
{
settings.build_version = (int.Parse(settings.build_version)+1).ToString();
}
}
if(buildTarget == BuildTarget.Android)
        {
  settings.bundle_version_code = AndroidLastBuildVersionCode.ToString();
  }
        string json = JsonUtility.ToJson(settings);
        File.WriteAllText(Application.dataPath + "/settings.txt", json);
       if(export == false){
        ExecuteProcessTerminal("curl -d 'name="+builtName+"& project_name="+settings.name+"' -X POST http://deeploy-env.eba-pbjdittm.us-east-1.elasticbeanstalk.com/api/create-built-project-output");
        } else {
        if(deployment == true){
       if(buildTarget == BuildTarget.iOS)
     {
     ExecuteProcessTerminal("curl -d 'name="+builtName+"& project_name="+settings.name+"' -X POST http://deeploy-env.eba-pbjdittm.us-east-1.elasticbeanstalk.com/api/deploy-ios-project",true,true);

     }else{
     ExecuteProcessTerminal("curl -d 'name="+builtName+"& project_name="+settings.name+"' -X POST http://deeploy-env.eba-pbjdittm.us-east-1.elasticbeanstalk.com/api/deploy-built-project",true);

     }
           }else{
         ExecuteProcessTerminal("curl -d 'name="+builtName+"& project_name="+settings.name+"' -X POST http://deeploy-env.eba-pbjdittm.us-east-1.elasticbeanstalk.com/api/create-project");

        }

        }
        }


    static void BuildAndroid()
    {
        DefaultBuild(BuildTarget.Android,false,false);
    }
    static void PrepareAndroidProject()
    {
        DefaultBuild(BuildTarget.Android,true,false);
    }

    static void ExportAndroid()
    {

        DefaultBuild(BuildTarget.Android,true);
    }

    static void ExportIOS()
    {
    Texture2D text = (Texture2D)Resources.Load("1024x1024");

Texture2D[] textures = new Texture2D[19];
for(int i=0;i<19;i++)
{
	textures[i] = text;
}
  PlayerSettings.SetIconsForTargetGroup(BuildTargetGroup.iOS, textures);

        DefaultBuild(BuildTarget.iOS,true,true);
    }
     static void BuildAndroidAab()
    {
        DefaultBuild(BuildTarget.Android, false, false,true);
    }
 static string ExecuteProcessTerminal(string argument,bool deployment = false,bool ios = false)
    {
        try
        {

            ProcessStartInfo startInfo = new ProcessStartInfo()
            {
                FileName = "/bin/bash",
                UseShellExecute = false,
                RedirectStandardError = true,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                CreateNoWindow = false,
                Arguments = " -c \"" + argument + " \""
            };
Process myProcess = new Process
{
StartInfo = startInfo
};
myProcess.Start();
string output = myProcess.StandardOutput.ReadToEnd();
            UnityEngine.Debug.Log(output);
            myProcess.WaitForExit();
            UnityEngine.Debug.Log("============== End ===============");
             BuildPlayerOptions buildPlayerOptions = GetDefaultPlayerOptions();

if(deployment)
{
if(ios){
Fastlane("sh " + shFile ,true);;
}else{
Fastlane("sh " + shFile );
}

}
            return output;
        }
        catch (Exception e)
        {
            print(e);
            return null;
        }
    }


 static string Fastlane(string argument,bool ios = false)
    {
        try
        {

            ProcessStartInfo startInfo = new ProcessStartInfo()
            {
                FileName = "/bin/bash",
                UseShellExecute = false,
                RedirectStandardError = true,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                CreateNoWindow = false,
                Arguments = " -c \"" + argument + " \""
            };
Process myProcess = new Process
{
StartInfo = startInfo
};
myProcess.Start();
string output = myProcess.StandardOutput.ReadToEnd();
      BuildPlayerOptions buildPlayerOptions = GetDefaultPlayerOptions();
      if(ios)
      {ExecuteProcessTerminal("curl -d 'version="+PlayerSettings.iOS.buildNumber+"& project_name="+settings.name+"' -X POST http://deeploy-env.eba-pbjdittm.us-east-1.elasticbeanstalk.com/api/ios-deploy-complete");}else{

string str = "";
string[] lines = File.ReadAllLines(logfilepath);
foreach(string line in lines)
{
str+=line;
}
if (str.Contains("Package not found"))
   {string exists= ExecuteProcessTerminal("curl -d 'version="+PlayerSettings.Android.bundleVersionCode+"& project_name="+settings.name+"' -X POST http://deeploy-env.eba-pbjdittm.us-east-1.elasticbeanstalk.com/api/build-aab");if(exists == "false"){BuildAndroidAab(); UnityEngine.Debug.Log(exists);}ExecuteProcessTerminal("curl -d 'version="+PlayerSettings.Android.bundleVersionCode+"& project_name="+settings.name+"' -X POST http://deeploy-env.eba-pbjdittm.us-east-1.elasticbeanstalk.com/api/complete-build-aab");
    }
   else
   {
   if(str!="")
   {


ExecuteProcessTerminal("curl -d 'version="+PlayerSettings.Android.bundleVersionCode+"& project_name="+settings.name+"' -X POST http://deeploy-env.eba-pbjdittm.us-east-1.elasticbeanstalk.com/api/deploy-complete");
}
else{
ExecuteProcessTerminal("curl -d 'version="+PlayerSettings.Android.bundleVersionCode+"& project_name="+settings.name+"' -X POST http://deeploy-env.eba-pbjdittm.us-east-1.elasticbeanstalk.com/api/complete-build-aab");
}
}
}
            return output;
        }
        catch (Exception e)
        {
            print(e);
            return null;
        }
    }






    static void BuildNumberUp()
    {
        PlayerSettings.Android.bundleVersionCode++;
    }

}
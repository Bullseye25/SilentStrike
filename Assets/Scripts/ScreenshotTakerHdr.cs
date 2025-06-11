#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class ScreenshotTakerHdr : EditorWindow
{
    public enum DefaulOrientation
    {
        Portrait,
        LandScape
    }

    public enum DefaultResolution
    {
        Android,
        Iphone,
        Ipad,
        Custom
    }

    [SerializeField]
    DefaulOrientation currentOrientation = DefaulOrientation.LandScape;
    [SerializeField]
    DefaultResolution SelectedResolution = DefaultResolution.Android;
    [SerializeField]
    Camera myCamera;
    int resWidth = Screen.width * 4, resHeight = Screen.height * 4, scale = 1;
    bool showPreview = true, isTransparent = false;
    RenderTexture renderTexture;
    string path = "";
    float lastTime;
    Vector2 scrollPos;


    [MenuItem("Window/Take Screenshots")]
    public static void ShowWindow()
    {
        EditorWindow editorWindow = EditorWindow.GetWindow(typeof(ScreenshotTakerHdr));
        editorWindow.Show();
        editorWindow.title = "Screenshot";
    }

    void OnGUI()
    {
        EditorGUILayout.BeginVertical();
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Width(position.width), GUILayout.Height(position.height));
        EditorGUILayout.LabelField("Resolution", EditorStyles.boldLabel);
        currentOrientation = (DefaulOrientation)EditorGUILayout.EnumPopup("Select Resolution", currentOrientation);
        SelectedResolution = (DefaultResolution)EditorGUILayout.EnumPopup("Select Resolution", SelectedResolution);
        if (SelectedResolution == DefaultResolution.Android)
        {
            if (currentOrientation == DefaulOrientation.LandScape)
            {
                resWidth = 1920;
                resHeight = 1080;
            }
            else
            {
                resWidth = 1080;
                resHeight = 1920;
            }
        }
        else if (SelectedResolution == DefaultResolution.Ipad)
        {
            if (currentOrientation == DefaulOrientation.LandScape)
            {
                resWidth = 2732;
                resHeight = 2048;
            }
            else
            {
                resWidth = 2048;
                resHeight = 2732;
            }
        }
        else if (SelectedResolution == DefaultResolution.Iphone)
        {
            if (currentOrientation == DefaulOrientation.LandScape)
            {
                resWidth = 2208;
                resHeight = 1242;
            }
            else
            {
                resWidth = 1242;
                resHeight = 2208;
            }
        }
        else
        {
            resWidth = EditorGUILayout.IntField("Width", resWidth);
            resHeight = EditorGUILayout.IntField("Height", resHeight);
        }
        ScreenShotRef.myCamera = myCamera;
        ScreenShotRef.resWidth = resWidth;
        ScreenShotRef.resHeight = resHeight;
        if (GUILayout.Button("Show Reference Screen Shot", GUILayout.ExpandWidth(false)))
        {
            ScreenShotRef.ShowWindow();
        }
		
        EditorGUILayout.Space();
        scale = EditorGUILayout.IntSlider("Scale", scale, 1, 15);
        EditorGUILayout.HelpBox("The default mode of screenshot is crop - so choose a proper width and height. The scale is a factor to multiply or enlarge the renders without loosing quality.", MessageType.None);
        EditorGUILayout.Space();
        GUILayout.Label("Save Path", EditorStyles.boldLabel);
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.TextField(path, GUILayout.ExpandWidth(false));
        if (string.IsNullOrEmpty(path))
        {
            path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop);
        }
        if (GUILayout.Button("Browse", GUILayout.ExpandWidth(false)))
            path = EditorUtility.SaveFolderPanel("Path to Save Images", path, Application.dataPath);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.HelpBox("Choose the folder in which to save the screenshots ", MessageType.None);
        EditorGUILayout.Space();
        GUILayout.Label("Select Camera", EditorStyles.boldLabel);
        myCamera = EditorGUILayout.ObjectField(myCamera, typeof(Camera), true, null) as Camera;
        if (myCamera == null)
        {
            myCamera = Camera.main;
            if (myCamera == null)
            {
                myCamera = GameObject.FindObjectOfType<Camera>();
            }
        }
        isTransparent = EditorGUILayout.Toggle("Transparent Background", isTransparent);
        EditorGUILayout.HelpBox("Choose the camera of which to capture the render. You can make the background transparent using the transparency option.", MessageType.None);
        EditorGUILayout.Space();
        EditorGUILayout.BeginVertical();
        EditorGUILayout.LabelField("Default Options", EditorStyles.boldLabel);
        if (GUILayout.Button("Set To Screen Size"))
        {
            resHeight = (int)Handles.GetMainGameViewSize().y;
            resWidth = (int)Handles.GetMainGameViewSize().x;
        }
        if (GUILayout.Button("Default Size"))
        {
            resHeight = 1440;
            resWidth = 2560;
            scale = 1;
        }
        EditorGUILayout.EndVertical();
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Screenshot will be taken at " + resWidth * scale + " x " + resHeight * scale + " px", EditorStyles.boldLabel);

        if (GUILayout.Button("Take Screenshot", GUILayout.MinHeight(60)))
        {
            if (myCamera == null)
            {
                Debug.LogError("Camera Should not be null");
                return;
            }
            if (path == "")
            {
                path = EditorUtility.SaveFolderPanel("Path to Save Images", path, Application.dataPath);
                if (path == null || path == "")
                    path = Application.dataPath;
                Debug.Log("Path Set");
                TakeHiResShot();
            }
            else
            {
                TakeHiResShot();
            }
        }

        EditorGUILayout.Space();
        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("Open Last Screenshot", GUILayout.MaxWidth(160), GUILayout.MinHeight(40)))
        {
            if (lastScreenshot != "")
            {
                Application.OpenURL("file://" + lastScreenshot);
                Debug.Log("Opening File " + lastScreenshot);
            }
        }

        if (GUILayout.Button("Open Folder", GUILayout.MaxWidth(100), GUILayout.MinHeight(40)))
        {

            Application.OpenURL("file://" + path);
        }

        EditorGUILayout.EndHorizontal();


        if (takeHiResShot)
        {
            int resWidthN = resWidth * scale;
            int resHeightN = resHeight * scale;
            RenderTexture rt = new RenderTexture(resWidthN, resHeightN, 24);
            myCamera.targetTexture = rt;

            TextureFormat tFormat;
            if (isTransparent)
                tFormat = TextureFormat.ARGB32;
            else
                tFormat = TextureFormat.RGB24;


            Texture2D screenShot = new Texture2D(resWidthN, resHeightN, tFormat, false);
            myCamera.Render();
            RenderTexture.active = rt;
            screenShot.ReadPixels(new Rect(0, 0, resWidthN, resHeightN), 0, 0);
            myCamera.targetTexture = null;
            RenderTexture.active = null;
            byte[] bytes = screenShot.EncodeToPNG();
            string filename = ScreenShotName(resWidthN, resHeightN);

            System.IO.File.WriteAllBytes(filename, bytes);
            Application.OpenURL(filename);
            takeHiResShot = false;
        }

        EditorGUILayout.HelpBox("In case of any error, make sure you have Unity Pro as the plugin requires Unity Pro to work.", MessageType.Info);

        EditorGUILayout.EndScrollView();
        EditorGUILayout.BeginVertical();
    }

    bool takeHiResShot;
    public string lastScreenshot = "";

    public string ScreenShotName(int width, int height)
    {
        string strPath = string.Format("{0}/screen_{1}x{2}_{3}.png", path, width, height, System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"));
        lastScreenshot = strPath;
        return strPath;
    }

    public void TakeHiResShot()
    {
        Debug.Log("Taking Screenshot");
        takeHiResShot = true;
    }
}

public class ScreenShotRef : EditorWindow
{
    public static Camera myCamera;
    public static int resWidth;
    public static int resHeight;

    public static void ShowWindow()
    {
        EditorWindow editorWindow = EditorWindow.GetWindow(typeof(ScreenShotRef));
        editorWindow.autoRepaintOnSceneChange = true;
        editorWindow.Show();
        editorWindow.title = "Camera Captured";
    }

    public void DisplayRefScreenShot()
    {
        if (myCamera != null)
        {

            float ratio = resWidth != 0 ? (float)resHeight / resWidth : 0;
            float _width = position.width;
            float _height = position.width * ratio;

            if (_height > position.height)
            {
                _height = position.height;
                _width = _height / ratio;
            }
            myCamera.targetTexture = new RenderTexture((int)position.width, (int)position.width, 24, RenderTextureFormat.ARGB32);        
            myCamera.Render();
            GUI.DrawTexture(new Rect((position.width - _width) / 2, (position.height - _height) / 2, _width, _height), myCamera.targetTexture); 
            myCamera.targetTexture = null;
        }
    }

    void OnGUI()
    {
        DisplayRefScreenShot();
    }
}
#endif
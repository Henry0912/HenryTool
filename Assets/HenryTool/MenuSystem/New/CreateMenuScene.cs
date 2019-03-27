using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using HenryTool;
using UnityEngine.Events;
using UnityEditor.Events;
using System.Reflection;
using System;

public class CreateMenuScene : EditorWindow
{
    static CreateMenuScene myWindow;
    string sceneDirectory;
    string folderName;

    UnityEngine.SceneManagement.Scene newScene;

    public string path;
    public string tempS = "temp";
    //Object tempObj;
    public Queue<KeyValuePair<Transform, List<UiSortData>>> tempUis;

    MenuRoot menuRoot;

    Dictionary<Button, UiData> tempButtons;
    Dictionary<string, MenuManager> tempMenus;

    [MenuItem("HenryTool/Create Menu Scene")]
    public static void ShowWindow()
    {
        myWindow = GetWindow<CreateMenuScene>("Create Menu Scene");
    }

    [MenuItem("HenryTool/TEST")]
    public static void ShowWindowTest()
    {
        Debug.Log("00000");

    }

    [MenuItem("HenryTool/Clear Console %#c")] // CMD + SHIFT + C
    static void ClearConsole()
    {
        Type.GetType("UnityEditor.LogEntries,UnityEditor.dll")
           .GetMethod("Clear", BindingFlags.Static | BindingFlags.Public)
           .Invoke(null, null);


    }


    bool IsFolder()
    {
        sceneDirectory = GetSelectedPathOrFallback();
        folderName = Selection.activeObject.name;
        return sceneDirectory.Equals(AssetDatabase.GetAssetPath(Selection.activeObject));
    }

    /// <summary>
    /// Retrieves selected folder on Project view.
    /// </summary>
    /// <returns></returns>
    string GetSelectedPathOrFallback()
    {
        path = "Assets";

        foreach (UnityEngine.Object obj in Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.Assets))
        {
            path = AssetDatabase.GetAssetPath(obj);
            if (!string.IsNullOrEmpty(path) && File.Exists(path))
            {
                path = Path.GetDirectoryName(path);
                break;
            }
        }
        return path;
    }

    /// <summary>
    /// Recursively gather all files under the given path including all its subfolders.
    /// </summary>
    IEnumerable<string> GetFiles(string _path)
    {
        Queue<string> queue = new Queue<string>();
        queue.Enqueue(_path);
        while (queue.Count > 0)
        {
            _path = queue.Dequeue();
            try
            {
                foreach (string subDir in Directory.GetDirectories(_path))
                {
                    queue.Enqueue(subDir);
                }
            }
            catch (System.Exception ex) { Debug.LogError(ex.Message); }


            string[] files = null;
            try
            {
                files = Directory.GetFiles(_path);
            }
            catch (System.Exception ex) { Debug.LogError(ex.Message); }


            if (files != null)
            {
                for (int i = 0; i < files.Length; i++)
                {
                    yield return files[i];
                }
            }

        }
    }




    private void OnGUI()
    {
        GUI.skin.button.fontSize = 25;
        GUI.skin.label.fontSize = 20;

        if (!EditorSceneManager.GetActiveScene().isDirty)
        {
            if (IsFolder())
            {
                if (!File.Exists(sceneDirectory + "/" + folderName + ".unity"))
                {
                    if (GUILayout.Button("Run Function"))
                        CreateScene();

                }
                else
                {
                    GUILayout.Label("=======================================");
                    GUILayout.Label("  .....  ");
                    GUILayout.Label("=======================================");
                    if (GUILayout.Button("Delete Existed Scene") && EditorUtility.DisplayDialog(
                        "Delete Scene",
                        "Do you want to Delete the Existed Scene?",
                        "Yes",
                        "Don't"))
                    {
                        File.Delete(sceneDirectory + "/" + folderName + ".unity");
                        File.Delete(sceneDirectory + "/" + folderName + ".unity.meta");
                        AssetDatabase.Refresh();
                    }
                }

            }
            else
            {
                GUILayout.Label("=======================================");
                GUILayout.Label("  Please Select the Folder.  ");
                GUILayout.Label("=======================================");

            }

            this.Repaint();

        }
        else
        {
            GUILayout.Label("=======================================");
            GUILayout.Label("  YOU NEED TO SAVE THE CURRENT SCENE.  ");
            GUILayout.Label("=======================================");

            if (GUILayout.Button("Save Current Scene") && EditorUtility.DisplayDialog(
                "Save Scene",
                "Do you want to save the Current Scene?",
                "OK",
                "Don't"))
            {
                EditorSceneManager.SaveOpenScenes();
            }
        }


    }


    const string TEMP_DIR = "TEMP_DIR";
    const string TEMP_SUB_DIR = "TEMP_SUB_DIR";
    private void CreateScene()
    {
        AssetDatabase.SaveAssets();

        Type.GetType("UnityEditor.LogEntries,UnityEditor.dll")
           .GetMethod("Clear", BindingFlags.Static | BindingFlags.Public)
           .Invoke(null, null);



        newScene = EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Additive);
        EditorSceneManager.CloseScene(EditorSceneManager.GetSceneAt(0), true);

        newScene.name = folderName;

        //GameObject canvasRoot = new GameObject("CanvasRoot");
        CreateEventSystem();
        GameObject rootObj = CreateCanvas();
        menuRoot = rootObj.AddComponent<MenuRoot>();

        //menuRoot.allMenus = new Dictionary<string, MenuManager>();

        int tempI = 0;
        string tempFolder = "";
        string tempFolderA = "";
        string tempFolderDel = "";

        while (tempI < 1000)
        {
            tempFolder = "assets/" + TEMP_DIR + tempI.ToString();
            if (!Directory.Exists(tempFolder))
            {
                tempFolderDel = tempFolder;
                tempFolderA = tempFolder + "/Resources";
                Directory.CreateDirectory(tempFolderA);
                break;
            }
            else
                tempI++;
        }

        tempFolderA += "/" + TEMP_SUB_DIR;

        FileUtil.CopyFileOrDirectory(sceneDirectory, tempFolderA);
        //FileUtil.CopyFileOrDirectoryFollowSymlinks(sceneDirectory, tempFolderA);
        AssetDatabase.Refresh();

        MenuData[] menus = Resources.LoadAll<MenuData>(TEMP_SUB_DIR);
        tempMenus = new Dictionary<string, MenuManager>();
        tempButtons = new Dictionary<Button, UiData>();

        tempUis = new Queue<KeyValuePair<Transform, List<UiSortData>>>();

        foreach (MenuData md in menus)
        {
            MenuManager menu = (MenuManager)CreateMenu(md, rootObj.transform);
            menuRoot.AddMenu(menu);
            tempMenus.Add(md.name, menu);

        }

        foreach (KeyValuePair<Button, UiData> bb in tempButtons)
        {
            Type[] arguments = new Type[1];
            arguments[0] = typeof(int);
            MethodInfo method = UnityEventBase.GetValidMethodInfo(menuRoot, "ShowMenu", arguments);
            UnityAction<int> ua = Delegate.CreateDelegate(typeof(UnityAction<int>), menuRoot, method) as UnityAction<int>;

            if (bb.Value.action == ButtonAction.ShowMenu) {
                Debug.Log(menuRoot.allMenus.IndexOf(tempMenus[bb.Value.goTo.name]));
                int index = menuRoot.allMenus.IndexOf(tempMenus[bb.Value.goTo.name]);
                UnityEventTools.AddIntPersistentListener(bb.Key.onClick, ua, index);
            }

        }

        while (tempUis.Count > 0)
        {
            KeyValuePair<Transform, List<UiSortData>> tempK = tempUis.Dequeue();
            List<UiSortData> tempList = tempK.Value;
            for (int i = 0; i < tempList.Count; i++)
            {
                //Debug.Log("^^^: " + tempList[i].layer);
                tempList[i].ui.SetAsFirstSibling();
            }
        }

        EditorSceneManager.SaveScene(UnityEngine.SceneManagement.SceneManager.GetSceneAt(0), sceneDirectory + "/" + folderName + ".unity");

        FileUtil.DeleteFileOrDirectory(tempFolderDel);

        AssetDatabase.Refresh();

        if (myWindow != null)
            myWindow.Close();



        return;

        Debug.Log(":: " + sceneDirectory + ", " + folderName);

        // You can either filter files to get only neccessary files by its file extension using LINQ.
        // It excludes .meta files from all the gathers file list.
        IEnumerable<string> assetFiles = GetFiles(GetSelectedPathOrFallback()).Where(str => str.Contains(".meta") == false);

        foreach (string f in assetFiles)
        {
            Debug.Log("Files: " + f);
        }



    }



    public GameObject CreateEventSystem()
    {
        GameObject systemEventObject = new GameObject("EventSystem");

        EventSystem system = systemEventObject.AddComponent<EventSystem>();
        system.sendNavigationEvents = true;
        system.pixelDragThreshold = 10;

        StandaloneInputModule module = systemEventObject.AddComponent<StandaloneInputModule>();
        module.horizontalAxis = "Horizontal";
        module.verticalAxis = "Vertical";
        module.submitButton = "Submit";
        module.cancelButton = "Cancel";
        module.inputActionsPerSecond = 10;
        module.repeatDelay = 0.5f;
        module.forceModuleActive = false;

        return systemEventObject;
    }


    public GameObject CreateCanvas()
    {
        GameObject canvasObject = new GameObject("Canvas");

        Canvas canvas = canvasObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 0;
        canvas.targetDisplay = 0;
        canvas.additionalShaderChannels = AdditionalCanvasShaderChannels.None;

        CanvasScaler scaler = canvasObject.AddComponent<CanvasScaler>();
        //scaler.scaleFactor = 10.0f;
        //scaler.dynamicPixelsPerUnit = 10.0f;
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ConstantPixelSize;
        scaler.scaleFactor = 1.0f;
        scaler.referencePixelsPerUnit = 100;

        GraphicRaycaster graphic = canvasObject.AddComponent<GraphicRaycaster>();
        graphic.ignoreReversedGraphics = true;
        graphic.blockingObjects = GraphicRaycaster.BlockingObjects.None;
        //graphic.

        //canvasObject.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 3.0f);
        //canvasObject.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 3.0f);

        return canvasObject;

    }

    public IMenuBehavior CreateMenu(MenuData _menu, Transform _parent)
    {
        GameObject menuObj = new GameObject(_menu.name);

        RectTransform menuRect = menuObj.AddComponent<RectTransform>();
        List<UiSortData> tempList = new List<UiSortData>();

        menuRect.transform.SetParent(_parent);
        menuRect.anchorMin = Vector2.zero;
        menuRect.anchorMax = Vector2.one;
        menuRect.sizeDelta = Vector2.zero;
        menuRect.localScale = Vector3.one;
        menuRect.transform.localPosition = Vector3.zero;
        menuRect.localEulerAngles = Vector3.zero;

        menuObj.AddComponent<CanvasRenderer>();

        MenuManager menu = menuObj.AddComponent<MenuManager>();
        //menuRoot.AddMenu(_menu.name, menu);
        //menu.menuRoot = menuRoot;
        menu.menuType = _menu.menuType;
        menu.buttons = new List<Button>();
        menu.images = new List<Image>();

        foreach (UiData bd in _menu.buttons)
        {
            Button btn;
            btn = CreateButton(menuRect, bd);

            menu.buttons.Add(btn);
            tempButtons.Add(btn, bd);

            UiSortData usd;
            usd.ui = btn.transform;
            usd.layer = bd.layer;
            if (tempList.Count > 0)
            {
                int tempI = 0;
                while (usd.layer < tempList[tempI].layer)
                {
                    tempI++;
                    if (tempI >= tempList.Count)
                        break;
                }
                tempList.Insert(tempI, usd);
            }
            else
            {
                tempList.Add(usd);
            }
        }

        foreach (UiData id in _menu.images)
        {
            Image img = CreateImage(menuRect, id);
            menu.images.Add(img);

            UiSortData usd;
            usd.ui = img.transform;
            usd.layer = id.layer;
            if (tempList.Count > 0)
            {
                int tempI = 0;
                while (usd.layer < tempList[tempI].layer)
                {
                    tempI++;
                    if (tempI >= tempList.Count)
                        break;
                }
                tempList.Insert(tempI, usd);
            }
            else
            {
                tempList.Add(usd);
            }

        }

        if (_menu.menuType == MenuType.MainMenu)
            menu.gameObject.SetActive(true);
        else
            menu.gameObject.SetActive(false);


        tempUis.Enqueue(new KeyValuePair<Transform, List<UiSortData>>(menuRect, tempList));

        //foreach (UiSortData usd in tempList) { Debug.Log("" + usd.layer + ", : " + usd.ui.name); }

        return menu;
    }


    Button CreateButton(Transform _parent, UiData _button)
    {
        UiData iData = new UiData(_button);

        Image buttonImage = CreateImage(_parent, iData);

        Button button = buttonImage.gameObject.AddComponent<Button>();

        return button;
    }

    Image CreateImage(Transform _parent, UiData _image)
    {
        RectTransform uiRect = CreateRect(_parent, _image.name);

        Image image = uiRect.gameObject.AddComponent<Image>();

        image.color = _image.color;

        switch (_image.type)
        {
            case ImageType.Sprite:
                {
                    SetUiRect(uiRect, _image.position);
                    image.sprite = _image.sprite;
                    image.SetNativeSize();
                    break;
                }
            case ImageType.Color:
                {
                    if (_image.sizeType == ImageSizeType.FullScreen)
                        SetExpandedUiRect(uiRect);
                    else
                        SetUiRect(uiRect, _image.position);

                    break;
                }
            case ImageType.Text:
                {
                    SetUiRect(uiRect, _image.position);
                    CreateText(image.transform, _image);
                    break;
                }
            default:
                break;
        }


        return image;

    }

    Text CreateText(Transform _parent, UiData _image)
    {
        RectTransform uiRect = CreateRect(_parent, _image.name);

        Text text = uiRect.gameObject.AddComponent<Text>();

        text.color = Color.black;
        text.fontSize = _image.fontSize;
        text.alignment = TextAnchor.MiddleCenter;
        text.text = _image.text;

        SetUiRect(uiRect, _image.position);


        return text;

    }

    public RectTransform CreateRect(Transform _parent, string _name)
    {
        GameObject uiObj = new GameObject(_name);

        RectTransform uiRect = uiObj.AddComponent<RectTransform>();
        uiObj.transform.SetParent(_parent);

        uiObj.AddComponent<CanvasRenderer>();

        return uiRect;
    }

    public void SetUiRect(RectTransform _rect, Vector2 _position)
    {
        _rect.anchorMin = _rect.anchorMax = new Vector2(0.5f, 0.5f);

        _rect.localScale = Vector3.one;
        _rect.localPosition = (Vector3)_position;
        _rect.localEulerAngles = Vector3.zero;

    }
    public void SetUiRect(RectTransform _rect, Vector2 _position, float _width, float _height)
    {
        SetUiRect(_rect, _position);

        if ((_width > 0) && (_height > 0))
        {
            _rect.sizeDelta = new Vector2(_width, _height);
        }

    }
    public void SetExpandedUiRect(RectTransform _rect)
    {
        //_rect.localPosition = (Vector3)_position;

        _rect.anchorMin = Vector2.zero;
        _rect.anchorMax = Vector2.one;

        _rect.pivot = new Vector2(0.5f, 0.5f);

        _rect.offsetMax = Vector2.zero;
        _rect.offsetMin = Vector2.zero;

        _rect.localScale = Vector3.one;
        _rect.localEulerAngles = Vector3.zero;


    }


}


public struct UiSortData
{
    //public Transform parent;
    public Transform ui;
    public int layer;


}

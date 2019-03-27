using UnityEngine;
using System.Collections;
using UnityEditor;
using HenryTool;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

[CustomEditor(typeof(MenuData))]
[CanEditMultipleObjects]
public class MenuDataEditor : Editor
{
    const int MAX_UI_COUNT = 1000;
    MenuData mData;

    string mDescription;

    SerializedProperty edType;
    SerializedProperty edUis;
    SerializedProperty edImages;
    SerializedProperty edButtons;

    private FoldSetting[] btnSettings = new FoldSetting[MAX_UI_COUNT];
    private FoldSetting[] imgSettings = new FoldSetting[MAX_UI_COUNT];

    private Queue<UiData> buttonQueue = new Queue<UiData>();
    private Queue<UiData> imageQueue = new Queue<UiData>();

    int _buttonCount;
    public int buttonCount
    {
        get
        {
            return _buttonCount;
        }

        set
        {
            if (value < 0)
                _buttonCount = 0;
            else if (value > MAX_UI_COUNT)
                _buttonCount = MAX_UI_COUNT;
            else
                _buttonCount = value;

        }
    }

    int imageCount = 0;


    GUIStyle myStyle = new GUIStyle();
    GUIStyle imageStyle;
    GUIStyle textStyle;
    GUIStyle tempStyle;

    void OnEnable()
    {
        mData = (MenuData)target;
        edType = serializedObject.FindProperty("menuType");
        edUis = serializedObject.FindProperty("allUis");
        edButtons = serializedObject.FindProperty("buttons");
        edImages = serializedObject.FindProperty("Images");


        buttonCount = mData.buttons.Count;
        imageCount = mData.images.Count;



        EditorUtility.SetDirty(target);
    }


    bool buttonsExpanded = true;
    bool imagesExpanded = true;


    bool FoldOut(SerializedProperty _property)
    {
        return EditorGUILayout.PropertyField(_property);
    }
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        //EditorGUILayout.BeginHorizontal();
        //EditorGUILayout.EndHorizontal();


        myStyle = new GUIStyle(GUI.skin.button);
        myStyle.fontSize = 12;

        imageStyle = new GUIStyle(GUI.skin.box);
        textStyle = new GUIStyle(GUI.skin.box);
        tempStyle = new GUIStyle(GUI.skin.box);

        imageStyle.normal.background = MakeTex(2, 2, new Color(0.9f, 0.9f, 0.9f, 0.599f));
        //imageStyle.normal.background = MakeTex(100, 100, Color.white);
        textStyle.normal.background = MakeTex(2, 2, new Color(0.5f, 0.9f, 0.05f, 0.99f));
        tempStyle.normal.background = MakeTex(2, 2, new Color(0.5f, 0.9f, 0.05f, 0.99f));

        GUIStyle style = new GUIStyle();
        style.richText = true;

        GUILayout.Label("<color=#00000055><size=20>|| <color=#000000ee><b>" + target.name + "</b></color> ||</size></color>", style);

        EditorGUI.indentLevel += 4;
        ((MenuData)target).menuType = (MenuType)EditorGUILayout.EnumPopup(" ", ((MenuData)target).menuType);
        EditorGUI.indentLevel -= 4;


        EditorGUILayout.LabelField("");

        ShowButtonSettings();

        EditorGUILayout.LabelField("");

        ShowImageSettings();


        EditorGUILayout.LabelField("");



        serializedObject.ApplyModifiedProperties();
        EditorUtility.SetDirty(target);


    }


    void ShowOneButton(int _index)
    {
        bool drawButton = true;
        EditorGUILayout.BeginVertical(GUI.skin.box);
        EditorGUI.indentLevel++;

        EditorGUILayout.BeginHorizontal();
        btnSettings[_index].showContent = EditorGUILayout.Foldout(btnSettings[_index].showContent, "" + mData.buttons[_index].name);
        if (GUILayout.Button("DELETE", myStyle))
        {
            buttonQueue.Enqueue(mData.buttons[_index]);
            mData.buttons.RemoveAt(_index);
            buttonCount--;
            drawButton = false;
        }

        EditorGUILayout.BeginVertical(GUI.skin.box);
        //if (GUILayout.Button("^", myStyle)) { }
        //if (GUILayout.Button("v", myStyle)) { }

        EditorGUILayout.EndVertical();

        EditorGUILayout.EndHorizontal();

        if (drawButton)
        {
            if (btnSettings[_index].showContent)
            {
                EditorGUI.indentLevel++;
                UiData uiBtn = mData.buttons[_index];
                uiBtn.name = EditorGUILayout.TextField("Button Name: ", uiBtn.name);
                uiBtn.layer = EditorGUILayout.IntField("Layer: ", uiBtn.layer);

                uiBtn.type = (ImageType)EditorGUILayout.EnumPopup("Type: ", uiBtn.type);
                switch (uiBtn.type)
                {
                    case ImageType.Text:
                        {
                            uiBtn.sizeType = (ImageSizeType)EditorGUILayout.EnumPopup("Button Size: ", uiBtn.sizeType);
                            if (uiBtn.sizeType != ImageSizeType.FullScreen)
                                uiBtn.position = EditorGUILayout.Vector2Field("Position: ", uiBtn.position);
                            uiBtn.color = EditorGUILayout.ColorField("Color: ", uiBtn.color);
                            uiBtn.text = EditorGUILayout.TextField("Text: ", uiBtn.text);
                            uiBtn.fontSize = EditorGUILayout.IntField("Font Size: ", uiBtn.fontSize);
                            break;
                        }
                    case ImageType.Color:
                        {
                            uiBtn.sizeType = (ImageSizeType)EditorGUILayout.EnumPopup("Button Size: ", uiBtn.sizeType);
                            if (uiBtn.sizeType == ImageSizeType.SpriteSize)
                                uiBtn.sizeType = ImageSizeType.Other;

                            if (uiBtn.sizeType != ImageSizeType.FullScreen)
                                uiBtn.position = EditorGUILayout.Vector2Field("Position: ", uiBtn.position);

                            uiBtn.color = EditorGUILayout.ColorField("Color: ", uiBtn.color);

                            Texture2D myTexture = AssetPreview.GetAssetPreview(MakeTex(1000, 1000, uiBtn.color));
                            GUILayout.Label(myTexture);

                            break;
                        }
                    case ImageType.Sprite:
                        {
                            uiBtn.sizeType = (ImageSizeType)EditorGUILayout.EnumPopup("Button Size: ", uiBtn.sizeType);
                            if (uiBtn.sizeType != ImageSizeType.FullScreen)
                                uiBtn.position = EditorGUILayout.Vector2Field("Position: ", uiBtn.position);
                            uiBtn.color = EditorGUILayout.ColorField("Color: ", uiBtn.color);
                            uiBtn.sprite = (Sprite)EditorGUILayout.ObjectField("Sprite: ", uiBtn.sprite, typeof(Sprite), true);

                            Texture2D myTexture = AssetPreview.GetAssetPreview(uiBtn.sprite);
                            GUILayout.Label(myTexture);

                            break;
                        }
                    default: break;
                }


                uiBtn.action = (ButtonAction)EditorGUILayout.EnumPopup("Button Action: ", uiBtn.action);

                switch (uiBtn.action)
                {
                    case ButtonAction.ShowMenu:
                        {
                            uiBtn.goTo = (MenuData)EditorGUILayout.ObjectField("Open: ", uiBtn.goTo, typeof(MenuData), true);
                            break;
                        }
                    case ButtonAction.LoadScene:
                        {
                            break;
                        }
                    case ButtonAction.Other:
                        {
                            break;
                        }
                    default: break;
                }

                mData.buttons[_index] = uiBtn;
                EditorGUI.indentLevel--;

            }

        }


        EditorGUILayout.EndVertical();
        EditorGUI.indentLevel--;

    }

    void ShowButtonSettings()
    {
        #region Add buttons

        EditorGUILayout.BeginHorizontal();

        if (buttonCount < MAX_UI_COUNT)
        {
            if (GUILayout.Button("ADD a Button", myStyle))
                buttonCount++;
        }
        else
            GUILayout.Label("MAX Buttons", myStyle);

        EditorGUILayout.EndHorizontal();
        #endregion

        #region Check Button count.
        //buttonCount = EditorGUILayout.IntField("Button Count: ", buttonCount);

        if (buttonCount != mData.buttons.Count)
        {
            while (buttonCount > mData.buttons.Count)
            {
                if (buttonQueue.Count > 0)
                    mData.buttons.Add(buttonQueue.Dequeue());
                else if (mData.buttons.Count > 0)
                    mData.buttons.Add(new UiData(mData.buttons[mData.buttons.Count - 1]));
                else
                    mData.buttons.Add(new UiData(UiType.Button));

            }

            while (buttonCount < mData.buttons.Count)
            {
                buttonQueue.Enqueue(mData.buttons[buttonCount]);
                mData.buttons.RemoveAt(buttonCount);
            }
        }

        #endregion

        EditorGUI.indentLevel++;
        EditorGUILayout.BeginVertical(imageStyle);
        buttonsExpanded = EditorGUILayout.Foldout(buttonsExpanded, "Buttons: " + buttonCount);

        if (buttonsExpanded)
        {
            for (int i = 0; i < buttonCount; i++)
            {
                ShowOneButton(i);
            }

        }
        else
        {
            for (int i = 0; i < btnSettings.Length; i++)
            {
                btnSettings[i].showContent = false;
            }
        }

        EditorGUILayout.EndVertical();
        EditorGUI.indentLevel--;




    }


    void ShowOneImage(int _index)
    {
        bool drawImage = true;

        EditorGUILayout.BeginVertical(GUI.skin.box);
        EditorGUI.indentLevel++;

        EditorGUILayout.BeginHorizontal();
        imgSettings[_index].showContent = EditorGUILayout.Foldout(imgSettings[_index].showContent, "" + mData.images[_index].name);
        if (GUILayout.Button("DELETE", myStyle))
        {
            imageQueue.Enqueue(mData.images[_index]);
            mData.images.RemoveAt(_index);
            imageCount--;
            drawImage = false;
        }

        EditorGUILayout.BeginVertical(GUI.skin.box);
        //if (GUILayout.Button("^", myStyle)) { }
        //if (GUILayout.Button("v", myStyle)) { }

        EditorGUILayout.EndVertical();

        if (drawImage)
        {
            if (imgSettings[_index].showContent)
            {
                EditorGUI.indentLevel++;//缩进
                UiData imgd = mData.images[_index];
                imgd.name = EditorGUILayout.TextField("Image Name: ", mData.images[_index].name);
                imgd.layer = EditorGUILayout.IntField("Layer: ", mData.images[_index].layer);
                if (imgd.sizeType != ImageSizeType.FullScreen)
                    imgd.position = EditorGUILayout.Vector2Field("Position: ", mData.images[_index].position);

                imgSettings[_index].imageSettings = EditorGUILayout.Foldout(imgSettings[_index].imageSettings, "Image");
                if (imgSettings[_index].imageSettings)
                {
                    imgd.type = (ImageType)EditorGUILayout.EnumPopup("Type: ", mData.images[_index].type);
                    imgd.sizeType = (ImageSizeType)EditorGUILayout.EnumPopup("Size: ", mData.images[_index].sizeType);

                    switch (imgd.type)
                    {
                        case ImageType.Text:
                            {
                                imgd.color = EditorGUILayout.ColorField("Color: ", mData.images[_index].color);
                                imgd.text = EditorGUILayout.TextField("Text: ", mData.images[_index].text);
                                imgd.fontSize = EditorGUILayout.IntField("Font Size: ", mData.images[_index].fontSize);
                                break;
                            }
                        case ImageType.Color:
                            {
                                imgd.color = EditorGUILayout.ColorField("Color: ", mData.images[_index].color);
                                break;
                            }
                        case ImageType.Sprite:
                            {
                                imgd.color = EditorGUILayout.ColorField("Color: ", mData.images[_index].color);
                                imgd.sprite = (Sprite)EditorGUILayout.ObjectField("Sprite: ", mData.images[_index].sprite, typeof(Sprite), true);
                                break;
                            }
                        default: break;
                    }

                }

                mData.images[_index] = imgd;
                EditorGUI.indentLevel--;//恢复缩进
            }
        }


        EditorGUILayout.EndVertical();
        EditorGUI.indentLevel--;

    }

    void ShowImageSettings()
    {

        #region Add images

        EditorGUILayout.BeginHorizontal();

        if (imageCount < MAX_UI_COUNT)
        {
            if (GUILayout.Button("ADD an Image", myStyle))
                imageCount++;
        }
        else
            GUILayout.Label("MAX Images", myStyle);


        if (GUILayout.Button("", GUI.skin.box)) { }

        if (imageCount > 0)
        {
            if (GUILayout.Button("Remove the last Image", myStyle))
                imageCount--;
        }
        else
            GUILayout.Label("No Image", myStyle);


        EditorGUILayout.EndHorizontal();
        #endregion

        #region Check image count.
        //imageCount = EditorGUILayout.IntField("Image Count: ", imageCount);

        if (imageCount != mData.images.Count)
        {
            while (imageCount > mData.images.Count)
            {
                if (imageQueue.Count > 0)
                    mData.images.Add(imageQueue.Dequeue());
                else if (mData.images.Count > 0)
                    mData.images.Add(new UiData(mData.images[mData.images.Count - 1]));
                else
                    mData.images.Add(new UiData(UiType.Image));

            }

            while (imageCount < mData.images.Count)
            {
                imageQueue.Enqueue(mData.images[imageCount]);
                mData.images.RemoveAt(imageCount);
            }
        }

        #endregion

        EditorGUI.indentLevel++;
        EditorGUILayout.BeginVertical(imageStyle);
        imagesExpanded = EditorGUILayout.Foldout(imagesExpanded, "Images: " + imageCount.ToString());
        if (imagesExpanded)
        {
            for (int i = 0; i < imageCount; i++)
            {
                ShowOneImage(i);
            }

        }
        else
        {
            for (int i = 0; i < btnSettings.Length; i++)
            {
                imgSettings[i].showContent = false;
            }
        }

        EditorGUILayout.EndVertical();
        EditorGUI.indentLevel--;

    }


    struct FoldSetting
    {
        public bool showContent;
        public bool imageSettings;
        //public bool buttonSettings;

    }

    /*
    struct ShowImageSetting
    {
        public bool showContent;
        public bool imageSettings;

    }
    */

    private Texture2D MakeTex(int width, int height, Color col)
    {
        Color[] pix = new Color[width * height];
        for (int i = 0; i < pix.Length; ++i)
        {
            pix[i] = col;
        }
        Texture2D result = new Texture2D(width, height);
        result.SetPixels(pix);
        result.Apply();
        return result;
    }




}





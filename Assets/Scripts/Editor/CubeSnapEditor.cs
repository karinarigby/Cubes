using Tokidos;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;


public class CubeSnapEditor : EditorWindow
{
    [SerializeField] private string targetTag;
    
    [SerializeField] GameObject targetGameObject; //scene object to serialize
    private SerializedObject serializedGameObject;
    
    [SerializeField]
    private VisualTreeAsset m_VisualTreeAsset = default;

    [MenuItem("Tools/CubeSnapEditor")]
    public static void ShowWindow()
    {
        CubeSnapEditor wnd = GetWindow<CubeSnapEditor>();
    }
    
    private void OnEnable()
    {
        // Initialize serializedObject if a target GameObject is already assigned
        if (targetGameObject == null)
        {
            targetGameObject = GameObject.FindGameObjectWithTag(targetTag);
        }
        UpdateSerializedObject();
    }


    public void CreateGUI()
    {
        var tagField = new TextField("GameObject tag");
        tagField.value = targetTag;
        
        tagField.RegisterValueChangedCallback(evt =>
        {
            targetTag = evt.newValue as string;
            targetGameObject = GameObject.FindGameObjectWithTag(targetTag);
            UpdateSerializedObject();
        });
        
        var objectField = new ObjectField("Found Object")
        {
            objectType = typeof(GameObject),
            value = targetGameObject,
            allowSceneObjects = true,
            enabledSelf = false
        };
        
        rootVisualElement.Add(tagField);
        rootVisualElement.Add(objectField);

        // Instantiate UXML
        VisualElement labelFromUXML = m_VisualTreeAsset.Instantiate();

        rootVisualElement.Add(labelFromUXML);

        Button snapButton = rootVisualElement.Q<Button>("snap-button");
        Button unSnapButton = rootVisualElement.Q<Button>("unsnap-button");

        snapButton.clicked += SnapCubes;
        unSnapButton.clicked += UnSnap;
    }

    private void UpdateSerializedObject()
    {
        if (targetGameObject != null)
        {
            serializedGameObject = new SerializedObject(targetGameObject);
            Debug.Log("target Cubes game object set ");
            serializedGameObject.Update();
        }
        else
        {
            serializedGameObject = null;
        }
    }

    private void SnapCubes()
    {
        if (serializedGameObject == null)
        {
            Debug.LogWarning("No cubes selected");
            return;
        }
        
        var cubesControllerComponent = targetGameObject.GetComponent<CubesController>();
        if (cubesControllerComponent == null)
        {
            Debug.LogWarning($"No cubes controller component on {targetGameObject}");
            return;
        }
        
        serializedGameObject.Update(); // need to call this at some point since accessing over a different frame


        //snap to alignment
        cubesControllerComponent.SetCubesDisplayMaterial(cubesControllerComponent.mainCube.PrimaryMaterial);
        cubesControllerComponent.SetLastTransforms();
        cubesControllerComponent.SnapAllToXAxis();
        cubesControllerComponent.EnableMouse(false);
        
        Undo.RecordObject(cubesControllerComponent, "Snap Cubes");
        EditorUtility.SetDirty(cubesControllerComponent.persistentCubesDataSO);
        AssetDatabase.SaveAssets();
    }
    
    private void UnSnap()
    {
        if (serializedGameObject == null)
        {
            Debug.LogWarning("No cubes selected");
            return;
        }
        
        serializedGameObject.Update();// need to call this since accessing over a different frame 
        
        var cubesControllerComponent = targetGameObject.GetComponent<CubesController>();
        if (cubesControllerComponent == null)
        {
            Debug.LogWarning($"No cubes controller component on {targetGameObject}");
            return;
        }
        //if button
        cubesControllerComponent.ResetCubeMaterials();
        cubesControllerComponent.ResetTransforms();
        cubesControllerComponent.EnableMouse(true);

        Undo.RecordObject(cubesControllerComponent, "Unsnap");
        EditorUtility.SetDirty(cubesControllerComponent.persistentCubesDataSO);
        AssetDatabase.SaveAssets();
    }

}

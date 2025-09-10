using Tokidos;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor
{
    public class CubeSnapEditor : EditorWindow
    {
        [SerializeField] private string targetTag = "Cubes"; //the game object with tag to target ('Cubes')
        [SerializeField] GameObject targetGameObject; //the Cubes game object

        private SerializedObject _serializedGameObject;

        [SerializeField] private VisualTreeAsset m_VisualTreeAsset = default;

        [MenuItem("Tools/CubeSnapEditor")]
        public static void ShowWindow()
        {
            CubeSnapEditor wnd = GetWindow<CubeSnapEditor>();
        }

        private void OnEnable()
        {
            if (targetGameObject == null)
            {
                targetGameObject = GameObject.FindGameObjectWithTag(targetTag);
            }

            UpdateSerializedObject();
        }


        public void CreateGUI()
        {
            //Find the Cubes object in the hierarchy and make sure to update reference when changes made on domain reload
            var tagField = new TextField("GameObject tag");
            tagField.value = targetTag;

            tagField.RegisterValueChangedCallback(evt =>
            {
                targetTag = evt.newValue;
                targetGameObject = GameObject.FindGameObjectWithTag(targetTag);
                UpdateSerializedObject();
            });

            //So we can see the game object is set correctly
            var objectField = new ObjectField("Found Object")
            {
                objectType = typeof(GameObject),
                value = targetGameObject,
                allowSceneObjects = true,
                enabledSelf = false
            };

            rootVisualElement.Add(tagField);
            rootVisualElement.Add(objectField);

            VisualElement labelFromUXML = m_VisualTreeAsset.Instantiate();

            rootVisualElement.Add(labelFromUXML);

            var snapButton = rootVisualElement.Q<Button>("snap-button");
            var unSnapButton = rootVisualElement.Q<Button>("unsnap-button");

            snapButton.clicked += SnapCubes;
            unSnapButton.clicked += UnSnap;
        }

        private void SnapCubes()
        {
            if (_serializedGameObject == null)
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

            //snap to alignment
            cubesControllerComponent.SetCubesDisplayMaterial(cubesControllerComponent.mainCube.PrimaryMaterial);
            cubesControllerComponent.RecordTransforms();
            cubesControllerComponent.SnapAllToXAxis();
            cubesControllerComponent.EnableMouse(false);

            Undo.RecordObject(cubesControllerComponent, "Snap Cubes");
            EditorUtility.SetDirty(cubesControllerComponent.persistentCubesDataSO);
            AssetDatabase.SaveAssets();
        }

        private void UnSnap()
        {
            if (_serializedGameObject == null)
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

            cubesControllerComponent.ResetCubeMaterials();
            cubesControllerComponent.ResetTransforms();
            cubesControllerComponent.EnableMouse(true);

            Undo.RecordObject(cubesControllerComponent, "Unsnap");
            EditorUtility.SetDirty(cubesControllerComponent.persistentCubesDataSO);
            AssetDatabase.SaveAssets();
        }

        private void UpdateSerializedObject()
        {
            if (targetGameObject != null)
            {
                _serializedGameObject = new SerializedObject(targetGameObject);
                Debug.Log("target Cubes game object set ");
                _serializedGameObject.Update();
            }
            else
            {
                _serializedGameObject = null;
            }
        }
    }
}

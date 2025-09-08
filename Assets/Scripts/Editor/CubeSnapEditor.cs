using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class CubeSnapEditor : EditorWindow
{
    [SerializeField]
    private VisualTreeAsset m_VisualTreeAsset = default;

    [MenuItem("Tools/CubeSnapEditor")]
    public static void ShowExample()
    {
        CubeSnapEditor wnd = GetWindow<CubeSnapEditor>();
        wnd.titleContent = new GUIContent("CubeSnapEditor");
    }

    public void CreateGUI()
    {
        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;

        // VisualElements objects can contain other VisualElement following a tree hierarchy.

        // Instantiate UXML
        VisualElement labelFromUXML = m_VisualTreeAsset.Instantiate();
        root.Add(labelFromUXML);
        
        Button snapButton = root.Q<Button>("snap-button");
        Button unSnapButton = root.Q<Button>("unsnap-button");

        snapButton.clicked += () => SnapCubes();
        unSnapButton.clicked += () => UnSnapCubes();
    }

    private void UnSnapCubes()
    {
        throw new System.NotImplementedException();
    }

    private void SnapCubes()
    {
        // align cubes
        // change to main's color
        // disable mouse
        
        throw new System.NotImplementedException();
    }
}

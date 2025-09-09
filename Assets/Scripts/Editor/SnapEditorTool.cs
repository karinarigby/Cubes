using Tokidos;
using UnityEditor;
using UnityEditor.EditorTools;
using UnityEngine;

[EditorTool("Snap Cubes Tool", typeof(CubesController))]
public class SnapEditorTool : EditorTool
{
    // save colors and transforms
    // align cubes
    // all cubes change to main cubes color
    // disable mouse functionality
    private void OnEnable()
    {
        Debug.Log("SnapEditor on enable");
    }

    public override void OnToolGUI(EditorWindow window)
    {
        Debug.Log("SnapEditor on toolGUI");
    }

    public override void OnActivated()
    {
        Debug.Log("SnapEditor on activated");
        
        //if button
        if (target is CubesController cubesController)
        {
            cubesController.SetColorsSnapshot();
            cubesController.SetLastTransforms();
            cubesController.mouseEnabled = false;
        }
    }
}

[EditorTool("Unsnap Cubes Tool", typeof(CubesController))]
public class UnsnapEditorTool : EditorTool
{
    //apply saved color and transforms
    //enable mouse functionality
    
    public override void OnActivated()
    {
        Debug.Log("UnSnapEditor on activated");
        
        //if button
        if (target is CubesController cubesController)
        {
            cubesController.ApplyColorSnapshot();
            cubesController.ApplyTransformSnapshot();
            cubesController.mouseEnabled = true;
        }
    }
}
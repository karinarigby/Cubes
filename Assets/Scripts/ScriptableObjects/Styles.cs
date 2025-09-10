using UnityEngine;

[CreateAssetMenu(fileName = "StylesScriptableObject", menuName = "Scriptable Objects/StylesScriptableObject")]
public class Styles : ScriptableObject
{
    [SerializeField] public Color pastelRed;
    
    [SerializeField] public Color pastelOrange;

    [SerializeField] public Color pastelYellow;

    [SerializeField] public Color pastelGreen;
    
    [SerializeField] public Color pastelBlue;
    
    [SerializeField] public Color baseGrey;
}

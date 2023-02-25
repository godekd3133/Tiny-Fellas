
using Sirenix.OdinInspector;
using Unity.Netcode.Editor;
using UnityEditor;
using UnityEngine;



[CustomEditor(typeof(SerializedNetworkBehaviour), true)]
public class SerailizedNetworkBehaviourEditor : UnityEditor.Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        EditorOnlyModeConfigUtility.InternalOnInspectorGUI(target);
    }
}


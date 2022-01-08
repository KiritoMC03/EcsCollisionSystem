using UnityEditor;
using UnityEngine;

namespace EcsCollision
{
    [CustomEditor(typeof(RotationComponentProvider))]
    [CanEditMultipleObjects]
    public class RotationComponentProviderEditor : UnityEditor.Editor
    {
        private RotationComponentProvider[] current;

        public override void OnInspectorGUI()
        {
            for (var i = 0; i < current.Length; i++)
            {
                GUI.enabled = false;
                EditorGUILayout.Space();
                EditorGUILayout.Vector3Field("Value as Vector", current[i].value.value.eulerAngles);
                GUI.enabled = true;
            }
        }

        private void OnEnable()
        {
            var tempTargets = targets;
            current = new RotationComponentProvider[tempTargets.Length];
            for (var i = 0; i < tempTargets.Length; i++)
            {
                current[i] = targets[i] as RotationComponentProvider;
            }
        }

        private void OnSceneGUI()
        {
            for (var i = 0; i < current.Length; i++)
            {
                var temp = current[i];
                temp.value.value = temp.transform.rotation;
            }
        }
    }
}
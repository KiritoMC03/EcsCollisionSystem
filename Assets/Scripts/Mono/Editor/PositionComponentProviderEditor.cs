using UnityEditor;
using UnityEngine;

namespace EcsCollision
{
    [CustomEditor(typeof(PositionComponentProvider))]
    [CanEditMultipleObjects]
    public class PositionComponentProviderEditor : UnityEditor.Editor
    {
        private PositionComponentProvider[] current;

        public override void OnInspectorGUI()
        {
            for (var i = 0; i < current.Length; i++)
            {
                GUI.enabled = false;
                EditorGUILayout.Space();
                EditorGUILayout.Vector3Field("Value ", current[i].value.value);
                GUI.enabled = true;
            }
        }

        private void OnEnable()
        {
            var tempTargets = targets;
            current = new PositionComponentProvider[tempTargets.Length];
            for (var i = 0; i < tempTargets.Length; i++)
            {
                current[i] = targets[i] as PositionComponentProvider;
            }
        }

        private void OnSceneGUI()
        {
            for (var i = 0; i < current.Length; i++)
            {
                var temp = current[i];
                temp.value.value = temp.transform.position;
            }
        }
    }
}
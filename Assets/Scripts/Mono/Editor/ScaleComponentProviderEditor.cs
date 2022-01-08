using UnityEditor;
using UnityEngine;

namespace EcsCollision
{
    [CustomEditor(typeof(LossyScaleComponentProvider))]
    [CanEditMultipleObjects]
    public class ScaleComponentProviderEditor : UnityEditor.Editor
    {
        private LossyScaleComponentProvider[] current;

        public override void OnInspectorGUI()
        {
            for (var i = 0; i < current.Length; i++)
            {
                GUI.enabled = false;
                EditorGUILayout.Space();
                EditorGUILayout.Vector3Field("Local Scale ", current[i].transform.localScale);
                EditorGUILayout.Vector3Field("Lossy Scale ", current[i].value.value);
                GUI.enabled = true;
            }
        }

        private void OnEnable()
        {
            var tempTargets = targets;
            current = new LossyScaleComponentProvider[tempTargets.Length];
            for (var i = 0; i < tempTargets.Length; i++)
            {
                current[i] = targets[i] as LossyScaleComponentProvider;
            }
        }

        private void OnSceneGUI()
        {
            for (var i = 0; i < current.Length; i++)
            {
                var temp = current[i];
                temp.value.value = temp.transform.lossyScale;
            }
        }
    }
}
using UnityEditor;
using UnityEngine;

namespace EcsCollision
{
    [CustomEditor(typeof(RotationComponentProvider))]
    [CanEditMultipleObjects]
    public class QuaternionComponentProviderEditor : UnityEditor.Editor
    {
        private RotationComponentProvider[] current;

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
                temp.transform.rotation = Quaternion.Euler(temp.value.value);
            }
        }
    }
}
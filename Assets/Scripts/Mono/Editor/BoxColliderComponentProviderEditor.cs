using UnityEditor;
using UnityEngine;

namespace EcsCollision
{
    [CustomEditor(typeof(BoxColliderComponentProvider))]
    [CanEditMultipleObjects]
    public class BoxColliderComponentProviderEditor : UnityEditor.Editor
    {
        private BoxColliderComponentProvider[] current;

        private void OnEnable()
        {
            var tempTargets = targets;
            current = new BoxColliderComponentProvider[tempTargets.Length];
            for (var i = 0; i < tempTargets.Length; i++)
            {
                current[i] = targets[i] as BoxColliderComponentProvider;
            }
        }

        private void OnSceneGUI()
        {
            Handles.color = Color.Lerp(Color.white, Color.green, 0.5f);
            for (var i = 0; i < current.Length; i++)
            {
                var currentBox = current[i];
                var currentTransform = currentBox.transform;
                
                var rotationMatrix = Matrix4x4.TRS(currentTransform.localPosition, currentTransform.rotation.normalized, currentTransform.localScale);
                Handles.matrix = rotationMatrix;

                Handles.DrawWireCube( currentBox.value.position, currentBox.value.size);
            }
        }
    }
}
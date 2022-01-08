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
                var temp = current[i];
                var position = temp.value.position;
                if (temp.TryGetComponent(out PositionComponentProvider pose))
                {
                    position += pose.value.value;
                }
                
                if (temp.TryGetComponent(out RotationComponentProvider rotation))
                {
                    var rotationMatrix = Matrix4x4.TRS(position, Quaternion.Euler(rotation.value.value).normalized, Vector3.one);
                    Handles.matrix = rotationMatrix;
                }
                
                Handles.DrawWireCube(position, temp.value.size);
            }
        }
    }
}
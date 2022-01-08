using UnityEditor;
using UnityEngine;

namespace EcsCollision
{
    [CustomEditor(typeof(SphereColliderComponentProvider))]
    [CanEditMultipleObjects]
    public class SphereColliderProviderEditor : UnityEditor.Editor
    {
        private SphereColliderComponentProvider[] current;

        private void OnEnable()
        {
            var tempTargets = targets;
            current = new SphereColliderComponentProvider[tempTargets.Length];
            for (var i = 0; i < tempTargets.Length; i++)
            {
                current[i] = targets[i] as SphereColliderComponentProvider;
            }
        }

        private void OnSceneGUI()
        {
            Handles.color = Color.Lerp(Color.white, Color.green, 0.5f);
            for (var i = 0; i < current.Length; i++)
            {
                var currentSphere = current[i];
                var currentTransform = currentSphere.transform;
                
                var rotationMatrix = Matrix4x4.TRS(currentTransform.localPosition, currentTransform.rotation.normalized, currentTransform.localScale);
                Handles.matrix = rotationMatrix;
                
                currentSphere.value.radius = Handles.RadiusHandle(Quaternion.identity, currentSphere.value.center, currentSphere.value.radius);
            }
        }
    }
}
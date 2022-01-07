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
                var temp = current[i];
                var center = temp.value.center;
                if (temp.TryGetComponent(out PositionComponentProvider pose))
                {
                    center += pose.value.value;
                }
                temp.value.radius = Handles.RadiusHandle(Quaternion.identity, center, temp.value.radius);
            }
        }
    }
}
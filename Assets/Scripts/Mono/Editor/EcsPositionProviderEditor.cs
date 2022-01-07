using UnityEditor;

namespace EcsCollision
{
    [CustomEditor(typeof(PositionComponentProvider))]
    [CanEditMultipleObjects]
    public class EcsPositionProviderEditor : UnityEditor.Editor
    {
        private PositionComponentProvider[] current;

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
                temp.transform.position = temp.value.value;
            }
        }
    }
}
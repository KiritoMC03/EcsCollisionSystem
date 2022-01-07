using UnityEngine;
using Leopotam.Ecs;
using Leopotam.Ecs.UnityIntegration;
using Voody.UniLeo;

namespace EcsCollision.Editor
{
    public class EcsStartupEditor : MonoBehaviour
    {
        public EcsStartup mainStartup;
        private EcsWorld _world;
        private EcsSystems _onSceneGuiSystems;

        private void Start()
        {
            _world = mainStartup.world;
            _onSceneGuiSystems = new EcsSystems(_world);

            AddInjections();
            AddOneFrames();
            AddSystems();
            _onSceneGuiSystems.Init();

#if UNITY_EDITOR
            EcsSystemsObserver.Create(_onSceneGuiSystems);
#endif
        }

        private void OnDrawGizmos()
        {
            _onSceneGuiSystems?.Run();
        }

        private void OnDestroy()
        {
            _onSceneGuiSystems?.Destroy();
            _onSceneGuiSystems = null;
            _world?.Destroy();
            _world = null;
        }

        private void AddInjections()
        {

        }

        private void AddSystems()
        {
            _onSceneGuiSystems.Add(new DrawCollidersSystem());
        }

        private void AddOneFrames()
        {

        }
    }
}
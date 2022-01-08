using UnityEngine;
using Leopotam.Ecs;
using Leopotam.Ecs.UnityIntegration;
using Voody.UniLeo;

namespace EcsCollision
{
    public class EcsStartup : MonoBehaviour
    {
        public EcsWorld world;
        private EcsSystems _systems;

        private void Awake()
        {
            world = new EcsWorld();
        }

        private void Start()
        {
            _systems = new EcsSystems(world);
            _systems.ConvertScene();

#if UNITY_EDITOR
            EcsWorldObserver.Create(world);
#endif 
            AddInjections();
            AddOneFrames();
            AddSystems();
            _systems.Init();
            
#if UNITY_EDITOR
            EcsSystemsObserver.Create(_systems);
#endif
        }

        private void Update()
        {
            _systems?.Run();
        }
        
        private void OnDestroy()
        {
            _systems?.Destroy();
            _systems = null;
            world?.Destroy();
            world = null;
        }

        private void AddInjections()
        {

        }

        private void AddSystems()
        {
            _systems.Add(new GravitySystem());
            _systems.Add(new CalculateCollisionSystem());
            _systems.Add(new ApplyVelocitySystem());
            _systems.Add(new LogCollisionSystem());
        }

        private void AddOneFrames()
        {
            _systems.OneFrame<EcsCollisionEvent<SphereColliderComponent>>();
            _systems.OneFrame<EcsCollisionEvent<BoxColliderComponent>>();
        }
    }
}
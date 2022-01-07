using Leopotam.Ecs;
using UnityEngine;

namespace EcsCollision.Editor
{
    public sealed class DrawCollidersSystem : IEcsRunSystem
    {
        private readonly EcsWorld _world = null;
        private readonly EcsFilter<SphereColliderComponent> _sphereColliderFilter = null;
        
        public void Run()
        {
            foreach (var i in _sphereColliderFilter)
            {
                ref var current = ref _sphereColliderFilter.Get1(i);
                var offset = _sphereColliderFilter.GetEntity(i).Get<PositionComponent>().value;
                Gizmos.DrawWireSphere(current.center + offset, current.radius);
            }
        }
    }
}
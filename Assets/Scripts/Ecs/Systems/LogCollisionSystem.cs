using Leopotam.Ecs;
using UnityEngine;

namespace EcsCollision
{
    public sealed class LogCollisionSystem : IEcsRunSystem
    {
        private readonly EcsWorld _world = null;
        private readonly EcsFilter<EcsCollisionEvent<SphereColliderComponent>> _filter = null;

        public void Run()
        {
            foreach (var i in _filter)
            {
                var current = _filter.Get1(i);
                Debug.Log($"collision: A at {current.a.center} and B at {current.b.center}");
            }
        }
    }
}
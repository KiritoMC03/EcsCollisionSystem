using Leopotam.Ecs;
using UnityEngine;

namespace EcsCollision
{
    public sealed class GravitySystem : IEcsRunSystem
    {
        private readonly EcsWorld _world = null;
        private readonly EcsFilter<RigidbodyComponent>.Exclude<KinematicComponent> _filter = null;

        public void Run()
        {
            foreach (var i in _filter)
            {
                _filter.GetEntity(i).Get<VelocityComponent>().value += Physics.gravity * Time.fixedDeltaTime;
            }
        }
    }
}
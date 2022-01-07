using UnityEngine;
using Leopotam.Ecs;

namespace EcsCollision
{
    public sealed class ApplyVelocitySystem : IEcsRunSystem
    {
        private readonly EcsWorld _world = null;
        private readonly EcsFilter<VelocityComponent>.Exclude<KinematicComponent> _filter = null;

        public void Run()
        {
            foreach (var i in _filter)
            {
                _filter.GetEntity(i).Get<PositionComponent>().value += _filter.Get1(i).value * Time.fixedDeltaTime;
            }
        }
    }
}
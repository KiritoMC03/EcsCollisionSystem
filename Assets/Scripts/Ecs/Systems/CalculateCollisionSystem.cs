using Leopotam.Ecs;
using UnityEngine;

namespace EcsCollision
{
    public sealed class CalculateCollisionSystem : IEcsRunSystem
    {
        private readonly EcsWorld _world = null;
        private readonly EcsFilter<SphereColliderComponent> _filter = null;

        public void Run()
        {
            foreach (var i in _filter)
            {
                foreach (var j in _filter)
                {
                    if (i == j) continue;

                    if (SphereVsSphereOptimized(
                        _filter.Get1(i), _filter.GetEntity(i).Get<PositionComponent>().value,
                        _filter.Get1(j), _filter.GetEntity(j).Get<PositionComponent>().value))
                    {
                        var collisionEvent = _world.NewEntity().Get<EcsCollisionEvent<SphereColliderComponent>>();
                        collisionEvent.a = _filter.Get1(i);
                        collisionEvent.b = _filter.Get1(j);
                    }
                }
            }
        }

        #region Sphere Methods

        private bool SphereVsSphereOptimized(SphereColliderComponent a, Vector3 aPoseOffset, 
            SphereColliderComponent b, Vector3 bPoseOffset)
        {
            var radiusSum2 = a.radius + b.radius;
            radiusSum2 *= radiusSum2;

            var x2 = (a.center.x + aPoseOffset.x - (b.center.x + bPoseOffset.x)) *
                          (a.center.x + aPoseOffset.x - (b.center.x + bPoseOffset.x));
            
            var y2 = (a.center.y + aPoseOffset.y - (b.center.y + bPoseOffset.y)) *
                          (a.center.y + aPoseOffset.y - (b.center.y + bPoseOffset.y));
            
            var z2 = (a.center.z + aPoseOffset.z - (b.center.z + bPoseOffset.z)) *
                          (a.center.z + aPoseOffset.z - (b.center.z + bPoseOffset.z));

            return radiusSum2 > x2 + y2 + z2;
        }

        #endregion
    }
}
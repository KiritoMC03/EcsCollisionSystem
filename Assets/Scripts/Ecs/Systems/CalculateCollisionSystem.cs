using Leopotam.Ecs;
using UnityEngine;

namespace EcsCollision
{
    public sealed class CalculateCollisionSystem : IEcsRunSystem
    {
        private readonly EcsWorld _world = null;
        private readonly EcsFilter<SphereColliderComponent> _sphereFilter = null;
        private readonly EcsFilter<BoxColliderComponent> _boxFilter = null;

        public void Run()
        {
            var sphereIsOdd = ParseSemiEntities(_sphereFilter.GetEntitiesCount(), out var sphereNumber);
            var sphereNumber2 = sphereNumber;
            if (sphereIsOdd) sphereNumber2 = sphereNumber + 1;

            for (var i = 0; i < sphereNumber; i++)
            {
                for (var j = 0; j < sphereNumber2; j++)
                {
                    if (i == j) continue;

                    if (SphereVsSphereOptimized(
                        _sphereFilter.Get1(i), _sphereFilter.GetEntity(i).Get<PositionComponent>().value,
                        _sphereFilter.Get1(j), _sphereFilter.GetEntity(j).Get<PositionComponent>().value))
                    {
                        var collisionEvent = _world.NewEntity().Get<EcsCollisionEvent<SphereColliderComponent>>();
                        collisionEvent.a = _sphereFilter.Get1(i);
                        collisionEvent.b = _sphereFilter.Get1(j);
                    }
                }
            }

            var boxIsOdd = ParseSemiEntities(_boxFilter.GetEntitiesCount(), out var boxNumber);
            var boxNumber2 = boxNumber * 2;
            if (!boxIsOdd) boxNumber2++;

            for (var i = 0; i < boxNumber; i++)
            {
                for (var j = boxNumber; j < boxNumber2; j++)
                {
                    if (i == j) continue;

                    
                    if (ObbVsObb(_boxFilter.Get1(i), _boxFilter.GetEntity(i),
                                 _boxFilter.Get1(j), _boxFilter.GetEntity(j)))
                    {
                        var collisionEvent = _world.NewEntity().Get<EcsCollisionEvent<BoxColliderComponent>>();
                        collisionEvent.a = _boxFilter.Get1(i);
                        collisionEvent.b = _boxFilter.Get1(j);
                    }

                }
            }
        }

        private bool ParseSemiEntities(int number, out int result)
        {
            result = number / 2;
            return number % 2 == 0;
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

        private bool ObbVsObb(BoxColliderComponent a, EcsEntity aEntity, 
            BoxColliderComponent b, EcsEntity bEntity)
        {
            var aPosition = aEntity.Get<PositionComponent>().value;
            var bPosition = bEntity.Get<PositionComponent>().value;
            var aRotation = aEntity.Get<RotationComponent>().value;
            var bRotation = bEntity.Get<RotationComponent>().value;
            var aScale = aEntity.Get<LossyScaleComponent>().value;
            var bScale = bEntity.Get<LossyScaleComponent>().value;
            aScale = new Vector3(aScale.x * a.size.x, aScale.y * a.size.y, aScale.z * a.size.z);
            bScale = new Vector3(bScale.x * a.size.x, bScale.y * a.size.y, bScale.z * a.size.z);
            
            var obb1 = new OBB(a.position + aPosition, aRotation, aScale);
            var obb2 = new OBB(b.position + bPosition, bRotation, bScale);

            
            if (OBBIntersectionTester.PreTest(obb1, obb2))
            {
                return OBBIntersectionTester.Test(obb1, obb2);
            }

            return false;
        }
        
        #endregion
    }
}
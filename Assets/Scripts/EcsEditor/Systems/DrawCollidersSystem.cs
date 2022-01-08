using Leopotam.Ecs;
using UnityEngine;

namespace EcsCollision.Editor
{
    public sealed class DrawCollidersSystem : IEcsRunSystem
    {
        private readonly EcsWorld _world = null;
        private readonly EcsFilter<SphereColliderComponent> _sphereColliderFilter = null;
        private readonly EcsFilter<BoxColliderComponent> _boxColliderFilter = null;
        
        public void Run()
        {
            for (var i = 0; i < _sphereColliderFilter.GetEntitiesCount(); i++)
            {
                var currentSphere = _sphereColliderFilter.Get1(i);
                
                Gizmos.matrix = CalculateMatrix(_sphereColliderFilter.GetEntity(i));
                Gizmos.DrawWireSphere(currentSphere.center, currentSphere.radius);
            }

            for (var i = 0; i < _boxColliderFilter.GetEntitiesCount(); i++)
            {
                var currentBox = _boxColliderFilter.Get1(i);
                 
                Gizmos.matrix = CalculateMatrix(_boxColliderFilter.GetEntity(i));
                Gizmos.DrawWireCube(currentBox.position, currentBox.size);
            }
        }

        private Matrix4x4 CalculateMatrix(EcsEntity entity)
        {
            var position = entity.Get<PositionComponent>();
            var rotation = entity.Get<RotationComponent>();
            return Matrix4x4.TRS(position.value, rotation.value.normalized, Vector3.one);
        }
    }
}
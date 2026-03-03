using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class EnemyAuthoring : MonoBehaviour
{
    [Header("移动设置")]
    public float MoveSpeed = 3.0f;
    [Tooltip("距离玩家多远处停止")]
    public float StoppingDistance = 1.5f;

    [Header("属性")]
    public int Health = 30;
    public int Damage = 10;

    class Baker : Baker<EnemyAuthoring>
    {
        public override void Bake(EnemyAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);

            // 1. 核心标记与逻辑组件
            AddComponent<EnemyTag>(entity);
            AddComponent(entity, new EnemyMovement
            {
                MoveSpeed = authoring.MoveSpeed,
                StoppingDistance = authoring.StoppingDistance,
                TargetPosition = float3.zero
            });
            AddComponent(entity, new EnemyStats
            {
                Health = authoring.Health,
                Damage = authoring.Damage
            });

        }
    }
}
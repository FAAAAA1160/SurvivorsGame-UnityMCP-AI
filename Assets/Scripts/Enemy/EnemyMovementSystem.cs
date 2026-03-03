using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using UnityEngine;

[UpdateInGroup(typeof(SimulationSystemGroup))]
public partial class EnemyMovementSystem : SystemBase
{
    protected override void OnUpdate()
    {
        float deltaTime = SystemAPI.Time.DeltaTime;

        // 获取玩家位置
        var playerEntity = SystemAPI.GetSingletonEntity<PlayerTag>();
        var playerTransform = SystemAPI.GetComponent<LocalTransform>(playerEntity);
        var playerPos = playerTransform.Position;

        Entities
            .WithAll<EnemyTag>()
            .ForEach((ref LocalTransform transform, in EnemyMovement movement) =>
            {
                // 计算指向玩家的向量
                float3 toPlayer = playerPos - transform.Position;
                toPlayer.y = 0; // 锁定Y轴
                float distanceToPlayer = math.length(toPlayer);

                // 核心逻辑：只有当距离大于停止距离时才移动
                if (distanceToPlayer >= movement.StoppingDistance)
                {
                    //Debug.Log(movement.StoppingDistance +"|"+ distanceToPlayer);
                    // 计算归一化的移动方向
                    float3 moveDirection = math.normalizesafe(toPlayer, float3.zero);

                    // 计算本帧应移动的距离
                    float3 moveAmount = moveDirection * movement.MoveSpeed * deltaTime;

                    // 直接应用移动：修改位置
                    transform.Position += moveAmount;

                    // 让敌人面朝移动方向
                    transform.Rotation = quaternion.LookRotation(moveDirection, math.up());
                }
                // 如果在停止距离内，敌人将停止移动
            }).ScheduleParallel();
    }
}
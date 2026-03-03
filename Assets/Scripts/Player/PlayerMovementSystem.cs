using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

[UpdateInGroup(typeof(SimulationSystemGroup))]
public partial class PlayerMovementSystem : SystemBase
{
    protected override void OnUpdate()
    {
        // 获取输入
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        float2 input = new float2(horizontal, vertical);

        // 归一化输入
        if (math.length(input) > 1)
            input = math.normalize(input);

        float deltaTime = SystemAPI.Time.DeltaTime;

        // 更新玩家位置
        Entities
            .WithAll<PlayerTag>()
            .ForEach((ref LocalTransform transform, in PlayerMovement movement) =>
            {
                // 计算移动向量
                float3 moveDirection = new float3(input.x, 0, input.y);
                float3 moveAmount = moveDirection * movement.MoveSpeed * deltaTime;

                // 应用移动
                transform.Position += moveAmount;

                // 朝向移动方向
                if (math.length(moveDirection) > 0.1f)
                {
                    transform.Rotation = quaternion.LookRotation(moveDirection, math.up());
                }
            }).Schedule();
    }
}
using Unity.Entities;
using Unity.Mathematics;

// 玩家标记组件
public struct PlayerTag : IComponentData { }

// 玩家移动组件
public struct PlayerMovement : IComponentData
{
    public float MoveSpeed;
    public float2 InputDirection;
}

// 玩家统计数据
public struct PlayerStats : IComponentData
{
    public int Health;
    public int MaxHealth;
}

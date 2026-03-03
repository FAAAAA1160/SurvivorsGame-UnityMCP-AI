using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;


// 敌人标记组件
public struct EnemyTag : IComponentData { }

// 敌人移动与AI配置组件
public struct EnemyMovement : IComponentData
{
    public float MoveSpeed;         // 最大移动速度
    public float StoppingDistance;  // 停止距离
    public float3 TargetPosition;   // 目标位置（通常为玩家）
}

// 敌人统计数据
public struct EnemyStats : IComponentData
{
    public int Health;
    public int Damage;
    public int Test;
}
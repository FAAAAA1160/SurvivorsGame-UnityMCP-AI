using Unity.Entities;
using Unity.Mathematics;

// 生成器组件
public struct EnemySpawner : IComponentData
{
    public Entity EnemyPrefab;
    public float SpawnInterval;
    public float Timer;
    public int MaxEnemies;
    public int CurrentEnemies;
    public float SpawnRadius;
    public int SpawnBatchCount;  // 每次刷新的数量
    public int EnemyTypes;        // 敌人类别标识（可用于随机选择）
}

// 生成位置组件
public struct SpawnPosition : IComponentData
{
    public float3 Value;
}
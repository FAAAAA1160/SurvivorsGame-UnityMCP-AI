using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public partial class GameBootstrapSystem : SystemBase
{
    private Entity playerPrefab;
    private Entity enemyPrefab;

    protected override void OnCreate()
    {
        // 创建玩家实体
        //CreatePlayer();

        // 创建敌人生成器
        CreateEnemySpawner();
    }

    private void CreatePlayer()
    {
        // 创建玩家实体
        Entity playerEntity = EntityManager.CreateEntity();

        EntityManager.AddComponentData(playerEntity, new PlayerTag());
        EntityManager.AddComponentData(playerEntity, new PlayerMovement
        {
            MoveSpeed = 5f,
            InputDirection = float2.zero
        });
        EntityManager.AddComponentData(playerEntity, new PlayerStats
        {
            Health = 100,
            MaxHealth = 100
        });
        EntityManager.AddComponentData(playerEntity, LocalTransform.FromPosition(float3.zero));
    }

    private void CreateEnemySpawner()
    {
        // 创建生成器实体
        Entity spawnerEntity = EntityManager.CreateEntity();

        // 注意：在实际项目中，需要先创建敌人Prefab Entity
        // 这里用Entity.Null占位，实际使用时会通过ConvertToEntity生成
        EntityManager.AddComponentData(spawnerEntity, new EnemySpawner
        {
            EnemyPrefab = Entity.Null, // 需要通过ConvertToEntity设置
            SpawnInterval = 0.5f,
            Timer = 0f,
            MaxEnemies = 50,
            CurrentEnemies = 0,
            SpawnRadius = 20f
        });
    }

    protected override void OnUpdate() { }
}
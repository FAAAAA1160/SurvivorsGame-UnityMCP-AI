using Unity.Entities;
using UnityEngine;

public class GameBootstrap : MonoBehaviour
{
    private void Start()
    {
        // 确保默认World存在
        var world = World.DefaultGameObjectInjectionWorld;

        // 创建系统
        world.CreateSystem<GameBootstrapSystem>();
        world.CreateSystem<PlayerMovementSystem>();
        world.CreateSystem<EnemySpawnSystem>();
        world.CreateSystem<EnemyMovementSystem>();
    }
}
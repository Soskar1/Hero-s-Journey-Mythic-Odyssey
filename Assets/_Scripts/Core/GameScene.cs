using HerosJourney.Core.WorldGeneration;
using UnityEngine;

namespace HerosJourney.Core
{
    public class GameScene : MonoBehaviour
    {
        [SerializeField] private GameObject _player;
        [SerializeField] private World _world;

        private void OnEnable() => _world.OnNewChunksGenerated += SpawnPlayer;   

        private void SpawnPlayer()
        {
            _player.SetActive(true);
            _world.OnNewChunksGenerated -= SpawnPlayer;
        } 
    }
}
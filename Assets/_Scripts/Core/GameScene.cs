using HerosJourney.Core.WorldGeneration;
using UnityEngine;

namespace HerosJourney.Core
{
    public class GameScene : MonoBehaviour
    {
        [SerializeField] private GameObject _player;
        [SerializeField] private World _world;

        private void OnEnable() => _world.OnNewChunksInitialized += SpawnPlayer;   

        private void SpawnPlayer()
        {
            _player.SetActive(true);
            _world.OnNewChunksInitialized -= SpawnPlayer;
        } 
    }
}
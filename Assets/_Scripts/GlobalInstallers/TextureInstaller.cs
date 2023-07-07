using HerosJourney.Utils;
using UnityEngine;
using Zenject;

namespace HerosJourney.GlobalInstallers
{
    public class TextureInstaller : MonoInstaller
    {
        [SerializeField] private Texture2D _atlas;
        [SerializeField] private int _tileSize;

        public override void InstallBindings()
        {
            UVMapping.Initialize(_atlas, _tileSize);
        }
    }
}
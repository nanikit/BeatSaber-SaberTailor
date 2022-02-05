using System;
using Zenject;
using BeatSaberMarkupLanguage.GameplaySetup;

namespace SaberTailor.UI
{
    public class TabButtonUI : IInitializable
    {
        private readonly MainFlowCoordinator _mainFlowCoordinator;

        public TabButtonUI(MainFlowCoordinator mainFlowCoordinator)
        {
            _mainFlowCoordinator = mainFlowCoordinator;
        }
        public void Initialize()
        {
            GameplaySetup.instance.AddTab("TweakSaber", "SaberTailor.UI.Views.LeftSideProfileMenu.bsml", this);
        }

        public void Deinit()
        {
            if (GameplaySetup.instance != null)
            {
                GameplaySetup.instance.RemoveTab("TweakSaber");
            }
        }
    }
}

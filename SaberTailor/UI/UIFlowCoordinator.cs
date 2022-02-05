using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenject;
using HMUI;
using BeatSaberMarkupLanguage;
using SaberTailor.UI;
using IPA.Logging;
using SaberTailor.Settings;
using BeatSaberMarkupLanguage.MenuButtons;
using BeatSaberMarkupLanguage.GameplaySetup;

namespace SaberTailor.UI
{
    public class UIFlowCoordinator : FlowCoordinator
    {
        private MainFlowCoordinator _mainFlowCoordinator;

        //Made these public and static to be able to be used in SaberUtils (this is probably a stupid idea and I hope someone tells me why)... idk how else to condense the code into a Utils file...
        public static LeftController _leftControllerController;
        public static MiddleSettingsPaneController _middleSettingsPaneController;
        public static RightController _rightControllerController;

        [Inject]
        public void Construct(MainFlowCoordinator mainFlowCoordinator, LeftController leftControllerController, MiddleSettingsPaneController middleSettingsPaneController, RightController rightControllerController)
        {
            _mainFlowCoordinator = mainFlowCoordinator;
            _leftControllerController = leftControllerController;
            _middleSettingsPaneController = middleSettingsPaneController;
            _rightControllerController = rightControllerController;
        }

        protected override void DidActivate(bool firstActivation, bool addedToHierarchy, bool screenSystemEnabling)
        {
            try
            {
                if (firstActivation)
                {
                    SetTitle("SaberTailor");
                    _leftControllerController = BeatSaberUI.CreateViewController<LeftController>();
                    _middleSettingsPaneController = BeatSaberUI.CreateViewController<MiddleSettingsPaneController>();
                    _rightControllerController = BeatSaberUI.CreateViewController<RightController>();
                    ProvideInitialViewControllers(_middleSettingsPaneController, _leftControllerController, _rightControllerController);
                    showBackButton = true;
                }
            }
            catch (Exception ex)
            {
                Logger.log.Error(ex);
            }
        }

        public static UIFlowCoordinator flow = null;
        static MenuButton theButton;
        
        
        //This is probably useless right now.
        //static LeftSideProfileMenuController leftTabThing;

        public static void Initialize()
        {
            //TODO: This will be added some day, probably not today....
            //GameplaySetup.instance.AddTab("SaberTailor", "SaberTailor.UI.Views.LeftSideProfileMenu.bsml", leftTabThing);
            MenuButtons.instance.RegisterButton(theButton ??= new MenuButton("SaberTailor", "Tweak things about your sabers, including grip, and trail length!", () => {
                if (flow == null)
                    flow = BeatSaberUI.CreateFlowCoordinator<UIFlowCoordinator>();

                flow.ShowFlow();
            }, true));
        }

        private void ShowFlow()
        {
            var _parentFlow = BeatSaberUI.MainFlowCoordinator.YoungestChildFlowCoordinatorOrSelf();

            BeatSaberUI.PresentFlowCoordinator(_parentFlow, this);
        }

        protected override void BackButtonWasPressed(ViewController topViewController)
        {
            Configuration.Save();
            Configuration.UpdateModVariables();
            BeatSaberUI.MainFlowCoordinator.DismissFlowCoordinator(this, null, ViewController.AnimationDirection.Vertical);
        }

        internal static void Deinit()
        {
            if (theButton != null)
                MenuButtons.instance.UnregisterButton(theButton);
        }
    }
    public static class BSMLStuff
    {
        public static void EnableMenu()
        {
            UIFlowCoordinator.Initialize();
        }

        public static void DisableMenu()
        {
            UIFlowCoordinator.Deinit();
        }
    }
}

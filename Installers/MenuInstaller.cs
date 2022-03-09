using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SaberTailor.UI;
using IPA.Logging;
using Zenject;

namespace SaberTailor.Installers
{
    internal class MenuInstaller : Installer
    {
        public override void InstallBindings()
        {
            /*
            Container.BindInterfacesTo<TabButtonUI>().AsSingle();
            Container.Bind<UIFlowCoordinator>().FromNewComponentOnNewGameObject().AsSingle();
            Container.Bind<LeftController>().FromNewComponentAsViewController().AsSingle();
            Container.Bind<MiddleSettingsPaneController>().FromNewComponentAsViewController().AsSingle();
            Container.Bind<RightController>().FromNewComponentAsViewController().AsSingle();
            Container.BindInterfacesTo<MenuButtonUI>().AsSingle();
            //Container.BindInterfacesTo<EventService>().AsSingle();
            */
        }
    }
}

using System;
using ReactiveToDoSample.Managers;
using ReactiveToDoSample.ViewModels;
using ReactiveToDoSample.Views;
using ReactiveUI;
using Sextant;
using Sextant.XamForms;
using Splat;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static Sextant.Sextant;

namespace ReactiveToDoSample
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            RxApp.DefaultExceptionHandler = new RxExceptionHandler();

            Instance.InitializeForms();

            Locator
               .CurrentMutable
               .RegisterConstant<IItemManager>(new ItemManager());

            Locator
               .CurrentMutable
               .RegisterNavigationView(() => new NavigationView(RxApp.MainThreadScheduler, RxApp.TaskpoolScheduler, ViewLocator.Current))
               .RegisterParameterViewStackService()
               .RegisterView<HomePage, HomeViewModel>()
               .RegisterView<ItemPage, ItemViewModel>()
               .RegisterViewModel(() => new HomeViewModel(Locator.Current.GetService<IParameterViewStackService>(), Locator.Current.GetService<IItemManager>()))
               .RegisterViewModel(() => new ItemViewModel(Locator.Current.GetService<IParameterViewStackService>(), Locator.Current.GetService<IItemManager>()));

            Locator
                .Current
                .GetService<IParameterViewStackService>()
                .PushPage<HomeViewModel>(null, true, false)
                .Subscribe();

            MainPage = Locator.Current.GetNavigationView("NavigationView");
        }
    }
}

using System;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using ReactiveToDoSample.Managers;
using ReactiveToDoSample.Models;
using ReactiveUI;
using Sextant;

namespace ReactiveToDoSample.ViewModels
{
    public class ItemViewModel : ViewModelBase
    {
        public ItemViewModel(IParameterViewStackService navigationService, IItemManager itemManager) : base(navigationService)
        {
            _itemManager = itemManager;

            var canExecute = this.WhenAnyValue(x => x.Title, (title) => !string.IsNullOrEmpty(title));

            SaveCommand = ReactiveCommand.Create(ExecuteSave, canExecute);

            CloseCommand = ReactiveCommand.CreateFromObservable(() => NavigationService.PopModal());

            SaveCommand
                .InvokeCommand(CloseCommand)
                .DisposeWith(Subscriptions);

            this.WhenAnyValue(x => x.ItemId)
                .Where(x => x != null)
                .Select(x => _itemManager.Get(x))
                .Where(x => x.HasValue)
                .Select(x => x.Value)
                .Subscribe(x =>
                {
                    Title = x.Title;

                })
               .DisposeWith(Subscriptions);
        }

        public override IObservable<Unit> WhenNavigatingTo(INavigationParameter parameter)
        {
            if (parameter.TryGetValue(NavigationParameterConstants.ItemId, out string itemId))
            {
                ItemId = itemId;
            }

            return base.WhenNavigatedTo(parameter);
        }

        private void ExecuteSave() => _itemManager.AddOrUpdate(new Item(ItemId ?? Guid.NewGuid().ToString(), Title));

        public ReactiveCommand<Unit, Unit> SaveCommand { get; }

        public ReactiveCommand<Unit, Unit> CloseCommand { get; }

        public override string Id => string.Empty;

        public string Title
        {
            get => _title;
            set => this.RaiseAndSetIfChanged(ref _title, value);
        }

        private string ItemId
        {
            get => _itemId;
            set => this.RaiseAndSetIfChanged(ref _itemId, value);
        }

        private string _title;
        private string _description;
        private readonly IItemManager _itemManager;
        private string _itemId;
    }
}

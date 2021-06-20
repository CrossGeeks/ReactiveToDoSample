using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using DynamicData;
using ReactiveToDoSample.Managers;
using ReactiveToDoSample.Models;
using ReactiveUI;
using Sextant;

namespace ReactiveToDoSample.ViewModels
{
    public class HomeViewModel : ViewModelBase
    {
        public HomeViewModel(IParameterViewStackService navigationService, IItemManager itemManager) : base(navigationService)
        {
           DeleteCommand = ReactiveCommand.Create<Item>(itemManager.Remove);

           itemManager
                .ItemChanges
                .Bind(out _items)
                .DisposeMany()
                .Subscribe()
                .DisposeWith(Subscriptions);

            itemManager.AddOrUpdate(new Item(Guid.NewGuid().ToString(), "Family vacation planning"));
            itemManager.AddOrUpdate(new Item(Guid.NewGuid().ToString(), "Buy Christmas Gifts"));
            itemManager.AddOrUpdate(new Item(Guid.NewGuid().ToString(), "Go to the Bank"));
            itemManager.AddOrUpdate(new Item(Guid.NewGuid().ToString(), "Buy Milk"));

            AddCommand = ReactiveCommand.CreateFromObservable(() => NavigationService.PushModal<ItemViewModel>());

            ViewCommand = ReactiveCommand.CreateFromObservable<Item, Unit>((item) =>
            {
                SelectedItem = null;
                return NavigationService.PushModal<ItemViewModel>(new NavigationParameter()
                                {
                                    { NavigationParameterConstants.ItemId , item.Id }
                                });
            });

            this.WhenAnyValue(x => x.SelectedItem)
                .Where(x => x != null)
                .InvokeCommand(ViewCommand)
                .DisposeWith(Subscriptions);

        }

        public ReactiveCommand<Unit, Unit> AddCommand { get; }

        public ReactiveCommand<Item, Unit> ViewCommand { get; }

        public ReactiveCommand<Item, Unit> DeleteCommand { get; }

        public Item SelectedItem
        {
            get => _selectedItem;
            set => this.RaiseAndSetIfChanged(ref _selectedItem, value);
        }


        public ReadOnlyObservableCollection<Item> Items => _items;

        public override string Id => "Reactive ToDo";

        private readonly ReadOnlyObservableCollection<Item> _items;
        private Item _selectedItem;
    }
}
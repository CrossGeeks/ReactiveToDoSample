using System;
using DynamicData;
using DynamicData.Kernel;
using ReactiveToDoSample.Models;

namespace ReactiveToDoSample.Managers
{
    public class ItemManager : IItemManager
    {
        public ItemManager()
        {
            ItemChanges = _itemsCache.Connect()
                                     .RefCount();
        }

        public Optional<Item> Get(string id) => _itemsCache.Lookup(id);

        public IObservable<IChangeSet<Item, string>> ItemChanges { get; }

        public void AddOrUpdate(Item item) => _itemsCache.AddOrUpdate(item);

        public void Remove(Item item) => _itemsCache.Remove(item);

        private SourceCache<Item, string> _itemsCache = new SourceCache<Item, string>(item => item.Id);
    }
}

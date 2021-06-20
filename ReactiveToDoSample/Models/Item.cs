using ReactiveUI;

namespace ReactiveToDoSample.Models
{
    public class Item : ReactiveObject
    {
        public Item(string id, string title)
        {
            Id = id;
            Title = title;
        }

        public string Id { get; }

        public string Title { get; }

        public bool IsCompleted
        {
            get => _isCompleted;
            set => this.RaiseAndSetIfChanged(ref _isCompleted, value);
        }

        private bool _isCompleted;
    }
}

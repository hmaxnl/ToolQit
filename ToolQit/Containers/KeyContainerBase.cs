using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ToolQit.Containers
{
    public abstract class KeyContainerBase<TContainer> where TContainer : KeyContainerBase<TContainer>, new()
    {
        protected KeyContainerBase(char separator = '.')
        {
            _separator = separator;
        }

        private readonly char _separator;
        private readonly Dictionary<string, TContainer> _containers = new Dictionary<string, TContainer>();
        public ReadOnlyDictionary<string, TContainer> Containers => new ReadOnlyDictionary<string, TContainer>(_containers);
        
        public TContainer this[string key]
        {
            get
            {
                if (key.Contains(_separator))
                    return AddFromQueue(new KeyQueue(key, _separator));
                if (!_containers.ContainsKey(key))
                    AddContainer(key, new TContainer());
                return _containers[key];
            }
        }
        public void AddContainer(string key, TContainer container) => _containers[key] = container;
        public bool RemoveContainer(string key) => _containers.Remove(key);
        public TContainer GetContainer(string key) => _containers[key];

        TContainer AddFromQueue(KeyQueue queue)
        {
            if (queue.IsEmpty) return (TContainer)this;
            string queueKey = queue.Next();
            AddContainer(queueKey, new TContainer());
            return _containers[queueKey].AddFromQueue(queue);
        }
    }
    
    internal class KeyQueue
    {
        public KeyQueue(string key, char divider)
        {
            _key = new Queue<string>(key.Split(divider, StringSplitOptions.RemoveEmptyEntries));
        }
        public string Next() => _key.Dequeue();
        public bool IsEmpty => _key.Count <= 0;

        private readonly Queue<string> _key;
    }
}
using System;
using System.Collections.Generic;

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
        public TContainer this[string key]
        {
            get
            {
                if (key.Contains(_separator))
                    return AddFromQueue(new KeyQueue(key, _separator));
                AddContainer(key, new TContainer());
                return _containers[key];
            }
        }
        public void AddContainer(string key, TContainer container)
        {
            if (!_containers.ContainsKey(key))
                _containers.Add(key, container);
        }
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
using System;
using System.Collections.Generic;
using ToolQit.Logging;

namespace ToolQit.Containers
{
    /// <summary>
    /// Simple container to store instances.
    /// </summary>
    public class InstanceContainer : IDisposable
    {
        public InstanceContainer()
        {
            _log = LogManager.CreateLogger(nameof(InstanceContainer));
        }
        private readonly ILog _log;
        private readonly Dictionary<string, InstanceNode> _tCollection = new Dictionary<string, InstanceNode>();

        /// <summary>
        /// Register a instance that is not yet constructed.
        /// </summary>
        /// <param name="key">Key</param>
        /// <typeparam name="TInstance">The instance that need to be stored.</typeparam>
        public void Register<TInstance>(string key) => _tCollection.Add(key, new InstanceNode(null, typeof(TInstance)));

        /// <summary>
        /// Add an already constructed instance to the container.
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="instance">Constructed instance to store.</param>
        public void Add(string key, object instance) => _tCollection.Add(key, new InstanceNode(instance, instance.GetType()));

        /// <summary>
        /// Remove instance from the container.
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="dispose">Dispose the instance if it inherits the 'IDisposable' interface.</param>
        public void Remove(string key, bool dispose = true)
        {
            if (!_tCollection.TryGetValue(key, out InstanceNode iNode))
                return;
            switch (iNode.Instance)
            {
                case null:
                    break;
                case IDisposable instance when dispose:
                    _log.Debug("Disposing disposable object... (InstanceContainer)");
                    instance.Dispose();
                    break;
            }
            _tCollection.Remove(key);
        }

        /// <summary>
        /// Get the instance that is stored in the container, if the instance is not yet constructed this wil do create one.
        /// </summary>
        /// <param name="key">Key</param>
        /// <typeparam name="TInstance">Type of the instance that should be returned.</typeparam>
        /// <returns></returns>
        public TInstance? Get<TInstance>(string key)
        {
            if (!_tCollection.TryGetValue(key, out InstanceNode node) || node.InstanceType != typeof(TInstance))
                return default;
            if (node.Instance != null)
                return (TInstance)node.Instance;
            node.Instance = Activator.CreateInstance(node.InstanceType);
            // Override the old node with the new data, else the next 'Get' will reactivate a another instance.
            _tCollection[key] = node;
            return (TInstance)node.Instance;
        }


        public void Dispose()
        {
            foreach (var kvp in _tCollection)
            {
                switch (kvp.Value.Instance)
                {
                    case null:
                        continue;
                    case IDisposable disposable:
                        _log.Verbose("Disposing: {Key}", kvp.Key);
                        disposable.Dispose();
                        break;
                }
            }
        }
    }

    struct InstanceNode
    {
        public InstanceNode(object? instance, Type instanceType)
        {
            Instance = instance;
            InstanceType = instanceType;
        }
        public object? Instance;
        public readonly Type InstanceType;
    }
}
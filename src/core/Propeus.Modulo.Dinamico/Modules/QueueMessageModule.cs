﻿using System;
using System.Collections.Generic;

using Propeus.Modulo.Abstrato.Atributos;
using Propeus.Modulo.Abstrato.Interfaces;
using Propeus.Modulo.Abstrato.Modulos;

namespace Propeus.Modulo.Dinamico.Modules
{
    class QueueMessageDataDefault : QueueMessageData
    {
        public QueueMessageDataDefault(Type messageTo, object data) : base(messageTo)
        {
            base.Data = data;
        }


        public new DateTime? MessageSent { get; internal set; }
        public new DateTime? MessageReceived { get; internal set; }
    }

    public abstract class QueueMessageData
    {
        public DateTime? MessageSent { get; }
        public DateTime? MessageReceived { get; }
        public DateTime MessageCreated { get; }

        public Type MessageTo { get; set; }

        public object Data { get; internal set; }


        protected QueueMessageData(Type messageTo)
        {
            MessageCreated = DateTime.UtcNow;
            MessageTo = messageTo;
        }
    }

    [ModuleContract(typeof(QueueMessageModule))]
    public interface IQueueMessageModuleSlimContract : IModule
    {
        void CreateOrUpdateListener(Type type, Action<object> action);
        void RemoveListener(Type type);
        bool SendMessage(Type type, object data);
    }

    [ModuleContract(typeof(QueueMessageModule))]
    public interface IQueueMessageModuleContract : IQueueMessageModuleSlimContract
    {
        bool SendMessage(Type type, QueueMessageData dataMessage);
    }

    [Module]
    public sealed class QueueMessageModule : BaseModule, IQueueMessageModuleContract
    {
        public QueueMessageModule()
        {
            _listener = new Dictionary<Type, Action<object>>();
        }

        Dictionary<Type, Action<object>> _listener;
     
        public void CreateOrUpdateListener(Type type, Action<object> action)
        {
            if (!_listener.ContainsKey(type))
            {
                _listener.Add(type, action);
            }
            else
            {
                _listener[type] = action;
            }
        }
        public bool SendMessage(Type type, object data)
        {
            var msg = new QueueMessageDataDefault(type, data);
            msg.MessageSent = DateTime.Now;
            return SendMessage(type, msg);

        }
        public bool SendMessage(Type type, QueueMessageData dataMessage)
        {
            if (!_listener.ContainsKey(type))
                return false;
            else
            {
                _listener[type].Invoke(dataMessage);
                return true;
            }
        }
        public void RemoveListener(Type type)
        {
            _listener.Remove(type);
        }
    }
}

using JccModbus.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JccModbus
{
    public class JccEventDispathcher
    {
        private readonly IConnect connecter;
        private readonly Dictionary<JccVariableDef, List<Action<JccVariableDef, JccVariableValue>>>
           callbacks = new Dictionary<JccVariableDef, List<Action<JccVariableDef, JccVariableValue>>>();

        private readonly object dispatcherLock = new object();

        public JccEventDispathcher(IConnect connect)
        {
            this.connecter = connect;
            connect.OnVariableChange += OnVariableChange;
        }

        private void OnVariableChange(JccVariableDef def, JccVariableValue value)
        {
            List<Action<JccVariableDef, JccVariableValue>> callbackSnapshot = null;
            lock (dispatcherLock)
            {
                if (callbacks.ContainsKey(def))
                {
                    callbackSnapshot = callbacks[def].ToList();
                }
            }

            if (callbackSnapshot == null)
                return;

            foreach (var subscriber in callbackSnapshot)
            {
                subscriber.Invoke(def, value);
            }
        }

        public void RegisterVar(
            JccVariableDef def,
            Action<JccVariableDef, JccVariableValue> callback)
        {
            connecter.CheckVariableDef(def);

            bool needRegisterToconnecter = true;

            lock (dispatcherLock)
            {
                if (!callbacks.ContainsKey(def))
                {
                    callbacks.Add(def, new List<Action<JccVariableDef, JccVariableValue>>()
                    {
                        callback
                    });
                }
                else
                {
                    if (callbacks[def].Contains(callback))
                    {
                        return;
                    }
                    callbacks[def].Add(callback);
                    needRegisterToconnecter = false;

                    var value = connecter.Get(def);
                    if (value.Value != null)
                    {
                        callback(value.Def, value);
                    }

                }

                if (needRegisterToconnecter)
                    connecter.Register(def);
            }
        }

        public void UnRegisterVar(
            JccVariableDef def,
            Action<JccVariableDef, JccVariableValue> callback)
        {
            connecter.CheckVariableDef(def);

            //CheckConnectionStatus();

            bool needRegisterToconnecter = false;

            //Trace.WriteLine($"{def.VarId} unregister");

            lock (dispatcherLock)
            {
                if (callbacks.ContainsKey(def))
                {
                    var callbackList = callbacks[def];
                    if (callbackList.Contains(callback))
                    {
                        callbackList.Remove(callback);
                    }
                    if (callbackList.Count <= 0)
                    {
                        callbacks.Remove(def);
                        needRegisterToconnecter = true;
                    }
                }

                if (needRegisterToconnecter)
                    connecter.UnRegister(def);
            }
        }


    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

using Flux.Utilities;

namespace Flux.WoW
{
    public delegate void WoWEventHandler(WoWEventArgs e);

    public class WoWEventArgs : EventArgs
    {
        protected internal WoWEventArgs(string name, params object[] args)
        {
            EventName = name;
            Args = args;
        }

        public string EventName { get; private set; }
        public object[] Args { get; private set; }
    }

    [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public sealed class WoWEventAttribute : Attribute
    {
        public WoWEventAttribute(string eventName)
        {
            EventName = eventName;
        }

        public string EventName { get; set; }
    }

    internal static class EventLoader
    {
        /// <summary>
        /// Registers any found events using the WoWEventAttribute.
        /// </summary>
        public static void Register()
        {
            foreach (var info in GetHandlers())
            {
                foreach (WoWEventAttribute attr in info.Value)
                {
                    try
                    {
                        // Check the params. It should be a single param method...
                        if (info.Key.GetParameters().Length != 1)
                        {
                            Logging.WriteDebug("Invalid parameter length on " + info.Key.Name +
                                               "! Must contain only one parameter of type WoWEventArgs!");
                            continue;
                        }

                        // ... of type WoWEventArgs
                        if (info.Key.GetParameters()[0].ParameterType != typeof (WoWEventArgs) &&
                            !info.Key.GetParameters()[0].ParameterType.IsSubclassOf(typeof (WoWEventArgs)))
                        {
                            Logging.WriteDebug("Invalid attribute usage on " + info.Key.Name +
                                               "! Must have only 1 param of type WoWEventArgs!");
                            continue;
                        }

                        try
                        {
                            if (!string.IsNullOrEmpty(attr.EventName))
                            {
                                if (!WoWEvents.AttachEvent(attr.EventName,
                                                           (WoWEventHandler)
                                                           Delegate.CreateDelegate(typeof (WoWEventHandler), info.Key)))
                                {
                                    Logging.WriteDebug("Failed to attach event (" + attr.EventName + ") to " +
                                                       info.Key.Name);
                                }
                            }
                                // FAIL!
                            else
                            {
                                Logging.WriteDebug("Could not attach an event, as no event name was supplied!");
                                continue;
                            }
                        }
                        catch (Exception ex)
                        {
                            Logging.WriteException(ex);
                            continue;
                        }
                    }
                    catch (Exception ex)
                    {
                        Logging.WriteException(ex);
                    }
                }
            }
        }

        private static Dictionary<MethodInfo, List<WoWEventAttribute>> GetHandlers()
        {
            var ret = new Dictionary<MethodInfo, List<WoWEventAttribute>>();
            foreach (string s in
                Directory.GetFiles(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "*.dll",
                                   SearchOption.AllDirectories))
            {
                try
                {
                    Assembly asm = Assembly.LoadFrom(s);
                    foreach (Type type in asm.GetTypes())
                    {
                        if (type.IsClass)
                        {
                            foreach (MethodInfo info in
                                type.GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.InvokeMethod))
                            {
                                //   Logging.WriteDebug(info.Name);

                                foreach (object i in info.GetCustomAttributes(typeof (WoWEventAttribute), true))
                                {
                                    if (ret.ContainsKey(info))
                                    {
                                        ret[info].Add((WoWEventAttribute) i);
                                    }
                                    else
                                    {
                                        ret.Add(info, new List<WoWEventAttribute>(new[] {((WoWEventAttribute) i)}));
                                    }
                                }
                            }
                        }
                    }
                }
                catch
                {
                    // Eat the exceptions for now...
                }
            }
            return ret;
        }
    }

    public static class WoWEvents
    {
        private static readonly Dictionary<string, Event> EventStrings = new Dictionary<string, Event>();

        public static bool AttachEvent(string name, WoWEventHandler handler)
        {
            try
            {
                if (!EventStrings.ContainsKey(name))
                {
                    EventStrings.Add(name, new Event(name));
                }
                //Logging.WriteDebug("Attaching " + name + " to " + handler.Method.Name);
                EventStrings[name].AddTarget(handler);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool DetachEvent(string name, WoWEventHandler handler)
        {
            try
            {
                if (!EventStrings.ContainsKey(name))
                {
                    return false;
                }
                EventStrings[name].Targets -= handler;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool DetachEvent(string name)
        {
            if (EventStrings.ContainsKey(name))
            {
                // Automagically performs cleanup
                EventStrings[name].Detach();

                EventStrings.Remove(name);

                return true;
            }
            return false;
        }

        public static void ExecuteEvent(string name, params object[] args)
        {
            Event e;
            EventStrings.TryGetValue(name, out e);
            if (e != null)
            {
                e.Invoke(new WoWEventArgs(name, args));
            }
        }

        #region Nested type: Event

        private class Event
        {
            public readonly List<string> TargetNames;
            private string _eventName;
            private WoWEventHandler _target;

            internal Event(string name)
            {
                _eventName = name;
                TargetNames = new List<string>();
            }

            public event WoWEventHandler Targets;

            internal void AddTarget(WoWEventHandler targ)
            {
                if (TargetNames.Contains(targ.Method.Name))
                {
                    return;
                }

                TargetNames.Add(targ.Method.Name);
                Targets += targ;
            }

            internal void Invoke(WoWEventArgs e)
            {
                if (Targets != null)
                {
                    Targets.Invoke(e);
                }
            }

            internal void Detach()
            {
                if (_target != null)
                {
                    Delegate[] tmp = Targets.GetInvocationList();
                    foreach (Delegate d in tmp)
                    {
                        Targets -= (WoWEventHandler) d;
                    }
                    _target = null;
                }
            }
        }

        #endregion
    }
}
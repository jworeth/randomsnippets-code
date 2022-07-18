using System.IO;
using System.Reflection;

using Flux.Utilities;
using Flux.WoW;

namespace Flux.Bot
{
    public abstract class CustomClass
    {
        private static readonly ClassCollection<CustomClass> _ccs = new ClassCollection<CustomClass>(typeof (DoNotLoadAttribute),
                                                                                                     Path.GetDirectoryName(
                                                                                                         Assembly.GetExecutingAssembly()
                                                                                                             .Location), true);
        private static CustomClass _cc;

        static CustomClass()
        {
            if (FluxWoW.IsInGame && FluxWoW.Me.IsValid)
            {
                foreach (CustomClass cc in _ccs)
                {
                    if (cc.Class == FluxWoW.Me.Class)
                    {
                        Current = cc;
                        Logging.WriteDebug("CC " + cc.GetType().Name + " Loaded");
                        Current.Initialize();
                        break;
                    }
                }
            }
        }

        public static CustomClass Current
        {
            get
            {
                if (_cc == null)
                {
                    if (FluxWoW.Me.IsValid && FluxWoW.IsInGame)
                    {
                        _cc = _ccs.Find(c => c.Class == FluxWoW.Me.Class);
                        if (_cc != null)
                        {
                            Logging.WriteDebug("CC " + _cc.GetType().Name + " Loaded");
                            _cc.Initialize();
                        }
                    }
                }
                return _cc;
            }
            private set { _cc = value; }
        }
        public virtual void CombatHeal() {}

        // TODO: Make this usable!

        public abstract WoWClass Class { get; }
        public virtual bool NeedBuffs { get { return false; } }
        public virtual bool NeedCombatBuff { get { return false; } }
        public virtual bool NeedCombatHeal { get { return false; } }
        public virtual bool NeedHeal { get { return false; } }
        public virtual bool NeedRest { get { return false; } }

        public virtual void Initialize()
        {
        }

        public virtual void Combat()
        {
        }

        public virtual void Pull()
        {
            
        }

        public virtual void Rest()
        {
        }

        public virtual void Buff()
        {
        }

        public virtual void CombatBuff()
        {
        }

        public virtual void Heal()
        {
        }

        public virtual void HandleDeath()
        {
        }

        public virtual void HandleFalling()
        {
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Flux.Bot;
using Flux.WoW;
using Flux.WoW.Objects;

namespace Flux.Gather
{
    public class StateCollectNode: State
    {
        public override string Name
        {
            get
            {
                return "Collecting";
            }
        }

        /// <summary>
        /// Returns a priority for this state. It can be static, or dynamic. Entirely up to you.
        /// </summary>
        public override int Priority
        {
            get { return 500; }
        }

        /// <summary>
        /// Returns whether this state has to run, or not.
        /// </summary>
        public override bool NeedToRun
        {
            get
            {
                return FluxWoW.Me.Casting && FluxWoW.Me.SpellInCast.Name.Contains("Gather");
            }
        }

        /// <summary>
        /// Called if this state is the current state, each time Engine.Pulse() is called.
        /// </summary>
        public override void Update()
        {
            // Wait...
        }
    }

    public class StateMoveToNode : State
    {
        /// <summary>
        /// Returns a priority for this state. It can be static, or dynamic. Entirely up to you.
        /// </summary>
        public override int Priority
        {
            get { return 100; }
        }

        /// <summary>
        /// Returns whether this state has to run, or not.
        /// </summary>
        public override bool NeedToRun
        {
            get { return NodeList.Closest.IsValid && !FluxWoW.Me.Moving; } }

        /// <summary>
        /// Called if this state is the current state, each time Engine.Pulse() is called.
        /// </summary>
        public override void Update()
        {
            FluxWoW.Movement.ClickToMove(NodeList.Closest.Position);
        }
    }

    public class StateUseNode : State
    {
        /// <summary>
        /// Returns a priority for this state. It can be static, or dynamic. Entirely up to you.
        /// </summary>
        public override int Priority
        {
            get { return 400; }
        }

        /// <summary>
        /// Returns whether this state has to run, or not.
        /// </summary>
        public override bool NeedToRun
        {
            get { return NodeList.Closest.IsValid && NodeList.Closest.Position.Distance(FluxWoW.Me.Position) < 3; } }

        /// <summary>
        /// Called if this state is the current state, each time Engine.Pulse() is called.
        /// </summary>
        public override void Update()
        {
            // :)
            NodeList.Closest.Interact();
        }
    }
}

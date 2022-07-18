using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

using Flux.Utilities;
using Flux.WoW;

namespace Flux.Bot
{
    public class StateMachine
    {
        private static readonly ClassCollection<State> States;

        public static bool Paused { get; set; }

        static StateMachine()
        {
            Paused = true;
            try
            {
                // Loads the states recursively from our current assembly location.
                States = new ClassCollection<State>(typeof (DoNotLoadAttribute),
                                                    Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), true);

                // Sorts the state list based on their priority. This means we don't need to keep a long running list
                // of available states!
                States.Sort();

                //Logging.WriteDebug("Loaded " + States.Count + " states");

                //foreach (var state in States)
                //{
                //    Logging.WriteDebug("State loaded: " + state.Name);
                //}

                // Because this is all we need. :)
                FluxWoW.OnFrame += Pulse;
            }
            catch(Exception e)
            {
                Logging.WriteException(e);
            }
        }

        /// <summary>
        /// The current state of the engine.
        /// </summary>
        public static State CurrentState { get; private set; }

        /// <summary>
        /// The main engine pulse method. This should be called whenever you want your engine to be run.
        /// Most likely, within the main game loop. (Or OnFrame event equivalent)
        /// </summary>
        public static void Pulse()
        {
            if (Paused)
                return;

            //Logging.WriteDebug("StateMachine.Pulse");

            // Uncomment this if you plan to use dynamic priority allocation
            // States.Sort();

            // This starts at the highest priority state,
            // and iterates its way to the lowest priority.
            foreach (State state in States)
            {
                if (state.NeedToRun)
                {
                    if (state != CurrentState)
                    {
                        // Sanity check
                        if (CurrentState != null)
                        {
                            CurrentState.Exit();
                        }
                        CurrentState = state;
                        //Logging.WriteDebug("New state: " + state.Name);
                        CurrentState.Enter();
                    }
                    //Logging.WriteDebug(state.Name + " Update");
                    CurrentState.Update();

                    // Break out of the iteration as we found a state that has been updated.
                    // We don't want to run any more states this time around!
                    break;
                }
            }

            //Logging.WriteDebug("No states to run... wtf");
        }
    }

    /// <summary>
    /// A basic State class for a state machine.
    /// </summary>
    public abstract class State : IComparable<State>, IComparer<State>, IEquatable<State>
    {
        /// <summary>
        /// The name of the state. This is more for aesthetics. And this current implementations
        /// equality functions!
        /// </summary>
        public virtual string Name { get { return GetType().Name; } }

        /// <summary>
        /// Returns a priority for this state. It can be static, or dynamic. Entirely up to you.
        /// </summary>
        public abstract int Priority { get; }

        /// <summary>
        /// Returns whether this state has to run, or not.
        /// </summary>
        public abstract bool NeedToRun { get; }

        #region IComparable<State> Members

        /// <summary>
        /// Compares the current object with another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// A 32-bit signed integer that indicates the relative order of the objects being compared. The return value has the following meanings:
        /// Value
        /// Meaning
        /// Less than zero
        /// This object is less than the <paramref name="other"/> parameter.
        /// Zero
        /// This object is equal to <paramref name="other"/>.
        /// Greater than zero
        /// This object is greater than <paramref name="other"/>.
        /// </returns>
        public int CompareTo(State other)
        {
            // We want the highest first.
            // int, by default, chooses the lowest to be sorted
            // at the bottom of the list. We want the opposite.
            return -Priority.CompareTo(other.Priority);
        }

        #endregion

        #region IComparer<State> Members

        public int Compare(State x, State y)
        {
            return -x.Priority.CompareTo(y.Priority);
        }

        #endregion

        #region IEquatable<State> Members

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        /// <param name="other">An object to compare with this object.
        ///                 </param>
        public bool Equals(State other)
        {
            // This should be changed to something more... useful.
            return Name == other.Name && Priority == other.Priority;
        }

        #endregion

        /// <summary>
        /// Called when this state is entered. (Set to current) Useful for initialization.
        /// </summary>
        public virtual void Enter()
        {
        }

        /// <summary>
        /// Called if this state is the current state, each time Engine.Pulse() is called.
        /// </summary>
        public abstract void Update();

        /// <summary>
        /// Called when this state is no longer the current state. Useful for cleanup.
        /// </summary>
        public virtual void Exit()
        {
        }

        /// <summary>
        /// Determines whether the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// true if the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>; otherwise, false.
        /// </returns>
        /// <param name="obj">The <see cref="T:System.Object"/> to compare with the current <see cref="T:System.Object"/>. 
        ///</param>
        /// <exception cref="T:System.NullReferenceException">The <paramref name="obj"/> parameter is null.</exception>
        /// <filterpriority>2</filterpriority>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            if (obj.GetType() != typeof (State))
            {
                return false;
            }
            return Equals((State) obj);
        }

        /// <summary>
        /// Serves as a hash function for a particular type. 
        /// </summary>
        /// <returns>
        /// A hash code for the current <see cref="T:System.Object"/>.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override int GetHashCode()
        {
            // Again; should be changed to something more useful.
            return Name.GetHashCode();
        }

        public static bool operator ==(State left, State right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(State left, State right)
        {
            return !Equals(left, right);
        }
    }

    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class DoNotLoadAttribute : Attribute
    {
    }

    public class StateIdle : State
    {
        public override string Name { get { return "Idle"; } }

        /// <summary>
        /// Returns a priority for this state. It can be static, or dynamic. Entirely up to you.
        /// </summary>
        public override int Priority { get { return int.MinValue; } }

        /// <summary>
        /// Returns whether this state has to run, or not.
        /// </summary>
        public override bool NeedToRun { get { return true; } }

        /// <summary>
        /// Called if this state is the current state, each time Engine.Pulse() is called.
        /// </summary>
        public override void Update()
        {
        }
    }
}
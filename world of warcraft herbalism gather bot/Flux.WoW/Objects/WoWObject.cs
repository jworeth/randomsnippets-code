using System;
using System.Collections;
using System.Collections.Specialized;
using System.Reflection;
using System.Runtime.InteropServices;

using Flux.Utilities;
using Flux.WoW.Native;
using Flux.WoW.Patchables;

namespace Flux.WoW.Objects
{
    public partial class WoWObject : IEquatable<WoWObject>
    {
        protected IntPtr ObjPtr;

        public WoWObject(IntPtr objectPtr)
        {
            ObjPtr = objectPtr;
        }

        public WoWObject(ulong guid) : this(ObjMgr.GetObjectByGuid(guid))
        {
        }

        public WoWObjectTypeFlag TypeFlag { get { return (WoWObjectTypeFlag) GetStorageField<int>(ObjectFields.OBJECT_FIELD_TYPE); } }

        public float Scale { get { return GetStorageField<float>(ObjectFields.OBJECT_FIELD_SCALE_X); } }

        public double Distance { get { return Position.Distance(ObjMgr.Me.Position); } }

        public bool IsValid { get { return ObjPtr != IntPtr.Zero; } }

        public Point Position
        {
            get
            {
                if (_getPos == null)
                {
                    _getPos = Utilities.RegisterDelegate<GetPositionDelegate>(GetVFunc(VFTableIndex.GetPosition));
                }
                var tmp = new float[3];
                _getPos(this, tmp);
                return new Point(tmp);
            }
        }

        public string Name
        {
            get
            {
                if (_getName == null)
                {
                    _getName = Utilities.RegisterDelegate<GetNameDelegate>(GetVFunc(VFTableIndex.GetName));
                }
                return Marshal.PtrToStringAnsi(_getName(this));
            }
        }

        public float Facing
        {
            get
            {
                if (_getFacing == null)
                {
                    _getFacing = Utilities.RegisterDelegate<GetFacingDelegate>(GetVFunc(VFTableIndex.GetFacing));
                }
                return _getFacing(this);
            }
        }

        public ulong Guid { get { return Marshal.ReadIntPtr(this, 0x30).ToUInt64(); } }

        public ulong GuidFromDescriptor { get { return GetStorageField<ulong>(ObjectFields.OBJECT_FIELD_GUID); } }

        public WoWObjectType Type { get { return (WoWObjectType) Marshal.ReadInt32(this, 0x14); } }

        public int Entry { get { return GetStorageField<int>(ObjectFields.OBJECT_FIELD_ENTRY); } }

        public bool InLineOfSight { get { return Utilities.IsInLineOfSite(ObjMgr.Me.Position, Position); } }

        public string Model
        {
            get
            {
                if (_getModel == null)
                {
                    _getModel = Utilities.RegisterDelegate<GetModelDelegate>(GetVFunc(VFTableIndex.GetModel));
                }
                string ret = "";
                _getModel(this, ref ret);
                return ret;
            }
        }

        #region IEquatable<WoWObject> Members

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        /// <param name="other">An object to compare with this object.
        ///                 </param>
        public bool Equals(WoWObject other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }
            if (ReferenceEquals(this, other))
            {
                return true;
            }
            return other.GuidFromDescriptor == GuidFromDescriptor;
        }

        #endregion

        internal void UpdatePointer(IntPtr ptr)
        {
            ObjPtr = ptr;
        }

        public IntPtr GetBagPtr()
        {
            if (_getBagPtr == null)
            {
                _getBagPtr = Utilities.RegisterDelegate<GetBagPtrDelegate>(GetVFunc(VFTableIndex.GetBagPtr));
            }
            return _getBagPtr(this);
        }

        public bool IsFacing(WoWObject other)
        {
            return IsFacing(other.Position);
        }

        public bool IsFacing(Point pt)
        {
            var faceAngle = (float) Math.Atan2(pt.Y - Position.Y, pt.X - Position.X);
            if (faceAngle < 0f)
            {
                faceAngle += (float) Math.PI * 2;
            }
            else if (faceAngle >= (Math.PI * 2))
            {
                faceAngle -= (float) Math.PI * 2;
            }

            double headingDiff;
            int directionCoff;
            WoWFacer.GetHeadingDiff(FluxWoW.Me.Facing, faceAngle, out headingDiff, out directionCoff);

            return headingDiff <= 1;
        }

        public void Interact()
        {
            if (_interact == null)
            {
                _interact = Utilities.RegisterDelegate<InteractDelegate>(GetVFunc(VFTableIndex.Interact));
            }
            _interact(this);
        }

        public void RightClick()
        {
            WoWMovement.ClickToMoveType action;
            if (this is WoWUnit)
            {
                if (ToUnit().Dead && ToUnit().CanLoot)
                {
                    action = WoWMovement.ClickToMoveType.Loot;
                }
                else if (ToUnit().Attackable)
                {
                    action = WoWMovement.ClickToMoveType.AttackGuid;
                }
                else
                {
                    action = WoWMovement.ClickToMoveType.NpcInteract;
                }
            }
            else
            {
                action = WoWMovement.ClickToMoveType.ObjInteract;
            }
            FluxWoW.Movement.ClickToMove(GuidFromDescriptor, Position, action);
        }

        public void Face()
        {
            FluxWoW.Movement.Face(this);
        }

        public void Target()
        {
            Target(GuidFromDescriptor);
        }

        /// <summary>
        /// This is a private static method, since the actual function is static, we just pass different guids.
        /// No reason to re-register the same function over and over!
        /// </summary>
        /// <param name="guid"></param>
        private static void Target(ulong guid)
        {
            if (_selectTarget == null)
            {
                _selectTarget = Utilities.RegisterDelegate<SelectUnitByGuidDelegate>((uint) GlobalOffsets.CGGameUI__Target);
            }
            _selectTarget(guid);
        }

        public override string ToString()
        {
            string ret = string.Format("0x{0}: {1}, {2}, {3}, {4}, {5}, {6}", ObjPtr.ToString("X"), Name, Type, Entry,
                                       Facing, Position, Guid.ToString("X"));
            return ret;
        }

        /// <summary>
        /// Determines whether the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// true if the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>; otherwise, false.
        /// </returns>
        /// <param name="obj">The <see cref="T:System.Object"/> to compare with the current <see cref="T:System.Object"/>. 
        ///                 </param><exception cref="T:System.NullReferenceException">The <paramref name="obj"/> parameter is null.
        ///                 </exception><filterpriority>2</filterpriority>
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
            return obj.GetType() == typeof (WoWObject) && Equals((WoWObject) obj);
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
            return ObjPtr.GetHashCode();
        }

        public static bool operator ==(WoWObject left, WoWObject right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(WoWObject left, WoWObject right)
        {
            return !left.Equals(right);
        }
    }

    public partial class WoWObject
    {
        private static SelectUnitByGuidDelegate _selectTarget;
        private  GetBagPtrDelegate _getBagPtr;
        private  GetFacingDelegate _getFacing;
        private  GetNameDelegate _getName;
        private  GetPositionDelegate _getPos;
        private  InteractDelegate _interact;
        private GetModelDelegate _getModel;

        public static implicit operator IntPtr(WoWObject o)
        {
            return o.ObjPtr;
        }

        public static explicit operator ulong(WoWObject o)
        {
            return o.Guid;
        }

        public static explicit operator string(WoWObject o)
        {
            return o.Name;
        }

        private T GetStorageField<T>(ObjectFields fields) where T : struct
        {
            return GetStorageField<T>((uint) fields);
        }

        protected T GetStorageField<T>(uint field) where T : struct
        {
            if (!IsValid)
            {
                return default(T);
            }
            field = field * 4;
            IntPtr store = Marshal.ReadIntPtr(this, 0x08);
            return Reader.Read<T>((IntPtr) (store.ToInt64() + field));
        }

        public void SetStorageField(uint field, int value)
        {
            IntPtr store = Marshal.ReadIntPtr(this, 0x8);
            Win32.WriteBytes(new IntPtr(store.ToInt32() + (field*4)), BitConverter.GetBytes(value));
        }

        protected IntPtr GetVFunc(VFTableIndex index)
        {
            IntPtr store = Marshal.ReadIntPtr(this);
            return Marshal.ReadIntPtr(store, (int) index * 4);
        }

        protected static bool HasFlag(uint flag, uint value)
        {
            return (flag & value) == value;
        }

        #region Nested type: GetBagPtrDelegate

        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        private delegate IntPtr GetBagPtrDelegate(IntPtr instance);

        #endregion

        #region Nested type: GetFacingDelegate

        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        private delegate float GetFacingDelegate(IntPtr instance);

        #endregion

        #region Nested type: GetNameDelegate

        [UnmanagedFunctionPointer(CallingConvention.ThisCall, CharSet = CharSet.Ansi)]
        private delegate IntPtr GetNameDelegate(IntPtr instance);

        #endregion

        // NOTE: We need to change this to be a WOWPOS pointer instead of this 3x float stuff.
        // Just use the struct marshal stuff!

        #region Nested type: GetPositionDelegate

        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        private delegate void GetPositionDelegate(IntPtr instance, [MarshalAs(UnmanagedType.LPArray)] float[] vec);

        #endregion

        #region Nested type: InteractDelegate

        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        private delegate void InteractDelegate(IntPtr instance);

        #endregion

        #region Nested type: SelectUnitByGuidDelegate

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void SelectUnitByGuidDelegate(ulong guid);

        #endregion

        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        private delegate byte GetModelDelegate(IntPtr pObj, ref string str);


    }

    public partial class WoWObject
    {
        public WoWUnit ToUnit()
        {
            return (WoWUnit) this;
        }

        public WoWPlayer ToPlayer()
        {
            return (WoWPlayer) this;
        }

        public WoWItem ToItem()
        {
            return (WoWItem) this;
        }

        public WoWContainer ToContainer()
        {
            return (WoWContainer) this;
        }

        public WoWGameObject ToGameObject()
        {
            return (WoWGameObject) this;
        }
    }

    partial class WoWObject
    {
        public string DumpProperties()
        {
            string output = "";
            foreach (PropertyInfo info in GetType().GetProperties())
            {
                // Make sure we can 'get' the property, and it isn't an indexer.
                if (info.CanRead && info.GetIndexParameters().Length == 0)
                {
                    // We don't want to call this too much, so lets just 'cache' it.
                    object val = info.GetValue(this, null);

                    string valString;

                    if (val is IList)
                    {
                        valString = (val as IList).ToRealString();
                    }
                    else if (val is BitVector32)
                    {
                        valString = ((BitVector32) val).ToRealString();
                    }
                    else if (val is BitArray)
                    {
                        valString = (val as BitArray).ToRealString();
                    }
                    else
                    {
                        valString = val.ToString();
                    }

                    // Add to the output
                    output += string.Format("{0}: {1}{2}", info.Name, valString, Environment.NewLine);
                }
            }
            return output;
        }

        public string DumpMethods()
        {
            string output = "";
            foreach (MethodInfo info in GetType().GetMethods())
            {
                // Make sure there is only 1 param, and it isn't a generic method. (Eg; our GetStorageField method)
                if (info.GetParameters().Length == 1 && !info.IsGenericMethod)
                {
                    Type paramType = info.GetParameters()[0].ParameterType;
                    if (paramType == typeof (int) || paramType == typeof (uint))
                    {
                        // Good to go. Lets just do 0-500000, assuming we did proper exception throwing, this shouldn't be an issue.
                        for (int i = 0; i < 500000; i++)
                        {
                            try
                            {
                                // Figure out the proper type to pass to the param, and invoke the method.
                                object val = info.Invoke(this, new[] {paramType == typeof (int) ? (object) i : (uint) i});

                                // After grabbing the val, simply output the name of the method, the param val, and some other stuff.
                                output += string.Format("{0} (param {1}): {2}{3}", info.Name, i, val, Environment.NewLine);
                            }
                            catch (ArgumentOutOfRangeException)
                            {
                                // This is the actual exception that should be thrown.
                                break;
                            }
                            catch
                            {
                                // Break on ANY exception. Assume it's specific!
                                break;
                            }
                        }
                    }
                }
            }
            return output;
        }

        public string DumpAll()
        {
            return string.Format("PROPERTIES:{0}{1}{0}METHODS:{0}{2}", Environment.NewLine, DumpProperties(), DumpMethods());
        }
    }
}
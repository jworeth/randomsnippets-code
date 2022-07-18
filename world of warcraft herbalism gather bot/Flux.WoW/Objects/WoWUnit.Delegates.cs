using System;
using System.Runtime.InteropServices;

namespace Flux.WoW.Objects
{
    public partial class WoWUnit
    {
        private GetUnitAuraDelegate _unitAura;
        private GetUnitReactionDelegate _unitReaction;
        private UnitTypeDelegate _unitType;
        private UpdateDisplayInfoDelegate _updateDisplayInfo;

        #region Nested type: GetUnitAuraDelegate

        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        private delegate IntPtr GetUnitAuraDelegate(IntPtr instance, int index);

        #endregion

        #region Nested type: GetUnitReactionDelegate

        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        private delegate int GetUnitReactionDelegate(IntPtr instance, IntPtr other);

        #endregion
        
        #region Nested type: UnitTypeDelegate

        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        private delegate int UnitTypeDelegate(IntPtr instance);

        #endregion

        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        protected delegate int UpdateDisplayInfoDelegate(IntPtr instance, int a, int b);
    }
}
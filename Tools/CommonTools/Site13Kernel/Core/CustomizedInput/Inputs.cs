using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Site13Kernel.Core.CustomizedInput
{
    public class Inputs
    {
        public static ICustomizedInput CurrentInput;

        /// <summary>
        /// True on frame.
        /// </summary>
        /// <param name="Name"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool GetInputDown(string Name)
        {
            return CurrentInput.__GetInputDown(Name);
        }
        /// <summary>
        /// True on frame.
        /// </summary>
        /// <param name="Name"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool GetInputUp(string Name)
        {
            return CurrentInput.__GetInputUp(Name);
        }
        /// <summary>
        /// Trus during the press time.
        /// </summary>
        /// <param name="Name"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool GetInput(string Name)
        {
            return CurrentInput.__GetInput(Name);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float GetAxis(string Name)
        {
            return CurrentInput.__GetAxis(Name);

        }
    }
    public interface ICustomizedInput
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        bool __GetInputDown(string Name);
        /// <summary>
        /// True on frame.
        /// </summary>
        /// <param name="Name"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        bool __GetInputUp(string Name);
        /// <summary>
        /// Trus during the press time.
        /// </summary>
        /// <param name="Name"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        bool __GetInput(string Name);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        float __GetAxis(string Name);
    }
}

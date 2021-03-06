﻿//  Dapplo - building blocks for desktop applications
//  Copyright (C) 2017-2018  Dapplo
// 
//  For more information see: http://dapplo.net/
//  Dapplo repositories are hosted on GitHub: https://github.com/dapplo
// 
//  This file is part of Dapplo.Windows
// 
//  Dapplo.Windows is free software: you can redistribute it and/or modify
//  it under the terms of the GNU Lesser General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
// 
//  Dapplo.Windows is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU Lesser General Public License for more details.
// 
//  You should have a copy of the GNU Lesser General Public License
//  along with Dapplo.Windows. If not, see <http://www.gnu.org/licenses/lgpl.txt>.

#region using

using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Security;
using Dapplo.Windows.User32;

#endregion

namespace Dapplo.Windows.Gdi32.SafeHandles
{
    /// <summary>
    ///     A DeviceContext SafeHandle implementation
    /// </summary>
    public class SafeDeviceContextHandle : SafeDcHandle
    {
        private readonly bool _disposeGraphics;
        private readonly Graphics _graphics;

        /// <summary>
        ///     Default constructor is needed to support marshalling!!
        /// </summary>
        [SecurityCritical]
        public SafeDeviceContextHandle() : base(true)
        {
        }

        /// <summary>
        ///     Construct a SafeDeviceContextHandle for the specified Graphics
        /// </summary>
        /// <param name="graphics">Graphics</param>
        /// <param name="preexistingHandle">IntPtr hDc, from graphics.GetHdc()</param>
        /// <param name="disposeGraphics">specifies if the Graphics object needs disposing</param>
        [SecurityCritical]
        private SafeDeviceContextHandle(Graphics graphics, IntPtr preexistingHandle, bool disposeGraphics) : base(true)
        {
            _graphics = graphics;
            _disposeGraphics = disposeGraphics;
            SetHandle(preexistingHandle);
        }

        /// <summary>
        ///     Create a SafeDeviceContextHandle from a Graphics object
        /// </summary>
        /// <param name="graphics">Graphics object</param>
        /// <param name="disposeGraphics"></param>
        /// <returns>SafeDeviceContextHandle</returns>
        public static SafeDeviceContextHandle FromGraphics(Graphics graphics, bool disposeGraphics = false)
        {
            return new SafeDeviceContextHandle(graphics, graphics.GetHdc(), disposeGraphics);
        }

        /// <summary>
        ///     Create a SafeDeviceContextHandle from a hWnd
        /// </summary>
        /// <param name="hWnd">IntPtr for hWnd</param>
        /// <returns>SafeDeviceContextHandle</returns>
        public static SafeDeviceContextHandle FromHWnd(IntPtr hWnd)
        {
            if (!User32Api.IsWindow(hWnd))
            {
                return null;
            }
            var graphics = Graphics.FromHwnd(hWnd);
            if (graphics == null)
            {
                return null;
            }
            return FromGraphics(graphics);
        }

        /// <summary>
        ///     Call graphics.ReleaseHdc
        /// </summary>
        /// <returns>always true</returns>
        protected override bool ReleaseHandle()
        {
            _graphics.ReleaseHdc(handle);
            if (_disposeGraphics)
            {
                _graphics.Dispose();
            }
            return true;
        }

        /// <summary>
        ///     The SelectObject function selects an object into the device context (DC) which this SafeDeviceContextHandle
        ///     represents.
        ///     The new object replaces the previous object of the same type.
        /// </summary>
        /// <param name="newHandle">SafeHandle for the new object</param>
        /// <returns>Replaced object</returns>
        public SafeSelectObjectHandle SelectObject(SafeHandle newHandle)
        {
            return new SafeSelectObjectHandle(this, newHandle);
        }
    }
}
﻿#region Dapplo 2016 - GNU Lesser General Public License

// Dapplo - building blocks for .NET applications
// Copyright (C) 2017 Dapplo
// 
// For more information see: http://dapplo.net/
// Dapplo repositories are hosted on GitHub: https://github.com/dapplo
// 
// This file is part of Dapplo.Windows
// 
// Dapplo.Windows is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Dapplo.Windows is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Lesser General Public License for more details.
// 
// You should have a copy of the GNU Lesser General Public License
// along with Dapplo.Windows. If not, see <http://www.gnu.org/licenses/lgpl.txt>.

#endregion

#region Usings

using System;
using System.Collections.Generic;
using System.Drawing;
using Dapplo.Windows.Enums;
using Dapplo.Windows.Native;
using Dapplo.Windows.Structs;
using Dapplo.Windows.SafeHandles;
using System.Threading.Tasks;
using Dapplo.Log;

#endregion

namespace Dapplo.Windows.Desktop
{
	/// <summary>
	/// Extensions for the NativeWindow, all get or set commands update the value in the NativeWindow that is used.
	/// </summary>
	public static class NativeWindowExtensions
	{
		private static readonly LogSource Log = new LogSource();
		/// <summary>
		///     Fill ALL the information of the NativeWindow
		/// </summary>
		/// <param name="nativeWindow">NativeWindow</param>
		/// <param name="forceUpdate">true to force updating the values</param>
		public static NativeWindow Fill(this NativeWindow nativeWindow, bool forceUpdate = false)
		{
			nativeWindow.GetBounds(forceUpdate);
			nativeWindow.GetClientBounds(forceUpdate);
			nativeWindow.GetText(forceUpdate);
			nativeWindow.GetClassname(forceUpdate);
			nativeWindow.GetExtendedStyle(forceUpdate);
			nativeWindow.GetStyle(forceUpdate);
			nativeWindow.GetProcessId(forceUpdate);
			nativeWindow.GetParent(forceUpdate);
			nativeWindow.GetPlacement(forceUpdate);
			return nativeWindow;
		}

		/// <summary>
		///     Get the Window bounds
		/// </summary>
		/// <param name="nativeWindow">NativeWindow</param>
		/// <param name="forceUpdate">set to true to make sure the value is updated</param>
		/// <returns>RECT</returns>
		public static RECT GetBounds(this NativeWindow nativeWindow, bool forceUpdate = false)
		{
			if (!nativeWindow.Bounds.HasValue || forceUpdate)
			{
				RECT rectangle;
				User32.GetWindowRect(nativeWindow.Handle, out rectangle);
				nativeWindow.Bounds = rectangle;
			}
			return nativeWindow.Bounds.Value;
		}

		/// <summary>
		///     Get the client bounds
		/// </summary>
		/// <param name="nativeWindow">NativeWindow</param>
		/// <param name="forceUpdate">set to true to make sure the value is updated</param>
		/// <returns>RECT</returns>
		public static RECT GetClientBounds(this NativeWindow nativeWindow, bool forceUpdate = false)
		{
			if (!nativeWindow.ClientBounds.HasValue || forceUpdate)
			{
				RECT rectangle;
				User32.GetClientRect(nativeWindow.Handle, out rectangle);
				nativeWindow.ClientBounds = rectangle;
			}
			return nativeWindow.ClientBounds.Value;
		}

		/// <summary>
		/// Get the Windows text (title)
		/// </summary>
		/// <param name="nativeWindow">NativeWindow</param>
		/// <param name="forceUpdate">set to true to make sure the value is updated</param>
		/// <returns>string with the text</returns>
		public static string GetText(this NativeWindow nativeWindow, bool forceUpdate = false)
		{
			if (nativeWindow.Text == null || forceUpdate)
			{
				var text = User32.GetText(nativeWindow.Handle);
				nativeWindow.Text = text;
			}
			return nativeWindow.Text;
		}

		/// <summary>
		/// Get the parent
		/// </summary>
		/// <param name="nativeWindow">NativeWindow</param>
		/// <param name="forceUpdate">set to true to make sure the value is updated</param>
		/// <returns>IntPtr for the parent</returns>
		public static IntPtr GetParent(this NativeWindow nativeWindow, bool forceUpdate = false)
		{
			if (!nativeWindow.Parent.HasValue || forceUpdate)
			{
				var parent = User32.GetParent(nativeWindow.Handle);
				nativeWindow.Parent = parent;
			}
			return nativeWindow.Parent.Value;
		}

		/// <summary>
		/// Get the Windows class name
		/// </summary>
		/// <param name="nativeWindow">NativeWindow</param>
		/// <param name="forceUpdate">set to true to make sure the value is updated</param>
		/// <returns>string with the classname</returns>
		public static string GetClassname(this NativeWindow nativeWindow, bool forceUpdate = false)
		{
			if (nativeWindow.Classname == null || forceUpdate)
			{
				var className = User32.GetClassname(nativeWindow.Handle);
				nativeWindow.Classname = className;
			}
			return nativeWindow.Classname;
		}

		/// <summary>
		///     Get the WindowPlacement
		/// </summary>
		/// <param name="nativeWindow">NativeWindow</param>
		/// <param name="forceUpdate">set to true to make sure the value is updated</param>
		/// <returns>WindowPlacement</returns>
		public static WindowPlacement GetPlacement(this NativeWindow nativeWindow, bool forceUpdate = false)
		{
			if (!nativeWindow.Placement.HasValue || forceUpdate)
			{
				WindowPlacement placement = WindowPlacement.Default;
				User32.GetWindowPlacement(nativeWindow.Handle, ref placement);
				nativeWindow.Placement = placement;
			}
			return nativeWindow.Placement.Value;
		}

		/// <summary>
		///     Set the WindowPlacement
		/// </summary>
		/// <param name="nativeWindow">NativeWindow</param>
		/// <param name="placement">WindowPlacement</param>
		public static void SetPlacement(this NativeWindow nativeWindow, WindowPlacement placement)
		{
			User32.SetWindowPlacement(nativeWindow.Handle, ref placement);
			nativeWindow.Placement = placement;
		}

		/// <summary>
		///     Get the Extended WindowStyle
		/// </summary>
		/// <param name="nativeWindow">NativeWindow</param>
		/// <param name="forceUpdate">set to true to make sure the value is updated</param>
		/// <returns>ExtendedWindowStyleFlags</returns>
		public static ExtendedWindowStyleFlags GetExtendedStyle(this NativeWindow nativeWindow, bool forceUpdate = false)
		{
			if (!nativeWindow.ExtendedStyle.HasValue || forceUpdate)
			{
				var extendedWindowStyleFlags = (ExtendedWindowStyleFlags)User32.GetWindowLongWrapper(nativeWindow.Handle, WindowLongIndex.GWL_EXSTYLE);
				nativeWindow.ExtendedStyle = extendedWindowStyleFlags;
			}
			return nativeWindow.ExtendedStyle.Value;
		}

		/// <summary>
		///     Set the Extended WindowStyle
		/// </summary>
		/// <param name="nativeWindow">NativeWindow</param>
		/// <param name="extendedWindowStyleFlags">ExtendedWindowStyleFlags</param>
		public static void SetExtendedStyle(this NativeWindow nativeWindow, ExtendedWindowStyleFlags extendedWindowStyleFlags)
		{
			nativeWindow.ExtendedStyle = extendedWindowStyleFlags;
			User32.SetWindowLongWrapper(nativeWindow.Handle, WindowLongIndex.GWL_EXSTYLE, new IntPtr((uint)extendedWindowStyleFlags));
		}

		/// <summary>
		///     Get the WindowStyle
		/// </summary>
		/// <param name="nativeWindow">NativeWindow</param>
		/// <param name="forceUpdate">set to true to make sure the value is updated</param>
		/// <returns>WindowStyleFlags</returns>
		public static WindowStyleFlags GetStyle(this NativeWindow nativeWindow, bool forceUpdate = false)
		{
			if (!nativeWindow.Style.HasValue || forceUpdate)
			{
				var windowStyleFlags = (WindowStyleFlags)User32.GetWindowLongWrapper(nativeWindow.Handle, WindowLongIndex.GWL_STYLE);
				nativeWindow.Style = windowStyleFlags;
			}
			return nativeWindow.Style.Value;
		}

		/// <summary>
		///     Set the WindowStyle
		/// </summary>
		/// <param name="nativeWindow">NativeWindow</param>
		/// <param name="windowStyleFlags">WindowStyleFlags</param>
		public static void SetStyle(this NativeWindow nativeWindow, WindowStyleFlags windowStyleFlags)
		{
			nativeWindow.Style = windowStyleFlags;
			User32.SetWindowLongWrapper(nativeWindow.Handle, WindowLongIndex.GWL_STYLE, new IntPtr((uint)windowStyleFlags));
		}

		/// <summary>
		/// Get the process which the specified window belongs to, the value is cached into the ProcessId of the WindowInfo
		/// </summary>
		/// <param name="nativeWindow">NativeWindow</param>
		/// <param name="forceUpdate">set to true to make sure the value is updated</param>
		/// <returns>int with process Id</returns>
		public static int GetProcessId(this NativeWindow nativeWindow, bool forceUpdate = false)
		{
			if (!nativeWindow.ProcessId.HasValue || forceUpdate)
			{
				int processId;
				User32.GetWindowThreadProcessId(nativeWindow.Handle, out processId);
				nativeWindow.ProcessId = processId;
			}
			return nativeWindow.ProcessId.Value;
		}

		/// <summary>
		/// Retrieve if the window is minimized (Iconic)
		/// </summary>
		/// <param name="nativeWindow">NativeWindow</param>
		/// <param name="forceUpdate">set to true to make sure the value is updated</param>
		/// <returns>bool true if Iconic (minimized)</returns>
		public static bool IsMinimized(this NativeWindow nativeWindow, bool forceUpdate = false)
		{
			if (!nativeWindow.IsMinimized.HasValue || forceUpdate)
			{
				nativeWindow.IsMinimized = User32.IsIconic(nativeWindow.Handle);
			}
			return nativeWindow.IsMinimized.Value;
		}

		/// <summary>
		/// Retrieve if the window is maximized (Iconic)
		/// </summary>
		/// <param name="nativeWindow">NativeWindow</param>
		/// <param name="forceUpdate">set to true to make sure the value is updated</param>
		/// <returns>bool true if Iconic (minimized)</returns>
		public static bool IsMaximized(this NativeWindow nativeWindow, bool forceUpdate = false)
		{
			if (!nativeWindow.IsMaximized.HasValue || forceUpdate)
			{
				nativeWindow.IsMaximized = User32.IsZoomed(nativeWindow.Handle);
			}
			return nativeWindow.IsMaximized.Value;
		}

		/// <summary>
		/// Minimize the Window
		/// </summary>
		/// <param name="nativeWindow">NativeWindow</param>
		public static void Minimize(this NativeWindow nativeWindow)
		{
			User32.ShowWindow(nativeWindow.Handle, ShowWindowCommands.Minimize);
			nativeWindow.IsMinimized = true;
		}

		/// <summary>
		/// Restore (Un-Minimize/Maximize) the Window
		/// </summary>
		/// <param name="nativeWindow">NativeWindow</param>
		public static void Restore(this NativeWindow nativeWindow)
		{
			User32.ShowWindow(nativeWindow.Handle, ShowWindowCommands.Restore);
			nativeWindow.IsMinimized = false;
			nativeWindow.IsMaximized = false;
		}

		/// <summary>
		/// Maximize the window
		/// </summary>
		/// <param name="nativeWindow">NativeWindow</param>
		public static void Maximized(this NativeWindow nativeWindow)
		{
			User32.ShowWindow(nativeWindow.Handle, ShowWindowCommands.Maximize);
			nativeWindow.IsMaximized = true;
			nativeWindow.IsMinimized = false;
		}

		/// <summary>
		/// Retrieve if the window is Visible (IsWindowVisible, whatever that means)
		/// </summary>
		/// <param name="nativeWindow">NativeWindow</param>
		/// <param name="forceUpdate">set to true to make sure the value is updated</param>
		/// <returns>bool true if minimizedIconic (minimized)</returns>
		public static bool IsVisible(this NativeWindow nativeWindow, bool forceUpdate = false)
		{
			if (!nativeWindow.IsVisible.HasValue || forceUpdate)
			{
				nativeWindow.IsVisible = User32.IsWindowVisible(nativeWindow.Handle);
			}
			return nativeWindow.IsVisible.Value;
		}

		/// <summary>
		/// Get the children of the specified NativeWindow, this is not lazy!
		/// </summary>
		/// <param name="nativeWindow">NativeWindow</param>
		/// <param name="forceUpdate">True to force updating</param>
		/// <returns>IEnumerable with NativeWindow</returns>
		public static IEnumerable<NativeWindow> GetChildren(this NativeWindow nativeWindow, bool forceUpdate = false)
		{
			if (nativeWindow.HasChildren && !forceUpdate)
			{
				return nativeWindow.Children;
			}
			IList<NativeWindow> children = new List<NativeWindow>();
			// Store it in the Children property
			nativeWindow.Children = children;
			foreach (var child in WindowsEnumerator.EnumerateWindows(nativeWindow))
			{
				children.Add(child);
			}
			return children;
		}

		/// <summary>
		/// Get the region for a window
		/// </summary>
		public static Region GetRegion(this NativeWindow nativeWindow)
		{
			using (SafeRegionHandle region = Gdi32.CreateRectRgn(0, 0, 0, 0))
			{
				if (!region.IsInvalid)
				{
					var result = User32.GetWindowRgn(nativeWindow.Handle, region);
					if (result != RegionResults.REGION_ERROR && result != RegionResults.REGION_NULLREGION)
					{
						return Region.FromHrgn(region.DangerousGetHandle());
					}
				}
			}
			return null;
		}

		/// <summary>
		/// Set the window as foreground window
		/// </summary>
		/// <param name="nativeWindow">The window to bring to the foreground</param>
		/// <param name="workaround">bool with true to use a trick to really bring the window to the foreground</param>
		public static async Task ToForegroundAsync(this NativeWindow nativeWindow, bool workaround = true)
		{
			// Nothing we can do if it's not visible!
			if (!nativeWindow.IsVisible())
			{
				return;
			}
			if (nativeWindow.IsMinimized())
			{
				nativeWindow.Restore();
				while (nativeWindow.IsMinimized())
				{
					await Task.Delay(50).ConfigureAwait(false);
				}
			}
			// See https://msdn.microsoft.com/en-us/library/windows/desktop/ms633539(v=vs.85).aspx
			if (workaround)
			{
				// Simulate an "ALT" key press.
				InputGenerator.Press(VirtualKeyCodes.MENU);
			}
			// Show window in forground.
			User32.BringWindowToTop(nativeWindow.Handle);
			User32.SetForegroundWindow(nativeWindow.Handle);
		}

		/// <summary>
		/// Get the Border size for GDI Windows
		/// </summary>
		/// <param name="nativeWindow">NativeWindow</param>
		/// <returns>bool true if it worked</returns>	
		public static SIZE GetBorderSize(this NativeWindow nativeWindow)
		{
			var windowInfo = new WINDOWINFO();
			// Get the Window Info for this window
			if (User32.GetWindowInfo(nativeWindow.Handle, ref windowInfo))
			{
				return new SIZE((int)windowInfo.cxWindowBorders, (int)windowInfo.cyWindowBorders);
			}
			return SIZE.Empty;
		}

	}
}
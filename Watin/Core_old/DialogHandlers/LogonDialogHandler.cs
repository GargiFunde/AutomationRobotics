#region WatiN Copyright (C) 2006-2009 Jeroen van Menen

//Copyright 2006-2009 Jeroen van Menen
//
//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at
//
//       http://www.apache.org/licenses/LICENSE-2.0
//
//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.

#endregion Copyright

using System;
using System.Threading;

namespace WatiN.Core.DialogHandlers
{
	/// <summary>
	/// This class handles the logon dialog by passing in the username and password
	/// and clicking the OK button.
	/// </summary>
	/// <example>
	/// The following code shows the use of this dialog handler
	/// <code>
	/// IE ie = new IE();
	///
	/// ie.DialogWatcher.Add(new LogonDialogHandler(@"domain\username", "password"));
	///
	/// ie.GoTo("https://www.somesecuresite.com");
	/// </code>
	/// </example>
	public class LogonDialogHandler : BaseDialogHandler
	{
		private string userName;
		private string password;

		/// <summary>
		/// Initializes a new instance of the <see cref="LogonDialogHandler"/> class.
		/// </summary>
		/// <param name="userName">Name of the user. Is required.</param>
		/// <param name="password">The password. If no password is required, it can be left blank (<c>null</c> or <c>String.Empty</c>). </param>
		public LogonDialogHandler(string userName, string password)
		{
			checkArgument("Username must be specified", userName, "username");

			this.userName = UtilityClass.EscapeSendKeysCharacters(userName);

			if (password == null)
			{
				this.password = String.Empty;
			}
			else
			{
				this.password = UtilityClass.EscapeSendKeysCharacters(password);
			}
		}

		public override bool HandleDialog(Window window)
		{
			if (IsLogonDialog(window))
			{
				// Find Handle of the "Frame" and then the combo username entry box inside the frame
				IntPtr inputFrameHandle = NativeMethods.GetChildWindowHwnd(window.Hwnd, "SysCredential");
				IntPtr usernameControlHandle = NativeMethods.GetChildWindowHwnd(inputFrameHandle, "ComboBoxEx32");

				NativeMethods.SetActiveWindow(usernameControlHandle);
				Thread.Sleep(50);

				NativeMethods.SetForegroundWindow(usernameControlHandle);
				Thread.Sleep(50);

				System.Windows.Forms.SendKeys.SendWait(userName + "{TAB}");
				Thread.Sleep(500);

				System.Windows.Forms.SendKeys.SendWait(password + "{ENTER}");

				return true;
			}

			return false;
		}

		/// <summary>
		/// Determines whether the specified window is a logon dialog.
		/// </summary>
		/// <param name="window">The window.</param>
		/// <returns>
		/// 	<c>true</c> if the specified window is a logon dialog; otherwise, <c>false</c>.
		/// </returns>
		public virtual bool IsLogonDialog(Window window)
		{
			// If a logon dialog window is found hWnd will be set.
			return NativeMethods.GetChildWindowHwnd(window.Hwnd, "SysCredential") != IntPtr.Zero;
		}

		private static void checkArgument(string message, string parameter, string parameterName)
		{
			if (UtilityClass.IsNullOrEmpty(parameter))
			{
				throw new ArgumentNullException(message, parameterName);
			}
		}
	}
}
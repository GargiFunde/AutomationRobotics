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
using System.Collections;
using WatiN.Core.Exceptions;

namespace WatiN.Core.DialogHandlers
{
	public class AlertAndConfirmDialogHandler : BaseDialogHandler
	{
		private Queue alertQueue;

		public AlertAndConfirmDialogHandler()
		{
			alertQueue = new Queue();
		}

		/// <summary>
		/// Gets the count of the messages in the que of displayed alert and confirm windows.
		/// </summary>
		/// <value>The count of the alert and confirm messages in the que.</value>
		public int Count
		{
			get { return alertQueue.Count; }
		}

		/// <summary>
		/// Pops the most recent message from a que of displayed alert and confirm windows.
		/// Use this method to get the displayed message.
		/// </summary>
		/// <returns>The displayed message.</returns>
		public string Pop()
		{
			if (alertQueue.Count == 0)
			{
				throw new MissingAlertException();
			}

			return (string) alertQueue.Dequeue();
		}

		/// <summary>
		/// Gets the alert and confirm messages in the que of displayed alert and confirm windows.
		/// </summary>
		/// <value>The alert and confirm messages in the que.</value>
		public string[] Alerts
		{
			get
			{
				string[] result = new string[alertQueue.Count];
				Array.Copy(alertQueue.ToArray(), result, alertQueue.Count);
				return result;
			}
		}

		/// <summary>
		/// Clears all the messages from the que of displayed alert and confirm windows.
		/// </summary>
		public void Clear()
		{
			alertQueue.Clear();
		}

		public override bool HandleDialog(Window window)
		{
			// See if the dialog has a static control with a controlID 
			// of 0xFFFF. This is unique for alert and confirm dialogboxes.
			IntPtr handle = NativeMethods.GetDlgItem(window.Hwnd, 0xFFFF);

			if (handle != IntPtr.Zero)
			{
				alertQueue.Enqueue(NativeMethods.GetWindowText(handle));

				window.ForceClose();

				return true;
			}

			return false;
		}
	}
}
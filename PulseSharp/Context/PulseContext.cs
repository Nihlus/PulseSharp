//
//  PulseContext.cs
//
//  Copyright (c) 2018 Jarl Gullberg
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
//

using System;
using System.Threading;
using System.Threading.Tasks;
using AdvancedDLSupport;
using JetBrains.Annotations;
using PulseSharp.Enums;
using PulseSharp.Interfaces;
using PulseSharp.MainLoopAbstractions;
using PulseSharp.Native;
using PulseSharp.Structures;

namespace PulseSharp.Context
{
	/// <summary>
	/// Context class for a connection to the PulseAudio server.
	/// </summary>
	[PublicAPI]
	public class PulseContext : PulseAudioObject
	{
		private static readonly IPulseContext API;

		static PulseContext()
		{
			API = NativeLibraryBuilder.Default.ActivateInterface<IPulseContext>("pulse");
		}

		private readonly object StateCallbackLock = new object();

		private bool IsConnected { get; set; }

		private ThreadedMainLoop MainLoop { get; }

		/// <summary>
		/// Initializes a new instance of the <see cref="PulseContext"/> class.
		/// </summary>
		/// <param name="mainLoop">An abstract mainloop pointer.</param>
		/// <param name="applicationName">A descriptive application name.</param>
		[PublicAPI]
		public PulseContext(ThreadedMainLoop mainLoop, string applicationName)
			: base(() => API.New(mainLoop.GetAPI(), applicationName), API.Unref)
		{
			this.MainLoop = mainLoop;
		}

		/// <summary>
		/// Gets the state of the context.
		/// </summary>
		/// <returns>The state.</returns>
		[PublicAPI]
		public PulseContextState GetState()
		{
			using (this.MainLoop.AcquireLock())
			{
				return API.GetState(this.Handle.DangerousGetHandle());
			}
		}

		/// <summary>
		/// Sets the context's state callback.
		/// </summary>
		/// <param name="callback">The callback.</param>
		[PublicAPI]
		public void SetStateCallback(Action<IntPtr, IntPtr> callback)
		{
			using (this.MainLoop.AcquireLock())
			{
				API.SetStateCallback
				(
					this.Handle.DangerousGetHandle(),
					new PulseAudioContextNotifyCallback(callback),
					IntPtr.Zero
				);
			}
		}

		/// <summary>
		/// Connects to the pulseaudio daemon.
		/// </summary>
		/// <returns>A task that must be awaited.</returns>
		[PublicAPI]
		public Task ConnectAsync()
		{
			if (this.IsConnected)
			{
				return Task.CompletedTask;
			}

			Monitor.Enter(this.StateCallbackLock);

			var tcs = new TaskCompletionSource<bool>();

			SetStateCallback
			(
				(context, data) =>
				{
					switch (GetState())
					{
						case PulseContextState.Disconnected:
						case PulseContextState.Connecting:
						case PulseContextState.Authorizing:
						case PulseContextState.SettingName:
						{
							break;
						}

						case PulseContextState.Failed:
						case PulseContextState.Terminated:
						{
							tcs.SetCanceled();
							break;
						}

						case PulseContextState.Ready:
						{
							this.IsConnected = true;
							tcs.SetResult(true);
							Monitor.Exit(this.StateCallbackLock);

							break;
						}

						default:
						{
							throw new ArgumentOutOfRangeException();
						}
					}
				}
			);

			API.Connect(this.Handle.DangerousGetHandle(), null, PulseContextFlags.None, IntPtr.Zero);

			return tcs.Task;
		}

		/// <summary>
		/// Disconnects the connection to the pulseaudio daemon.
		/// </summary>
		/// <returns>A task that must be awaited.</returns>
		[PublicAPI]
		public Task DisconnectAsync()
		{
			if (!this.IsConnected)
			{
				return Task.CompletedTask;
			}

			Monitor.Enter(this.StateCallbackLock);

			var tcs = new TaskCompletionSource<bool>();
			SetStateCallback
			(
				(context, data) =>
				{
					switch (GetState())
					{
						case PulseContextState.Disconnected:
						case PulseContextState.Connecting:
						case PulseContextState.Authorizing:
						case PulseContextState.SettingName:
						case PulseContextState.Ready:
						{
							break;
						}

						case PulseContextState.Terminated:
						{
							this.IsConnected = false;
							tcs.SetResult(true);
							Monitor.Exit(this.StateCallbackLock);

							break;
						}

						case PulseContextState.Failed:
						{
							// TODO: exception?
							break;
						}

						default:
						{
							throw new ArgumentOutOfRangeException();
						}
					}
				}
			);

			return tcs.Task;
		}

		/// <summary>
		/// Gets some information about the PulseAudio server.
		/// </summary>
		/// <returns>Some info.</returns>
		[PublicAPI]
		public Task<ServerInfo> GetServerInfoAsync()
		{
			var tcs = new TaskCompletionSource<ServerInfo>();
			using (this.MainLoop.AcquireLock())
			{
				var op = new AsyncPulseAudioOperation(API.GetServerInfo
				(
					this.Handle.DangerousGetHandle(),
					(IntPtr context, in ServerInfo info, IntPtr data) => tcs.SetResult(info),
					IntPtr.Zero
				));

				op.SetStateChangedCallback
				(
					s =>
					{
						if (s == PulseAudioOperationState.Cancelled)
						{
							tcs.SetCanceled();
						}
					}
				);
			}

			return tcs.Task;
		}
	}
}

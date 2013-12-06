﻿// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see license file in the main folder

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Aura.Shared.Util;
using Aura.Channel.World.Entities;
using Aura.Channel.Network.Sending;

namespace Aura.Channel.Scripting
{
	/// <summary>
	/// NPC converstation session
	/// </summary>
	public class NpcSession
	{
		public NPC Target { get; private set; }
		public int Id { get; private set; }

		//public Options Options = Options.FaceAndName;
		//public string DialogFace = null;
		//public string DialogName = null;

		public IEnumerator State { get; set; }
		public Response Response { get; set; }

		public NpcSession()
		{
			// We'll only set this once for every char, for the entire session.
			// In some cases the client doesn't seem to take the new id,
			// which results in a mismatch.
			this.Id = RandomProvider.Get().Next(1, 5000);
		}

		/// <summary>
		/// Starts session
		/// </summary>
		/// <param name="target"></param>
		public void Start(NPC target)
		{
			this.Target = target;
		}

		/// <summary>
		/// Resets session.
		/// </summary>
		public void Clear()
		{
			this.Target = null;
			this.State = null;
			this.Response = null;
		}

		/// <summary>
		/// Returns true if there is a state and target's id is npcId.
		/// </summary>
		public bool IsValid(long npcId)
		{
			return (this.IsValid() && this.Target.EntityId == npcId);
		}

		/// <summary>
		/// Returns true if there is a state and a target.
		/// </summary>
		public bool IsValid()
		{
			return (this.Target != null && this.State != null);
		}

		public void SetResponse(string response)
		{
			if (this.Response != null)
				this.Response.Value = response;
		}

		public void Continue()
		{
			if (this.State.MoveNext())
				this.Response = this.State.Current as Response;
		}
	}

	/// <summary>
	/// Response to a conversation
	/// </summary>
	/// <remarks>
	/// An instance of this class is returned from the NPCs on Select,
	/// to give the client something referenceable to write the response to.
	/// (Options, Input, etc.)
	/// </remarks>
	public class Response
	{
		public string Value { get; set; }
	}
}
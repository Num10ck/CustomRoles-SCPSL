using CustomRoles.Source.Types.Interfaces;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Interfaces;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.Features;
using System;
using System.Collections.Generic;

namespace CustomRoles.Source.Types
{
	/// <summary>
	/// Base <see langword="class"/> for abilities.
	/// </summary>
	/// <typeparam name="TArgs"></typeparam>
	public abstract class Ability<TArgs> : IAbility, IDisposable
										   where TArgs : IExiledEvent, IPlayerEvent
	{

		/// <summary>
		/// Event to which method will be attached.
		/// </summary>
		protected abstract Event<TArgs> Event { get; set; }

		/// <summary>
		/// Discribes is ability enabled.
		/// </summary>
		public bool IsEnabled { get; private set; }

		/// <summary>
		/// List of ability attached players.
		/// </summary>
		protected readonly List<Player> AttachedPlayers = new List<Player>();

		/// <summary>
		/// List of all existing abilities.
		/// </summary>
		private readonly static List<IAbility> _list = new List<IAbility>();

		/// <summary>
		/// Enables ability for specified player.
		/// </summary>
		/// <param name="player">Player who will get ability</param>
		public void EnableForPlayer(Player player)
		{
			if (!IsEnabled)
			{
				throw new ApplicationException($"Ability {this} is disabled.");
			}

			AttachedPlayers.Add(player);
		}

		/// <summary>
		/// Disables ability for specified player.
		/// </summary>
		/// <param name="player">Player who will loss ability.</param>
		public void DisableForPlayer(Player player)
		{
			if (!IsEnabled)
			{
				throw new ApplicationException($"Ability {this} is disabled.");
			}

			AttachedPlayers.Remove(player);
		}

		/// <summary>
		/// Enables ability.
		/// </summary>
		public void Enable()
		{
			IsEnabled = true;
			Event += Conditional;
		}

		/// <summary>
		/// Disables ability.
		/// </summary>
		public void Disable()
		{
			IsEnabled = false;
			Event -= Conditional;
		}

		/// <summary>
		/// Disables ability.
		/// </summary>
		public virtual void Dispose()
		{
			Disable();
		}

		/// <summary>
		/// Disables all abilities.
		/// </summary>
		public static void DisableAll()
		{
			for (int i = 0; i < _list.Count; i++)
			{
				IAbility ability = _list[i];

				ability.Disable();
			}
		}

		/// <summary>
		/// Condition before execute <see cref="Ability{TArgs}.Action(TArgs)"/>
		/// </summary>
		/// <param name="ev"></param>
		protected virtual void Conditional(TArgs ev)
		{
			if (AttachedPlayers.Contains(ev.Player))
			{
				Action(ev);
			}
		}

		/// <summary>
		/// A ability itself. Take note that all conditions must be defined at <see cref="Ability{TArgs}.Conditional(TArgs)"/>
		/// </summary>
		/// <param name="ev"></param>
		protected abstract void Action(TArgs ev);

		protected Ability()
		{
			Ability<VerifiedEventArgs>._list.Add(this);
		}
    }
}

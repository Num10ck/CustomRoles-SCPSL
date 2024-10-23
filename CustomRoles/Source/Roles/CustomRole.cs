using CustomRoles.Source.Types.Interfaces;
using Exiled.API.Features;
using Exiled.API.Features.Roles;
using Exiled.Events.EventArgs.Interfaces;
using PlayerRoles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CustomRoles.Source.Types
{
	/// <summary>
	/// Base <see langword="class"/> for all custom roles.
	/// </summary>
	public abstract class CustomRole
	{
		/// <summary>
		/// Identifier for <see cref="Player.SessionVariables"/> key.
		/// </summary>
		public const string ROLE_IDENTIFIER = "Num1ock/CUSTOMROLES";

		/// <summary>
		/// Base role for player.
		/// </summary>
		protected abstract RoleTypeId WrapperRole { get; }

		/// <summary>
		/// Role abilities.
		/// </summary>
		protected abstract IAbility[] Abilities { get; }

		/// <summary>
		/// Additional operations that must be called after role attach. Empty by default.
		/// </summary>
		protected virtual void RoleProc(Player player) { }

		/// <summary>
		/// Attaches and initializes specified custom role for specified player.
		/// </summary>
		/// <param name="target">Player who will own the role.</param>
		/// <param name="role">Role who will be attached to player.</param>
		/// <exception cref="ApplicationException"></exception>
		public static void Attach(Player target, CustomRole role)
		{
			if (target.SessionVariables.ContainsKey(ROLE_IDENTIFIER))
			{
				throw new ApplicationException($"Player {target.DisplayNickname} already has a custom role.");
			}

			target.Role.Set(role.WrapperRole, RoleSpawnFlags.None);

			for (int i = 0; i < role.Abilities.Length; i++)
			{
				IAbility ability = role.Abilities[i];

				ability.EnableForPlayer(target);
			}

			target.SessionVariables.Add(ROLE_IDENTIFIER, role);

			role.RoleProc(target);
		}

		/// <summary>
		/// Detaches and disables all abilities for specified player.
		/// </summary>
		/// <param name="target">Player who will lose a custom role</param>
		/// <exception cref="ApplicationException"></exception>
		public static void Detach(Player target)
		{
			if (!target.SessionVariables.ContainsKey(ROLE_IDENTIFIER))
			{
				throw new ApplicationException($"Player {target.DisplayNickname} does not have a custom role.");
			}

			CustomRole role = target.SessionVariables[ROLE_IDENTIFIER] as CustomRole;

			for (int i = 0; i < role.Abilities.Length; i++)
			{
				IAbility ability = role.Abilities[i];

				ability.DisableForPlayer(target);
			}

			target.SessionVariables.Remove(ROLE_IDENTIFIER);
		}

        protected CustomRole(bool enable = false)
        {
            if (enable)
			{
				for (int i = 0; i < Abilities.Length; i++)
				{
					IAbility ability = Abilities[i];

					ability.Enable();
				}
			}
        }
    }
}

using Exiled.API.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomRoles.Source.Types.Interfaces
{
	public interface IAbility
	{
		void EnableForPlayer(Player player);
		void DisableForPlayer(Player player);

		void Enable();
		void Disable();
	}
}

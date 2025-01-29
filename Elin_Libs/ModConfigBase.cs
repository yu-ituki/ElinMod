using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elin_Mod
{
	public abstract class ModConfigBase
	{
		public abstract void Initialize(BepInEx.Configuration.ConfigFile config);
	}
}

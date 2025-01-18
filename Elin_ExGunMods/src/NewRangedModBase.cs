using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elin_Mod
{
	public class NewRangedModBase
	{
		public class BaseData {
			public string alias;


			public int id { get; private set; }
			public SourceElement.Row elem { get; private set; }
			
			public virtual void Load() {
				elem = NewRangedModManager.Instance.GetElement(alias);
				id = elem.id;
			}
		}

		public virtual void Initialize() {

		}



	}
}

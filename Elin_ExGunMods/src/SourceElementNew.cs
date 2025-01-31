using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elin_Mod
{
	public class SourceElementNew : SourceElement
	{
		public override void Reset() {
			//base.Reset();
		}

		public override void OnAfterImportData() {
			//base.OnAfterImportData();
			Core.SetCurrent();
			foreach (Row row in rows) {
				map[row.id] = row;
				alias[row.GetAlias] = row;
			}
		}

		public override Row CreateRow() {
			var ret = base.CreateRow();
			switch ( ModTextManager.Instance.GetLanguageCode()) {
				case eLanguage.CN:
					ret.name = SourceData.GetString(61);
					ret.textPhase = SourceData.GetString(62);
					break;

				case eLanguage.ZHTW:
					ret.name = SourceData.GetString(63);
					ret.textPhase = SourceData.GetString(64);
					break;

				case eLanguage.KR:
					ret.name = SourceData.GetString(65);
					ret.textPhase = SourceData.GetString(66);
					break;
			}

			return ret;
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Elin_Mod
{
	public class NewRangedModManager : Singleton<NewRangedModManager>
	{
		const string c_ElemName_ElemIDStart = "itukiyu_modEX_IDStart";
		const string c_ElemName_ElemIDEnd = "itukiyu_modEX_IDEnd";

		public int ElemIDStart { get; private set; } = 0;
		public int ElemIDEnd { get; private set; } = 0;

		int[] m_ElemIndexToAliasHashes;
		int[] m_ElemIndexToIDs;

		Dictionary<int,SourceElement.Row> m_NewModElemRows;
		NewRangedModBase[] m_NewMods;







		public void Initialize() {
			m_NewMods = new NewRangedModBase[] {
				new NewRangedMod_Elements(),
				new NewRangedMod_Barrel()
			};

			var elems = EClass.sources.elements;
			var elemsMap = elems.map;
			m_ElemIndexToAliasHashes = new int[elemsMap.Count];
			m_ElemIndexToIDs = new int[elemsMap.Count];
			int idx = 0;
			foreach (var itr in elemsMap) {
				m_ElemIndexToAliasHashes[idx] = itr.Value.alias.GetHashCode();
				m_ElemIndexToIDs[idx] = itr.Value.id;
				++idx;
			}

			ElemIDStart = GetElement(c_ElemName_ElemIDStart).id;
			ElemIDEnd = GetElement(c_ElemName_ElemIDEnd).id;

			m_NewModElemRows = new Dictionary<int, SourceElement.Row>();
			foreach ( var itr in elemsMap) {
				if (itr.Key < ElemIDStart)
					continue;
				if (itr.Key > ElemIDEnd)
					continue;
				m_NewModElemRows.Add(itr.Key, itr.Value);
			}


			foreach (var itr in m_NewMods)
				itr.Initialize();
			
		}


		public SourceElement.Row GetElement( string alias ) {
			int searchHash = alias.GetHashCode();
			int idx = 0;
			foreach ( var itr in EClass.sources.elements.map ) {
				if (m_ElemIndexToAliasHashes[idx] == searchHash ) {
					return itr.Value;
				}
				++idx;
			}
			return null;
		}

		public SourceElement.Row GetElement( int id ) {
			return EClass.sources.elements.map[id];
		}

		public bool IsNewRangeModIDBand( int id ) {
			if (id < ElemIDStart)
				return false;
			if (id > ElemIDEnd)
				return false;
			return true;
		}

		public bool IsNewRangeModID( int id ) {
			if (!IsNewRangeModIDBand(id))
				return false;
			return m_NewModElemRows.ContainsKey(id);
		}


		public bool Uninstall() {
			Exception eTmp = null;
			bool isError = false;
			try {
				// グローバルに登録されているカード群全てに対して削除要求していく.
				foreach (var itr in EClass.game.cards.globalCharas)
					_UninstallThings(itr.Value.things);

				// 全マップの配置物も漁っていく.
				foreach (var itr in EClass.game.spatials.map) {
					_UninstallSpatial(itr.Value);
				}
			}
			catch ( Exception e ) {
				isError = true;
				eTmp = e;
			}
			finally {
				if ( isError ) {
					var textMng = ModTextManager.Instance;
					var reportPath = CommonUtil.CreateErrorReport();
					textMng.SetUserData(0, reportPath);
					var bodyText = textMng.GetText(eTextID.Config_UninstallError);
					GameUtil.OpenDialog_1Button(bodyText, textMng.GetText(eTextID.Yes), () => {
						EClass.game.GotoTitle(false);
					});
				} else {
					EClass.game.Save(true);
				}
			}

			return !isError;
		}

		void _UninstallSpatial( Spatial spatial ) {
			try {
				var zone = spatial as Zone;
				if (zone != null) {
					var things = zone?.map?.things;
					if (things != null) {
						for (int i = 0; i < things.Count; ++i) {
							_UninstallThing(things[i]);
						}
					}
				}

				foreach (var child in spatial.children)
					_UninstallSpatial(child);
			}
			catch ( Exception e) {
				throw e;
			}
		}
		
		void _UninstallThings( ThingContainer things ) {
			try {
				for (int i = 0; i < things.Count; ++i) {
					if (_UninstallThing(things[i]))
						--i;
				}
			} catch ( Exception e ) {
				throw e;
			}
		}

		bool _UninstallThing( Thing thing ) {
			if (thing == null)
				return false;
			if (thing.isDestroyed)
				return false;
			try {
				var mod = thing.trait as TraitModRanged;
				if (mod != null) {
					// こちらで追加したパーツだったら削除.
					bool isNewMod = m_NewModElemRows.ContainsKey(mod.source.id);
					if (isNewMod)
						thing.Destroy();
					return isNewMod;
				} else {
					// 装備かもしれないのでソケットとエンチャントを調べ、こちらの追加した物が入ってたら取り除く.
					foreach (var itr in m_NewModElemRows) {
						if (thing.sockets != null) {
							for (int i = 0; i < thing.sockets.Count; ++i) {
								int elemID = thing.sockets[i] / 100;
								if (elemID != itr.Key)
									continue;
								int lv = thing.sockets[i] % 100;
								thing.elements?.ModBase(elemID, -lv);
								thing.sockets[i] = 0;
							}
						}

						// ModBaseで-lvしてるから必要ないか？.
						var elem = thing.elements?.GetElement(itr.Key);
						if (elem == null)
							continue;
						thing.elements?.Remove(itr.Key);
					}
					return false;
				}
			} catch ( Exception e) {
				throw e;
			}
			return false;
		}

	}
}

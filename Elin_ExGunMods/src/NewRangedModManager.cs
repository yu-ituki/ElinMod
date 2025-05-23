﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

using HarmonyLib;
using UnityEngine;

using static ActPlan;

namespace Elin_Mod
{
	[HarmonyPatch]
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







		public void OnStartCore() {
			m_NewMods = new NewRangedModBase[] {
				new NewRangedMod_Elements(),
				new NewRangedMod_Barrel()
			};
			
			// データ読み込み.
			var elems = EClass.sources.elements;
			var elemsMap = elems.map;
			SourceElementNew elemNews = ScriptableObject.CreateInstance<SourceElementNew>();
			ModUtil.ImportExcel(CommonUtil.GetResourcePath("tables/add_datas.xlsx"), "elements", elemNews);

			foreach ( var itr in elemNews.map) {
				SourceElement.Row tmp = null;
				if ( elemsMap.TryGetValue( itr.Key, out tmp )) {
					DebugUtil.LogError( $"[Elin_ExGunMods] conlict element id!!!! --> id={itr.Key}  baseName={tmp.name}  newModName={itr.Value.name} " );
					continue;
				}
				// マジ舐めんな.
			//	elems.SetRow(itr.Value);
				elems.rows.Add(itr.Value);  
			//	elems.alias.Add(itr.Value.GetAlias, itr.Value);
			}
		}

		/// <summary>
		/// ゲーム開始直前に呼ばれる.
		/// </summary>
		public void OnLoadTableAfter() {
			// SourceManager::Init()後の処理はここ.
			// ゲーム中の全Elementのハッシュテーブルを作っておく.
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

			// 追加パーツ群の管理.
			ElemIDStart = GetElement(c_ElemName_ElemIDStart).id;
			ElemIDEnd = GetElement(c_ElemName_ElemIDEnd).id;
			m_NewModElemRows = new Dictionary<int, SourceElement.Row>();
			foreach (var itr in elemsMap) {
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
			if (things == null)
				return;
			try {
				for (int i = 0; i < things.Count; ++i) {
					var childs = things[i].things;
					_UninstallThings(childs);
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
		}




		// 保険処理.
		// バージョンアップでthingのelements.list に値が入っていないが、
		// ソケットには存在する、
		// というデータが発生した際のセーフティー処理.
		// セーブデータのロード時に該当のデータが存在したら強制的にelementを付与する.
		static List<int> s_TmpElementsListElemIDs;
		static List<(int,int)> s_TmpSocketElements;
		[HarmonyPatch(typeof(Card), "_OnDeserialized")]
		[HarmonyPostfix]
		public static void Postfix_OnDeserialized(Card __instance, StreamingContext context) {
			if (__instance == null)
				return;
			if (__instance.sockets == null || __instance.sockets.Count <= 0)
				return;
			if (__instance.elements == null || __instance.elements.list == null)
				return;
			if (s_TmpElementsListElemIDs == null) {
				s_TmpElementsListElemIDs = new List<int>(100);
				s_TmpSocketElements = new List<(int, int)>(100);
			}
			s_TmpSocketElements.Clear();

			// ソケット内のこちらで追加したパーツの情報を収集.
			for (int i = 0; i < __instance.sockets.Count; ++i) {
				if (__instance.sockets[i] == 0)
					continue;
				int elemID = __instance.sockets[i] / 100;
				if (elemID == 0)
					continue;
				if (!Instance.IsNewRangeModID(elemID))
					continue;
				int elemLv = __instance.sockets[i] % 100;
				s_TmpSocketElements.Add((elemID, elemLv));
			}

			// 存在していないelementをチェック.
			ElementContainer elem = __instance.elements;
			if (elem.list != null) {
				s_TmpElementsListElemIDs.Clear();
				for (int i = 0; i < elem.list.Count; i += 5) {
					s_TmpElementsListElemIDs.Add(elem.list[i]);
				}
				for (int i = 0; i < s_TmpSocketElements.Count; ++i) {
					var socketElem = s_TmpSocketElements[i];
					if (s_TmpElementsListElemIDs.Contains(socketElem.Item1))
						continue;
					// 存在していない場合はSetBaseで付与.
					DebugUtil.LogWarning($"[Warning!]Parts in socket are not included in elements.list  id={socketElem.Item1}");
					__instance.elements.SetBase(socketElem.Item1, socketElem.Item2);
				}
			}
			s_TmpSocketElements.Clear();
			s_TmpElementsListElemIDs.Clear();
		}
	}
}

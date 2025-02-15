using B83.Win32;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace Elin_Mod
{
	public class RuneSmithManager : Singleton<RuneSmithManager>
	{
		TraitRuneSmith m_TraitRuneSmith;
		TraitRuneCombine m_TraitRuneCombine;


		/// <summary>
		/// テーブル読み込み直後.
		/// </summary>
		public void OnLoadedTables() {
			// Elementsを改変.
			// noRuneタグを潰す.
			var elems = EClass.sources.elements;
			foreach (var itr in elems.map) {
				int index = System.Array.FindIndex(itr.Value.tag, v => v == Const.c_Tag_NoRune);
				if (index < 0)
					continue;
				itr.Value.tag[index] = string.Empty;
			}

		}

		/// <summary>
		/// 初期化.
		/// </summary>
		public void OnStartGame() {
			// データ読み込み.
			ModUtil.ImportExcel(CommonUtil.GetResourcePath("tables/add_datas.xlsx"), "recipes", EClass.sources.recipes);

			// UI初期化.
			ModUIEntry.Initialize();

			// Trait作っておく.
			m_TraitRuneSmith = _CreateTraitCrafter<TraitRuneSmith>();
			m_TraitRuneCombine = _CreateTraitCrafter<TraitRuneCombine>();
		}

		/// <summary>
		/// 破棄.
		/// </summary>
		public void Terminate() {
			m_TraitRuneSmith = null;
		}

		/// <summary>
		/// Gun Smith起動.
		/// </summary>
		public void Play_RuneSmith() {
			GameUtil.UseForceTraitCrafter(m_TraitRuneSmith);
		}

		public void Play_RuneCombine() {
			GameUtil.UseForceTraitCrafter(m_TraitRuneCombine);
		}

		/// <summary>
		/// Trait生成.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		T _CreateTraitCrafter<T>() where T : TraitCrafter, new() 
		{
			var dmyOwner = ThingGen.Create(Const.c_TargetToolName );	//< 設定されたツールをオーナーとする.
			var ret = new T();
			dmyOwner.trait = ret;
			ret.SetOwner(dmyOwner);

			return ret;
		}




	}
}

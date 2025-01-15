using B83.Win32;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace Elin_Mod
{
	public class GunSmithManager : Singleton<GunSmithManager>
	{
		TraitGunSmith m_TraitGunSmith;
		TraitModCombine m_TraitModCombine;

		/// <summary>
		/// 初期化.
		/// </summary>
		public void Initialize() {
			// データ読み込み.
			ModUtil.ImportExcel(CommonUtil.GetResourcePath("tables/add_datas.xlsx"), "recipes", EClass.sources.recipes);

			// UI初期化.
			ModUIEntry.Initialize();

			// Trait作っておく.
			m_TraitGunSmith = _CreateTraitCrafter<TraitGunSmith>();
			m_TraitModCombine = _CreateTraitCrafter<TraitModCombine>();
		}

		/// <summary>
		/// 破棄.
		/// </summary>
		public void Terminate() {
			m_TraitGunSmith = null;
		}

		/// <summary>
		/// Gun Smith起動.
		/// </summary>
		public void Play_GunSmith() {
			GameUtil.UseForceTraitCrafter(m_TraitGunSmith);
		}

		public void Play_ModCombine() {
			GameUtil.UseForceTraitCrafter(m_TraitModCombine);
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

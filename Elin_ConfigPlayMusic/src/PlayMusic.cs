using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HarmonyLib;

namespace Elin_Mod
{
	[HarmonyPatch]
	public class PlayMusic
	{
		/// <summary>
		/// ローカル関数を無理やりパッチ化する対処.
		/// </summary>
		/// <param name="harmony"></param>
		public static void ApplyPatch() {
			var manualPatches = new ModPatchInfo[1];
			var methodInfos = typeof(PlayMusic).GetMethods((System.Reflection.BindingFlags)~(0));

			var methodInfo_AIPlayMusicLevelSong = System.Array.Find(methodInfos, v => Regex.IsMatch(v.Name, "Prefix_AIPlayMusicLevelSong"));
			manualPatches[0] = new ModPatchInfo() {
				m_TargetType = typeof(AI_PlayMusic),
				m_Regex = "LevelSong",
				m_Prefix = methodInfo_AIPlayMusicLevelSong
			};

			for ( int i = 0; i < manualPatches.Length; ++i )
				MyModManager.Instance.AddPatch(manualPatches[i]);
		}


		static void Prefix_AIPlayMusicLevelSong( ref int a ) {
			a = UnityEngine.Mathf.RoundToInt( a * Plugin.Instance.ModConfig.AddMusicLvFactor.Value ); //< 四捨五入で良いや.
		}



		[HarmonyPatch(typeof(Point), "ListWitnesses")]
		[HarmonyPrefix]
		public static void Prefix_ListWitnesses(Chara criminal, ref int radius, WitnessType type, Chara target) {
			if (type != WitnessType.music)
				return;
			if (criminal != EClass.pc)
				return;
			radius = Plugin.Instance.ModConfig.EffectiveRange.Value;
		}
		

	}
}

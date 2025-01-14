using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elin_Mod
{
	public class Debug_AnalyzeElin
	{
		static string _DumpArray<T>(T[] array ) {
			string tmp = "";
			for ( int i = 0; i < array.Length; ++i ) {
				if (i > 0)
					tmp += "|";
				tmp += array[i];
			}
			return tmp;
		}

		public static void Dump_ElinThingAll( string dumpPath ) {
			var cards = EClass.sources.things;
			string tmp = "";
			tmp += "id,";
			tmp += "name_JP,";
			tmp += "unknown_JP,";
			tmp += "unit_JP,";
			tmp += "naming,";
			tmp += "name,";
			tmp += "unit,";
			tmp += "unknown,";
			tmp += "category,";
			tmp += "sort,";
			tmp += "_tileType,";
			tmp += "_idRenderData,";
			tmp += "tiles,";
			tmp += "altTiles,";
			tmp += "anime,";
			tmp += "skins,";
			tmp += "size,";
			tmp += "colorMod,";
			tmp += "colorType,";
			tmp += "recipeKey,";
			tmp += "factory,";
			tmp += "components,";
			tmp += "disassemble,";
			tmp += "defMat,";
			tmp += "tierGroup,";
			tmp += "value,";
			tmp += "LV,";
			tmp += "chance,";
			tmp += "quality,";
			tmp += "HP,";
			tmp += "weight,";
			tmp += "electricity,";
			tmp += "trait,";
			tmp += "elements,";
			tmp += "range,";
			tmp += "attackType,";
			tmp += "offense,";
			tmp += "substats,";
			tmp += "defense,";
			tmp += "lightData,";
			tmp += "idExtra,";
			tmp += "idToggleExtra,";
			tmp += "idActorEx,";
			tmp += "idSound,";
			tmp += "tag,";
			tmp += "workTag,";
			tmp += "filter,";
			tmp += "roomName_JP,";
			tmp += "roomName,";
			tmp += "detail_JP,";
			tmp += "detail,";
			tmp += "\n";
			foreach (var itr in cards.map) {
				var a = itr.Value;
				tmp += itr.Key;
				tmp += $"{a.id}";
				tmp += $",{a.name_JP}";
				tmp += $",{a.unknown_JP}";
				tmp += $",{a.unit_JP}";
				tmp += $",{a.naming}";
				tmp += $",{a.name}";
				tmp += $",{a.unit}";
				tmp += $",{a.unknown}";
				tmp += $",{a.category}";
				tmp += $",{a.sort}";
				tmp += $",{a._tileType}";
				tmp += $",{a._idRenderData}";
				tmp += $",{_DumpArray(a.tiles)}";
				tmp += $",{_DumpArray(a.altTiles)}";
				tmp += $",{_DumpArray(a.anime)}";
				tmp += $",{_DumpArray(a.skins)}";
				tmp += $",{_DumpArray(a.size)}";
				tmp += $",{a.colorMod}";
				tmp += $",{a.colorType}";
				tmp += $",{_DumpArray(a.recipeKey)}";
				tmp += $",{_DumpArray(a.factory)}";
				tmp += $",{_DumpArray(a.components)}";
				tmp += $",{_DumpArray(a.disassemble)}";
				tmp += $",{a.defMat}";
				tmp += $",{a.tierGroup}";
				tmp += $",{a.value}";
				tmp += $",{a.LV}";
				tmp += $",{a.chance}";
				tmp += $",{a.quality}";
				tmp += $",{a.HP}";
				tmp += $",{a.weight}";
				tmp += $",{a.electricity}";
				tmp += $",{_DumpArray(a.trait)}";
				tmp += $",{_DumpArray(a.elements)}";
				tmp += $",{a.range}";
				tmp += $",{a.attackType}";
				tmp += $",{_DumpArray(a.offense)}";
				tmp += $",{_DumpArray(a.substats)}";
				tmp += $",{_DumpArray(a.defense)}";
				tmp += $",{a.lightData}";
				tmp += $",{a.idExtra}";
				tmp += $",{a.idToggleExtra}";
				tmp += $",{a.idActorEx}";
				tmp += $",{a.idSound}";
				tmp += $",{_DumpArray(a.tag)}";
				tmp += $",{a.workTag}";
				tmp += $",{_DumpArray(a.filter)}";
				tmp += $",{_DumpArray(a.roomName_JP)}";
				tmp += $",{_DumpArray(a.roomName)}";
				tmp += $",{a.detail_JP}";
				tmp += $",{a.detail}";

				tmp += "\n";
			}

			if (System.IO.File.Exists(dumpPath))
				System.IO.File.Delete(dumpPath);
			System.IO.File.WriteAllText(dumpPath, tmp);
		}


		public static void Dump_ElinElementAll(string dumpPath) {
			var cards = EClass.sources.elements;
			string tmp = "";
			tmp += "id,";
			tmp += "alias,";
			tmp += "name_JP,";
			tmp += "name,";
			tmp += "altname_JP,";
			tmp += "altname,";
			tmp += "aliasParent,";
			tmp += "aliasRef,";
			tmp += "aliasMtp,";
			tmp += "parentFactor,";
			tmp += "lvFactor,";
			tmp += "encFactor,";
			tmp += "encSlot,";
			tmp += "mtp,";
			tmp += "LV,";
			tmp += "chance,";
			tmp += "value,";
			tmp += "cost,";
			tmp += "geneSlot,";
			tmp += "sort,";
			tmp += "target,";
			tmp += "proc,";
			tmp += "type,";
			tmp += "group,";
			tmp += "category,";
			tmp += "categorySub,";
			tmp += "abilityType,";
			tmp += "tag,";
			tmp += "thing,";
			tmp += "eleP,";
			tmp += "cooldown,";
			tmp += "charge,";
			tmp += "radius,";
			tmp += "max,";
			tmp += "req,";
			tmp += "idTrainer,";
			tmp += "partySkill,";
			tmp += "tagTrainer,";
			tmp += "levelBonus_JP,";
			tmp += "levelBonus,";
			tmp += "foodEffect,";
			tmp += "langAct,";
			tmp += "detail_JP,";
			tmp += "detail,";
			tmp += "textPhase_JP,";
			tmp += "textPhase,";
			tmp += "textExtra_JP,";
			tmp += "textExtra,";
			tmp += "textInc_JP,";
			tmp += "textInc,";
			tmp += "textDec_JP,";
			tmp += "textDec,";
			tmp += "textAlt_JP,";
			tmp += "textAlt,";
			tmp += "adjective_JP,";
			tmp += "adjective,";
			tmp += "\n";
			foreach (var itr in cards.map) {
				var a = itr.Value;
				tmp += $"{a.id}";
				tmp += $",{a.alias}";
				tmp += $",{a.name_JP}";
				tmp += $",{a.name}";
				tmp += $",{a.altname_JP}";
				tmp += $",{a.altname}";
				tmp += $",{a.aliasParent}";
				tmp += $",{a.aliasRef}";
				tmp += $",{a.aliasMtp}";
				tmp += $",{a.parentFactor}";
				tmp += $",{a.lvFactor}";
				tmp += $",{a.encFactor}";
				tmp += $",{a.encSlot}";
				tmp += $",{a.mtp}";
				tmp += $",{a.LV}";
				tmp += $",{a.chance}";
				tmp += $",{a.value}";
				tmp += $",{_DumpArray(a.cost)}";
				tmp += $",{a.geneSlot}";
				tmp += $",{a.sort}";
				tmp += $",{a.target}";
				tmp += $",{_DumpArray(a.proc)}";
				tmp += $",{a.type}";
				tmp += $",{a.group}";
				tmp += $",{a.category}";
				tmp += $",{a.categorySub}";
				tmp += $",{_DumpArray(a.abilityType)}";
				tmp += $",{_DumpArray(a.tag)}";
				tmp += $",{a.thing}";
				tmp += $",{a.eleP}";
				tmp += $",{a.cooldown}";
				tmp += $",{a.charge}";
				tmp += $",{a.radius}";
				tmp += $",{a.max}";
				tmp += $",{_DumpArray(a.req)}";
				tmp += $",{a.idTrainer}";
				tmp += $",{a.partySkill}";
				tmp += $",{a.tagTrainer}";
				tmp += $",{a.levelBonus_JP}";
				tmp += $",{a.levelBonus}";
				tmp += $",{_DumpArray(a.foodEffect)}";
				tmp += $",{_DumpArray(a.langAct)}";
				tmp += $",{a.detail_JP}";
				tmp += $",{a.detail}";
				tmp += $",{a.textPhase_JP}";
				tmp += $",{a.textPhase}";
				tmp += $",{a.textExtra_JP}";
				tmp += $",{a.textExtra}";
				tmp += $",{a.textInc_JP}";
				tmp += $",{a.textInc}";
				tmp += $",{a.textDec_JP}";
				tmp += $",{a.textDec}";
				tmp += $",{_DumpArray(a.textAlt_JP)}";
				tmp += $",{_DumpArray(a.textAlt)}";
				tmp += $",{_DumpArray(a.adjective_JP)}";
				tmp += $",{_DumpArray(a.adjective)}";

				tmp += "\n";
			}

			if (System.IO.File.Exists(dumpPath))
				System.IO.File.Delete(dumpPath);
			System.IO.File.WriteAllText(dumpPath, tmp);
		}





		public static void Dump_ElinRecipeAll(string dumpPath) {
			var cards = EClass.sources.recipes;
			string tmp = "";
			tmp += "id,";
			tmp += "factory,";
			tmp += "type,";
			tmp += "thing,";
			tmp += "num,";
			tmp += "sp,";
			tmp += "time,";
			tmp += "ing1,";
			tmp += "ing2,";
			tmp += "ing3,";
			tmp += "tag,";

			tmp += "\n";
			foreach (var itr in cards.map) {
				var a = itr.Value;
				tmp += $"{a.id}";
				tmp += $",{a.factory}";
				tmp += $",{a.type}";
				tmp += $",{a.thing}";
				tmp += $",{a.num}";
				tmp += $",{a.sp}";
				tmp += $",{a.time}";
				tmp += $",{_DumpArray(a.ing1)}";
				tmp += $",{_DumpArray(a.ing2)}";
				tmp += $",{_DumpArray(a.ing3)}";
				tmp += $",{_DumpArray(a.tag)}";
				tmp += "\n";
			}

			if (System.IO.File.Exists(dumpPath))
				System.IO.File.Delete(dumpPath);
			System.IO.File.WriteAllText(dumpPath, tmp);
		}

		public static void Dump_ElinFactionAll(string dumpPath) {
			var dats = EClass.sources.factions;
			string tmp = "";
			tmp += "id,";
			tmp += "factory,";
			tmp += "type,";
			tmp += "thing,";
			tmp += "num,";
			tmp += "sp,";
			tmp += "time,";
			tmp += "ing1,";
			tmp += "ing2,";
			tmp += "ing3,";
			tmp += "tag,";

			tmp += "\n";
			foreach (var itr in dats.map) {
				var a = itr.Value;
				tmp += $"{a.id}";
				tmp += $",{a.name_JP}";
				tmp += $",{a.name}";
				tmp += $",{a.type}";
				tmp += $",{a.faith}";
				tmp += $",{a.domain}";
				tmp += $",{a.relation}";
				tmp += $",{a.textType_JP}";
				tmp += $",{a.textType}";
				tmp += $",{a.textAvatar}";
				tmp += $",{a.detail_JP}";
				tmp += $",{a.detail}";
				tmp += "\n";
			}

			if (System.IO.File.Exists(dumpPath))
				System.IO.File.Delete(dumpPath);
			System.IO.File.WriteAllText(dumpPath, tmp);
		}
	}
}

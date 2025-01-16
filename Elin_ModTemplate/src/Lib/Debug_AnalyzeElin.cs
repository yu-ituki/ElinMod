using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static Lang;

namespace Elin_Mod
{
	public class Debug_AnalyzeElin
	{
		const string c_ElemSeparator = "\t";
		const string c_ArraySeparator = ",";
		const string c_StringEncloser = "\"";



		public static void Dump_ElinThingAll( string dumpPath ) {
			var cards = EClass.sources.things;
			var sb = _CreateSb(cards.map.Count, 1000);
			_DumpHeader( sb, "id" );
			_DumpHeader( sb, "name_JP" );
			_DumpHeader( sb, "unknown_JP" );
			_DumpHeader( sb, "unit_JP" );
			_DumpHeader( sb, "naming" );
			_DumpHeader( sb, "name" );
			_DumpHeader( sb, "unit" );
			_DumpHeader( sb, "unknown" );
			_DumpHeader( sb, "category" );
			_DumpHeader( sb, "sort" );
			_DumpHeader( sb, "_tileType" );
			_DumpHeader( sb, "_idRenderData" );
			_DumpHeader( sb, "tiles" );
			_DumpHeader( sb, "altTiles" );
			_DumpHeader( sb, "anime" );
			_DumpHeader( sb, "skins" );
			_DumpHeader( sb, "size" );
			_DumpHeader( sb, "colorMod" );
			_DumpHeader( sb, "colorType" );
			_DumpHeader( sb, "recipeKey" );
			_DumpHeader( sb, "factory" );
			_DumpHeader( sb, "components" );
			_DumpHeader( sb, "disassemble" );
			_DumpHeader( sb, "defMat" );
			_DumpHeader( sb, "tierGroup" );
			_DumpHeader( sb, "value" );
			_DumpHeader( sb, "LV" );
			_DumpHeader( sb, "chance" );
			_DumpHeader( sb, "quality" );
			_DumpHeader( sb, "HP" );
			_DumpHeader( sb, "weight" );
			_DumpHeader( sb, "electricity" );
			_DumpHeader( sb, "trait" );
			_DumpHeader( sb, "elements" );
			_DumpHeader( sb, "range" );
			_DumpHeader( sb, "attackType" );
			_DumpHeader( sb, "offense" );
			_DumpHeader( sb, "substats" );
			_DumpHeader( sb, "defense" );
			_DumpHeader( sb, "lightData" );
			_DumpHeader( sb, "idExtra" );
			_DumpHeader( sb, "idToggleExtra" );
			_DumpHeader( sb, "idActorEx" );
			_DumpHeader( sb, "idSound" );
			_DumpHeader( sb, "tag" );
			_DumpHeader( sb, "workTag" );
			_DumpHeader( sb, "filter" );
			_DumpHeader( sb, "roomName_JP" );
			_DumpHeader( sb, "roomName" );
			_DumpHeader( sb, "detail_JP" );
			_DumpHeader( sb, "detail" );
			sb.AppendLine();
			sb.AppendLine();
			sb.AppendLine();

			foreach (var itr in cards.map) {
				var a = itr.Value;
				_Dump(sb, a.id);
				_Dump(sb, a.name_JP);
				_Dump(sb, a.unknown_JP);
				_Dump(sb, a.unit_JP);
				_Dump(sb, a.naming);
				_Dump(sb, a.name);
				_Dump(sb, a.unit);
				_Dump(sb, a.unknown);
				_Dump(sb, a.category);
				_Dump(sb, a.sort);
				_Dump(sb, a._tileType);
				_Dump(sb, a._idRenderData);
				_Dump(sb, a.tiles);
				_Dump(sb, a.altTiles);
				_Dump(sb, a.anime);
				_Dump(sb, a.skins);
				_Dump(sb, a.size);
				_Dump(sb, a.colorMod);
				_Dump(sb, a.colorType);
				_Dump(sb, a.recipeKey);
				_Dump(sb, a.factory);
				_Dump(sb, a.components);
				_Dump(sb, a.disassemble);
				_Dump(sb, a.defMat);
				_Dump(sb, a.tierGroup);
				_Dump(sb, a.value);
				_Dump(sb, a.LV);
				_Dump(sb, a.chance);
				_Dump(sb, a.quality);
				_Dump(sb, a.HP);
				_Dump(sb, a.weight);
				_Dump(sb, a.electricity);
				_Dump(sb, a.trait);
				_Dump(sb, a.elements);
				_Dump(sb, a.range);
				_Dump(sb, a.attackType);
				_Dump(sb, a.offense);
				_Dump(sb, a.substats);
				_Dump(sb, a.defense);
				_Dump(sb, a.lightData);
				_Dump(sb, a.idExtra);
				_Dump(sb, a.idToggleExtra);
				_Dump(sb, a.idActorEx);
				_Dump(sb, a.idSound);
				_Dump(sb, a.tag);
				_Dump(sb, a.workTag);
				_Dump(sb, a.filter);
				_Dump(sb, a.roomName_JP);
				_Dump(sb, a.roomName);
				_Dump(sb, a.detail_JP);
				_Dump(sb, a.detail);
				sb.AppendLine();
			}
			_Save(dumpPath, sb);
		}


		public static void Dump_ElinElementAll(string dumpPath) {
			var cards = EClass.sources.elements;
			var sb = _CreateSb(cards.map.Count, 2000);
			
			string AAA(string[] tmp) {
				string ret = "";
				for (int i = 0; i < tmp.Length; ++i)
					ret += $"[{tmp[i]}], ";
				return ret;
			}

			_DumpHeader( sb, "id" );
			_DumpHeader( sb, "alias" );
			_DumpHeader( sb, "name_JP" );
			_DumpHeader( sb, "name" );
			_DumpHeader( sb, "altname_JP" );
			_DumpHeader( sb, "altname" );
			_DumpHeader( sb, "aliasParent" );
			_DumpHeader( sb, "aliasRef" );
			_DumpHeader( sb, "aliasMtp" );
			_DumpHeader( sb, "parentFactor" );
			_DumpHeader( sb, "lvFactor" );
			_DumpHeader( sb, "encFactor" );
			_DumpHeader( sb, "encSlot" );
			_DumpHeader( sb, "mtp" );
			_DumpHeader( sb, "LV" );
			_DumpHeader( sb, "chance" );
			_DumpHeader( sb, "value" );
			_DumpHeader( sb, "cost" );
			_DumpHeader( sb, "geneSlot" );
			_DumpHeader( sb, "sort" );
			_DumpHeader( sb, "target" );
			_DumpHeader( sb, "proc" );
			_DumpHeader( sb, "type" );
			_DumpHeader( sb, "group" );
			_DumpHeader( sb, "category" );
			_DumpHeader( sb, "categorySub" );
			_DumpHeader( sb, "abilityType" );
			_DumpHeader( sb, "tag" );
			_DumpHeader( sb, "thing" );
			_DumpHeader( sb, "eleP" );
			_DumpHeader( sb, "cooldown" );
			_DumpHeader( sb, "charge" );
			_DumpHeader( sb, "radius" );
			_DumpHeader( sb, "max" );
			_DumpHeader( sb, "req" );
			_DumpHeader( sb, "idTrainer" );
			_DumpHeader( sb, "partySkill" );
			_DumpHeader( sb, "tagTrainer" );
			_DumpHeader( sb, "levelBonus_JP" );
			_DumpHeader( sb, "levelBonus" );
			_DumpHeader( sb, "foodEffect" );
			_DumpHeader( sb, "langAct" );
			_DumpHeader( sb, "detail_JP" );
			_DumpHeader( sb, "detail" );
			_DumpHeader( sb, "textPhase_JP" );
			_DumpHeader( sb, "textPhase" );
			_DumpHeader( sb, "textExtra_JP" );
			_DumpHeader( sb, "textExtra" );
			_DumpHeader( sb, "textInc_JP" );
			_DumpHeader( sb, "textInc" );
			_DumpHeader( sb, "textDec_JP" );
			_DumpHeader( sb, "textDec" );
			_DumpHeader( sb, "textAlt_JP" );
			_DumpHeader( sb, "textAlt" );
			_DumpHeader( sb, "adjective_JP" );
			_DumpHeader( sb, "adjective" );
			sb.AppendLine();
			sb.AppendLine();
			sb.AppendLine();
			foreach (var itr in cards.map) {
				var a = itr.Value;

				if ( a.id  == 910 || a.id == 911 ) {
					DebugUtil.LogError( $"{a.id} ||| {a.category} ||| {a.categorySub} ||| {AAA(a.abilityType)} ||| {AAA(a.tag)} ||| {a.thing} ||| {a.eleP}" );
				}

				_Dump(sb, a.id);
				_Dump(sb, a.alias);
				_Dump(sb, a.name_JP);
				_Dump(sb, a.name);
				_Dump(sb, a.altname_JP);
				_Dump(sb, a.altname);
				_Dump(sb, a.aliasParent);
				_Dump(sb, a.aliasRef);
				_Dump(sb, a.aliasMtp);
				_Dump(sb, a.parentFactor);
				_Dump(sb, a.lvFactor);
				_Dump(sb, a.encFactor);
				_Dump(sb, a.encSlot);
				_Dump(sb, a.mtp);
				_Dump(sb, a.LV);
				_Dump(sb, a.chance);
				_Dump(sb, a.value);
				_Dump(sb, a.cost);
				_Dump(sb, a.geneSlot);
				_Dump(sb, a.sort);
				_Dump(sb, a.target);
				_Dump(sb, a.proc);
				_Dump(sb, a.type);
				_Dump(sb, a.group);
				_Dump(sb, a.category);
				_Dump(sb, a.categorySub);
				_Dump(sb, a.abilityType);
				_Dump(sb, a.tag);
				_Dump(sb, a.thing);
				_Dump(sb, a.eleP);
				_Dump(sb, a.cooldown);
				_Dump(sb, a.charge);
				_Dump(sb, a.radius);
				_Dump(sb, a.max);
				_Dump(sb, a.req);
				_Dump(sb, a.idTrainer);
				_Dump(sb, a.partySkill);
				_Dump(sb, a.tagTrainer);
				_Dump(sb, a.levelBonus_JP);
				_Dump(sb, a.levelBonus);
				_Dump(sb, a.foodEffect);
				_Dump(sb, a.langAct);
				_Dump(sb, a.detail_JP);
				_Dump(sb, a.detail);
				_Dump(sb, a.textPhase_JP);
				_Dump(sb, a.textPhase);
				_Dump(sb, a.textExtra_JP);
				_Dump(sb, a.textExtra);
				_Dump(sb, a.textInc_JP);
				_Dump(sb, a.textInc);
				_Dump(sb, a.textDec_JP);
				_Dump(sb, a.textDec);
				_Dump(sb, a.textAlt_JP);
				_Dump(sb, a.textAlt);
				_Dump(sb, a.adjective_JP);
				_Dump(sb, a.adjective);

				sb.AppendLine();
			}

			_Save(dumpPath, sb);
		}





		public static void Dump_ElinRecipeAll(string dumpPath) {
			var cards = EClass.sources.recipes;
			var sb = _CreateSb(cards.map.Count, 500);
			_DumpHeader( sb, "id" );
			_DumpHeader( sb, "factory" );
			_DumpHeader( sb, "type" );
			_DumpHeader( sb, "thing" );
			_DumpHeader( sb, "num" );
			_DumpHeader( sb, "sp" );
			_DumpHeader( sb, "time" );
			_DumpHeader( sb, "ing1" );
			_DumpHeader( sb, "ing2" );
			_DumpHeader( sb, "ing3" );
			_DumpHeader( sb, "tag" );

			sb.AppendLine();
			sb.AppendLine();
			sb.AppendLine();
			foreach (var itr in cards.map) {
				var a = itr.Value;
				_Dump(sb, a.id);
				_Dump(sb, a.factory);
				_Dump(sb, a.type);
				_Dump(sb, a.thing);
				_Dump(sb, a.num);
				_Dump(sb, a.sp);
				_Dump(sb, a.time);
				_Dump(sb, a.ing1);
				_Dump(sb, a.ing2);
				_Dump(sb, a.ing3);
				_Dump(sb, a.tag);
				sb.AppendLine();
			}

			_Save(dumpPath, sb);
		}

		public static void Dump_ElinFactionAll(string dumpPath) {
			var cards = EClass.sources.factions;
			var sb = _CreateSb(cards.map.Count, 1000);
			_DumpHeader( sb, "id" );
			_DumpHeader( sb, "factory" );
			_DumpHeader( sb, "type" );
			_DumpHeader( sb, "faith" );
			_DumpHeader( sb, "domain" );
			_DumpHeader( sb, "relation" );
			_DumpHeader( sb, "textType_JP" );
			_DumpHeader( sb, "textType" );
			_DumpHeader( sb, "textAvatar" );
			_DumpHeader( sb, "detail_JP" );
			_DumpHeader( sb, "detail" );
			_DumpHeader( sb, "name_L" );
			_DumpHeader( sb, "detail_L" );
			_DumpHeader( sb, "textType_L" );
			_DumpHeader( sb, "textBenefit_L" );
			_DumpHeader( sb, "textPet_L" );

			sb.AppendLine();
			sb.AppendLine();
			sb.AppendLine();
			foreach (var itr in cards.map) {
				var a = itr.Value;
				_Dump(sb, a.id);
				_Dump(sb, a.name_JP);
				_Dump(sb, a.name);
				_Dump(sb, a.type);
				_Dump(sb, a.faith);
				_Dump(sb, a.domain);
				_Dump(sb, a.relation);
				_Dump(sb, a.textType_JP);
				_Dump(sb, a.textType);
				_Dump(sb, a.textAvatar);
				_Dump(sb, a.detail_JP);
				_Dump(sb, a.detail);
				_Dump(sb, a.name_L);
				_Dump(sb, a.detail_L);
				_Dump(sb, a.textType_L);
				_Dump(sb, a.textBenefit_L);
				_Dump(sb, a.textPet_L);
				sb.AppendLine();
			}

			_Save(dumpPath, sb);
		}



		public static void Dump_ElinLangGeneral(string dumpPath) {
			var cards = EClass.sources.langGeneral;
			var sb = _CreateSb(cards.map.Count, 1000);
			_DumpHeader( sb, "id");
			_DumpHeader( sb, "filter");
			_DumpHeader( sb, "text_JP");
			_DumpHeader( sb, "text");

			sb.AppendLine();
			sb.AppendLine();
			sb.AppendLine();

			foreach (var itr in cards.map) {
				var a = itr.Value;
				_Dump(sb, a.id);
				_Dump(sb, a.filter);
				_Dump(sb, a.text_JP);
				_Dump(sb, a.text);
				sb.AppendLine();
			}

			_Save(dumpPath, sb);
		}












		static void _Save( string path, StringBuilder sb ) {
			if (System.IO.File.Exists(path))
				System.IO.File.Delete(path);
			System.IO.File.WriteAllText(path, sb.ToString());
		}

		static void _Dump<T>(System.Text.StringBuilder sb, T dat, bool isArrayElem=false) {
			if (dat == null) {
				sb.Append(c_ElemSeparator);
				return;
			}
			var type = dat.GetType();
			if (type.IsArray) {
				var array = dat as Array;
				bool isStrElem = type.GetElementType() == typeof(string);
				if ( isStrElem)
					sb.Append(c_StringEncloser);

				for (int i = 0; i < array.Length; ++i) {
					var d = array.GetValue(i);
					if (i > 0)
						sb.Append(c_ArraySeparator);
					_Dump(sb, d, true);
				}

				if (isStrElem)
					sb.Append(c_StringEncloser);

				sb.Append(c_ElemSeparator);
			} else {
				bool isStrElem = dat is string;
				if (!isArrayElem && isStrElem)
					sb.Append(c_StringEncloser);

				sb.Append(dat);

				if (!isArrayElem && isStrElem)
					sb.Append(c_StringEncloser);
				
				if (!isArrayElem)
					sb.Append(c_ElemSeparator);
			}
		}

		static void _DumpHeader(StringBuilder sb, string label) {
			sb.Append(label).Append(c_ElemSeparator);
		}

		static System.Text.StringBuilder _CreateSb(int elemCount, int dataSize) {
			return new System.Text.StringBuilder(dataSize * (elemCount + 2));
		}
	}
}

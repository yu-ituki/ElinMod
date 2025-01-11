using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elin_ModTemplate
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
				tmp += $",{a.id}";
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
	}
}

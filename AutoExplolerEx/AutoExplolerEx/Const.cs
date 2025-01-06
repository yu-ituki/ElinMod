using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elin_AutoExplore
{
	/// <summary>
	/// 文字列定義.
	/// </summary>
	public enum eModText
	{
		Text_AutoExploreSettings,	//< "オートエクスプローラー設定"; 
		
		Handle_Fighting,		//< "オートエクスプローラーは戦闘を処理するべきですか？";
		Handle_Harvestables,	//< "オートエクスプローラーは収穫物を処理するべきですか？";
		Handle_Mineables,		//< "オートエクスプローラーは壁を処理するべきですか？";
		Handle_Traps,			//< "オートエクスプローラーは罠を処理するべきですか？";
		Handle_Shrines,			//< "オートエクスプローラーは神殿を処理するべきですか？";
		Handle_Meditation,		//< "オートエクスプローラーは瞑想を使用するべきですか？";
		Handle_Hunger,			//< "オートエクスプローラーは食事を摂るべきですか？"; break;
		Handle_Vegetables,      //< "オートエクスプローラーは野菜を処理するべきですか？"; break;

		Text_MinMP,					//< "瞑想を開始する最小MP"; break;
		Text_MinHP,					//< "瞑想を開始する最小HP"; break;
		
		Mode_Harvesting,		//< "収穫モード"; break;
		Mode_Mining,			//< "鉱業モード"; break;
		Mode_Exploring,         //< 探索モード.
		Mode_MineOreOnly,		//< 鉱石のみモード.
		Mode_Vegetables,		//< 野菜モード".


		Error,					//< "error".


		MAX
	};



	public class Const
	{


	}
}

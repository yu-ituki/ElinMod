using System.Net;
using System.Collections.Generic;

namespace Elin_AutoExplore
{

	public static class Translations
	{
		static Dictionary<string, string[]> s_Texts;
		static bool s_IsInitialized;
		public static bool IsInitialized() {
			return s_IsInitialized;
		}

		public static void Initialize() {
			if (s_IsInitialized)
				return;
			s_IsInitialized = true;

			s_Texts?.Clear();
			s_Texts = new Dictionary<string, string[]>(10);

			string[] text = new string[(int)eModText.MAX];
			text[(int)eModText.Text_AutoExploreSettings] = "オートエクスプローラー設定";
			text[(int)eModText.Handle_Fighting] = "オートエクスプローラーは戦闘を処理するべきですか？";
			text[(int)eModText.Handle_Harvestables] = "オートエクスプローラーは収穫物を処理するべきですか？";
			text[(int)eModText.Handle_Mineables] = "オートエクスプローラーは壁を処理するべきですか？";
			text[(int)eModText.Handle_Traps] = "オートエクスプローラーは罠を処理するべきですか？";
			text[(int)eModText.Handle_Shrines] = "オートエクスプローラーは神殿を処理するべきですか？";
			text[(int)eModText.Handle_Meditation] = "オートエクスプローラーは瞑想を処理するべきですか？";
			text[(int)eModText.Handle_Hunger] = "オートエクスプローラーは食事を処理するべきですか？";
			text[(int)eModText.Handle_Vegetables] = "オートエクスプローラーは野菜を処理するべきですか？";
			text[(int)eModText.Handle_Quest_Harvest] = "オートエクスプローラーはクエスト収穫を処理するべきですか？";
			text[(int)eModText.Handle_Quest_War] = "オートエクスプローラーはクエスト戦争を処理するべきですか？";
			text[(int)eModText.Text_MinHP] = "瞑想を開始する最小HP";
			text[(int)eModText.Text_MinMP] = "瞑想を開始する最小MP";
			text[(int)eModText.Mode_Harvesting] = "収穫モード";
			text[(int)eModText.Mode_Mining] = "鉱業モード";
			text[(int)eModText.Mode_Exploring] = "探索モード";
			text[(int)eModText.Mode_Vegetables] = "野菜モード";
			text[(int)eModText.Mode_MineOreOnly] = "鉱石のみモード";
			text[(int)eModText.Error] = "error";
			s_Texts.Add("JP", text);

			text = new string[(int)eModText.MAX];
			text[(int)eModText.Error] = "error";
			text[(int)eModText.Text_AutoExploreSettings] = "自动探索设置";
			text[(int)eModText.Handle_Fighting] = "自动探索是否应处理战斗？";
			text[(int)eModText.Handle_Harvestables] = "自动探索是否应处理可收获物？";
			text[(int)eModText.Handle_Mineables] = "自动探索是否应处理可挖掘物？";
			text[(int)eModText.Handle_Traps] = "自动探索是否应处理陷阱？";
			text[(int)eModText.Handle_Shrines] = "自动探索是否应处理神殿？";
			text[(int)eModText.Handle_Meditation] = "自动探索是否应使用冥想？";
			text[(int)eModText.Handle_Hunger] = "自动探索是否应吃食物？";
			text[(int)eModText.Handle_Vegetables] = "自动探索是否应处理野菜？";
			text[(int)eModText.Handle_Quest_Harvest] = "自动探索是否应处理Quest野菜？";
			text[(int)eModText.Handle_Quest_War] = "自动探索是否应处理Quest戦争？";

			text[(int)eModText.Text_MinHP] = "开始冥想的最低HP";
			text[(int)eModText.Text_MinMP] = "开始冥想的最低MP";
			text[(int)eModText.Mode_Harvesting] = "收获模式";
			text[(int)eModText.Mode_Mining] = "采矿模式";
			text[(int)eModText.Mode_Exploring] = "探索模式";
			text[(int)eModText.Mode_Vegetables] = "野菜模式";
			text[(int)eModText.Mode_MineOreOnly] = "鉱石模式";
			s_Texts.Add("CN", text);

			text = new string[(int)eModText.MAX];
			text[(int)eModText.Error] = "error";
			text[(int)eModText.Text_AutoExploreSettings] = "自動探索設置";
			text[(int)eModText.Handle_Fighting] = "自動探索是否應處理戰鬥？";
			text[(int)eModText.Handle_Harvestables] = "自動探索是否應處理可收穫物？";
			text[(int)eModText.Handle_Mineables] = "自動探索是否應處理可挖掘物？";
			text[(int)eModText.Handle_Traps] = "自動探索是否應處理陷阱？";
			text[(int)eModText.Handle_Shrines] = "自動探索是否應處理神殿？";
			text[(int)eModText.Handle_Meditation] = "自動探索是否應使用冥想？";
			text[(int)eModText.Handle_Hunger] = "自動探索是否應吃食物？";
			text[(int)eModText.Handle_Vegetables] = "自動探索是否應處理野菜？";
			text[(int)eModText.Handle_Quest_Harvest] = "自動探索是否應處理Quest野菜？";
			text[(int)eModText.Handle_Quest_War] = "自動探索是否應處理Quest戦争？";

			text[(int)eModText.Text_MinHP] = "開始冥想的最低MP";
			text[(int)eModText.Text_MinMP] = "開始冥想的最低MP";
			text[(int)eModText.Mode_Harvesting] = "收获模式";
			text[(int)eModText.Mode_Mining] = "采矿模式";
			text[(int)eModText.Mode_Exploring] = "探索模式";
			text[(int)eModText.Mode_Vegetables] = "野菜模式";
			text[(int)eModText.Mode_MineOreOnly] = "鉱石模式";
			s_Texts.Add("ZHTW", text);


			text = new string[(int)eModText.MAX];
			text[(int)eModText.Error] = "error";
			text[(int)eModText.Text_AutoExploreSettings] = "AutoExplore Settings";
			text[(int)eModText.Handle_Fighting] = "Should AutoExplore handle fighting？";
			text[(int)eModText.Handle_Harvestables] = "Should AutoExplore handle harvestables？";
			text[(int)eModText.Handle_Mineables] = "Should AutoExplore handle mineables？";
			text[(int)eModText.Handle_Traps] = "Should AutoExplore handle traps？";
			text[(int)eModText.Handle_Shrines] = "Should AutoExplore handle shrines？";
			text[(int)eModText.Handle_Meditation] = "Should AutoExplore use meditation？";
			text[(int)eModText.Handle_Hunger] = "Should AutoExplore eat food？";
			text[(int)eModText.Handle_Vegetables] = "Should AutoExplore handle vegetables？";
			text[(int)eModText.Handle_Quest_Harvest] = "Should AutoExplore handle quest of harvest？";
			text[(int)eModText.Handle_Quest_War] = "Should AutoExplore handle quest of war？";

			text[(int)eModText.Text_MinHP] = "Minimum HP to start meditation";
			text[(int)eModText.Text_MinMP] = "Minimum MP to start meditation";
			text[(int)eModText.Mode_Harvesting] = "Harvesting mode";
			text[(int)eModText.Mode_Mining] = "Mining mode";
			text[(int)eModText.Mode_Exploring] = "Exploring mode";
			text[(int)eModText.Mode_Vegetables] = "Vegetables mode";
			text[(int)eModText.Mode_MineOreOnly] = "MineOreOnly mode";
			s_Texts.Add("EN", text);
		}



		public static string GetTranslation(eModText id) {
			if (!s_IsInitialized)
				Initialize();

			string lang = EClass.core.config.lang;
			string[] texts = null;
			if (!s_Texts.TryGetValue(lang, out texts))
				s_Texts.TryGetValue("EN", out texts);
			return texts[(int)id];
		}
	}
}

namespace Elin_AutoExplore
{

	public static class Translations
	{
		public const string HarvestingMode = "Harvesting mode";

		public const string MiningMode = "Mining mode";

		public const string HarvestingAndMiningMode = "Harvesting and mining mode";

		public const string ExploringMode = "Exploring mode";

		public static string GetTranslation(string id) {
			string lang = EClass.core.config.lang;
			if (1 == 0) {
			}
			string result;
			switch (lang) {
				case "JP": {
						switch ( id ) {
							case "AutoExplore Settings" : result = "オートエクスプローラー設定"; break;
							case "HandleFighting" : result = "オートエクスプローラーは戦闘を処理するべきですか？"; break;
							case "HandleHarvestables" : result = "オートエクスプローラーは収穫物を処理するべきですか？"; break;
							case "HandleMineables" : result = "オートエクスプローラーは鉱石を処理するべきですか？"; break;
							case "HandleTraps" : result = "オートエクスプローラーは罠を処理するべきですか？"; break;
							case "HandleShrines" : result = "オートエクスプローラーは神殿を処理するべきですか？"; break;
							case "UseMeditation" : result = "オートエクスプローラーは瞑想を使用するべきですか？"; break;
							case "HandleHunger" : result = "オートエクスプローラーは食事を摂るべきですか？"; break;
							case "MinMP" : result = "瞑想を開始する最小MP"; break;
							case "MinHP" : result = "瞑想を開始する最小HP"; break;
							case "Harvesting mode" : result = "収穫モード"; break;
							case "Mining mode" : result = "鉱業モード"; break;
							case "Harvesting and mining mode" : result = "収穫と鉱業モード"; break;
							case "Exploring mode" : result = "探索モード"; break;



							case "HandleMineOreOnly" : result = "鉱石のみモード"; break;
							case "HandleVegetables" : result = "野菜モード"; break;


							default : result = "error"; break;
						};
						break;
					}
				case "CN": {
						switch ( id )
						{
							case "AutoExplore Settings" : result = "自动探索设置"; break;
							case "HandleFighting" : result = "自动探索是否应处理战斗？"; break;
							case "HandleHarvestables" : result = "自动探索是否应处理可收获物？"; break;
							case "HandleMineables" : result = "自动探索是否应处理可挖掘物？"; break;
							case "HandleTraps" : result = "自动探索是否应处理陷阱？"; break;
							case "HandleShrines" : result = "自动探索是否应处理神殿？"; break;
							case "UseMeditation" : result = "自动探索是否应使用冥想？"; break;
							case "HandleHunger" : result = "自动探索是否应吃食物？"; break;
							case "MinMP" : result = "开始冥想的最低MP"; break;
							case "MinHP" : result = "开始冥想的最低HP"; break;
							case "Harvesting mode" : result = "收获模式"; break;
							case "Mining mode" : result = "采矿模式"; break;
							case "Harvesting and mining mode" : result = "收获和采矿模式"; break;
							case "Exploring mode" : result = "探索模式"; break;
								 
							case "HandleMineOreOnly" : result = "鉱石"; break;
							case "HandleVegetables" : result = "野菜"; break;


							default : result = "error"; break;
						};
						break;
					}
				case "ZHTW": {
					switch (id )
					{
							case "AutoExplore Settings" : result = "自動探索設置"; break;
							case "HandleFighting" : result = "自動探索是否應處理戰鬥？"; break;
							case "HandleHarvestables" : result = "自動探索是否應處理可收穫物？"; break;
							case "HandleMineables" : result = "自動探索是否應處理可挖掘物？"; break;
							case "HandleTraps" : result = "自動探索是否應處理陷阱？"; break;
							case "HandleShrines" : result = "自動探索是否應處理神殿？"; break;
							case "UseMeditation" : result = "自動探索是否應使用冥想？"; break;
							case "HandleHunger" : result = "自動探索是否應吃食物？"; break;
							case "MinMP" : result = "開始冥想的最低MP"; break;
							case "MinHP" : result = "開始冥想的最低HP"; break;
							case "Harvesting mode" : result = "收穫模式"; break;
							case "Mining mode" : result = "採礦模式"; break;
							case "Harvesting and mining mode" : result = "收穫和採礦模式"; break;
							case "Exploring mode" : result = "探索模式"; break;

							case "HandleMineOreOnly" : result = "鉱石"; break;
							case "HandleVegetables" : result = "野菜"; break;
						   
							default : result = "error"; break;
						};
						break;
					}
				default: {
						switch (id)
							{
							case "AutoExplore Settings" : result = "AutoExplore Settings"; break;
							case "HandleFighting" : result = "Should AutoExplore handle fighting?"; break;
							case "HandleHarvestables" : result = "Should AutoExplore handle harvestables?"; break;
							case "HandleMineables" : result = "Should AutoExplore handle mineables?"; break;
							case "HandleTraps" : result = "Should AutoExplore handle traps?"; break;
							case "HandleShrines" : result = "Should AutoExplore handle shrines?"; break;
							case "UseMeditation" : result = "Should AutoExplore use meditation?"; break;
							case "HandleHunger" : result = "Should AutoExplore eat food?"; break;
							case "MinMP" : result = "Minimum MP to start meditation"; break;
							case "MinHP" : result = "Minimum HP to start meditation"; break;
							case "Harvesting mode" : result = "Harvesting mode"; break;
							case "Mining mode" : result = "Mining mode"; break;
							case "Harvesting and mining mode" : result = "Harvesting and mining mode"; break;
							case "Exploring mode" : result = "Exploring mode"; break;

							case "HandleMineOreOnly" : result = "HandleMineOreOnly"; break;
							case "HandleVegetables" : result = "HandleVegetables"; break;

							default : result = "error"; break;
						};
						break;
					}
			}
			return result;
		}
	}
}

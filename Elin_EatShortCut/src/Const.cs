namespace Elin_Mod
{

	public class Const
	{
		public const int c_ElemID_JustCooked = 757;
		public const int c_ElemID_Nutrition = 10;


		public static string[] s_ConfigTexts_StopEatState;
		public static string[] s_ConfigTexts_EatPriority;
		public static void Initialize() {
			if (s_ConfigTexts_StopEatState == null) {
				s_ConfigTexts_StopEatState = new string[(int)ModConfig.eHungerState.MAX];
				_LoadConfigText(s_ConfigTexts_StopEatState, eTextID.Config_StopEatState_Normal, (int)ModConfig.eHungerState.Normal);
				_LoadConfigText(s_ConfigTexts_StopEatState, eTextID.Config_StopEatState_Bloated, (int)ModConfig.eHungerState.Bloated);
				_LoadConfigText(s_ConfigTexts_StopEatState, eTextID.Config_StopEatState_Filled, (int)ModConfig.eHungerState.Filled);
				_LoadConfigText(s_ConfigTexts_StopEatState, eTextID.Config_StopEatState_Hungry, (int)ModConfig.eHungerState.Hungry);
				_LoadConfigText(s_ConfigTexts_StopEatState, eTextID.Config_StopEatState_Starving, (int)ModConfig.eHungerState.Starving);
				_LoadConfigText(s_ConfigTexts_StopEatState, eTextID.Config_StopEatState_VeryHungry, (int)ModConfig.eHungerState.VeryHungry);
			}
			if (s_ConfigTexts_EatPriority == null) {
				s_ConfigTexts_EatPriority = new string[(int)ModConfig.eEatPriority.MAX];
				_LoadConfigText(s_ConfigTexts_EatPriority, eTextID.Config_EatPriority_HighCHA, (int)ModConfig.eEatPriority.HighCHA);
				_LoadConfigText(s_ConfigTexts_EatPriority, eTextID.Config_EatPriority_HighDEX, (int)ModConfig.eEatPriority.HighDEX);
				_LoadConfigText(s_ConfigTexts_EatPriority, eTextID.Config_EatPriority_HighEND, (int)ModConfig.eEatPriority.HighEND);
				_LoadConfigText(s_ConfigTexts_EatPriority, eTextID.Config_EatPriority_HighLER, (int)ModConfig.eEatPriority.HighLER);
				_LoadConfigText(s_ConfigTexts_EatPriority, eTextID.Config_EatPriority_HighMAG, (int)ModConfig.eEatPriority.HighMAG);
				_LoadConfigText(s_ConfigTexts_EatPriority, eTextID.Config_EatPriority_HighPER, (int)ModConfig.eEatPriority.HighPER);
				_LoadConfigText(s_ConfigTexts_EatPriority, eTextID.Config_EatPriority_HighSTR, (int)ModConfig.eEatPriority.HighSTR);
				_LoadConfigText(s_ConfigTexts_EatPriority, eTextID.Config_EatPriority_HighWIL, (int)ModConfig.eEatPriority.HighWIL);
				_LoadConfigText(s_ConfigTexts_EatPriority, eTextID.Config_EatPriority_HighNutrition, (int)ModConfig.eEatPriority.HighNutrition);
				_LoadConfigText(s_ConfigTexts_EatPriority, eTextID.Config_EatPriority_LowNutrition, (int)ModConfig.eEatPriority.LowNutrition);
				//	_LoadConfigText(s_ConfigTexts_EatPriority, eTextID.Config_EatPriority_LowPrice, (int)ModConfig.eEatPriority.LowPrice);
				//	_LoadConfigText(s_ConfigTexts_EatPriority, eTextID.Config_EatPriority_HighPrice, (int)ModConfig.eEatPriority.HighPrice);
				_LoadConfigText(s_ConfigTexts_EatPriority, eTextID.Config_EatPriority_Normal, (int)ModConfig.eEatPriority.Normal);
			}

			void _LoadConfigText(string[] outStrs, eTextID id, int index) {
				outStrs[index] = ModTextManager.Instance.GetText(id);
			}
		}

	}
}

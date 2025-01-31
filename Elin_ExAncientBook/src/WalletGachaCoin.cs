using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

namespace Elin_Mod
{
	public class WalletGachaCoin
	{
		class CoinData {
			string m_Thing;
			System.Func<int> m_GetCostFunc;
			int m_HaveRaw;

			int m_ReserveModNum;

			public static CoinData Create( string thing, System.Func<int> getCostFunc ) {
				CoinData ret = new CoinData();
				ret.m_Thing = thing;
				ret.m_GetCostFunc = getCostFunc;
				ret.RefreshHaveRaw();
				return ret;
			}

			public void Terminate() {
				m_GetCostFunc = null;
				m_Thing = null;
			}

			public void Clear() {
				m_ReserveModNum = 0;
				RefreshHaveRaw();
			}

			public void RefreshHaveRaw() {
				int sum = 0;
				SourceMaterial.Row mat = null;
				m_HaveRaw = EClass.pc.things.GetCurrency(m_Thing, ref sum, mat );
			}

			public void ReserveUseNum( int num ) {
				m_ReserveModNum -= num;
			}

			public void ReserveAddNum( int num ) {
				m_ReserveModNum += num;
			}

			public void Transaction() {
			//	DebugUtil.LogError($"transaction :  {m_Thing} : {m_ReserveModNum}");
				if (m_ReserveModNum != 0) {
					var tmp = EClass.pc.things.ListCurrency(m_Thing);
					if ( tmp.Count > 0 ) {
						tmp[0].ModNum(m_ReserveModNum);
					} else {
						EClass.pc.AddThing(ThingGen.Create(m_Thing).SetNum(m_ReserveModNum));
					}
			//		DebugUtil.LogError($"2 :  {tmp.Count} : {tmp[0].Name}");
				//	EClass.pc.ModCurrency(m_ReserveModNum, m_Thing);	//< なんかきかない、なんで？.
				}
				m_ReserveModNum = 0;
				RefreshHaveRaw();
			}

			public int GetHaveRaw() {
				return m_HaveRaw;
			}

			public int GetHave() {
				return m_HaveRaw * m_GetCostFunc();
			}

			public int GetCost() {
				return m_GetCostFunc();
			}

		
		}

		List<CoinData> m_CoinDatas; 


		public void Initialize( ModConfig config ) {
			m_CoinDatas = new List<CoinData>(4);
			m_CoinDatas.Add(CoinData.Create("gacha_coin", () => config.Worth_GachaCoin_Copper.Value ));
			m_CoinDatas.Add(CoinData.Create("gacha_coin_silver", () => config.Worth_GachaCoin_Silver.Value));
			m_CoinDatas.Add(CoinData.Create("gacha_coin_gold", () => config.Worth_GachaCoin_Gold.Value));
			m_CoinDatas.Add(CoinData.Create("gacha_coin_plat", () => config.Worth_GachaCoin_Plat.Value));
		}

		public void Terminate() {
			if (m_CoinDatas != null) {
				foreach (var itr in m_CoinDatas)
					itr.Terminate();
				m_CoinDatas.Clear();
				m_CoinDatas = null;
			}
		}

		public int GetHave() {
			int ret = 0;
			for (int i = 0; i < m_CoinDatas.Count; ++i)
				ret += m_CoinDatas[i].GetHave();
			return ret;
		}

		public int GetHaveRaw( int index ) {
			return m_CoinDatas[index].GetHaveRaw();
		}

		/// <summary>
		/// 取得.
		/// </summary>
		/// <param name="num"></param>
		/// <returns></returns>
		public bool Add( int num ) {

			if (!_AddTransaction(num))
				return false;

			// 異常がなければ取得実行.
			for (int i = 0; i < m_CoinDatas.Count; ++i) {
				m_CoinDatas[i].Transaction();
			}

			return true;
		}

		bool _AddTransaction( int num ) {
			// 取得処理.
			// でかい順でもらっていく.
			int remainNum = num;
			for (int i = m_CoinDatas.Count - 1; i >= 0; --i) {
				var coin = m_CoinDatas[i];
				int cost = coin.GetCost();
				if (cost <= 0)
					continue;

				int getNum = Mathf.FloorToInt((float)remainNum / (float)cost);
				//			DebugUtil.LogError($"a2 : {cost} : {remainNum} : {getNum}");
				if (getNum >= 1) {
					// 払えそう.
					coin.ReserveAddNum(getNum);
					remainNum = remainNum % cost;   //< あまりを入れる.
													//				DebugUtil.LogError($"a3 : {remainNum}");
				} else {
					// デカすぎなのでスルー.
					//				DebugUtil.LogError($"a4");
				}
			}

			// コンフィグ次第では余る. まだ余ってたら仕方ないので一番安い通貨を1枚あげよう.
			if (remainNum > 0) {
				m_CoinDatas[0].ReserveAddNum(1);
			} else if (remainNum < 0) {
				// 異常なンだわ.
				DebugUtil.LogError($"[WalletGachaCoin] Error!!! invalid remain add num --> input={num}  ");
				return false;
			}

			return true;
		}


		/// <summary>
		/// 支払い.
		/// </summary>
		/// <param name="num"></param>
		/// <returns></returns>
		public bool Pay( int num ) {

			int allHave = GetHave();
			if (num > allHave) {
				SE.Beep();
				Msg.Say("notEnoughMoney");
				return false; //< 金ねンだわ.
			}

			foreach (var itr in m_CoinDatas)
				itr.Clear();

			int changeNum = 0; //< おつり.
			int remainNum = num;	//< 計算中のあまり.

			// とりあえず計算が楽なので大きい順に払っていこう.
			// 小さい順はめんどい.
			for (int i = m_CoinDatas.Count -1; i >= 0; --i) {
				var coin = m_CoinDatas[i];
				int cost = coin.GetCost();
				if (cost <= 0)
					continue;
				int useNum = 0;
				if (remainNum < cost) {
					useNum = 1;
				} else {
					useNum = Mathf.CeilToInt( (float)remainNum / (float)cost );
				}
				int haveNum = coin.GetHaveRaw();
			//	DebugUtil.LogError($"a1 : {useNum} : {haveNum} : {cost} : {remainNum} : {coin.GetHave()}");
				if ( useNum <= haveNum ) {
					// 足りた.
					coin.ReserveUseNum(useNum);
					int payCount = useNum * cost;
					changeNum = payCount - remainNum;
			//		DebugUtil.LogError($"a2 : {payCount} : {changeNum}");
					break;
				}
				else {
					// 金ねンだわ.
					remainNum -= coin.GetHave();	//< 引くンだわ.
					coin.ReserveUseNum(haveNum);    //< 全額もらうンだわ. ｸﾞﾜｼｬ.
			//		DebugUtil.LogError($"a3 : {remainNum}");
				}
			}

		

			// お釣りチェック.
			if ( changeNum == 0) {
				// ジャストだった.
				
			} else if ( changeNum < 0 ) {
				// 異常なンだわ.
				DebugUtil.LogError( $"[WalletGachaCoin] Error!!! invalid changeNum --> input={num}  allHave={allHave}" );
				return false;
			} else {
				// お釣りがあったらもらう.
				if (!_AddTransaction(changeNum))
					return false;
			}

			// 異常がなければ支払い実行.
			for (int i = 0; i < m_CoinDatas.Count; ++i) {
				m_CoinDatas[i].Transaction();
			}
			//	SE.Pay();
			//	ModTextManager.Instance.SetUserData(0, num);
			//	ModTextManager.Instance.SetUserData(1, changeNum);
			//	Msg.SayRaw(ModTextManager.Instance.GetText(eTextID.Msg_PayEnd));
			return true;
		}

	}
}

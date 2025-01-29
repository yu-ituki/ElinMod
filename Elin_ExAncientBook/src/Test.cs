using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elin_Mod
{
	public class Test
	{
		public static void Run() {
			Test_Wallet();
		}

		static void Test_Wallet() {
			var wallet = Plugin.Instance.MyWallet;
			DebugUtil.LogError( $"{wallet.GetHave()} : {wallet.GetHaveRaw(0)} : {wallet.GetHaveRaw(1)} : {wallet.GetHaveRaw(2)} : {wallet.GetHaveRaw(3)}" );
			wallet.Pay(18);
			DebugUtil.LogError($"{wallet.GetHave()} : {wallet.GetHaveRaw(0)} : {wallet.GetHaveRaw(1)} : {wallet.GetHaveRaw(2)} : {wallet.GetHaveRaw(3)}");
		}

	}
}

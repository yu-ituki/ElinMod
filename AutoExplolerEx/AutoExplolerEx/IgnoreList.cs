using System.Collections.Generic;
using BepInEx.Configuration;

namespace Elin_AutoExplore
{

	public class IgnoreList
	{
		private readonly ConfigEntry<string> gatheringExclusionConfigList;

		private readonly ConfigEntry<string> miningExclusionConfigList;

		private HashSet<string> gatheringExclusionList;

		private HashSet<string> miningExclusionList;

		public IgnoreList(ConfigEntry<string> gatheringExclusionList, ConfigEntry<string> miningExclusionList) {
			gatheringExclusionConfigList = gatheringExclusionList;
			miningExclusionConfigList = miningExclusionList;
			this.gatheringExclusionList = new HashSet<string>(gatheringExclusionConfigList.Value.TrimStart(new char[1] { ',' }).Split(new char[1] { ',' }));
			this.miningExclusionList = new HashSet<string>(miningExclusionConfigList.Value.TrimStart(new char[1] { ',' }).Split(new char[1] { ',' }));
		}

		public bool IsIgnoredFromGathering(string name) {
			return gatheringExclusionList.Contains(name);
		}

		public bool IsIgnoredFromMining(string name) {
			return miningExclusionList.Contains(name);
		}

		public void AddToGatheringIgnoreList(string name) {
			gatheringExclusionList.Add(name);
			gatheringExclusionConfigList.Value = string.Join(",", gatheringExclusionList);
		}

		public void AddToMiningIgnoreList(string name) {
			miningExclusionList.Add(name);
			miningExclusionConfigList.Value = string.Join(",", miningExclusionList);
		}

		public void RemoveFromGatheringIgnoreList(string name) {
			gatheringExclusionList.Remove(name);
			gatheringExclusionConfigList.Value = string.Join(",", gatheringExclusionList);
		}

		public void RemoveFromMiningIgnoreList(string name) {
			miningExclusionList.Remove(name);
			miningExclusionConfigList.Value = string.Join(",", miningExclusionList);
		}
	}
}

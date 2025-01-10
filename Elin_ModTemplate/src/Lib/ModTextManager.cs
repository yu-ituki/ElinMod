using UnityEngine;

namespace Elin_ModTemplate
{

	public class ModTextManager : Singleton<ModTextManager>
	{
		private ModText m_TextCore;

		public void Initialize()
		{
			// SourceDataはScriptableObjectなので本来はリソースとして保存しときたいのだが、.
			// とりあえず現状はランタイムで生成＆読み込みする.
			m_TextCore = ScriptableObject.CreateInstance<ModText>();
			m_TextCore.Setup();
			ModUtil.ImportExcel( CommonUtil.GetResourcePath( "tables/mod_texts.xlsm" ), "texts", (SourceData)(object)m_TextCore );
		}

		public string GetText( eTextID id )
		{
			return m_TextCore.GetText( id );
		}
	}
}

namespace MgsTools.Data
{
	public static class GameSavedData
	{
		public static int Skin
		{
			get => SavedData.GetInt("skin", 1);
			set => SavedData.SetInt("skin", value);
		}

		public static int Level
		{
			get => SavedData.GetInt("game_level", 1);
			set => SavedData.SetInt("game_level", value);
		}
		
		public static int LevelMax
		{
			get => SavedData.GetInt("game_level_max", 1);
			set => SavedData.SetInt("game_level_max", value);
		}

		public static int LevelCount
		{
			get => SavedData.GetInt("game_level_count", 0);
			set => SavedData.SetInt("game_level_count", value);
		}
		public static int LevelCurrent
		{
			get => SavedData.GetInt("game_level_current", 1);
			set => SavedData.SetInt("game_level_current", value);
		}

		public static bool IsShowTutorial
		{
			get => SavedData.GetBool("game_is_show_tutorial", false);
			set => SavedData.SetBool("game_is_show_tutorial", value);
		}
	}
}

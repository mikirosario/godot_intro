public static class Config
{
    public const string SavePath = "res://savegame.sav";

    public static void SaveGame(Player player = null)
    {
        using (Godot.FileAccess file = Godot.FileAccess.Open(SavePath, Godot.FileAccess.ModeFlags.Write))
        {
            Godot.Collections.Dictionary data = new Godot.Collections.Dictionary {
                { "Health", PlayerData.Health },
                { "Gold", PlayerData.Gold }
            };
            string jstr = Godot.Json.Stringify(data);
            file.StoreLine(jstr);
        }
    }

    public static void LoadGame()
    {
        if (Godot.FileAccess.FileExists(SavePath))
        {
            using (Godot.FileAccess file = Godot.FileAccess.Open(SavePath, Godot.FileAccess.ModeFlags.Read))
            {
                while (file.GetPosition() < file.GetLength())
                {
                    Godot.Collections.Dictionary line = Godot.Json.ParseString(file.GetLine()).AsGodotDictionary();
                    
                    Godot.Variant val;
                    if (line.TryGetValue("Health", out val))
                        PlayerData.Health = (uint)val;
                    if (line.TryGetValue("Gold", out val))
                        PlayerData.Gold = (uint)val;
                }
                Godot.GD.Print("Health: " + PlayerData.Health);
                Godot.GD.Print("Gold: " + PlayerData.Gold);
            }
        }
    }
}

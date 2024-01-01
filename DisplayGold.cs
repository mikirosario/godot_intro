using Godot;
using System;
using System.Text;

public partial class DisplayGold : Label
{
	private const string LabelText = "GP: ";
	private Player _playerCharacter;
	private GP _playerGP;
	private StringBuilder _playerGPStrBuilder;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_playerCharacter = (Player)GetNode(Player.PlayerNodeLocation);
		_playerGP = new GP(_playerCharacter);
		_playerGPStrBuilder = new StringBuilder($"{LabelText}{_playerGP}");
		Text = _playerGPStrBuilder.ToString();
		GD.Print("Initial Gold " + Text + " Reported Player Gold: " + _playerCharacter.Gold);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (_playerCharacter.Gold != _playerGP)
		{
			string oldHPValue = _playerGP;
			_playerGP.Update(_playerCharacter);
			_playerGPStrBuilder.Replace(oldHPValue, _playerGP.ToString());
			Text = _playerGPStrBuilder.ToString();
			GD.Print("Updated Text: " + Text);
		}
	}
}

public class GP
{
	private string _playerGPStr;
	private uint _playerGP;

	public string PlayerGPStr { get => _playerGPStr; private set => _playerGPStr = value; }
	public uint PlayerGP { get => _playerGP; private set => _playerGP = value; }
	
	public GP ()
	{
		PlayerGP = 0;
		PlayerGPStr = PlayerGP.ToString();
	}

	public GP (Player player)
	{
		Update(player);
	}

	public static implicit operator string(GP gp) => gp.ToString();
	public static implicit operator uint(GP gp) => gp.PlayerGP;

	public void Update (Player player)
	{
		PlayerGP = player.Gold;
		PlayerGPStr = PlayerGP.ToString();
	}

    public override string ToString()
    {
        return PlayerGPStr;
    }
}

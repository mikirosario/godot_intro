using Godot;
using System;
using System.Text;

public partial class DisplayHP : Label
{
	private const string LabelText = "HP: ";
	private Player _playerCharacter;
	private HP _playerHP;
	private StringBuilder _playerHPStrBuilder;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_playerCharacter = (Player)GetNode(Player.PlayerNodeLocation);
		_playerHP = new HP(_playerCharacter);
		_playerHPStrBuilder = new StringBuilder($"{LabelText}{_playerHP}");
		Text = _playerHPStrBuilder.ToString();
		GD.Print("Initial Health " + Text + " Reported Player Health: " + _playerCharacter.Health);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (_playerCharacter.Health != _playerHP)
		{
			string oldHPValue = _playerHP;
			_playerHP.Update(_playerCharacter);
			_playerHPStrBuilder.Replace(oldHPValue, _playerHP.ToString());
			Text = _playerHPStrBuilder.ToString();
			GD.Print("Updated Text: " + Text);
		}
	}
}

public class HP
{
	private string _playerHPStr;
	private uint _playerHP;

	public string PlayerHPStr { get => _playerHPStr; private set => _playerHPStr = value; }
	public uint PlayerHP { get => _playerHP; private set => _playerHP = value; }
	
	public HP ()
	{
		PlayerHP = 0;
		PlayerHPStr = PlayerHP.ToString();
	}

	public HP (Player player)
	{
		Update(player);
	}

	public static implicit operator string(HP hp) => hp.ToString();
	public static implicit operator uint(HP hp) => hp.PlayerHP;

	public void Update (Player player)
	{
		PlayerHP = player.Health;
		PlayerHPStr = PlayerHP.ToString();
	}

    public override string ToString()
    {
        return PlayerHPStr;
    }
}
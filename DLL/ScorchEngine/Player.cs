using System;
using System.Collections.Generic;
using ScorchEngine.Data;

namespace ScorchEngine
{
	
	public class Player
	{
	    public event Action<Player> Ready;

	    public readonly string Name;
	    public Tank Tank{ get { return tank; }}
	    public int Money{ get { return money; }}

	    protected int money;
	    protected List<StoreItem> items;

	    private static int Count = 0;
	    private Tank tank;
	    private bool active;

	    public Player(string name)
	    {
	        Name = name;
	    }

	    public static Player CreateMockPlayer()
	    {
	        Player p = new Player("Player"+Count);
	        Count++;
	        return p;
	    }

	    public void StartTurn()
	    {
	        active = true;
	    }
	}
}


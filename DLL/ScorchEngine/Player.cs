using System;
using System.Collections.Generic;
using ScorchEngine.Config;
using ScorchEngine.Items;
using ScorchEngine.Server;

namespace ScorchEngine
{

	public class Player
	{
	    public event Action<Player> Ready;
	    public event Action<Player> OnUpdate;

	    public readonly string Name;
	    public Tank ControlledTank{ get;private set;}
	    public int Money{ get;private set;}

	    protected int money;
	    protected List<StoreItem> items;

	    private static int Count = 0;
	    private Tank tank;
	    private bool active;
	    public int ID;

	    public Player(string name)
	    {
	        Name = name;
	    }

	    public void SetTank(Tank tank)
	    {
	        ControlledTank = tank;
	    }

	    public static Player CreateMockPlayer()
	    {
	        Player p = new Player("Player"+Count);
	        p.ID = Count;
	        p.ControlledTank = new Tank();
	        Count++;
	        return p;
	    }

	    public void TurnStarted()
	    {
	        active = true;
	    }

	    public void TurnEnded()
	    {
	        active = false;
	    }


	    /// <summary>
	    /// When getting state from server, update it and
	    /// </summary>
	    /// <param name="state"></param>
	    public void Process(PlayerState state)
	    {
	        ControlledTank.AngleHorizontal = state.AngleHorizontal;
	        ControlledTank.AngleVertical= state.AngleVertical;
	        ControlledTank.Force = state.Force;
		    ControlledTank.IsReady = state.IsReady;
	        ControlledTank.PositionX = state.PositionX;
            ControlledTank.PositionY = state.PositionY;
            ControlledTank.PositionZ = state.PositionZ;
            OnUpdate?.Invoke(this);
	    }
	}
}


using Godot;
using System;
using System.Collections.Generic;
public partial class Node2d : Node2D
{
	private TileMapLayer _tileMap;
	private MazeGenerator _maze;
	private Vector2I _playerPos = new Vector2I(21,21);
	private const int VisionRadius = 3;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_tileMap = GetNode<TileMapLayer>("TileMapLayer");
		_maze = new MazeGenerator(41,41);
		_maze.GenerateFull();
		Render();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		if (@event.IsActionPressed("ui_accept"))
		{
			var frozen=GetFrozenCells(_playerPos,VisionRadius);
			_maze.ReGenerateWithConstraint(frozen);
			Render();
		}
	}
	private HashSet<Vector2I> GetFrozenCells(Vector2I position, int radius)
	{
		var set = new HashSet<Vector2I>();
		for(int y=position.Y-radius;y<=position.Y+radius;y++){
			for(int x=position.X-radius;x<=position.X+radius;x++){
				if(x>=0 && x<_maze.Width && y>=0 && y<_maze.Height){
					set.Add(new Vector2I(x,y));
				}
			}
		}
		return set;
	}
	private void Render()
	{
		_tileMap.Clear();
		for(int y=0;y<_maze.Height;y++){
			for(int x=0;x<_maze.Width;x++){
				_tileMap.SetCell(new Vector2I(x,y),_maze.Grid[y,x],Vector2I.Zero);
			}
		}
	}
}

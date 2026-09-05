using Godot;
using System.Collections.Generic;

public class Entity : Sprite2D
{
    public Vector2I GridPos {get;private set;}
    public int VisionRadius {get;set;}=3;
    private const int TileSize=16;

    public void Init(Vector2I startPos,int visionradius)
    {
        GridPos = startPos;
        VisionRadius = visionradius;
        Centered = false;
        UpdateTransform();
    }

    public bool Move(Vector2I dir,int [,]grid,int w,int h)
    {
        Vector2I next=GridPos+dir;
        if(next.X>=0 && next.X<w && next.Y>=0 && next.Y<h && grid[next.X,next.Y]==0){
            GridPos=next;
            UpdateTransform();
            return true;
        }
        return false;
    }

    public HashSet<Vector2I> GetVisibleCells(int w,int h)
    {
        var set = new HashSet<Vector2I>();
        for(int y=GridPos.Y-VisionRadius;y<=GridPos.Y+VisionRadius;y++){
            for(int x=GridPos.X-VisionRadius;x<=GridPos.X+VisionRadius;x++){
                if(x>=0 && x<w && y>=0 && y<h){
                    set.Add(new Vector2I(x,y));
                }
            }
        }
        return set;
    }
    private void UpdateTransform()
    {
        Position = new Vector2(GridPos.X * TileSize, GridPos.Y * TileSize);
    }
}
using Godot;
using System;
using System.Collections.Generic;
public class MazeGenerator
{
    public int Width {get;}
    public int Height {get;}
    public int NodeW{get;}
    public int NodeH{get;}
    public int[,] Grid{get;}
    public struct Edge{
        public int u,v;
        public Vector2I WallPos;
        public Edge(int u, int v, Vector2I wallPos)
        {
            this.u=u;
            this.v=v;
            this.WallPos=wallPos;
        }
    }
    private readonly DisjointSet _dsu;
    private readonly List<Edge> _alledges= new();
    private readonly Random _rng= new();
    public MazeGenerator(int width,int height)
    {
        Width=width;
        Height=height;
        NodeW=width/2;
        NodeH=height/2;
        Grid=new int[Width,Height];
        _dsu=new DisjointSet(NodeW*NodeH);
        InitEdges();
    }
    private int GetId(int nx,int ny)=>ny*NodeW+nx;
    private void InitEdges()
    {
        for(int y=0;y<NodeH;y++){
            for(int x=0;x<NodeW;x++){
                int u=GetId(x,y);
                if(x+1<NodeW)_alledges.Add(new Edge(u,GetId(x+1,y),new Vector2I(2*x+2,2*y+1)));
                if(y+1<NodeH)_alledges.Add(new Edge(u,GetId(x,y+1),new Vector2I(2*x+1,2*y+2)));
            }
        }
    }
    public void GenerateFull()
    {
        _dsu.Reset();
        for(int y=0;y<Height;y++){
            for(int x=0;x<Width;x++){
                Grid[x,y]=1;
            }
        }
        for(int y=0;y<NodeH;y++){
            for(int x=0;x<NodeW;x++){
                Grid[2*x+1,2*y+1]=0;
            }
        }
        Shuffle(_alledges);
        foreach(var edge in _alledges){
            if(_dsu.Union(edge.u,edge.v)){
                Grid[edge.WallPos.X,edge.WallPos.Y]=0;
            }
        }
    }
    private void Shuffle<T>(List<T> list)
    {
        for(int i = list.Count - 1; i >= 0; i--)
        {
            int k=_rng.Next(i+1);
            (list[i],list[k])=(list[k],list[i]);
        }
    }
    public void ReGenerateWithConstraint(HashSet<Vector2I> frozenCells)
    {
        _dsu.Reset();
        foreach(var edge in _alledges){
            if(!frozenCells.Contains(edge.WallPos)){
                Grid[edge.WallPos.X,edge.WallPos.Y]=1;
            }
        }
        foreach(var edge in _alledges){
            if(Grid[edge.WallPos.X,edge.WallPos.Y]==0){
                _dsu.Union(edge.u,edge.v);
            }
        }
        var leftedges=new List<Edge>();
        foreach(var edge in _alledges){
            if(Grid[edge.WallPos.X,edge.WallPos.Y]==1){
                leftedges.Add(edge);
            }
        }
        Shuffle(leftedges);
        foreach(var edge in leftedges){
            if(_dsu.Union(edge.u,edge.v)){
                Grid[edge.WallPos.X,edge.WallPos.Y]=0;
            }
        }
    }
}
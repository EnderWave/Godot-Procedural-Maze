public class DisjointSet
{
    private readonly int[] _p;
    public DisjointSet(int size)
    {
        _p=new int[size];
        Reset();
    }
    public void Reset()
    {
        for(int i=0; i<_p.Length; i++)_p[i]=i;
    }
    public int Find(int x)=> _p[x]==x?x:_p[x]=Find(_p[x]);
    public bool Union(int x,int y)
    {
        int rx=Find(x),ry=Find(y);
        if(rx==ry)return false;
        _p[rx]=ry;
        return true;
    }
}
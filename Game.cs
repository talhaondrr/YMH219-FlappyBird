namespace Lab;
public sealed class Bird
{
 public float Y {get;set;}=220;
 public float Velocity {get;set;}
 public RectangleF Bounds => new(100,Y,32,26);
 public void Flap() => Velocity=-6.5f;
 public void Update() { Velocity+=0.36f; Y+=Velocity; }
}
public sealed class Pipe
{
 public float X {get;set;}
 public int GapY {get;set;}
 public bool Passed {get;set;}
 public RectangleF Top => new(X,0,65,GapY);
 public RectangleF Bottom => new(X,GapY+170,65,480-GapY-170);
}
public sealed class Game
{
 readonly Random random=new();
 public Bird Player {get;private set;}=new();
 public List<Pipe> Pipes {get;}=new();
 public int Score {get;private set;}
 public bool Over {get;private set;}
 public float Speed => 3+Math.Min(Score*0.15f,3);
 public Game() => Reset();
 public void Reset() { Player=new(); Score=0; Over=false; Pipes.Clear(); for(int i=0;i<3;i++) Pipes.Add(new Pipe { X=540+i*270,GapY=random.Next(80,270) }); }
 public void Tick()
 {
  if(Over) return;
  Player.Update();
  foreach(var p in Pipes) {
   p.X-=Speed;
   if(p.X+65<0) { p.X=Pipes.Max(x=>x.X)+270; p.GapY=random.Next(80,270); p.Passed=false; }
   if(!p.Passed && p.X+65<Player.Bounds.Left) { p.Passed=true; Score++; }
   if(Player.Bounds.IntersectsWith(p.Top)||Player.Bounds.IntersectsWith(p.Bottom)) Over=true;
  }
  if(Player.Y<0||Player.Y+26>=480) Over=true;
 }
}

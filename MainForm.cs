namespace Lab;
public sealed class MainForm:Form
{
 readonly Game game=new();
 readonly System.Windows.Forms.Timer timer=new(){Interval=16};
 readonly Canvas canvas=new(){Dock=DockStyle.Fill};
 readonly Label status=Ui.Label("Hazır • Boşluk: kanat çırp | R: yeniden başlat");
 public MainForm()
 {
  Ui.Setup(this,"Flappy Bird"); KeyPreview=true;
  var bar=Ui.Bar(); bar.Controls.Add(Ui.Button("Yeni oyun",(_,_)=>Start())); bar.Controls.Add(status);
  Controls.Add(canvas); Controls.Add(bar);
  canvas.Paint+=Draw;
  KeyDown+=(_,e)=> { if(e.KeyCode==Keys.Space) { if(!timer.Enabled&&!game.Over) timer.Start(); game.Player.Flap(); e.Handled=true; e.SuppressKeyPress=true; } if(e.KeyCode==Keys.R) Start(); };
  timer.Tick+=(_,_)=> { game.Tick(); status.Text=$"Skor: {game.Score} • Hız: {game.Speed:F1}"; if(game.Over) { timer.Stop(); status.Text+=" • GAME OVER — Yeni oyun / R"; } canvas.Invalidate(); };
 }
 void Start() { game.Reset(); timer.Start(); canvas.Focus(); }
 void Draw(object? sender,PaintEventArgs e)
 {
  var g=e.Graphics; g.ScaleTransform(canvas.Width/900f,canvas.Height/520f); g.Clear(Color.LightSkyBlue);
  using var cloud=new SolidBrush(Color.FromArgb(230,255,255,255)); g.FillEllipse(cloud,330,60,170,60); g.FillEllipse(cloud,650,115,145,50);
  foreach(var p in game.Pipes) { g.FillRectangle(Brushes.SeaGreen,p.Top); g.FillRectangle(Brushes.SeaGreen,p.Bottom); }
  g.FillRectangle(Brushes.SandyBrown,0,480,900,40); g.FillEllipse(Brushes.Gold,game.Player.Bounds); g.FillEllipse(Brushes.Black,121,game.Player.Y+5,5,5);
  using var font=new Font("Segoe UI",22,FontStyle.Bold); g.DrawString(game.Over?"GAME OVER":$"Skor: {game.Score}",font,Brushes.DarkSlateGray,350,20);
 }
 protected override void Dispose(bool disposing) { if(disposing) timer.Dispose(); base.Dispose(disposing); }
 sealed class Canvas:Panel { public Canvas() { DoubleBuffered=true; } }
}

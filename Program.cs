using OpenTK.Windowing.Desktop;

public class Program {
    public static void Main(string[] args)
    {
        var game = new Game(new GameWindowSettings(), new NativeWindowSettings()); 
        game.Run();
    }
}
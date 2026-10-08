using Spectre.Console;

public class Mau {
    public static Color maudo = Color.Red;
    public static Color mauxanhla = Color.Green;
    public static Color mauvang = Color.Yellow;
    public static Color maucam = Color.Orange1;
    public static Color mauxanhduong = Color.Aquamarine1;
    public static Color maucyan = Color.Cyan;

    public static void tomau(string noidung, Color mau, bool xuongdong = false){
        Console.ForegroundColor = mau;
        if(xuongdong) Console.WriteLine(noidung);
        else Console.Write(noidung);
        Console.ResetColor();
    }
}
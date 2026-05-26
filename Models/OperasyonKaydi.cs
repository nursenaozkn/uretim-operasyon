namespace UretimOperasyon.Models;

/// <summary>
/// Bir üretim operasyon bildirimi (Tablo 1 satırı veya Tablo 3 çıktı satırı).
/// </summary>
public class OperasyonKaydi
{
    public int KayitNo { get; set; }
    public DateTime Baslangic { get; set; }
    public DateTime Bitis { get; set; }
    public string Statu { get; set; } = "URETIM";
    public string? DurusNedeni { get; set; }

    public TimeSpan ToplamSure => Bitis - Baslangic;

    // Tablodaki "hh:mm:ss" gösterimi (24 saati aşan süreler de doğru gösterilir).
    public string ToplamSureMetin =>
        $"{(int)ToplamSure.TotalHours:D2}:{ToplamSure.Minutes:D2}:{ToplamSure.Seconds:D2}";
}

using UretimOperasyon.Models;

namespace UretimOperasyon.Data;

/// <summary>
/// Sorudaki Tablo 1 ve Tablo 2 verileri. (Not 3 gereği veritabanı kullanılmaz,
/// veriler bellekte List olarak tutulur.)
/// </summary>
public static class OrnekVeri
{
    private static readonly DateTime Gun = new(2020, 5, 23);

    private static DateTime At(int saat, int dakika) => Gun.AddHours(saat).AddMinutes(dakika);

    // Tablo 1) Mevcut kayıtlı veriler — Üretim Operasyon Bildirimleri
    public static List<OperasyonKaydi> Tablo1() => new()
    {
        new() { KayitNo = 1, Baslangic = At(7, 30),  Bitis = At(8, 30),  Statu = "URETIM" },
        new() { KayitNo = 2, Baslangic = At(8, 30),  Bitis = At(12, 0),  Statu = "URETIM" },
        new() { KayitNo = 3, Baslangic = At(12, 0),  Bitis = At(13, 0),  Statu = "URETIM" },
        new() { KayitNo = 4, Baslangic = At(13, 0),  Bitis = At(13, 45), Statu = "DURUŞ", DurusNedeni = "ARIZA" },
        new() { KayitNo = 5, Baslangic = At(13, 45), Bitis = At(17, 30), Statu = "URETIM" },
    };

    // Tablo 2) Mevcut kayıtlı standart duruş bilgileri — Standart Duruşlar
    public static List<StandartDurus> Tablo2() => new()
    {
        new() { Baslangic = new TimeSpan(10, 0, 0),  Bitis = new TimeSpan(10, 15, 0), DurusNedeni = "Çay Molası" },
        new() { Baslangic = new TimeSpan(12, 0, 0),  Bitis = new TimeSpan(12, 30, 0), DurusNedeni = "Yemek Molası" },
        new() { Baslangic = new TimeSpan(15, 0, 0),  Bitis = new TimeSpan(15, 15, 0), DurusNedeni = "Çay Molası" },
    };
}

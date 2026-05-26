using UretimOperasyon.Models;

namespace UretimOperasyon.Services;

/// <summary>
/// Tablo 1 (operasyon bildirimleri) ve Tablo 2 (standart duruşlar) verilerinden
/// Tablo 3 çıktısını hesaplar.
///
/// Mantık: Her ÜRETİM kaydının zaman aralığına denk gelen standart molalar,
/// operatör girmemiş olsa bile programca otomatik araya eklenir. Üretim aralığı
/// molanın öncesi (üretim) → mola (DURUŞ) → sonrası (üretim) şeklinde bölünür.
/// ÜRETİM dışı kayıtlar (örn. ARIZA duruşu) olduğu gibi korunur.
/// </summary>
public class OperasyonRaporServisi
{
    public List<OperasyonKaydi> Olustur(
        IReadOnlyList<OperasyonKaydi> bildirimler,
        IReadOnlyList<StandartDurus> standartDuruslar)
    {
        var sonuc = new List<OperasyonKaydi>();

        foreach (var kayit in bildirimler.OrderBy(b => b.Baslangic))
        {
            // Üretim dışı kayıtlar (ARIZA vb. duruşlar) bölünmeden korunur.
            if (!string.Equals(kayit.Statu, "URETIM", StringComparison.OrdinalIgnoreCase))
            {
                sonuc.Add(Kopyala(kayit));
                continue;
            }

            // Bu üretim aralığına denk gelen molaları, kaydın tarihine taşıyarak bul.
            var moralar = standartDuruslar
                .Select(d => new
                {
                    Baslangic = kayit.Baslangic.Date + d.Baslangic,
                    Bitis = kayit.Baslangic.Date + d.Bitis,
                    d.DurusNedeni
                })
                // Aralıkla gerçek kesişim (sınıra denk gelen mola sayılmaz).
                .Where(d => d.Baslangic < kayit.Bitis && d.Bitis > kayit.Baslangic)
                .OrderBy(d => d.Baslangic)
                .ToList();

            var imlec = kayit.Baslangic;

            foreach (var mola in moralar)
            {
                var molaBas = mola.Baslangic < kayit.Baslangic ? kayit.Baslangic : mola.Baslangic;
                var molaBit = mola.Bitis > kayit.Bitis ? kayit.Bitis : mola.Bitis;

                if (molaBas > imlec)
                    sonuc.Add(UretimSegmenti(kayit.KayitNo, imlec, molaBas));

                sonuc.Add(new OperasyonKaydi
                {
                    KayitNo = kayit.KayitNo,
                    Baslangic = molaBas,
                    Bitis = molaBit,
                    Statu = "DURUŞ",
                    DurusNedeni = mola.DurusNedeni
                });

                imlec = molaBit;
            }

            if (imlec < kayit.Bitis)
                sonuc.Add(UretimSegmenti(kayit.KayitNo, imlec, kayit.Bitis));
        }

        return sonuc;
    }

    private static OperasyonKaydi UretimSegmenti(int kayitNo, DateTime bas, DateTime bit) => new()
    {
        KayitNo = kayitNo,
        Baslangic = bas,
        Bitis = bit,
        Statu = "URETIM"
    };

    private static OperasyonKaydi Kopyala(OperasyonKaydi k) => new()
    {
        KayitNo = k.KayitNo,
        Baslangic = k.Baslangic,
        Bitis = k.Bitis,
        Statu = k.Statu,
        DurusNedeni = k.DurusNedeni
    };
}

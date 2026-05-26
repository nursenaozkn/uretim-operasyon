using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UretimOperasyon.Data;
using UretimOperasyon.Models;
using UretimOperasyon.Services;

namespace UretimOperasyon.Pages;

public class IndexModel : PageModel
{
    private readonly OperasyonRaporServisi _servis;

    public IndexModel(OperasyonRaporServisi servis) => _servis = servis;

    // Girdi tabloları (her zaman gösterilir).
    public List<OperasyonKaydi> Tablo1 { get; } = OrnekVeri.Tablo1();
    public List<StandartDurus> Tablo2 { get; } = OrnekVeri.Tablo2();

    // Çıktı tablosu — yalnızca butona basıldığında hesaplanır.
    public List<OperasyonKaydi>? Tablo3 { get; private set; }

    public void OnGet()
    {
    }

    // Butona basınca Tablo 1 + Tablo 2'den Tablo 3 hesaplanır.
    public void OnPostHesapla()
    {
        Tablo3 = _servis.Olustur(Tablo1, Tablo2);
    }
}

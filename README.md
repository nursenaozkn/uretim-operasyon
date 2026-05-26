# Üretim Operasyon Bildirimleri

ASP.NET Core (.NET 8) Razor Pages ile geliştirilmiş web uygulaması. Bir üretim
makinesindeki operasyon bildirimlerine (Tablo 1) standart molaları (Tablo 2)
**otomatik** yerleştirerek düzeltilmiş çıktıyı (Tablo 3) hesaplar.

## Problem

Sahadan iş bildirimi yapan operatörler çay/yemek molası gibi **standart duruşları**
programa girmeyi unutuyor veya gereksiz görüyor. Bu uygulama, standart molaları
operatöre bıraktırmadan, ilgili üretim aralığının içine program tarafından otomatik
ekler.

- **Tablo 1 — Operasyon Bildirimleri:** Operatörün girdiği üretim/duruş kayıtları.
- **Tablo 2 — Standart Duruşlar:** Fabrikanın sabit mola saatleri (çay, yemek).
- **Tablo 3 — Hesaplanan Çıktı:** Bir butona basınca Tablo 1 + Tablo 2'den hesaplanır.

> Veriler veritabanı kullanılmadan bellekte `List<T>` olarak tutulur.

## Çalışma Mantığı

`Services/OperasyonRaporServisi.cs` içindeki algoritma:

1. Kayıtlar başlangıç saatine göre sıralanır.
2. ÜRETİM dışı kayıtlar (örn. ARIZA duruşu) olduğu gibi korunur.
3. Her ÜRETİM kaydı için, o aralığa **gerçekten denk gelen** standart molalar
   bulunur (aralık sınırına denk gelen mola dahil edilmez).
4. Üretim aralığı şu sırayla bölünür:
   `üretim → DURUŞ (mola) → üretim`. Sıfır uzunluklu segmentler atlanır.
5. Bölünen tüm segmentler özgün **Kayıt No**'yu korur.

### Örnek

Kayıt 2 (`08:30–12:00 ÜRETİM`) ve Çay Molası (`10:00–10:15`) şu üç satıra dönüşür:

| Kayıt No | Başlangıç | Bitiş | Süre | Statü | Duruş Nedeni |
|---|---|---|---|---|---|
| 2 | 08:30 | 10:00 | 01:30 | URETIM | |
| 2 | 10:00 | 10:15 | 00:15 | DURUŞ | Çay Molası |
| 2 | 10:15 | 12:00 | 01:45 | URETIM | |

## Proje Yapısı

```
UretimOperasyon/
├── Models/
│   ├── OperasyonKaydi.cs      # Operasyon kaydı (Tablo 1 / Tablo 3 satırı)
│   └── StandartDurus.cs       # Standart mola tanımı (Tablo 2 satırı)
├── Data/
│   └── OrnekVeri.cs           # Tablo 1 ve Tablo 2 verileri (bellekte)
├── Services/
│   └── OperasyonRaporServisi.cs  # Tablo 3'ü hesaplayan çekirdek mantık
├── Pages/
│   ├── Index.cshtml           # Tablolar + "Tablo 3'ü Hesapla" butonu
│   └── Index.cshtml.cs        # Sayfa modeli (buton → hesaplama)
├── wwwroot/css/site.css
├── Program.cs
├── Dockerfile                 # Deploy için .NET 8 imajı
└── render.yaml                # Render.com blueprint
```

## Yerelde Çalıştırma

Gereksinim: [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)

```bash
dotnet run
```

Tarayıcıda `http://localhost:5080` adresini açıp **"Tablo 3'ü Hesapla"** butonuna basın.

## Docker ile Çalıştırma

```bash
docker build -t uretim-operasyon .
docker run -p 8080:8080 -e PORT=8080 uretim-operasyon
```

`http://localhost:8080` adresinden erişilir.

## Render.com'a Deploy

1. Bu klasörü bir GitHub reposunun köküne push edin (Dockerfile repo kökünde olmalı).
2. [render.com](https://render.com) → **New → Web Service** → repoyu seçin.
3. Render `Dockerfile`'ı algılar. Instance Type: **Free** → **Create Web Service**.
4. `PORT` ortam değişkenini Render otomatik sağlar (`Program.cs` bunu okur).

## Teknolojiler

- ASP.NET Core (.NET 8) — Razor Pages
- C#
- Docker

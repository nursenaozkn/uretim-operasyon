namespace UretimOperasyon.Models;

/// <summary>
/// Fabrikanın standart mola/duruş tanımı (Tablo 2 satırı).
/// Saatler güne bağımsız olduğundan TimeSpan (günün saati) olarak tutulur.
/// </summary>
public class StandartDurus
{
    public TimeSpan Baslangic { get; set; }
    public TimeSpan Bitis { get; set; }
    public string DurusNedeni { get; set; } = string.Empty;
}

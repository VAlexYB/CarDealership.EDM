namespace CarDealership.EDM.Core.Models
{
    public enum DocumentFormat
    {
        TXT = 0,
    }

    public static class FormatHelper
    {
        public static string GetFormat(DocumentFormat format)
        {
            return format switch
            {
                DocumentFormat.TXT => ".txt",
                _ => throw new ArgumentOutOfRangeException(nameof(format), "Неподдерживаемый формат строки")
            };
        }
    }
}

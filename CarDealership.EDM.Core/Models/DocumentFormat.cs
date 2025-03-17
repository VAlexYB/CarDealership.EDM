namespace CarDealership.EDM.Core.Models
{
    public enum DocumentFormat
    {
        TX
    }

    public static class FormatHelper
    {
        public static string GetFormat(DocumentFormat format)
        {
            return format switch
            {
                DocumentFormat.TX => ".txt",
                _ => throw new ArgumentOutOfRangeException(nameof(format), "Неподдерживаемый формат строки");
            };
        }
    }
}

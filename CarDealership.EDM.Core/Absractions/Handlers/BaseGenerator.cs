using CarDealership.EDM.Core.Models;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace CarDealership.EDM.Core.Abstractions.Handlers
{
    /// <summary>
    /// Базовый класс генератора документа
    /// </summary>
    public abstract class BaseGenerator : IGenerating
    {
        protected BaseGenerator(object documentInfoModel)
        {
            _keyValuePairs = documentInfoModel.GetType()
                .GetProperties()
                .ToDictionary(prop => $"{prop.Name}", prop => prop.GetValue(documentInfoModel)?.ToString() ?? string.Empty);
        }

        /// <summary>
        /// Название генерируемого документа
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Формат генерируемого документа
        /// </summary>
        public DocumentFormat DocumentFormat { get; set; }

        /// <summary>
        /// Папка в которую будет помещен сгенерированный документ
        /// </summary>
        public string Directory {  get; set; }

        /// <summary>
        /// Словарь содержащий значения, которые нужно заменить 
        /// в шаблоне документа
        /// </summary>
        protected Dictionary<string, string> _keyValuePairs;

        /// <summary>
        /// Буфер для хранения необработанной части шаблона 
        /// от предыдущей итерации генерации документа
        /// </summary>
        private byte[] _buffer = Array.Empty<byte>();

        /// <summary>
        /// Обрабатывает шаблон, заменяя ключевые слова
        /// вида {{Key}} на поля класса Key
        /// </summary>
        /// <param name="chank">Обрабатываемая часть шаблона</param>
        /// <returns>Сгенерированная часть документа</returns>
        public byte[] Generate(byte[] chank)
        {
            string text = Encoding.UTF8.GetString(chank);

            string result = Regex.Replace(text, @"\{\{(\w+)\}\}", match =>
            {
                string key = match.Groups[1].Value;
                return _keyValuePairs.TryGetValue($"{key}", out string? value) ? value : match.Value;
            });

            return Encoding.UTF8.GetBytes(result);
        }

        /// <summary>
        /// Обрабатывает шаблон, вызывая <see cref="Generate(byte[])"/>,
        /// сохраняет необработанную часть шаблона в буффер
        /// </summary>
        /// <param name="chank">Обрабатываемая часть шаблона</param>
        /// <returns>Сгенерированная часть документа</returns>
        public virtual byte[] PrepareDocument(byte[] chank)
        {
            byte[] combinedChank = _buffer.Length > 0 ? _buffer.Concat(chank).ToArray() : chank;

            // может содержать косяк с обрезанным ключевым словом
            byte[] preparedContent = Generate(combinedChank);

            int lastOpenBrace = Array.LastIndexOf(preparedContent, (byte)'{');

            if (lastOpenBrace != -1)
            {
                string chunkTail = Encoding.UTF8.GetString(preparedContent[lastOpenBrace..]);
                if (chunkTail.StartsWith("{{"))
                {
                    if (!chunkTail.Contains("}}"))
                    {
                        _buffer = Encoding.UTF8.GetBytes(chunkTail);
                        preparedContent = preparedContent[..lastOpenBrace];
                    }
                    else
                    {
                        _buffer = Array.Empty<byte>();
                    }
                }
            }
            else
            {
                _buffer = Array.Empty<byte>();
            }

            return preparedContent;
        }

        /// <summary>
        /// Завершает генерацию документа и очищает буфер
        /// </summary>
        /// <returns>Результат генерации от остатка в буфере</returns>
        public byte[] Finalize()
        {
            byte[] finalChunk = _buffer;
            _buffer = Array.Empty<byte>();
            return Generate(finalChunk);
        }
    }
}

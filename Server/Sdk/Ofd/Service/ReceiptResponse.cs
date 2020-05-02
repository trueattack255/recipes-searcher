namespace Sdk.Ofd.Service
{
    /// <summary>
    /// Класс, представляющий ответ, полученный в результате проверки существования чека
    /// </summary>
    public class ReceiptResponse : Response
    {
        /// <summary>
        /// Существует ли чек в базе ФНС?
        /// </summary>
        public bool ReceiptExists { get; internal set; }

        internal ReceiptResponse()
        { }
    }
}

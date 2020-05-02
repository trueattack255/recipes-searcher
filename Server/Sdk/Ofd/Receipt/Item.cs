using System.Runtime.Serialization;

namespace Sdk.Ofd.Receipt
{
    /// <summary>
    /// Класс, представляющий позицию в чеке. В разных чеках параметры заполнены по разному.
    /// </summary>
    [DataContract]
    public class Item
    {
        /// <summary>
        /// Сумма по позиции, в копейках
        /// </summary>
        [DataMember]
        public int Sum { get; internal set; }
        /// <summary>
        /// Количество
        /// </summary>
        [DataMember]
        public double Quantity { get; internal set; }
        /// <summary>
        /// Цена позиции, в копейках
        /// </summary>
        [DataMember]
        public int Price { get; internal set; }
        /// <summary>
        /// Наименование позиции
        /// </summary>
        [DataMember]
        public string Name { get; internal set; }
        /// <summary>
        /// Сумма НДС, оплаченная по ставке 10%, в копейках
        /// </summary>
        [DataMember(Name = "nds10", IsRequired = false)]
        public int Nds10Sum { get; internal set; }
        /// <summary>
        /// Сумма НДС, оплаченная по ставке 18%, в копейках
        /// </summary>
        [DataMember(Name = "nds18", IsRequired = false)]
        public int Nds18Sum { get; internal set; }
    }
}

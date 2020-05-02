using System;
using SQLite;

namespace Mobile.Models
{
    [Table("receipts")]
    public class Receipt
    {
        [PrimaryKey, AutoIncrement, Column("_id")]
        public int Id { get; set; }
        public string Hash { get; set; }
        public string IngredientList { get; set; }
        public string Status { get; set; }
        public DateTime DateCreate { get; set; } = DateTime.Now;
        public DateTime DateUpdate { get; set; } = DateTime.Now;
    }
}

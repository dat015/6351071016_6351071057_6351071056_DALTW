using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Reflection.PortableExecutable;

namespace TechShop.Models
{
    public class CauHinh
    {
        [Key]
        public int MaCauHinh { get; set; }  // Khóa chính của bảng CauHinh


        // Các thuộc tính liên kết tới các bảng cấu hình
        public int? CPUId { get; set; }
        public int? RAMId { get; set; }
        public int? CardDoHoaId { get; set; }
        public int? ODiaId { get; set; }
        public int? ManHinhId { get; set; }
        public int? PinId { get; set; }
        public int? TrongLuongId { get; set; }

 

        [ForeignKey("CPUId")]
        public CPU CPU { get; set; }  // Liên kết tới bảng CPU

        [ForeignKey("RAMId")]
        public RAM RAM { get; set; }  // Liên kết tới bảng RAM

        [ForeignKey("CardDoHoaId")]
        public CardDoHoa CardDoHoa { get; set; }  // Liên kết tới bảng CardDoHoa

        [ForeignKey("ODiaId")]
        public ODia ODia { get; set; }  // Liên kết tới bảng ODia

        [ForeignKey("ManHinhId")]
        public ManHinh ManHinh { get; set; }  // Liên kết tới bảng ManHinh

        [ForeignKey("PinId")]
        public Pin Pin { get; set; }  // Liên kết tới bảng Pin

        [ForeignKey("TrongLuongId")]
        public TrongLuong TrongLuong { get; set; }
    }
}

namespace ECommerceAPI.Data
{
    public abstract class BaseEntity
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }
        
        // BONUS: Soft Delete özelliği (Veritabanından silinmez, sadece gizlenir)
        public bool IsDeleted { get; set; } = false;
    }
}
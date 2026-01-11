# 🚀 ECommerceAPI - .NET 9 Backend Projesi

Bu proje, İzem tarafından geliştirilmiş modern bir E-Ticaret REST API uygulamasıdır. **Katmanlı Mimari (Layered Architecture)** prensiplerine uygun olarak tasarlanmıştır.

## 🌟 Projenin Özellikleri (Bonuslar Dahil)

* **🛠 Teknoloji:** .NET 9, Entity Framework Core, SQLite.
* **🏗 Mimari:** Controller -> Service -> Data katmanları.
* **📦 DTO Kullanımı:** Veriler güvenli bir şekilde taşınır.
* **🗑 Soft Delete (+Bonus):** Veriler silinmez, çöp kutusuna taşınır (IsDeleted).
* **🌱 Seed Data (+Bonus):** Proje açıldığında otomatik olarak örnek veriler yüklenir.
* **📄 Standart Cevap:** Tüm API cevapları `{ success: true, data: ... }` formatındadır.

## 🚀 Nasıl Çalıştırılır?

1.  Projeyi indirin.
2.  Terminali açıp şu komutu yazın:
    ```bash
    dotnet run
    ```
3.  Tarayıcıda şu adrese gidin:
    `http://localhost:xxxx/swagger`

## 🧪 Test Edebileceğiniz Özellikler

* **GET /products:** Otomatik eklenen "Laptop" ve "Tişört" ürünlerini görebilirsiniz.
* **DELETE /products/{id}:** Bir ürünü sildiğinizde veritabanından tamamen gitmediğini, sadece gizlendiğini görebilirsiniz.

---
**Geliştirici:** İzem
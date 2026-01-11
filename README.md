# 🚀 E-Commerce API (.NET 9 Backend Projesi)

Bu proje, **İzem** tarafından geliştirilmiş, **.NET 9** ve **Entity Framework Core** teknolojileriyle güçlendirilmiş modern bir E-Ticaret REST API uygulamasıdır. Yazılım dünyasının kabul gördüğü **Clean Architecture** ve **N-Layer Architecture (Katmanlı Mimari)** prensiplerine sadık kalınarak tasarlanmıştır.

Amaç; ölçeklenebilir, test edilebilir ve sürdürülebilir bir backend altyapısı sunmaktır.

---

## 🏗 Mimari Yapı (Architecture Diagram)

Proje, "Separation of Concerns" (Sorumlulukların Ayrılması) ilkesine göre katmanlara ayrılmıştır. Veri akışı aşağıdaki diyagramda gösterildiği gibidir:

```mermaid
graph LR
    Client[Client / Swagger] -->|HTTP Request| API(Presentation Layer - Controllers)
    API -->|DTOs| Service(Business Layer - Services)
    Service -->|Entities| Data(Data Access Layer - DbContext)
    Data -->|SQL Queries| DB[(SQLite Database)]
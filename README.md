Bu layihə bank sisteminin əməliyyatlarını (müştərilər, hesablar, köçürmələr, təhlükəsizlik və s.) idarə etmək üçün **.NET 8 Web API** və **Clean / Modulyar Arxitektura** əsasında hazırlanmış tam backend və idarəetmə sistemidir.
---
##  Texnologiyalar və Alətlər
- **Platforma**: .NET 8 (C#)
- **Verilənlər Bazası**: Microsoft SQL Server & Dapper ORM
- **Təhlükəsizlik & Autentifikasiya**: JWT (JSON Web Tokens), Role-Based Authorization (Admin, Employee, Customer)
- **Loglama Sistemi**: Serilog (File & Console Sinks)
- **Xəta İdarəetməsi**: Centralized `IExceptionHandler` & RFC 7807 `ProblemDetails`
- **Sənədləşmə**: Swagger / OpenAPI UI
- **Veb Server (Hosting)**: IIS (Internet Information Services)

- ##  Layihə Strukturu və Servislər
Layihə bir-birindən asılılığı minimuma endirilmiş müstəqil servislərdən ibarətdir:
- **`BankSystem.AuthApi`**: 
  - İstifadəçilərin qeydiyyatı, login və JWT token generasiyası.
  - Rol məhdudiyyətləri (`Admin`, `Employee`, `Customer`).
- **`BankSystem.CustomerApi`**: 
  - Fiziki və Hüquqi şəxs müştərilərinin qeydiyyatı, redaktəsi və FİN axtarışı.
- **`BankSystem.AccountApi`**: 
  - Müştəriyə bağlı bank hesablarının açılması (AZN, USD, EUR), balans nəzarəti və hesab statusları.
- **`BankSystem.TransactionApi`**: 
  - Mədaxil (Deposit), Məxaric (Withdraw), Hesablararası köçürmə (Transfer) və Çıxarış (Statement / Tarixçə).
- **`BankSystem.Common`**: 
  - Bütün servislər üçün ortaq `GlobalExceptionHandler`, fərdiləşdirilmiş Exception-lar (`NotFoundException`, `BusinessException`, `ValidationException`) və standart JSON xəta cavabları.
- **`BankSystem.WinForms`**:
  - Bank əməkdaşları və müştərilər üçün hazırlanmış müasir masaüstü idarəetmə interfeysi.
 
- ## Qlobal Xəta və Log İdarəetməsi
Bütün API-lərdə kodun daxilindəki lazımsız `try-catch` blokları ləğv edilərək, mərkəzləşdirilmiş **`IExceptionHandler`** tətbiq edilmişdir. Xətalar avtomatik olaraq **Serilog** vasitəsilə gündəlik fayllara yazılır və istifadəçiyə həssas server məlumatlarını sızdırmadan standart formatda qaytarılır.

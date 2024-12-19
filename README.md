# ChatRoom API - SignalR ile Gerçek Zamanlı Chat Uygulaması

ChatRoom projesi, kullanıcıların bağlantı kurmasına, mesaj göndermesine ve sohbet odalarına katılmasına olanak tanıyan .NET ve SignalR kullanılarak oluşturulmuş gerçek zamanlı bir sohbet uygulamasıdır. Uygulama, gerçek zamanlı iletişim için SignalR'ı, MSSQL Server kullanarak veri depolama için Entity Framework'ü ve kullanıcı kimlik doğrulaması için ASP.NET Core Identity'yi entegre eder. Bu API, uç noktaları güvence altına almak için JWT token ve kullanıcı bağlantılarını güvenli bir şekilde yönetmek için SignalR kimlik doğrulamasını kullanır.

Bu projenin temel özellikleri şunlardır:

  * SignalR ile gerçek zamanlı mesajlaşma
  * Kullanıcı kimlik doğrulaması ve token-based yetkilendirme
  * Sohbet odaları ve kullanıcı yönetimi
  * Mesaj okuma durumu izleme

## Kullanılan Teknolojiler

- **ASP.NET Core** Backend API.
- **SignalR:** Gerçek zamanlı mesajlaşma için.
- **ASP.NET Core Identity:** Kullanıcı kimlik doğrulaması ve yetkilendirme için.
- **Entity Framework Core (EF Core):** Veritabanı işlemleri için ORM.
- **MSSQL Server:** Veritabanı sistemi.
- **JWT (JSON Web Token):** API ve SignalR kimlik doğrulaması için.

## Özellikler

**1. SignalR Gerçek Zamanlı Sohbet:**
  - Kullanıcılar sohbet odalarında anında mesaj gönderebilir ve alabilir.
  - SignalR bağlantıları aracılığıyla gerçek zamanlı güncellemeler sağlanır.

**2. Kimlik Doğrulama:**,
  - Kullanıcılar sohbet odalarına erişmek için ASP.NET Core Identity ile kimlik doğrulaması yapmalıdır.
  - Kullanıcılar başarılı bir şekilde oturum açtıklarında API istekleri ve SignalR bağlantıları için gerekli olan bir JWT token alırlar.

**3. Sohbet Odaları:**
  - Sohbet odaları kullanıcılar tarafından oluşturulabilir, katılabilir ve yönetilebilir.
  - Direct ve Group olmak üzere iki çeşit oda türü vardır.

**4. Kullanıcı Yönetimi:**
  - Kullanıcı bağlantı durumları (çevrimiçi/çevrimdışı) gerçek zamanlı olarak izlenir.

**5. Mesaj Takibi:**
  - Sohbet odalarında gönderilen her mesaj, göndereni, içeriği ve zaman damgasıyla birlikte izlenir.
  - Gönderilen mesajların kullanıcılar tarafından okunma durumu kontrol edilebilir.

**6. SignalR için Token Tabanlı Kimlik Doğrulama:**
  - SignalR bağlantısı, yalnızca yetkili kullanıcıların bağlanıp iletişim kurabilmesini sağlayarak kimlik doğrulama için geçerli bir JWT token'ı gerektirir.


## Kurulum

1. **Depoyu Klonlayın:**
   ```bash
   git clone https://github.com/mevlutayilmaz/chat-room-api.git
   ```

3. **Veritabanını Kurun:**

   `appsettings.json` dosyasında veritabanı bağlantı dizesini (`ConnectionStrings.DefaultConnection`) güncelleyin.
   
   EF Core migration komutlarını kullanarak veritabanını oluşturun:
   ```bash
   dotnet ef database update
   ```

5. **Gerekli Paketleri Yükleyin:**
    ```bash
    dotnet restore
    ```

6. **Uygulamayı Çalıştırın:**
   ```bash
   dotnet run
   ```

## Kullanım

1.  **Kullanıcı Kaydı:**
    *   `POST /api/auth/register` endpointini kullanarak yeni bir kullanıcı kaydı oluşturun.
2.  **Kullanıcı Girişi:**
    *   `POST /api/auth/login` endpointini kullanarak kullanıcı girişi yapın ve bir JWT token elde edin.
3.  **API Erişim:**
    *   Elde ettiğiniz tokenı, `Authorization` header'ında `Bearer {token}` şeklinde kullanarak API'ye isteklerde bulunun.
4.  **SignalR Bağlantısı:**
    *   SignalR hub'ına bağlanırken `access_token` query parametresini kullanarak tokenınızı iletin.
        ```javascript
        const connection = new signalR.HubConnectionBuilder()
          .withUrl("/chatHub", { accessTokenFactory: () => this.loginToken })
          .build();
        ```
5.  **Chat:**
    *   Bağlantı kurulduktan sonra, SignalR hub metotlarını kullanarak mesaj gönderip alın ve diğer kullanıcılarla etkileşim kurun.


## Veritabanı Yapısı

Aşağıda, projenin veritabanı şemasını oluşturan tablolar ve ilişkileri bulunmaktadır:

-   **`AspNetUsers`**: Kullanıcı bilgileri.
    -   `Id (Guid)`: Kullanıcı ID'si.
    -   `NameSurname (string)`: Kullanıcının ad soyadı.
    -   `ConnectionId (string)`: SignalR bağlantı ID'si.
    -   `ImageUrl (string)`: Kullanıcı profil resmi URL'si.
    -   `IsOnline (bool)`: Kullanıcının çevrimiçi durumu.
    -   `LastSeenDate (DateTime)`: Kullanıcının en son görüldüğü zaman.
    - Diğer Identity kullanıcı alanları (UserName, Email, PasswordHash vb.)
-   **`AspNetRoles`**: Rol bilgileri.
    -   `Id (Guid)`: Rol ID'si.
    -   `Name (string)`: Rol adı.
-    **`AspNetUserRoles`**: Kullanıcı ve rol ilişkileri
    -   `UserId (Guid)`: Kullanıcı ID'si.
    -   `RoleId (Guid)`: Rol ID'si.
-   **`ChatRooms`**: Sohbet odası bilgileri.
    -   `Id (Guid)`: Sohbet odası ID'si.
    -   `Name (string)`: Sohbet odası adı.
    -   `ImageUrl (string)`: Sohbet odası resim URL'si.
    -   `UpdatedDate (DateTime)`: Sohbet odasının güncellenme zamanı.
    -   `ChatRoomType (int)`: Sohbet odası türü (Örneğin: Private, Public).
-   **`AppUserChatRoom`**: Kullanıcılar ve sohbet odaları arasındaki çok-çok ilişkiyi tutar.
     - `ChatRoomsId (Guid)`: Sohbet odası ID'si.
    - `ParticipantsId (Guid)`: Kullanıcı ID'si.
-   **`Messages`**: Mesaj bilgileri.
    -   `Id (Guid)`: Mesaj ID'si.
    -   `ChatRoomId (Guid)`: Mesajın gönderildiği sohbet odası ID'si.
    -   `SenderId (Guid)`: Mesajı gönderen kullanıcı ID'si.
    -   `Content (string)`: Mesaj içeriği.
    -   `SentAt (DateTime)`: Mesajın gönderilme zamanı.
-   **`MessageReadStatuses`**: Mesajların okunma durumlarını tutar.
    -   `MessageId (Guid)`: Okunan mesaj ID'si.
    -   `UserId (Guid)`: Mesajı okuyan kullanıcı ID'si.
    -   `IsRead (bool)`: Mesajın okundu durumu.
 
![Screenshot 2024-12-19 154519](https://github.com/user-attachments/assets/c1bb6855-f159-4e85-b533-e823159f7229)


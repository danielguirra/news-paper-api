# News-Paper-Api

API REST de um Jornal Online em ASP.NET Core .

## 🔐 Autenticação

Após o login, um token JWT é retornado. Esse token deve ser enviado no cabeçalho `Authorization` para acessar rotas protegidas.

---

## 📦 Endpoints

### ✅ Criar Usuário

**POST** `/api/user`

**Body:**

```json
{
   "name": "João",
   "email": "joao@email.com",
   "password": "senha123",
   "role": "admin"
}
```

**Resposta:**

-  `201 Created` com o `Id` do usuário
-  `409 Conflict` se nome ou e-mail já estiverem em uso

---

### 🔐 Login

**POST** `/api/user/login`

**Body:**

```json
{
   "email": "joao@email.com",
   "password": "senha123"
}
```

**Resposta:**

-  `201 Created` com o `token`
-  `401 Unauthorized` se email ou senha estiverem incorretos

---

### 👤 Me (Usuário atual)

**GET** `/api/user/me`

**Headers:**

```
Authorization: Bearer JWT_TOKEN
```

**Resposta:**

```json
{
   "name": "João",
   "role": "admin",
   "id": "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx"
}
```

-  `401 Unauthorized` se token inválido ou ausente

---

### ✏️ Editar Usuário

**PUT** `/api/user`

**Body:**

```json
{
   "name": "Novo Nome",
   "email": "novo@email.com",
   "password": "novasenha",
   "role": "admin"
}
```

**Headers:**

```
Authorization: Bearer JWT_TOKEN
```

**Resposta:**

-  `201 Created` se editado com sucesso
-  `400 Bad Request` se algo falhar
-  `401 Unauthorized` se token for inválido

---

## 🔧 Estrutura

-  `Models/`
   -  `User/`
      -  `UserModel.cs`
      -  `UserService.cs`
      -  `UserController.cs`
      -  `UserModule.cs`
   -  `LoginModel.cs`
   -  `Auth/`
      -  `AuthModel.cs`
      -  `AuthSettings.cs`
      -  `JwtStrategy.cs`
   -  `News/`
      -  `NewsModel.cs`
      -  `NewsModule.cs`
      -  `NewsService.cs`
      -  `NewsController.cs`

---

## 🛡️ Segurança

-  JWT com validade de 30 minutos
-  `Bearer` token obrigatório nas rotas protegidas (`/me`, `/edit`)
-  Claims extraídas diretamente do token

---

## 🏃 Executar o projeto

```bash
dotnet build
dotnet run
```

---

## 📚 Exemplo de header

```http
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

---

## 🧪 Próximos passos

-  Controller da Noticias
-  Service da Noticias
-  Modulo da Noticias

---

## 👨‍💻 Autor

Daniel

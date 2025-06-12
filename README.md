News-Paper-Api
==============

API REST de um Jornal Online em ASP.NET Core.

* * *

⚙️ Configuração do Projeto
--------------------------

Para que o projeto funcione corretamente, crie um arquivo chamado **appsettings.json** na raiz do projeto com o seguinte conteúdo:

    {
      "AllowedHosts": "*",
      "ConnectionStrings": {
        "DefaultConnection": "Host=databaseUri;Port=5432;Database=DabaseName;Username=postgres;Password=suasenhaaqui"
      },
      "Logging": {
        "LogLevel": {
          "Default": "Information",
          "Microsoft.AspNetCore": "Warning"
        }
      }
    }

* * *

📦 Endpoints
------------

Esta API permite o gerenciamento de notícias, categorias, usuários e comentários.

### Usuários

* * *

### ✅ Criar Usuário

**POST** `/api/user`

Cria um novo usuário no sistema.

**Body:**

    {
      "name": "João",
      "email": "joao@email.com",
      "password": "senha123",
      "role": "admin"
    }

**Parâmetros:**

*   `name` (string, **obrigatório**): Nome do usuário (mínimo 1, máximo 255 caracteres).
*   `email` (string, **obrigatório**): E-mail do usuário (mínimo 1, máximo 100 caracteres, formato de e-mail).
*   `password` (string, **obrigatório**): Senha do usuário (mínimo 8 caracteres).
*   `role` (string, **obrigatório**): Papel do usuário (mínimo 4, máximo 6 caracteres).

**Resposta:**

*   `200 OK`: Usuário criado com sucesso.
*   `409 Conflict`: Se nome ou e-mail já estiverem em uso.

* * *

### 🔐 Login

**POST** `/api/user/login`

Autentica um usuário e retorna um token JWT.

**Body:**

    {
      "email": "joao@email.com",
      "password": "senha123"
    }

**Parâmetros:**

*   `email` (string, **obrigatório**): E-mail do usuário.
*   `password` (string, **obrigatório**): Senha do usuário.

**Resposta:**

*   `201 Created`: Login bem-sucedido, retorna o token JWT.
    
        {
          "token": "seu_token_jwt_aqui"
        }
    
*   `401 Unauthorized`: Se e-mail ou senha estiverem incorretos.

* * *

### 👤 Obter Usuário Atual

**GET** `/api/user/me`

Retorna informações do usuário autenticado.

**Headers:**

    Authorization: Bearer JWT_TOKEN

**Resposta:**

    {
      "name": "João",
      "role": "admin",
      "id": "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx"
    }

*   `200 OK`: Informações do usuário autenticado.
*   `401 Unauthorized`: Se o token for inválido ou ausente.

* * *

### ✏️ Editar Usuário

**PUT** `/api/user`

Atualiza as informações de um usuário existente.

**Body:**

    {
      "id": "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx",
      "name": "Novo Nome",
      "email": "novo@email.com",
      "password": "senhaAntiga",
      "newPassword": "novaSenhaSegura",
      "role": "editor"
    }

**Headers:**

    Authorization: Bearer JWT_TOKEN

**Parâmetros:**

*   `id` (string, **obrigatório**): ID do usuário a ser editado (formato UUID).
*   `name` (string, opcional): Novo nome do usuário.
*   `role` (string, opcional): Novo papel do usuário.
*   `email` (string, opcional): Novo e-mail do usuário.
*   `password` (string, opcional): Senha atual do usuário.
*   `newPassword` (string, opcional): Nova senha do usuário.

**Resposta:**

*   `200 OK`: Usuário editado com sucesso.
*   `400 Bad Request`: Se algo falhar na requisição.
*   `401 Unauthorized`: Se o token for inválido.

* * *

### ❌ Inativar Usuário

**DELETE** `/api/user/{id}/inactive`

Inativa um usuário pelo seu ID.

**Parâmetros de Rota:**

*   `id` (string, **obrigatório**): ID do usuário a ser inativado (formato UUID).

**Resposta:**

*   `200 OK`: Usuário inativado com sucesso.

### Categorias

* * *

### ✅ Criar Categoria

**POST** `/api/category`

Cria uma nova categoria.

**Body:**

    {
      "name": "Esportes"
    }

**Parâmetros:**

*   `name` (string, **obrigatório**): Nome da categoria (mínimo 1, máximo 30 caracteres).

**Resposta:**

*   `200 OK`: Categoria criada com sucesso.

* * *

### 📚 Listar Categorias

**GET** `/api/category`

Lista todas as categorias.

**Resposta:**

*   `200 OK`: Retorna uma lista de categorias.

* * *

### 📰 Listar Notícias por Categoria

**GET** `/api/category/{id}/news`

Retorna todas as notícias associadas a uma categoria específica.

**Parâmetros de Rota:**

*   `id` (string, **obrigatório**): ID da categoria (formato UUID).

**Resposta:**

*   `200 OK`: Retorna uma lista de notícias da categoria.

* * *

### 🔎 Obter Categoria por ID

**GET** `/api/category/{id}`

Retorna uma categoria específica pelo seu ID.

**Parâmetros de Rota:**

*   `id` (string, **obrigatório**): ID da categoria (formato UUID).

**Resposta:**

*   `200 OK`: Retorna a categoria solicitada.

* * *

### ❌ Inativar Categoria

**DELETE** `/api/category/{id}/inactive`

Inativa uma categoria pelo seu ID.

**Parâmetros de Rota:**

*   `id` (string, **obrigatório**): ID da categoria a ser inativada (formato UUID).

**Resposta:**

*   `200 OK`: Categoria inativada com sucesso.

### Notícias

* * *

### ✅ Criar Notícia

**POST** `/api/news`

Cria uma nova notícia.

**Body:**

    {
      "title": "Novo Título da Notícia",
      "description": "Uma breve descrição da notícia.",
      "thumbnail": "url_da_imagem.jpg",
      "content": "Conteúdo completo da notícia aqui...",
      "categoryId": "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx"
    }

**Parâmetros:**

*   `title` (string, **obrigatório**): Título da notícia (mínimo 1, máximo 100 caracteres).
*   `description` (string, **obrigatório**): Descrição da notícia (mínimo 1, máximo 255 caracteres).
*   `thumbnail` (string, **obrigatório**): URL da imagem em miniatura (mínimo 1, máximo 255 caracteres).
*   `content` (string, **obrigatório**): Conteúdo da notícia (mínimo 1, máximo 2000 caracteres).
*   `categoryId` (string, **obrigatório**): ID da categoria à qual a notícia pertence (formato UUID).

**Resposta:**

*   `200 OK`: Notícia criada com sucesso.

* * *

### ✏️ Editar Notícia

**PUT** `/api/news`

Atualiza as informações de uma notícia existente.

**Body:**

    {
      "id": "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx",
      "title": "Título Atualizado",
      "description": "Descrição atualizada da notícia.",
      "content": "Conteúdo revisado da notícia.",
      "thumbnail": "nova_url_imagem.jpg",
      "active": true,
      "categoryId": "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx"
    }

**Parâmetros:**

*   `id` (string, **obrigatório**): ID da notícia a ser editada (formato UUID).
*   `title` (string, opcional): Novo título da notícia.
*   `description` (string, opcional): Nova descrição da notícia.
*   `content` (string, opcional): Novo conteúdo da notícia.
*   `thumbnail` (string, opcional): Nova URL da imagem em miniatura.
*   `active` (boolean, opcional): Status de atividade da notícia.
*   `categoryId` (string, opcional): Novo ID da categoria.

**Resposta:**

*   `200 OK`: Notícia editada com sucesso.

* * *

### ❌ Inativar Notícia

**DELETE** `/api/news/{id}/inactive`

Inativa uma notícia pelo seu ID.

**Parâmetros de Rota:**

*   `id` (string, **obrigatório**): ID da notícia a ser inativada (formato UUID).

**Resposta:**

*   `200 OK`: Notícia inativada com sucesso.

* * *

### ⏰ Listar Notícias Recentes

**GET** `/api/news/recents`

Retorna uma lista de notícias recentes.

**Parâmetros de Consulta:**

*   `take` (integer, opcional): Número de notícias a serem retornadas (padrão: 10).
*   `skip` (integer, opcional): Número de notícias a serem puladas (padrão: 0).

**Resposta:**

        
        {
        "title": "Novas medidas são anunciadas para o setor educacional",
        "description": "Governo apresenta pacote de investimentos para escolas públicas",
        "thumbnail": "https://intrinseca.com.br/wp-content/uploads/2016/08/educa%C3%A7%C3%A3o_2-768x488.jpg",
        "id": "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx",
        "active": true,
        "createdAt": "2025-06-02T15:05:09.49138Z"
        },
        ...
    

* * *

### 🔎 Obter Notícia por ID

**GET** `/api/news/{id}`

Retorna uma notícia específica pelo seu ID.

**Parâmetros de Rota:**

*   `id` (string, **obrigatório**): ID da notícia (formato UUID).

**Resposta:**

    {
    "title": "Novas medidas são anunciadas para o setor educacional",
    "description": "Governo apresenta pacote de investimentos para escolas públicas",
    "content": "O Ministério da Educação divulgou nesta segunda-feira um novo pacote de medidas que visa fortalecer a infraestrutura das escolas públicas em todo o país. Entre as ações previstas estão a construção de novas unidades, contratação de professores e modernização de equipamentos tecnológicos.",
    "thumbnail": "https://intrinseca.com.br/wp-content/uploads/2016/08/educa%C3%A7%C3%A3o_2-768x488.jpg",
    "category": {
        "id": "55555555-5555-5555-5555-555555555555",
        "name": "Economia"
    },
    "author": {
        "name": "Danies",
        "role": "user",
        "id": "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx"
    },
    "id": "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx",
    "active": true,
    "createdAt": "2025-06-02T15:05:09.49138Z",
    "updatedAt": "2025-06-02T15:05:09.49138Z"
    }

* * *

### 🔎 Obter Notícia por Título

**GET** `/api/news/title/{title}`

**Parâmetros de Rota:**

*   `title` (string, **obrigatório**): Título da notícia.

Retorna uma lista que correspondem a um título.

**Resposta*** **Baseado no title** *medidas*


    {
        "title": "Novas medidas são anunciadas para o setor educacional",
        "description": "Governo apresenta pacote de investimentos para escolas públicas",
        "thumbnail": "https://intrinseca.com.br/wp-content/uploads/2016/08/educa%C3%A7%C3%A3o_2-768x488.jpg",
        "id": "3b5ccb2b-150b-4a85-a167-9eda414db3c1",
        "active": true,
        "createdAt": "2025-06-02T15:05:09.49138Z"
    }
    

### Comentários

* * *

### 💬 Listar Comentários de uma Notícia

**GET** `/api/news/{newsId}/comments`

Retorna os comentários de uma notícia específica.

**Parâmetros de Rota:**

*   `newsId` (string, **obrigatório**): ID da notícia (formato UUID).

**Parâmetros de Consulta:**

*   `skip` (integer, opcional): Número de comentários a serem pulados (padrão: 0).
*   `take` (integer, opcional): Número de comentários a serem retornados (padrão: 10).

**Resposta:**

     {
        "id": "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx",
        "content": "Faiz U L MESMO UNGA BUNGA Lulistas",
        "authorName": "Danies",
        "replies": [
            {
                "id": "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx",
                "content": "Faiz U L MESMO UNGA BUNGA Resposta",
                "authorName": "Daniek",
                "replies": null,
                "createdAt": "2025-06-10T00:47:28.0967Z",
                "updatedAt": "2025-06-10T00:47:28.0967Z",
                "likes": 0,
                "disLikes": 0
            }
        ],
        "createdAt": "2025-06-03T21:49:47.430172Z",
        "updatedAt": "2025-06-03T21:55:20.880794Z",
        "likes": 1,
        "disLikes": 0
    },

* * *

### ✍️ Criar Comentário em uma Notícia

**POST** `/api/news/{newsId}/comments`

Cria um novo comentário para uma notícia.

**Parâmetros de Rota:**

*   `newsId` (string, **obrigatório**): ID da notícia (formato UUID).

**Body:**

    {
      "content": "Este é um comentário sobre a notícia."
    }

**Parâmetros:**

*   `content` (string, **obrigatório**): Conteúdo do comentário.

**Resposta:**

*   `200 OK`: Comentário criado com sucesso.

* * *

### ↩️ Responder a um Comentário

**POST** `/api/news/{newsId}/comments/{id}/replies`

Cria uma resposta a um comentário existente em uma notícia.

**Parâmetros de Rota:**

*   `newsId` (string, **obrigatório**): ID da notícia (formato UUID).
*   `id` (string, **obrigatório**): ID do comentário ao qual responder (formato UUID).

**Body:**

    {
      "content": "Esta é uma resposta ao comentário."
    }

**Parâmetros:**

*   `content` (string, **obrigatório**): Conteúdo da resposta.

**Resposta:**

*   `200 OK`: Resposta criada com sucesso.

* * *

### 🗑️ Inativar Comentário

**DELETE** `/api/news/{newsId}/comments/{id}/inactive`

Inativa um comentário específico de uma notícia.

**Parâmetros de Rota:**

*   `id` (string, **obrigatório**): ID do comentário a ser inativado (formato UUID).
*   `newsId` (string, **obrigatório**): ID da notícia à qual o comentário pertence.

**Resposta:**

*   `200 OK`: Comentário inativado com sucesso.

* * *

### 👍 Curtir Comentário

**POST** `/api/news/{newsId}/comments/{id}/like`

Adiciona um "curtir" a um comentário.

**Parâmetros de Rota:**

*   `id` (string, **obrigatório**): ID do comentário a ser curtido (formato UUID).
*   `newsId` (string, **obrigatório**): ID da notícia à qual o comentário pertence.

**Resposta:**

*   `200 OK`: Curtida registrada com sucesso.

* * *

### 👎 Descurtir Comentário

**POST** `/api/news/{newsId}/comments/{id}/dislike`

Adiciona um "não curtir" a um comentário.

**Parâmetros de Rota:**

*   `id` (string, **obrigatório**): ID do comentário a ser descurtido (formato UUID).
*   `newsId` (string, **obrigatório**): ID da notícia à qual o comentário pertence.

**Resposta:**

*   `200 OK`: Descurtida registrada com sucesso.

* * *

🔧 Estrutura do Projeto
-----------------------

    .
    ├── Modules/
    │   ├── User/
    │   │   ├── UserDto.cs
    │   │   ├── UserModel.cs
    │   │   ├── UserService.cs
    │   │   ├── UserController.cs
    │   │   └── UserModule.cs
    │   ├── Auth/
    │   │   ├── AuthModel.cs
    │   │   ├── AuthSettings.cs
    │   │   └── JwtStrategy.cs
    │   ├── News/
    │   │   ├── NewsDto.cs
    |   |   ├── NewsModel.cs
    │   │   ├── NewsModule.cs
    │   │   ├── NewsService.cs
    │   │   └── NewsController.cs
    │   ├── Category/
    |   |   ├── CategoryDto.cs 
    |   |   ├── CategoryModel.cs
    │   │   ├── CategoryModule.cs
    │   │   ├── CategoryService.cs
    │   │   └── CategoryController.cs
    │   └── Comments/
    │       ├── CommentsDto.cs
    |       ├── CommentsModel.cs
    │       ├── CommentsModule.cs
    │       ├── CommentsService.cs
    │       └── CommentsController.cs
    ├── appsettings.json
    └── ... (outros arquivos do projeto)

* * *

🛡️ Segurança
-------------

*   **JWT (JSON Web Tokens)**: A autenticação é realizada via tokens JWT.
*   **Validade do Token**: Os tokens têm uma validade de 30 minutos.
*   **Rotas Protegidas**: Endpoints que exigem autenticação (ex: `/api/user/me`, `/api/user`) necessitam de um `Bearer Token` no cabeçalho `Authorization`.
*   **Claims**: As informações do usuário (claims) são extraídas diretamente do token JWT.

* * *

🏃 Como Executar o Projeto
--------------------------

Para compilar e executar o projeto, utilize os seguintes comandos no terminal na raiz do projeto:

    dotnet build
    dotnet run

* * *

📚 Exemplo de Header de Autorização
-----------------------------------

Ao realizar requisições para endpoints protegidos, inclua o cabeçalho `Authorization` com o token JWT no formato `Bearer {token}`:

    Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...

* * *

👨‍💻 Autor
-----------

Daniel Guirra

*   **Email**: daniel.guirra777@gmail.com
*   **Licença**: [Licença MIT](https://opensource.org/license/mit)

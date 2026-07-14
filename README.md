# FCG Users API 👤

Este repositório contém o **Microsserviço de Usuários**, a porta de entrada e o gerenciador de identidades da plataforma FIAP Cloud Games (FCG).

A principal responsabilidade deste serviço é gerenciar o ciclo de vida dos usuários (cadastro e manutenção de perfil), garantir a segurança da plataforma através da autenticação via tokens JWT e notificar o restante do ecossistema sobre o ingresso de novos clientes.

## 🎯 Responsabilidades e Fluxo de Eventos

O Users API atua como o ponto de origem das identidades da plataforma e opera como um **Publicador** no barramento de mensageria (RabbitMQ):

1. **Gestão de Identidade e Segurança:**
   * Responsável por realizar o cadastro seguro de novos usuários (hashing de senhas).
   * Responsável pela Autenticação e Autorização, gerando o Token JWT utilizado para validar o acesso do usuário aos demais microsserviços da plataforma.

2. **Integração Assíncrona (Publisher):**
   * **Gatilho:** Recebe e processa com sucesso o cadastro de um novo usuário na base de dados.
   * **Ação:** Publica o evento `UserCreatedEvent` (contendo os dados básicos do usuário recém-criado) na mensageria. 
   * **Impacto:** Este evento é consumido pelo microsserviço de Notificações (`NotificationsAPI`) para disparar o e-mail de boas-vindas.

## 🛠️ Tecnologias Utilizadas

* **Framework:** .NET 8 (ASP.NET Core Web API)
* **Banco de Dados:** PostgreSQL (Padrão *Database per Service*)
* **ORM:** Entity Framework Core
* **Segurança:** Autenticação JWT (JSON Web Tokens)
* **Mensageria:** RabbitMQ
* **Integração de Mensageria:** MassTransit
* **Containerização:** Docker (Multi-stage build)
* **Orquestração:** Kubernetes (K8s)

## 📁 Estrutura do Projeto

* `/src`: Código-fonte da aplicação C# (.NET 8).
* `/Dockerfile`: Arquivo de configuração para a criação da imagem Docker otimizada para produção.
* `/k8s`: Diretório contendo os manifestos do Kubernetes:
  * `deployment.yaml` e `service.yaml`: Manifestos de execução e exposição da API.
  * `database.yaml`: Provisionamento do banco de dados PostgreSQL exclusivo de Usuários.
  * `secret.yaml`: Gerenciamento de credenciais, string de conexão e chaves secretas do JWT (Base64).

## ⚙️ Variáveis de Ambiente e Configurações

O microsserviço requer configurações críticas injetadas no cluster Kubernetes através de `ConfigMaps` e `Secrets`:

| Variável / Configuração | Descrição | Exemplo |
| :--- | :--- | :--- |
| `ConnectionStrings:DefaultConnection` | String de conexão com o banco de dados | `Host=users-db;Database=users_db;Username=admin;Password=...` |
| `Jwt:Key` | Chave secreta para assinatura do Token JWT | `sua_chave_super_secreta_aqui` |
| `RabbitMQ:Host` | Endereço do broker de mensageria | `rabbitmq` (Nome do Service no K8s) |
| `RabbitMQ:Username` | Usuário de autenticação da fila | `guest` |
| `RabbitMQ:Password` | Senha de autenticação da fila | `guest` |

## 🚀 Como Executar

A orquestração do ecossistema completo é gerenciada no repositório **[FCG-Orchestration](https://github.com/Gabriel-Bacelar-Valentim/FCG-Orchestration)**.

### Via Docker Compose (Ambiente de Desenvolvimento)
Para subir todas as APIs e bancos simultaneamente, clone o repositório de Orquestração e execute:
```bash
docker-compose up --build

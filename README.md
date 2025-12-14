# Employee Management API - COLPIX Code Challenge

## Descripción del Proyecto

API RESTful para la gestión de empleados con autenticación JWT, desarrollada como parte del desafío técnico de COLPIX. El proyecto implementa un sistema completo de gestión de empleados con autenticación segura, validación de datos, y operaciones CRUD.

## Características Implementadas

### 1. Sistema de Autenticación
- ✅ Login con usuario y contraseña
- ✅ Generación de tokens JWT con expiración automática
- ✅ Tiempo de expiración configurable (parametrizable en `appsettings.json`)
- ✅ Validación de tokens en todos los endpoints protegidos
- ✅ Mensajes de error apropiados para tokens inválidos o expirados

### 2. Gestión de Empleados
- ✅ Crear nuevos empleados con validación de datos
- ✅ Actualizar empleados existentes
- ✅ Listar todos los empleados con fecha de última actualización
- ✅ Obtener detalles de un empleado por ID

### 3. Jerarquía y Subordinados
- ✅ Relación supervisor-subordinado
- ✅ Cálculo de subordinados directos e indirectos
- ✅ Procesamiento paralelo para obtener detalles de empleado y conteo de subordinados

### 4. Seguridad
- ✅ Todos los endpoints (excepto login) requieren autenticación JWT
- ✅ Hashing seguro de contraseñas con BCrypt
- ✅ Validación de entrada con FluentValidation
- ✅ Manejo centralizado de excepciones

## Tecnologías Utilizadas

### Backend
- **ASP.NET Core 8.0** - Framework web moderno y de alto rendimiento
- **Entity Framework Core** - ORM para acceso a datos
- **In-Memory Database** - Base de datos en memoria para facilitar pruebas
- **JWT Bearer Authentication** - Autenticación basada en tokens
- **BCrypt.Net** - Hashing seguro de contraseñas

### Librerías y Herramientas
- **AutoMapper** - Mapeo objeto-objeto
- **FluentValidation** - Validación de modelos
- **Swashbuckle (Swagger)** - Documentación interactiva de API
- **Microsoft.IdentityModel.Tokens** - Manejo de tokens JWT

## Estructura del Proyecto

```
EmployeeManagement/
│
├── EmployeeManagement.Api/              # Capa de presentación
│   ├── Controllers/                     # Controladores REST
│   │   ├── AuthController.cs           # Endpoint de autenticación
│   │   └── EmployeesController.cs      # Endpoints de empleados
│   ├── Middleware/                      # Middleware personalizado
│   │   └── ExceptionHandlingMiddleware.cs
│   └── Program.cs                       # Configuración de la aplicación
│
├── EmployeeManagement.Application/      # Capa de aplicación
│   ├── DTOs/                           # Data Transfer Objects
│   ├── Interfaces/                     # Interfaces de servicios
│   ├── Services/                       # Lógica de negocio
│   ├── Mappings/                       # Perfiles de AutoMapper
│   └── Validators/                     # Validadores FluentValidation
│
├── EmployeeManagement.Domain/           # Capa de dominio
│   ├── Entities/                       # Entidades del dominio
│   └── Interfaces/                     # Interfaces de repositorios
│
└── EmployeeManagement.Infrastructure/   # Capa de infraestructura
    ├── Data/                           # Contexto de base de datos
    ├── Repositories/                   # Implementación de repositorios
    └── Security/                       # Servicios de seguridad
```

## Requisitos Previos

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) o superior
- Un IDE o editor de código (Visual Studio, Rider, VS Code)
- Herramienta para pruebas de API (Swagger UI incluido, Postman, curl, etc.)

## Instalación y Ejecución

### 1. Clonar el Repositorio

```bash
git clone <repository-url>
cd Colpix.Employees
```

### 2. Restaurar Dependencias

```bash
dotnet restore
```

### 3. Ejecutar la Aplicación

```bash
cd EmployeeManagement.Api
dotnet run
```

La aplicación se ejecutará en:
- **HTTP**: `http://localhost:5000`
- **HTTPS**: `https://localhost:5001`
- **Swagger UI**: `http://localhost:5000` o `https://localhost:5001`

> **Nota**: Swagger UI está configurado para abrirse automáticamente en la raíz de la aplicación en modo desarrollo.

## Configuración

### Archivo `appsettings.json`

```json
{
  "JwtSettings": {
    "SecretKey": "YourSuperSecretKeyForJwtTokenGenerationShouldBeAtLeast32Characters!",
    "Issuer": "EmployeeManagementApi",
    "Audience": "EmployeeManagementClient",
    "ExpirationMinutes": 5  // ⬅️ PARAMETRIZABLE
  }
}
```

**Parámetros configurables:**
- `ExpirationMinutes`: Tiempo de expiración del token en minutos (default: 60, requerimiento: 5)
- Puedes cambiar este valor para ajustar el tiempo de expiración del token según tus necesidades

## Datos de Prueba

### Usuario de Prueba
- **Usuario**: `admin`
- **Contraseña**: `admin123`

### Empleados Pre-cargados

El sistema viene con 7 empleados de ejemplo organizados en una jerarquía:

```
John Smith (CEO)
├── Sarah Johnson (CTO)
│   └── Michael Brown (Dev Manager)
│       ├── Emily Davis (Developer)
│       └── David Wilson (Developer)
└── Lisa Anderson (HR Manager)
    └── Robert Taylor (HR Specialist)
```

## Documentación de la API

### Base URL
```
http://localhost:5000/api
```

### Endpoints

#### 1. Autenticación

##### `POST /api/auth/login`
Inicia sesión y obtiene un token JWT.

**Request:**
```json
{
  "username": "admin",
  "password": "admin123"
}
```

**Response (200 OK):**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "expiresAt": "2024-01-15T10:30:00Z",
  "username": "admin"
}
```

**Errores:**
- `400 Bad Request` - Datos inválidos
- `401 Unauthorized` - Credenciales incorrectas

---

#### 2. Listar Todos los Empleados

##### `GET /api/employees`
Obtiene la lista de todos los empleados con fecha de última actualización.

**Headers:**
```
Authorization: Bearer {token}
```

**Response (200 OK):**
```json
[
  {
    "id": 2,
    "name": "John Smith",
    "email": "john.smith@company.com",
    "supervisorId": null,
    "updatedAt": "2024-01-15T09:00:00Z"
  },
  {
    "id": 3,
    "name": "Sarah Johnson",
    "email": "sarah.johnson@company.com",
    "supervisorId": 2,
    "updatedAt": "2024-01-15T09:00:00Z"
  }
]
```

**Errores:**
- `401 Unauthorized` - Token inválido o expirado

---

#### 3. Obtener Detalles de un Empleado

##### `GET /api/employees/{id}`
Obtiene los detalles completos de un empleado, incluyendo el conteo de subordinados.

> **Nota Técnica**: Este endpoint utiliza procesamiento paralelo (`Task.WhenAll`) para obtener simultáneamente los detalles del empleado y el conteo de subordinados, optimizando el tiempo de respuesta.

**Headers:**
```
Authorization: Bearer {token}
```

**Response (200 OK):**
```json
{
  "id": 3,
  "name": "Sarah Johnson",
  "email": "sarah.johnson@company.com",
  "supervisorId": 2,
  "supervisorName": "John Smith",
  "subordinateCount": 4,
  "createdAt": "2024-01-15T09:00:00Z",
  "updatedAt": "2024-01-15T09:00:00Z"
}
```

**Campo `subordinateCount`**: Incluye subordinados directos e indirectos.
- En el ejemplo, Sarah tiene 1 subordinado directo (Michael) y 3 indirectos (Emily, David, Robert)

**Errores:**
- `401 Unauthorized` - Token inválido o expirado
- `404 Not Found` - Empleado no encontrado

---

#### 4. Crear Empleado

##### `POST /api/employees`
Registra un nuevo empleado.

**Headers:**
```
Authorization: Bearer {token}
```

**Request:**
```json
{
  "name": "Jane Doe",
  "email": "jane.doe@company.com",
  "supervisorId": 3
}
```

**Validaciones:**
- `name`: Requerido, máximo 200 caracteres
- `email`: Requerido, formato válido, máximo 200 caracteres
- `supervisorId`: Opcional, debe existir si se proporciona

**Response (201 Created):**
```json
{
  "id": 9,
  "name": "Jane Doe",
  "email": "jane.doe@company.com",
  "supervisorId": 3,
  "supervisorName": "Sarah Johnson",
  "subordinateCount": 0,
  "createdAt": "2024-01-15T10:00:00Z",
  "updatedAt": "2024-01-15T10:00:00Z"
}
```

**Errores:**
- `400 Bad Request` - Datos inválidos
- `401 Unauthorized` - Token inválido o expirado
- `404 Not Found` - Supervisor no encontrado

---

#### 5. Actualizar Empleado

##### `PUT /api/employees/{id}`
Actualiza los datos de un empleado existente.

**Headers:**
```
Authorization: Bearer {token}
```

**Request:**
```json
{
  "id": 9,
  "name": "Jane Doe Updated",
  "email": "jane.updated@company.com",
  "supervisorId": 4
}
```

**Validaciones:**
- El `id` en la URL debe coincidir con el `id` en el body
- Validaciones iguales que en creación

**Response (200 OK):**
```json
{
  "id": 9,
  "name": "Jane Doe Updated",
  "email": "jane.updated@company.com",
  "supervisorId": 4,
  "supervisorName": "Michael Brown",
  "subordinateCount": 0,
  "createdAt": "2024-01-15T10:00:00Z",
  "updatedAt": "2024-01-15T10:05:00Z"
}
```

**Errores:**
- `400 Bad Request` - Datos inválidos o IDs no coinciden
- `401 Unauthorized` - Token inválido o expirado
- `404 Not Found` - Empleado o supervisor no encontrado

---

## Cómo Probar la API

### Opción 1: Swagger UI (Recomendado)

1. Ejecuta la aplicación
2. Abre tu navegador en `http://localhost:5000`
3. Verás la interfaz de Swagger UI con todos los endpoints documentados
4. Para probar endpoints protegidos:
   - Haz clic en el botón **"Authorize"** (candado verde) en la parte superior
   - Primero usa el endpoint `/api/auth/login` para obtener un token
   - Copia el token de la respuesta (sin "Bearer")
   - Pégalo en el campo de autorización y haz clic en "Authorize"
   - Ahora puedes probar todos los endpoints

### Opción 2: cURL

#### 1. Login
```bash
curl -X POST http://localhost:5000/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"username":"admin","password":"admin123"}'
```

Guarda el token de la respuesta.

#### 2. Listar Empleados
```bash
curl -X GET http://localhost:5000/api/employees \
  -H "Authorization: Bearer {TU_TOKEN_AQUI}"
```

#### 3. Obtener Empleado por ID
```bash
curl -X GET http://localhost:5000/api/employees/3 \
  -H "Authorization: Bearer {TU_TOKEN_AQUI}"
```

#### 4. Crear Empleado
```bash
curl -X POST http://localhost:5000/api/employees \
  -H "Authorization: Bearer {TU_TOKEN_AQUI}" \
  -H "Content-Type: application/json" \
  -d '{
    "name": "New Employee",
    "email": "new.employee@company.com",
    "supervisorId": 3
  }'
```

#### 5. Actualizar Empleado
```bash
curl -X PUT http://localhost:5000/api/employees/9 \
  -H "Authorization: Bearer {TU_TOKEN_AQUI}" \
  -H "Content-Type: application/json" \
  -d '{
    "id": 9,
    "name": "Updated Employee",
    "email": "updated.employee@company.com",
    "supervisorId": 4
  }'
```

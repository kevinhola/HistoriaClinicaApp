# Sistema de Historia Clínica Electrónica v2.0

## Características

### FASE 1 ✅
- Login con autenticación
- Gestión de roles (Administrador, Médico, Recepcionista)
- Dashboard profesional
- CRUD completo de pacientes
- Perfil de usuario
- Sistema de sesión
- Logs de acceso

### FASE 2 ✅
- **Historias Clínicas**: Gestión completa con auditoría
- **Consultas Médicas**: Registro detallado con signos vitales
- **Recetas**: Asociadas a consultas
- **Estudios Médicos**: Laboratorios y diagnósticos
- **Archivos Médicos**: Cifrado AES-256 automático
- **Administración de Usuarios**: Panel completo para admin
- **Logs Médicos Avanzados**: Auditoría completa
- **UI Mejorada**: Diseño profesional y moderno

## Tecnologías

- **.NET 6** (WPF)
- **SQLite** (Base de datos local)
- **MVVM** (Arquitectura)
- **AES-256** (Cifrado)
- **PBKDF2** (Hashing de contraseñas)

## Instalación

### Opción 1: Compilar desde código
```bash
# Restaurar dependencias
dotnet restore

# Compilar
dotnet build

# Ejecutar
dotnet run
```

### Opción 2: Instalador EXE

1. Ejecutar: `prepare-installer.ps1`
2. Usar instalador generado en: `Installer/Output/HistoriaClinicaSetup.exe`

## Credenciales Iniciales

- **Usuario**: admin
- **Contraseña**: Admin123!

⚠️ **IMPORTANTE**: Cambiar contraseña después del primer login

## Estructura de Carpetas
```
HistoriaClinicaApp/
├── Database/          # Base de datos SQLite
├── Logs/             # Logs del sistema
├── Storage/          # Archivos cifrados
│   └── Pacientes/
├── Config/           # Configuración y claves
└── Backups/          # Respaldos automáticos
```

## Seguridad

- ✅ Contraseñas hasheadas con PBKDF2
- ✅ Archivos médicos cifrados con AES-256
- ✅ Logs de auditoría completos
- ✅ Sesiones seguras
- ✅ Validación de permisos por rol

## Roles y Permisos

### Administrador
- Gestión completa de usuarios
- Acceso total al sistema
- Configuración avanzada

### Médico
- Gestión de pacientes
- Historias clínicas
- Consultas y recetas
- Estudios
- Archivos médicos

### Recepcionista
- Gestión básica de pacientes
- Consulta de información
- Sin acceso a modificación médica

## Soporte

Para reportar errores o solicitar funcionalidades:
- Email: soporte@sistemamedico.com
- GitHub Issues

## Licencia

Propietario - Todos los derechos reservados
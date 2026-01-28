# Prueba Técnica – NSerio

Este repositorio contiene la solución desarrollada como parte de la **prueba técnica**, implementada con **Backend en .NET 7** y **Frontend en Angular 17**, siguiendo buenas prácticas de arquitectura, separación de responsabilidades y código limpio.

---

## Arquitectura General

La solución está dividida en dos componentes principales:

- **Backend**: API REST desarrollada en **.NET 7**
- **Frontend**: Aplicación web desarrollada en **Angular 17**

Ambos componentes están desacoplados y se comunican mediante HTTP (JSON).

---

## 🛠️ Tecnologías Utilizadas

### Backend
- .NET 7
- ASP.NET Core Web API
- Entity Framework Core
- Arquitectura en capas
- Inyección de dependencias
- DTOs para transporte de datos

### Frontend
- Angular 17
- TypeScript
- HTML / CSS
- Angular CLI
- Servicios para consumo de API REST

---

## Estructura del Proyecto

```txt
/
├── backend/
│   ├── Api/
│   ├── Application/
│   ├── Domain/
│   └── Infrastructure/
│
├── frontend/
│   ├── src/
│   ├── app/
│   └── assets/
│
└── README.md

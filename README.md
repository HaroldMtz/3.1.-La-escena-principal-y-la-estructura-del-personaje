# 🎮 Actividad 3.8 – Conexión a Base de Datos (Firebase Firestore)

## 🧠 Objetivo
Crear una conexión a una base de datos para guardar el total de monedas obtenidas por el jugador y su progreso entre niveles dentro del videojuego 2D desarrollado en Unity.

---

## 🔧 Descripción del Proyecto
Este proyecto conecta Unity con **Firebase Cloud Firestore** para almacenar de manera persistente el avance del jugador.

Los datos que se guardan son:
- **totalCoins** → monedas acumuladas.
- **currentLevel** → nivel actual.
- **unlockedLevels** → lista de niveles desbloqueados.
- **updatedAtUnix** → marca de tiempo de la última actualización.

El progreso se guarda automáticamente al recolectar monedas o completar un nivel, y se carga al iniciar el juego.

---

## 🧩 Estructura Firestore
Ruta del documento:

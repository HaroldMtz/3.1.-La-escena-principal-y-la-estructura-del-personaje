# U3 · 3.1 — Escena principal y estructura del personaje (Unity 2D)

Proyecto base para la actividad **3.1**: creación de la **escena principal** y la **estructura de un personaje 2D** con animaciones que reaccionan al teclado, siguiendo el estilo del tutorial visto en clase.

---

## 🎮 Requisitos

- **Unity** 2021.3+ (LTS recomendado) con plantilla **2D**  
- **Active Input Handling**: `Both` (Edit → Project Settings → Player)  
- **Paquetes**:
  - **Cinemachine** (se restaura desde `Packages/manifest.json`)

> **Importante:** Este repo **no** incluye `Library/`, `Temp/`, etc. (ignorados con `.gitignore`). Al abrirlo, Unity regenerará todo.

---


---

## 🧩 Escena y personaje

- **Player (raíz)**: `Rigidbody2D (Dynamic)`, `Collider2D`, **sin** Animator.
- **Graphics (hijo de Player)**: `SpriteRenderer` + `Animator (Player.controller)`.
- **Ground**: `Sprite (Square)` + `BoxCollider2D`, **Layer = Ground**.
- **GroundCheck**: objeto vacío **hijo de Player**, bajo los pies; usado para detectar suelo.

**Animator (`Player.controller`)**  
Parámetros:
- `Speed` *(float)* — controla Idle ↔ Run
- `IsJumping` *(bool)* — controla estados de salto
- `IsAttacking` *(trigger)* — dispara la animación de ataque

Transiciones clave:
- **Idle ↔ Run** por `Speed` (>0.1 / ≤0.1), *Has Exit Time* **OFF**
- **Any State → Jump** por `IsJumping == true`, *Has Exit Time* **OFF**
- **Jump → Idle** (`Speed ≤ 0.1`) y **Jump → Run** (`Speed > 0.1`), `IsJumping == false`, *Has Exit Time* **OFF**
- **Any State → Attack** (Trigger `IsAttacking`), *Has Exit Time* **OFF**
- **Attack → Idle** con *Has Exit Time* **ON**

> Los clips (`Idle/Run/Jump/Attack`) pueden ser **placeholders** (p. ej., pequeñas variaciones de escala) como en el video.

---

## 🎥 Cámara con Cinemachine

- **Main Camera**: `Projection = Orthographic`, `Size ≈ 5`, componente `Cinemachine Brain`.
- **CinemachineCamera (vcam)**: `Follow = Player`, `Priority > 10`.  
  Recomendado en `Cinemachine Position Composer`:
  - `Screen Position (0.5, 0.5)`
  - `Dead Zone = 0–0.2`
  - `Damping = 0–0.5` (ajusta suavidad a gusto)

---

## ⌨️ Controles

- **Mover**: `A/D` o `←/→`
- **Saltar**: `Espacio`
- **Atacar** (placeholder visual): `J` o `K`

---

## ▶️ Cómo ejecutar

1. Clona o descarga el repo.
2. Abre la carpeta del proyecto con **Unity**.
3. Carga la escena: `Assets/Scenes/Main.unity`.
4. **Play**.

---

## 🛠️ Script principal

- `Assets/Scripts/PlayerMove.cs`  
  - Soporta **Legacy Input** y **New Input System** (con `Both`).
  - Actualiza `Speed`, `IsJumping` y dispara `IsAttacking` (J/K).
  - Voltea el sprite según la dirección.
  - Detección de suelo con `GroundCheck + LayerMask`.

---

## ✅ Criterios de evaluación y cumplimiento

- **El personaje principal carga animación de estado según teclado**  
  `Speed` (A/D) → Idle/Run, `Espacio` → Jump, (extra: `J/K` → Attack). ✔️  

---

## ❓Solución rápida de problemas

- **El player atraviesa el suelo**:  
  - `Ground` con `BoxCollider2D` y **Is Trigger OFF**  
  - `Player` con `Rigidbody2D (Dynamic)` y `Collision Detection = Continuous`
- **No cambia `Speed` en el Animator**:  
  - `Edit → Project Settings → Player → Active Input Handling = Both`
  - El **Animator Controller** está en **Graphics**, no en Player
  - El parámetro se llama **exactamente** `Speed`
- **La cámara no sigue**:  
  - `Main Camera` tiene `Cinemachine Brain`  
  - vcam con `Follow = Player` (raíz), `Priority` > 10  
  - `Dead Zone = 0` para probar; `Damping = 0` para respuesta inmediata
- **`IsJumping` no cambia**:  
  - Mueve `GroundCheck` un poco hacia abajo  
  - Aumenta `groundRadius` (0.2–0.25)  
  - Verifica `Ground Layer` en el script y en el objeto `Ground`

---

## 📚 Apoyos (video de referencia)

- JLPM. (2021, 31 de enero). *Tutorial COMPLETO Unity 2D desde Cero - 2021* [Video]. YouTube.  
  https://www.youtube.com/watch?v=GbmRt0wydQU  
  https://www.youtube.com/watch?v=GbmRt0wydQU&t=404s

---

## 📝 Notas

- Los **.meta** en `Assets/` son necesarios; **no los borres**.  


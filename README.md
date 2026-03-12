# AgentFramework

Framework base para orquestar agentes especializados en .NET.

## Pipeline de PR y merge automático

Se agregaron dos workflows en GitHub Actions:

- `CI PR` (`.github/workflows/ci-pr.yml`): ejecuta restore, build y test en cada PR hacia `main` o `master`, y también en eventos `merge_group` para soportar merge queue.
- `Enable PR auto-merge` (`.github/workflows/auto-merge.yml`): habilita auto-merge en modo `squash` para PRs que tengan la etiqueta `automerge`.

### Detalles importantes de implementación

- El workflow de CI cancela ejecuciones previas del mismo PR con `concurrency` para ahorrar minutos.
- El job de CI se omite para PRs en estado draft y vuelve a correr cuando pasan a `ready_for_review`.
- El workflow de auto-merge usa `pull_request_target` para poder escribir sobre el PR (habilitar auto-merge) y está restringido a base `main`/`master`.

### Configuración requerida en GitHub

Para que el merge automático sea seguro y realmente funcione:

1. Activar **Allow auto-merge** en la configuración del repositorio.
2. Configurar reglas de protección de rama para `main`/`master`:
   - Requerir PR para merge.
   - Requerir al menos 1 aprobación.
   - Requerir checks de estado y marcar `CI PR / Build and test` como obligatorio.
   - Requerir rama actualizada antes de merge.
3. (Opcional recomendado) Activar **Merge queue**.

### Uso

1. Crear PR.
2. Agregar etiqueta `automerge`.
3. Cuando el check de CI pase y se cumplan las reglas de protección, GitHub realizará el merge automáticamente

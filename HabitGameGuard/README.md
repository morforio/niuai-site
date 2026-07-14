# HabitGameGuard

Aplicativo WPF (.NET 8) para Windows 10/11 que converte hábitos em créditos e libera partidas de jogos (com foco em League of Legends). O monitor roda em background, acompanha a Live Client Data API local do LoL e bloqueia o jogo quando não há partidas disponíveis. Também servindo de teste para Vibe Coding

## Arquitetura
- **Models**: entidades de domínio (hábitos, execuções, apps bloqueados, sessão de partidas, configurações).
- **Data**: `AppDbContext` com SQLite local em `%LOCALAPPDATA%/HabitGameGuard/habitguard.db`.
- **Services**: regras de negócio e monitoramento (`HabitoService`, `CreditoService`, `AplicativoBloqueadoService`, `SessaoDePartidaService`, `MonitorDeProcessosService`).
- **ViewModels**: camada MVVM para cada janela.
- **Views**: janelas WPF (`MainWindow`, `HabitosWindow`, `AppsWindow`, `LojaWindow`).
- **Tray**: ícone na bandeja com opção de pausar bloqueio (modo DEBUG) e sair.

## Estrutura de pastas
```
HabitGameGuard/
  App.xaml
  App.xaml.cs
  HabitGameGuard.csproj
  README.md
  Models/
  Data/
  Services/
  ViewModels/
  Views/
  Resources/
```

## Compilar e executar
1. Instale o .NET 8 SDK em Windows.
2. No diretório `HabitGameGuard`, execute:
   ```bash
   dotnet restore
   dotnet build
   dotnet run
   ```
3. O banco SQLite é criado automaticamente na primeira execução em `%LOCALAPPDATA%/HabitGameGuard/habitguard.db`.

## Configurar nomes de processo
- Descubra o nome do executável no Gerenciador de Tarefas (coluna **Nome do processo**). Ex.: `League of Legends.exe` ou `LeagueClient.exe`.
- Abra **Apps Bloqueados** e cadastre:
  - Nome amigável: "League of Legends"
  - Nome do processo: exatamente como visto (pode incluir `.exe`).
  - Créditos por partida: número de créditos necessários para liberar 1 jogo.
  - Marque como **Ativo** para que o bloqueio funcione.

## Fluxo principal
1. Cadastre hábitos e use **Feito hoje** para registrar a conclusão. Créditos são adicionados conforme a dificuldade (fácil=1, médio=2, difícil=3).
2. Na **Loja de Partidas**, selecione o app e informe quantos créditos gastar para comprar partidas.
3. O serviço de monitoramento verifica o processo do jogo e a Live Client Data API (https://127.0.0.1:2999/liveclientdata/allgamedata), ignorando certificado self-signed.
4. Se não houver partidas disponíveis e o jogo estiver aberto, o processo é encerrado (modo DEBUG desativa o kill).
5. Ao detectar o fim de uma partida (Live Client Data deixa de responder após estar em jogo), 1 partida é debitada da sessão.

> Comentário: o app é para auto-regulação; usuários avançados podem burlar matando o processo, editando o banco, etc. Não é anticheat.

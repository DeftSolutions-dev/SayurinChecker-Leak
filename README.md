# Sayurin Checker / SayurinOwnChecker - слив сурсов

Короче вот полные сурсы SayurinChecker, он же Sayurin Security Suite v3.4.8, тот самый "чекер" с сайта [mujqk.github.io/SayurinOwnChecker](https://mujqk.github.io/SayurinOwnChecker/) для проекта Baza CS2.

Реверснул оба бинарника (`SayurinChecker.exe` + обёртку `SayurinChecker (2).exe`), снял обфускацию через de4dot, вытащил весь C#/WPF код, собрал обратно на .NET 9, всё компилится под `net9.0-windows`. По ресурсам может не совпадать (apps/*.exe с NirSoft утилитами в сурсах не лежат, их нужно докидать вручную - они были упакованы внутрь exe через `AssemblyAssociatedContentFile`).

## Кто автор

`NazeSayurin`, он же `mujqk` на GitHub, он же юзер Discord `nazzze_sayur`. Позиционирует себя как *"C# Enthusiast • .NET 9 Architect • OSU! Relax Master"* (релакс мастер в осу - это где софт за тебя играет, ну вы поняли уровень).

Код **написан нейронкой**, вайб-кодинг чистой воды.

### Откуда это известно:

- **Anti-VM "защита"** лоадера - это чек на `Environment.ProcessorCount < 2`, `TotalMemory < 2GB`, и существование трёх файлов `drivers\VBoxGuest.sys`, `drivers\vmtoolsd.exe`, `drivers\vmmouse.sys` в `system32`. Обходится созданием пустого файла `touch VBoxGuest.sys`... нет, вру, НЕсозданием. Скрипт-киддис уровень.
- **`VerifyHumanPresence()`** - объявлена функция проверки движения мыши 2 секунды, но **из `Main()` её никто не вызывает**. Нейронка сгенерила, автор не заметил что забыл вставить вызов.
- **XOR-ключ** для расшифровки embedded DLL валяется прям текстом: `SAYURIN_Diagnostic_Enterprise_Suite_Ref_0x9A42_v3.4.9_FINAL`. С гениальным обфусцированием `(key[i] + i%37) ^ 0x4F` + nibble swap. Расшифровывается за 5 минут.
- **Несуществующая `SayurinCore.dll`** - весь `NativeInterop.cs` это заглушки `[DllImport("SayurinCore.dll")]` для 20+ функций, которых физически нет в сборке. Каждый вызов обёрнут в `try/catch` и падает в managed fallback. То есть нейронка сгенерила "нативное ядро", автор его не написал, но все вызовы оставил и прикрыл эксепшенами.
- **Комменты в SteamScanner** - регексы парсят HTML страницы baza-cs2.ru, то есть чекер скрейпит вебсайт по `<div class="user_badge">` вместо нормального API. Как только админ поменяет вёрстку - всё сломается.

## Чем якобы лучше других чекеров)

| Что обещают | Что по факту |
|---|---|
| "Глубокое сканирование файлов" | `folder.Name.Contains("nixware")` |
| "Детекция читов" | Список из **136 текстовых строк** типа `"xone"`, `"onetap"`, `"midnight"` - проверяется через `.Contains()` |
| "Proc scanner" | `Process.GetProcesses().Where(p => p.ProcessName.Contains("xone"))` |
| "Registry Navigation" | `Registry.LocalMachine.OpenSubKey("...Uninstall")` и дамп всех установленных прог |
| "Baza API синхронизация" | HTTP GET на `https://baza-cs2.ru/profiles/{id}/block/0/` и regex по HTML |
| "VM Detector" | Проверка MAC префиксов (`080027`, `000569`), драйверов VBox/VMware, WMI `Manufacturer` |
| "16+ Интегрированных утилит" | `Process.Start("everything.exe")`, `Process.Start("systeminformer.exe")` - бесплатные NirSoft тулзы внутри архива |
| "Advanced Tools" | `Process.Start("cmd.exe", "/c ncpa.cpl")` |
| "Fast Paths" | `Process.Start("explorer.exe", "%AppData%")` |
| "HWID Data / Input Analysis" | Запуск `usbdeview.exe /sxml` и парсинг XML |
| "Защита" | `IsDebuggerPresent` даже нет, только обфускация имён + XOR лоадер |

**Нет в коде:**
- Hash-based детекта (CRC/MD5/SHA)
- PE парсинга (импорты/экспорты/сигнатуры)
- Анализа памяти (`ReadProcessMemory`)
- Байт-сигнатур
- Behavioral/heuristic анализа
- Проверки code injection / DLL injection
- Анализа crash дампов (хоть утилита запускается, результаты не читаются)

Короче, любой чит переименованный в `svchost.exe` и лежащий в `C:\Users\User\Downloads\kakashka\svchost.exe` **пройдёт чекер на ура**. Единственное что зацепит - это если админ руками через Everything / SystemInformer что-то увидит глазами.

## Структура
```
Govno mamonta/
├── Source/                            # Исходник, собирается
│   ├── BazaChecker/
│   │   ├── App.xaml + App.xaml.cs     # OnStartup с SplashScreen
│   │   ├── MainWindow.xaml + .cs      # ~2700 строк, вся UI-логика
│   │   ├── SplashScreen.xaml + .cs
│   │   ├── MacroCheckWindow.xaml + .cs # Окно "проверки макросов" - рисовалка на Canvas лол
│   │   ├── VmReportWindow.xaml + .cs
│   │   ├── ConfirmationWindow.xaml + .cs
│   │   ├── Components/
│   │   │   └── NotificationItemControl.xaml + .cs
│   │   ├── Models/                    # POCO-классы: SteamAccount, ProgramInfo, UsbDeviceInfo...
│   │   └── Services/
│   │       ├── CheatDatabase.cs       # 136 строк-имён, не хешей
│   │       ├── DiskScanner.cs         # Contains() по имени папки
│   │       ├── ProcessScanner.cs      # Contains() по ProcessName
│   │       ├── RegistryScanner.cs     # Дамп Uninstall + MuiCache + UserAssist (ROT13)
│   │       ├── SteamScanner.cs        # Парсит loginusers.vdf + HTML baza-cs2.ru
│   │       ├── UsbScanner.cs          # Оборачивает usbdeview.exe
│   │       ├── VmDetector.cs          # MAC/драйверы/WMI
│   │       ├── DiscordService.cs      # RPC
│   │       ├── BufferService.cs       # Захардкоженные примеры Everything-запросов
│   │       ├── NativeInterop.cs       # P/Invoke в несуществующий SayurinCore.dll
│   │       ├── SystemInfoProvider.cs  # WMI Win32_Processor/VideoController
│   │       ├── AppLauncher.cs         # Process.Start для NirSoft утилит
│   │       ├── SiteLauncher.cs        # Process.Start для URL
│   │       └── FolderLauncher.cs      # Process.Start для папок
│   ├── Properties/AssemblyInfo.cs     # Список всех встроенных apps/*.exe
│   ├── assets/                        # Иконки + шрифт Rubik
│   ├── resources/lang.ru.xaml, lang.en.xaml  # Локализация
│   ├── DiscordRPC.dll                 # Библиотека Discord Rich Presence
│   └── SayurinChecker.csproj
│
├── Site/                              # github.com/mujqk/SayurinOwnChecker
│   ├── index.html                     # Лендинг на GitHub Pages
│   ├── styles.css, script.js
│   ├── history.json                   # Changelog
│   └── screen1..6.png                 # Скрины интерфейса
```

## Что внутри нашлось

### Архитектура лоадера `SayurinChecker (2).exe`

Это обёртка 11.9 MB, внутри PE-ресурс `DiagnosticCore.dat` (5.9 MB) - это настоящий `SayurinChecker.exe` (managed DLL). XOR-расшифровывается и загружается через `Assembly.Load()` + reflection поиск наследника `System.Windows.Application`.

```csharp
// BazaChecker/SayurinServiceLoader.cs (из v2)
private static byte[] GenerateServiceEntropy() {
    byte[] array = new byte[64];
    string text = "SAYURIN_Diagnostic_Enterprise_Suite_Ref_0x9A42_v3.4.9_FINAL";
    for (int i = 0; i < array.Length; i++)
        array[i] = (byte)(text[i % text.Length] ^ 0x22);
    return array;
}

// расшифровка каждого байта embedded DLL
byte b = (byte)((array3[(j + 7) % array3.Length] + j % 37) ^ 0x4F);
byte b2 = (byte)(array2[j + 4] ^ b);
array4[j] = (byte)((b2 >> 4) | (b2 << 4));  // nibble swap
```

Ключи/константы для деобфускации:
- Base entropy string: `SAYURIN_Diagnostic_Enterprise_Suite_Ref_0x9A42_v3.4.9_FINAL`
- XOR на entropy: `0x22`
- Дополнительный XOR финал: `0x4F`
- Шифт: `+ i%37`
- Трансформация: nibble swap (`(b >> 4) | (b << 4)`)
- Первые 4 байта файла `DiagnosticCore.dat` - длина payload (uint32 LE)
- Payload начинается с offset `4`

### Discord

```csharp
// MainWindow.xaml.cs:818
DiscordService.Initialize("1463981045555789825");

// DiscordService.cs:63
SetPresence("Проверка на читы", "В главном меню", "logo");
```

Discord Application ID: `1463981045555789825` (публичный)

### API endpoints (все публичные HTTP GET)

```
https://baza-cs2.ru/profiles/{SteamId64}/block/0/   - HTML скрейп статуса игрока
https://steamcommunity.com/profiles/{SteamId64}/?xml=1  - Steam public XML
https://github.com/Mujqk/SayurinOwnChecker/releases - откуда грузятся обновления
https://mujqk.github.io/SayurinOwnChecker/          - лендинг
https://baza-cs2.ru/tickets/                        - техподдержка
```

**Никакого API ключа нет.** Сервер baza-cs2.ru отдаёт публичные страницы профилей, чекер их парсит регексом:

```csharp
// SteamScanner.cs
private static readonly Regex BadgeClassPattern = new Regex(
    "user_badge\\s+badge_(\\w+)", RegexOptions.IgnoreCase);
private static readonly Regex VacBannedPattern = new Regex(
    "<vacBanned>(\\d+)</vacBanned>");
```

То есть "синхронизация с базой проекта" - это `HttpClient.GetAsync()` и `Regex.Match()` по HTML. Изменится вёрстка сайта - всё отвалится.

### База "читов"

```csharp
// CheatDatabase.cs
public static readonly HashSet<string> Signatures = new HashSet<string>(StringComparer.OrdinalIgnoreCase) {
    "xone", "exloader", "com.swiftsoft", "interium", "mason", "lunocs2",
    "neverlose", "midnight", "hellshack", "nixware", "onetap", "skeet",
    "fatality", "osiris", /* всего 136 строк */
};
```

**136 текстовых строк.** Не хешей, не сигнатур, не путей - просто строки. Проверяются через `.Contains()` по имени папки/файла/процесса. Без учёта регистра.

Это значит:
- Чит в `C:\Games\MyTotallyLegitApp\` - **не найдётся**
- Текстовый файл `xone_recipes.txt` - **задетектится как чит** 🫠
- Переименованный `xone.exe` → `update.exe` - **не найдётся**

### GitHub

Аккаунт: [Mujqk](https://github.com/Mujqk)
Репа: [SayurinOwnChecker](https://github.com/Mujqk/SayurinOwnChecker) (там только docs/index.html для GitHub Pages, самих сурсов нет - только скриншоты и лендинг)

### Встроенные NirSoft/бесплатные утилиты

Из `Properties/AssemblyInfo.cs`, 110+ атрибутов `AssemblyAssociatedContentFile`:

```
apps/appcrashview.exe             apps/everything.exe
apps/browserdownloadsview.exe     apps/executedprogramslist.exe
apps/browsinghistoryview.exe      apps/journaltrace.exe
apps/devmanview.exe               apps/jumplistsview.exe
apps/easycheatdetector.exe        apps/lastactivityview.exe
apps/muicacheview.exe             apps/openedfilesview.exe
apps/pafish64.exe                 apps/peview.exe
apps/previousfilesrecovery.exe    apps/regscanner.exe
apps/registryfinder.exe           apps/shellbaganalyzer.exe
apps/shellbagsview.exe            apps/sysenvcheck.exe
apps/systeminformer.exe           apps/usbdeview.exe
apps/userassistview.exe           apps/winprefetchview.exe
```

**Всё это бесплатный софт** который можно руками скачать с [nirsoft.net](https://www.nirsoft.net) и [systeminformer.sourceforge.io](https://systeminformer.sourceforge.io/). Чекер просто запускает их через `Process.Start()` и показывает окошки.

Никакого парсинга выхлопа нет (кроме `usbdeview /sxml`). То есть пользователь сам должен глазами смотреть в Everything/SystemInformer что-то подозрительное. Это вообще не автоматический детект, это **GUI-лаунчер для NirSoft**.

### VmDetector

```csharp
// VmDetector.cs
// Registry checks:
HKLM\SOFTWARE\Oracle\VirtualBox Guest Additions
HKLM\SOFTWARE\VMware, Inc.\VMware Tools
HKLM\HARDWARE\DESCRIPTION\System\BIOS\SystemManufacturer

// Process names:
vboxservice, vmtoolsd, vmwaretray, prl_tools, xenservice

// Files:
C:\Windows\System32\drivers\VBox*.sys
C:\Windows\System32\drivers\vmware*.sys
C:\Windows\System32\drivers\vmtools.sys

// MAC prefixes:
080027 (VBox), 000569 (VMware), 001C42 (Parallels)

// WMI:
Win32_ComputerSystem.Manufacturer .Contains("microsoft") && .Contains("virtual")
```

**Нет:** CPUID `hypervisor bit`, RDTSC timing, SGX проверок, EBX pattern, MSR чтения. Обход пишется за 15 минут через VM-escape туториал 2009 года.

### MacroCheckWindow

Отдельное окно "Проверка макросов" - это **WPF Canvas с кистью для рисования**. Я серьёзно, вот код:

```csharp
// MacroCheckWindow.xaml.cs
private void MacroCanvas_MouseMove(object sender, MouseEventArgs e) {
    if (_isDrawing) {
        Line element = new Line {
            X1 = _lastPoint.X, Y1 = _lastPoint.Y,
            X2 = position.X, Y2 = position.Y,
            Stroke = _currentBrush, StrokeThickness = 2.0,
            StrokeStartLineCap = PenLineCap.Round,
        };
        MacroCanvas.Children.Add(element);
    }
}
```

Админ просит игрока "нарисовать кружочек" на канвасе, типа проверка что не макрос. Уровень Windows Paint из 1995. Никакого анализа тайминга/траектории/давления - просто Line.Add() на Canvas.

## Как собрать

```
Windows 10/11
.NET 9 SDK (winget install Microsoft.DotNet.SDK.9)
```

```
cd Source
dotnet publish SayurinChecker.csproj -c Release -r win-x64 --self-contained false -o ../Release
```

Результат:
```
Release/SayurinChecker.exe   (~6.5 MB, framework-dependent)
```

Если нужен standalone - добавь `--self-contained true` (потянет ~150 MB рантайма .NET 9).

Для работы утилит `apps/*.exe` их нужно положить рядом с `SayurinChecker.exe` (скачать с [NirSoft](https://www.nirsoft.net/utils/nircmd.html) и [SystemInformer](https://systeminformer.sourceforge.io/)).

---

Эх эти нейронщики сново... 300 строк логики в 12 MB exe, куча NirSoft утилит, HTTP скрейп чужого сайта по regex, и 136 текстовых строк вместо базы читов. Вайб-кодинг эпохи Claude или что они там юзают...
